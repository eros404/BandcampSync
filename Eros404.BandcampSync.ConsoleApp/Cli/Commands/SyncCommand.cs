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

    public SyncCommand(ILogger logger, IBandcampApiService bandCampService, ILocalCollectionService localCollectionService, IBandcampWebDriverFactory webDriverFactory)
    {
        _logger = logger;
        _bandCampService = bandCampService;
        _localCollectionService = localCollectionService;
        _webDriverFactory = webDriverFactory;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, SyncSettings settings)
    {
        try
        {
            var compareResult = await CompareCollectionsCommand.CompareAsync(_bandCampService, _localCollectionService);
            if (compareResult == null)
                return 1;
            AnsiConsole.Write(compareResult.ToTable("Missing Items"));
            if (!compareResult.MissingAlbums.Any() && !compareResult.MissingTracks.Any())
                return 0;
            
            var selectedAlbums = SelectAlbumsToDownload(compareResult.MissingAlbums);
            var selectedTracks = SelectTracksToDownload(compareResult.MissingTracks);
            if (!selectedAlbums.Any() && !selectedTracks.Any())
                return 0;
            
            using var webDriver = _webDriverFactory.CreateWithIdentity();
            using var client = new HttpClient();
            await AnsiConsole.Status()
                .StartAsync("Downloading...", async _ =>
                {
                    foreach (var album in selectedAlbums)
                        await DownloadAlbum(webDriver, client, album, settings.AudioFormat);
                    
                    foreach (var track in selectedTracks)
                        await DownloadTrack(webDriver, client, track, settings.AudioFormat);
                });
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return 1;
        }
    }

    private async Task DownloadAlbum(IBandcampWebDriver webDriver, HttpClient client, Album album,
        AudioFormat audioFormat)
    {
        await DownloadCollectionItem(webDriver, client, album, album.ToString(), audioFormat,
            stream => _localCollectionService.AddAlbum(stream, album));
    }
    private async Task DownloadTrack(IBandcampWebDriver webDriver, HttpClient client, Track track,
        AudioFormat audioFormat)
    {
        await DownloadCollectionItem(webDriver, client, track, track.ToString(), audioFormat,
            stream => _localCollectionService.AddTrack(stream, track, audioFormat));
    }
    private static async Task DownloadCollectionItem(IBandcampWebDriver webDriver, HttpClient client, CollectionItem item,
        string itemDisplayName, AudioFormat audioFormat, Action<Stream> addToCollectionAction)
    {
        AnsiConsole.MarkupLine($"Preparing [blue]{itemDisplayName.EscapeMarkup()}[/] for download.");
        var link = webDriver.GetDownloadLink(item.RedownloadUrl ?? "", audioFormat);
        if (link == null)
        {
            AnsiConsole.MarkupLine($"Download link for [blue]{itemDisplayName.EscapeMarkup()}[/] expired.");
        }
        else
        {
            AnsiConsole.MarkupLine($"Downloading [blue]{itemDisplayName.EscapeMarkup()}[/].");
            var response = await client.GetAsync(link);
            response.EnsureSuccessStatusCode();
            addToCollectionAction(await response.Content.ReadAsStreamAsync());
        }
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