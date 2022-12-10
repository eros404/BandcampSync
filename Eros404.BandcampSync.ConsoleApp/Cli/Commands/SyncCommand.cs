using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands;

internal class SyncCommand : AsyncCommand<SyncSettings>
{
    private readonly ILocalCollectionService _localCollectionService;
    private readonly IBandcampWebDriverFactory _webDriverFactory;
    private readonly IMailService _mailService;
    private readonly IDownloadService _downloadService;
    private readonly IComparatorService _comparatorService;

    public SyncCommand(ILocalCollectionService localCollectionService, IBandcampWebDriverFactory webDriverFactory, IMailService mailService, IDownloadService downloadService, IComparatorService comparatorService)
    {
        _localCollectionService = localCollectionService;
        _webDriverFactory = webDriverFactory;
        _mailService = mailService;
        _downloadService = downloadService;
        _comparatorService = comparatorService;
    }

    private int _numberOfItemDownloaded;
    private int _numberOfNewLinkSent;

    public override async Task<int> ExecuteAsync(CommandContext context, SyncSettings settings)
    {
        var compareResult = await _comparatorService.CompareLocalWithBandcamp();
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

#if DEBUG
        _downloadService.DownloadStarted += (_, args) =>
            AnsiConsole.MarkupLine($"Start [blue]{args.Item.ToString().EscapeMarkup()}[/] download.");
#endif
        _downloadService.DownloadFinished += (_, args) =>
        {
            _localCollectionService.AddItem(args.Stream, args.Item, settings.AudioFormat);
            AnsiConsole.MarkupLine($"[blue]{args.Item.ToString().EscapeMarkup()}[/] downloaded.");
            _numberOfItemDownloaded++;
        };
        await AnsiConsole.Status()
            .StartAsync("Downloading...", async _ =>
            {
                await _downloadService.DownloadMissingAlbums(selectedAlbums
                    .Where(a => !string.IsNullOrEmpty(a.DownloadLink)).ToList());
                await _downloadService.DownloadMissingTracks(selectedTracks
                    .Where(a => !string.IsNullOrEmpty(a.DownloadLink)).ToList());
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

    private static List<MissingAlbum> SelectAlbumsToDownload(List<MissingAlbum> albums)
    {
        if (!albums.Any())
            return new List<MissingAlbum>();
        var dictionary = albums.ToDictionary(a => a.ToString().EscapeMarkup(), a => a);
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
        var dictionary = tracks.ToDictionary(track => track.ToString().EscapeMarkup(), a => a);
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