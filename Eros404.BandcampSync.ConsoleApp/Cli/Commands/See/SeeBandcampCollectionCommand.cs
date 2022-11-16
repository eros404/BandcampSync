using Eros404.BandcampSync.ConsoleApp.Cli.Settings.See;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.See
{
    internal class SeeBandcampCollectionCommand : AsyncCommand<SeeBandcampCollectionSettings>
    {
        private readonly IBandcampApiService _bandCampService;
        private readonly ILogger _logger;

        public SeeBandcampCollectionCommand(IBandcampApiService bandCampService, ILogger logger)
        {
            _bandCampService = bandCampService;
            _logger = logger;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, SeeBandcampCollectionSettings settings)
        {
            try
            {
                var fanId = await _bandCampService.GetFanIdAsync();
                if (fanId == null)
                    return 1;
                var collection = await _bandCampService.GetCollectionAsync((int)fanId);
                if (collection == null)
                    return 1;

                AnsiConsole.Write(collection.ToTable("Bandcamp Collection"));
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
