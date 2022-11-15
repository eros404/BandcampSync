using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands
{
    internal class CompareCollectionsCommand : AsyncCommand<CompareCollectionsSettings>
    {
        private readonly ILogger _logger;
        private readonly IBandcampApiService _bandCampService;
        private readonly ILocalCollectionService _localCollectionService;

        public CompareCollectionsCommand(ILogger logger, IBandcampApiService bandCampService, ILocalCollectionService localCollectionService)
        {
            _logger = logger;
            _bandCampService = bandCampService;
            _localCollectionService = localCollectionService;
        }

        public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] CompareCollectionsSettings settings)
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
                AnsiConsole.Write(bandcamp.Compare(local.Tracks).ToTable("Missing Items"));
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return 1;
            }
        }
    }
}
