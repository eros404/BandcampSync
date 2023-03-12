using Eros404.BandcampSync.ConsoleApp.Cli.Settings.See;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.See;

internal class SeeBandcampCollectionCommand : AsyncCommand<SeeBandcampCollectionSettings>
{
    private readonly IBandcampApiService _bandCampService;

    public SeeBandcampCollectionCommand(IBandcampApiService bandCampService)
    {
        _bandCampService = bandCampService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, SeeBandcampCollectionSettings settings)
    {
        var collection = await _bandCampService.GetCollectionAsync(settings.Search);
        if (collection == null)
            return -1;

        AnsiConsole.Write(collection.ToTable("Bandcamp Collection"));
        return 0;
    }
}