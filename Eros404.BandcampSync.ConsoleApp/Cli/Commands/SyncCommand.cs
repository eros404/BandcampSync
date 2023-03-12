using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands;

internal class SyncCommand : AsyncCommand<SyncSettings>
{
    private readonly IComparatorService _comparatorService;
    private readonly IDownloadService _downloadService;
    private readonly ILocalCollectionService _localCollectionService;
    private readonly IMailService _mailService;
    private readonly IBandcampWebDriverFactory _webDriverFactory;

    private int _numberOfItemDownloaded;
    private int _numberOfNewLinkSent;

    public SyncCommand(ILocalCollectionService localCollectionService, IBandcampWebDriverFactory webDriverFactory,
        IMailService mailService, IDownloadService downloadService, IComparatorService comparatorService)
    {
        _localCollectionService = localCollectionService;
        _webDriverFactory = webDriverFactory;
        _mailService = mailService;
        _downloadService = downloadService;
        _comparatorService = comparatorService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, SyncSettings settings)
    {
        CollectionCompareResult? compareResult = null;
        await AnsiConsole.Status()
            .StartAsync("Fetching collection...", async _ =>
            {
                compareResult = await _comparatorService.CompareLocalWithBandcamp(settings.Search);
            });
        if (compareResult == null)
            return -1;
        AnsiConsole.Write(compareResult.ToTable("Missing Items"));
        if (!compareResult.MissingAlbums.Any() && !compareResult.MissingTracks.Any())
            return 0;

        var selectedAlbums = MyConsole.SelectItems(compareResult.MissingAlbums, "Select albums to download", true);
        var selectedTracks = MyConsole.SelectItems(compareResult.MissingTracks, "Select tracks to download", true);
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
            AnsiConsole.MarkupLine(
                $"[green]{_numberOfNewLinkSent}[/] link{(_numberOfNewLinkSent > 1 ? "s" : "")} have been sent to {_mailService.EmailAddress}.");

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
                await _downloadService.Download(selectedAlbums
                    .Where(a => !string.IsNullOrEmpty(a.DownloadLink)).ToList());
                await _downloadService.Download(selectedTracks
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
}