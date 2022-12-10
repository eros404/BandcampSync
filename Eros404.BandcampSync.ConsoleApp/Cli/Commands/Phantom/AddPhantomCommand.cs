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
            selectedAlbums = SelectAlbumsToPhantomize(missingItems.MissingAlbums);
        }
        var selectedTracks = new List<MissingTrack>();
        if (missingItems.MissingTracks.Any())
        {
            selectedTracks = SelectTracksToPhantomize(missingItems.MissingTracks);
        }

        var selectedItems = new List<CollectionItem>(selectedAlbums);
        selectedItems.AddRange(selectedTracks);
        _phantomService.AddPhantoms(selectedItems);
        AnsiConsole.MarkupLine("[green]Done[/]");
        return 0;
    }
    
    private static List<MissingAlbum> SelectAlbumsToPhantomize(List<MissingAlbum> albums)
    {
        if (!albums.Any())
            return new List<MissingAlbum>();
        var dictionary = albums.ToDictionary(a => a.ToString().EscapeMarkup(), a => a);
        var selectedKeys = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[blue]Select albums to phantomize:[/]")
                .NotRequired()
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more albums)[/]")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle an album, [green]<enter>[/] to accept)[/]")
                .AddChoices(dictionary.Keys));
        return selectedKeys.Select(key => dictionary[key]).ToList();
    }
    private static List<MissingTrack> SelectTracksToPhantomize(List<MissingTrack> tracks)
    {
        if (!tracks.Any())
            return new List<MissingTrack>();
        var dictionary = tracks.ToDictionary(a => a.ToString().EscapeMarkup(), a => a);
        var selectedKeys = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[blue]Select tracks to phantomize:[/]")
                .NotRequired()
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more tracks)[/]")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle a track, [green]<enter>[/] to accept)[/]")
                .AddChoices(dictionary.Keys));
        return selectedKeys.Select(key => dictionary[key]).ToList();
    }
}