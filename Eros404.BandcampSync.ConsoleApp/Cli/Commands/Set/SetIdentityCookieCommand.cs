using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.Set
{
    internal class SetIdentityCookieCommand : Command<SetIdentityCookieSettings>
    {
        private readonly IUserSettingsService _userSettingsService;

        public SetIdentityCookieCommand(IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] SetIdentityCookieSettings settings)
        {
            _userSettingsService.UpdateValue(UserSettings.BandcampIdentityCookie, AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the cookie's value:").Secret()));
            AnsiConsole.MarkupLine("[green]Done[/]");
            return 0;
        }
    }
}
