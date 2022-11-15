using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands;

public class SyncCommand : AsyncCommand<SyncSettings>
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
            var fanId = await _bandCampService.GetFanIdAsync();
            if (fanId == null)
                return 1;
            var bandcamp = await _bandCampService.GetCollectionAsync((int)fanId);
            if (bandcamp == null)
                return 1;
            var local = _localCollectionService.GetLocalCollection(false);
            var compareResult = bandcamp.Compare(local.Tracks);
            AnsiConsole.Write(compareResult.ToTable("Missing Items"));
            using var webDriver = _webDriverFactory.CreateWithIdentity();
            webDriver.OpenDownloadPage(compareResult.MissingAlbums[0].RedownloadUrl);
            AnsiConsole.Confirm("Oui?");
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return 1;
        }
    }
}