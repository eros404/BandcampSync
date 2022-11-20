using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands;

public class AddItemsCommand : AsyncCommand<AddItemsSettings>
{
    private readonly IBandcampApiService _bandCampService;
    private readonly ILocalCollectionService _localCollectionService;
    private readonly IBandcampWebDriverFactory _webDriverFactory;
    private readonly IDownloadService _downloadService;

    public AddItemsCommand(IBandcampApiService bandCampService, ILocalCollectionService localCollectionService, IBandcampWebDriverFactory webDriverFactory, IDownloadService downloadService)
    {
        _bandCampService = bandCampService;
        _localCollectionService = localCollectionService;
        _webDriverFactory = webDriverFactory;
        _downloadService = downloadService;
    }
    
    private int _numberOfItemDownloaded;

    public override async Task<int> ExecuteAsync(CommandContext context, AddItemsSettings settings)
    {
        var fanId = await _bandCampService.GetFanIdAsync();
        if (fanId == null)
            return -1;
        var collection = await _bandCampService.GetCollectionAsync((int)fanId);
        if (collection == null)
            return -1;
        var itemsToDownload = new List<CollectionItem>();
        AnsiConsole.Status()
            .Start("Preparing download...", _ =>
            {
                using var webDriver = _webDriverFactory.CreateWithIdentity();
                for (var i = 0; i < settings.RedownLoadUrls!.Length; i++)
                {
                    var redownloadUrl = settings.RedownLoadUrls[i];
                    if (!long.TryParse(
                            System.Web.HttpUtility.ParseQueryString(new Uri(redownloadUrl).Query).Get("payment_id"),
                            out var paymentId))
                    {
                        AnsiConsole.MarkupLine($"[red][[{i}]] Link not valid.[/]");
                        continue;
                    }

                    var collectionItem = collection.GetAllItems().FirstOrDefault(item => item.GetPaymentId() == paymentId);
                    if (collectionItem == null)
                    {
                        AnsiConsole.MarkupLine($"[red][[{i}]] No item with this reference in your Bandcamp collection.[/]");
                        continue;
                    }
                    
                    var result = webDriver.GetDownloadLink(redownloadUrl, settings.AudioFormat, "");
                    if (result.HasExpired)
                    {
                        AnsiConsole.MarkupLine($"Download link for [blue]{collectionItem.ToString().EscapeMarkup()}[/] expired.");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[blue]{collectionItem.ToString().EscapeMarkup()}[/] ready for download.");
                        collectionItem.DownloadLink = result.DownloadLink;
                        itemsToDownload.Add(collectionItem);
                    }
                }
            });
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
                var albumsToDownload = new List<Album>();
                var tracksToDownload = new List<Track>();
                itemsToDownload.ForEach(item =>
                {
                    switch (item)
                    {
                        case Album album:
                            albumsToDownload.Add(album);
                            break;
                        case Track track:
                            tracksToDownload.Add(track);
                            break;
                    }
                });
                await _downloadService.DownloadMissingAlbums(albumsToDownload);
                await _downloadService.DownloadMissingTracks(tracksToDownload);
            });
        AnsiConsole.MarkupLine(
            $"[green]{_numberOfItemDownloaded}[/] item{(_numberOfItemDownloaded > 1 ? "s" : "")} downloaded.");
        return 0;
    }
}