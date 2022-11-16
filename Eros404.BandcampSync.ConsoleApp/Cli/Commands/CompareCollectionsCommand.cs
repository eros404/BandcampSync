using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.Core.Models;

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
                var compareResult = await CompareAsync(_bandCampService, _localCollectionService);
                if (compareResult == null)
                    return 1;
                AnsiConsole.Write(compareResult.ToTable("Missing Items"));
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return 1;
            }
        }

        public static async Task<CollectionCompareResult?> CompareAsync(IBandcampApiService bandCampService,
            ILocalCollectionService localCollectionService)
        {
            var fanId = await bandCampService.GetFanIdAsync();
            if (fanId == null)
                return null;
            var bandcamp = await bandCampService.GetCollectionAsync((int)fanId);
            if (bandcamp == null)
                return null;
            var local = localCollectionService.GetLocalCollection(false);
            return bandcamp.Compare(local.Tracks);
        }
    }
}
