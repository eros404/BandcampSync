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
            if (!selectedAlbums.Any())
                return 0;
            
            using var webDriver = _webDriverFactory.CreateWithIdentity();
            await AnsiConsole.Progress()
                .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(),
                    new RemainingTimeColumn(), new SpinnerColumn())
                .StartAsync(async ctx =>
                {
                    var task = ctx.AddTask($"Downloding {selectedAlbums.Count} albums");
                    foreach (var album in selectedAlbums)
                    {
                        var stream = await webDriver.DownloadItemAsync(album.RedownloadUrl ?? "", settings.AudioFormat);
                        if (stream != null)
                            _localCollectionService.AddAlbum(stream, album);
                        task.Increment(100 / selectedAlbums.Count);
                    }
                });
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return 1;
        }
    }

    private static List<MissingAlbum> SelectAlbumsToDownload(List<MissingAlbum> albums)
    {
        if (!albums.Any())
            return new List<MissingAlbum>();
        var dictionary = albums.ToDictionary(a => $"{a.Title} - {a.BandName}", a => a);
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
}