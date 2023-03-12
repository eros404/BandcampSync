using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands;

internal class CompareCollectionsCommand : AsyncCommand<CompareCollectionsSettings>
{
    private readonly IComparatorService _comparatorService;

    public CompareCollectionsCommand(IComparatorService comparatorService)
    {
        _comparatorService = comparatorService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CompareCollectionsSettings settings)
    {
        var compareResult = await _comparatorService.CompareLocalWithBandcamp(settings.Search);
        if (compareResult == null)
            return -1;
        AnsiConsole.Write(compareResult.ToTable("Missing Items"));
        return 0;
    }
}