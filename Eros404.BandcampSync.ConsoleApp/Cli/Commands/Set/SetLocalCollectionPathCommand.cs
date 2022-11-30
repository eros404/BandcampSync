using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.Set
{
    internal class SetLocalCollectionPathCommand : Command<SetLocalCollectionPathSettings>
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly ILogger _logger;

        public SetLocalCollectionPathCommand(IUserSettingsService userSettingsService, ILogger logger)
        {
            _userSettingsService = userSettingsService;
            _logger = logger;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] SetLocalCollectionPathSettings settings)
        {
            var newPath = settings.NewPath;
            if (!Directory.Exists(newPath))
                _logger.LogWarning("This directory does not exists on yout computer.");
            _userSettingsService.UpdateValue(UserSettings.LocalCollectionBasePath, newPath);
            AnsiConsole.MarkupLine("[green]Done[/]");
            return 0;
        }
    }
}
