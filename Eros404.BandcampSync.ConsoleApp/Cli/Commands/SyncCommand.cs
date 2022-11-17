using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands;

internal class SyncCommand : AsyncCommand<SyncSettings>
{
    private readonly ILogger _logger;
    private readonly IBandcampApiService _bandCampService;
    private readonly ILocalCollectionService _localCollectionService;
    private readonly IBandcampWebDriverFactory _webDriverFactory;
    private readonly IMailService _mailService;

    public SyncCommand(ILogger logger, IBandcampApiService bandCampService, ILocalCollectionService localCollectionService, IBandcampWebDriverFactory webDriverFactory, IMailService mailService)
    {
        _logger = logger;
        _bandCampService = bandCampService;
        _localCollectionService = localCollectionService;
        _webDriverFactory = webDriverFactory;
        _mailService = mailService;
    }

    private int _numberOfItemDownloaded;
    private int _numberOfNewLinkSent;

    public override async Task<int> ExecuteAsync(CommandContext context, SyncSettings settings)
    {
        var compareResult = await CompareCollectionsCommand.CompareAsync(_bandCampService, _localCollectionService);
        if (compareResult == null)
            return -1;
        AnsiConsole.Write(compareResult.ToTable("Missing Items"));
        if (!compareResult.MissingAlbums.Any() && !compareResult.MissingTracks.Any())
            return 0;
        
        var selectedAlbums = SelectAlbumsToDownload(compareResult.MissingAlbums);
        var selectedTracks = SelectTracksToDownload(compareResult.MissingTracks);
        if (!selectedAlbums.Any() && !selectedTracks.Any())
            return 0;
        
        AnsiConsole.Status()
            .Start("Preparing download...", _ =>
            {
                using var webDriver = _webDriverFactory.CreateWithIdentity();
                foreach (var album in selectedAlbums)
                    SetItemDownloadLink(webDriver, album, settings.AudioFormat);
                
                foreach (var track in selectedTracks)
                    SetItemDownloadLink(webDriver, track, settings.AudioFormat);
            });
        if (_numberOfNewLinkSent > 0)
        {
            AnsiConsole.MarkupLine(
                $"[green]{_numberOfNewLinkSent}[/] link{(_numberOfNewLinkSent > 1 ? "s" : "")} have been sent to {_mailService.EmailAddress}.");
        }

        await AnsiConsole.Status()
            .StartAsync("Downloading...", async _ =>
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
                await DownloadMissingAlbums(client, selectedAlbums);
                await DownloadMissingTracks(client, selectedTracks, settings.AudioFormat);
            });
        AnsiConsole.MarkupLine(
            $"[green]{_numberOfItemDownloaded}[/] item{(_numberOfItemDownloaded > 1 ? "s" : "")} downloaded.");

        return 0;
    }
    
    private void SetItemDownloadLink(IBandcampWebDriver webDriver, CollectionItem item, AudioFormat audioFormat)
    {
        var result = webDriver.GetDownloadLink(item.RedownloadUrl ?? "", audioFormat, _mailService.EmailAddress);
        if (result.HasExpired)
        {
            AnsiConsole.MarkupLine(
                $"Download link for [blue]{item.ToString().EscapeMarkup()}[/] expired. {(result.InvalidEmail ? $"The email address {_mailService.EmailAddress} was [red]rejected[/] by Bandcamp." : "[green]New link sent.[/]")}");
            if (!result.InvalidEmail)
                _numberOfNewLinkSent++;
        }

        item.DownloadLink = result.DownloadLink;
    }

    private async Task DownloadMissingAlbums(HttpClient client, List<MissingAlbum> missingAlbums)
    {
        const int batchSize = 25;
        var numberOfAlbumBatches = (int)Math.Ceiling((double)missingAlbums.Count / batchSize);
        for (var i = 0; i < numberOfAlbumBatches; i++)
        {
            var currentAlbums = missingAlbums.Skip(i * batchSize).Take(batchSize);
            var tasks = currentAlbums.Select(album => DownloadAlbum(client, album));
            await Task.WhenAll(tasks);
        }
    }
    private async Task DownloadAlbum(HttpClient client, Album album)
    {
        await DownloadCollectionItem(client, album,
            stream => _localCollectionService.AddAlbum(stream, album));
    }
    private async Task DownloadMissingTracks(HttpClient client, List<MissingTrack> missingTracks, AudioFormat audioFormat)
    {
        const int batchSize = 50;
        var numberOfAlbumBatches = (int)Math.Ceiling((double)missingTracks.Count / batchSize);
        for (var i = 0; i < numberOfAlbumBatches; i++)
        {
            var currentTracks = missingTracks.Skip(i * batchSize).Take(batchSize);
            var tasks = currentTracks.Select(track => DownloadTrack(client, track, audioFormat));
            await Task.WhenAll(tasks);
        }
    }
    private async Task DownloadTrack(HttpClient client, Track track, AudioFormat audioFormat)
    {
        await DownloadCollectionItem(client, track,
            stream => _localCollectionService.AddTrack(stream, track, audioFormat));
    }
    
    private async Task DownloadCollectionItem(HttpClient client, CollectionItem item, Action<Stream> addToCollectionAction)
    {
#if DEBUG
        AnsiConsole.MarkupLine($"Start [blue]{item.ToString().EscapeMarkup()}[/] download.");
#endif
        var response = await client.GetAsync(item.DownloadLink);
        response.EnsureSuccessStatusCode();
        addToCollectionAction(await response.Content.ReadAsStreamAsync());
        AnsiConsole.MarkupLine($"[blue]{item.ToString().EscapeMarkup()}[/] downloaded.");
        _numberOfItemDownloaded++;
    }

    private static List<MissingAlbum> SelectAlbumsToDownload(List<MissingAlbum> albums)
    {
        if (!albums.Any())
            return new List<MissingAlbum>();
        var dictionary = albums.ToDictionary(a => a.ToString(), a => a);
        var all = $"All albums ({albums.Count})";
        var selectedKeys = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[blue]Select albums to download:[/]")
                .NotRequired()
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more albums)[/]")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle an album, [green]<enter>[/] to accept)[/]")
                .AddChoices(all)
                .AddChoices(dictionary.Keys));
        return selectedKeys.Contains(all)
            ? albums
            : selectedKeys.Select(key => dictionary[key]).ToList();
    }
    private static List<MissingTrack> SelectTracksToDownload(List<MissingTrack> tracks)
    {
        if (!tracks.Any())
            return new List<MissingTrack>();
        var dictionary = tracks.ToDictionary(track => track.ToString(), a => a);
        var all = $"All tracks ({tracks.Count})";
        var selectedKeys = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[blue]Select tracks to download:[/]")
                .NotRequired()
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more tracks)[/]")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle a track, [green]<enter>[/] to accept)[/]")
                .AddChoices(all)
                .AddChoices(dictionary.Keys));
        return selectedKeys.Contains(all)
            ? tracks
            : selectedKeys.Select(key => dictionary[key]).ToList();
    }
}