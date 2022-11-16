using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands;

public class AddItemsCommand : AsyncCommand<AddItemsSettings>
{
    private readonly ILogger _logger;
    private readonly IBandcampApiService _bandCampService;
    private readonly ILocalCollectionService _localCollectionService;
    private readonly IBandcampWebDriverFactory _webDriverFactory;

    public AddItemsCommand(ILogger logger, IBandcampApiService bandCampService, ILocalCollectionService localCollectionService, IBandcampWebDriverFactory webDriverFactory)
    {
        _logger = logger;
        _bandCampService = bandCampService;
        _localCollectionService = localCollectionService;
        _webDriverFactory = webDriverFactory;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, AddItemsSettings settings)
    {
        var fanId = await _bandCampService.GetFanIdAsync();
        if (fanId == null)
            return -1;
        var collection = await _bandCampService.GetCollectionAsync((int)fanId);
        if (collection == null)
            return -1;
        using var webDriver = _webDriverFactory.CreateWithIdentity();
        using var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
        return await AnsiConsole.Status()
            .StartAsync("Downloading...", async _ =>
            {
                for (var i = 0; i < settings.RedownLoadUrls!.Length; i++)
                {
                    var redownloadUrl = settings.RedownLoadUrls[i];
                    if (!long.TryParse(
                            System.Web.HttpUtility.ParseQueryString(new Uri(redownloadUrl).Query).Get("payment_id"),
                            out var paymentId))
                    {
                        AnsiConsole.MarkupLine($"[red][[{i}]] Link not valid.[/]");
                        return -1;
                    }

                    var collectionItem = collection.GetAllItems().FirstOrDefault(i => i.GetPaymentId() == paymentId);
                    if (collectionItem == null)
                    {
                        AnsiConsole.MarkupLine($"[red][[{i}]] No item with this reference in your Bandcamp collection.[/]");
                        return -1;
                    }

                    switch (collectionItem)
                    {
                        case Album album:
                            await DownloadAlbum(webDriver, client, redownloadUrl, album, settings.AudioFormat);
                            break;
                        case Track track:
                            await DownloadTrack(webDriver, client, redownloadUrl, track, settings.AudioFormat);
                            break;
                    }
                }
                AnsiConsole.MarkupLine($"[green]Done[/]");
                return 0;
            });
    }
    
    private async Task DownloadAlbum(IBandcampWebDriver webDriver, HttpClient client, string redownloadUrl,
        Album album, AudioFormat audioFormat)
    {
        await DownloadCollectionItem(webDriver, client, redownloadUrl, album.ToString(), audioFormat,
            stream => _localCollectionService.AddAlbum(stream, album));
    }
    private async Task DownloadTrack(IBandcampWebDriver webDriver, HttpClient client, string redownloadUrl,
        Track track, AudioFormat audioFormat)
    {
        await DownloadCollectionItem(webDriver, client, redownloadUrl, track.ToString(), audioFormat,
            stream => _localCollectionService.AddTrack(stream, track, audioFormat));
    }
    
    private static async Task DownloadCollectionItem(IBandcampWebDriver webDriver, HttpClient client, string redownloadUrl,
        string itemDisplayName, AudioFormat audioFormat, Action<Stream> addToCollectionAction)
    {
        AnsiConsole.MarkupLine($"Preparing [blue]{itemDisplayName.EscapeMarkup()}[/] for download.");
        var result = webDriver.GetDownloadLink(redownloadUrl, audioFormat, "");
        if (result.HasExpired)
        {
            AnsiConsole.MarkupLine($"Download link for [blue]{itemDisplayName.EscapeMarkup()}[/] expired.");
        }
        else
        {
            AnsiConsole.MarkupLine($"Downloading [blue]{itemDisplayName.EscapeMarkup()}[/].");
            var response = await client.GetAsync(result.DownloadLink);
            response.EnsureSuccessStatusCode();
            addToCollectionAction(await response.Content.ReadAsStreamAsync());
        }
    }
}