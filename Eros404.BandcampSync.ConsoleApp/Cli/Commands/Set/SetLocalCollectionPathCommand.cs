using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.Set
{
    internal class SetLocalCollectionPathCommand : Command<SetLocalCollectionPathSettings>
    {
        private readonly IWritableOptions<LocalCollectionOptions> _localCollectionOptions;
        private readonly ILogger _logger;

        public SetLocalCollectionPathCommand(IWritableOptions<LocalCollectionOptions> localCollectionOptions, ILogger logger)
        {
            _localCollectionOptions = localCollectionOptions;
            _logger = logger;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] SetLocalCollectionPathSettings settings)
        {
            try
            {
                var newPath = settings.NewPath;
                if (!Directory.Exists(newPath))
                    _logger.LogWarning("This directory does not exists on yout computer.");
                _localCollectionOptions.Update(options => options.Path = newPath);
                AnsiConsole.MarkupLine("[green]Done[/]");
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
