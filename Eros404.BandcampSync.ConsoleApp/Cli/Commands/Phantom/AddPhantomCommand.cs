using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Phantom;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.Phantom;

public class AddPhantomCommand : AsyncCommand<AddPhantomSettings>
{
    private readonly IPhantomService _phantomService;
    private readonly IComparatorService _comparatorService;
    private readonly ILogger _logger;

    public AddPhantomCommand(IPhantomService phantomService, IComparatorService comparatorService, ILogger logger)
    {
        _phantomService = phantomService;
        _comparatorService = comparatorService;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, AddPhantomSettings settings)
    {
        var missingItems = await _comparatorService.CompareLocalWithBandcamp();
        if (missingItems == null)
            return -1;
        if (!missingItems.MissingAlbums.Any() && !missingItems.MissingTracks.Any())
        {
            _logger.LogWarning("You have no missing item in your local collection.");
            return 0;
        }

        var selectedAlbums = new List<MissingAlbum>();
        if (missingItems.MissingAlbums.Any())
        {
            selectedAlbums = MyConsole.SelectItems(missingItems.MissingAlbums, "Select albums to phantomize", false);
        }
        var selectedTracks = new List<MissingTrack>();
        if (missingItems.MissingTracks.Any())
        {
            selectedTracks = MyConsole.SelectItems(missingItems.MissingTracks, "Select tracks to phantomize", false);
        }

        var selectedItems = new List<CollectionItem>(selectedAlbums);
        selectedItems.AddRange(selectedTracks);
        _phantomService.AddPhantoms(selectedItems.ToArray());
        AnsiConsole.MarkupLine("[green]Done[/]");
        return 0;
    }
}