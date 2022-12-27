using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.See;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.See;

internal class SeePhantomCollectionCommand : Command<SeePhantomCollectionSettings>
{
    private readonly IPhantomService _phantomService;

    public SeePhantomCollectionCommand(IPhantomService phantomService)
    {
        _phantomService = phantomService;
    }

    [SuppressMessage("ReSharper", "RedundantNullableFlowAttribute")]
    public override int Execute([NotNull] CommandContext context, [NotNull] SeePhantomCollectionSettings settings)
    {
        var phantoms = _phantomService.GetPhantoms();
        AnsiConsole.Write(phantoms.ToTable("Phantoms"));
        return 0;
    }
}