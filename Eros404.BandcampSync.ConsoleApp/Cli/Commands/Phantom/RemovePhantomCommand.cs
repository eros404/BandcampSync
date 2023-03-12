using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Phantom;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.Phantom;

internal class RemovePhantomCommand : Command<RemovePhantomSettings>
{
    private readonly ILogger _logger;
    private readonly IPhantomService _phantomService;

    public RemovePhantomCommand(IPhantomService phantomService, ILogger logger)
    {
        _phantomService = phantomService;
        _logger = logger;
    }

    [SuppressMessage("ReSharper", "RedundantNullableFlowAttribute")]
    public override int Execute([NotNull] CommandContext context, [NotNull] RemovePhantomSettings settings)
    {
        var phantoms = _phantomService.GetPhantoms();
        if (!phantoms.Albums.Any() && !phantoms.Tracks.Any())
        {
            _logger.LogWarning("You have no missing item in your local collection.");
            return 0;
        }

        var phantomItems = new List<CollectionItem>(phantoms.Albums);
        phantomItems.AddRange(phantoms.Tracks);

        _phantomService.RemovePhantoms(MyConsole.SelectItems(phantomItems, "Select phantoms to remove", true)
            .ToArray());
        AnsiConsole.MarkupLine("[green]Done[/]");
        return 0;
    }
}