using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands
{
    internal class LoginCommand : Command<LoginSettings>
    {
        private readonly IBandcampWebDriverFactory _webDriverFactory;

        public LoginCommand(ILogger logger, IBandcampWebDriverFactory webDriverFactory)
        {
            _webDriverFactory = webDriverFactory;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] LoginSettings settings)
        {
            using var webDriver = _webDriverFactory.Create();
            if (!webDriver.Login(settings.UserName, AnsiConsole.Prompt(
                new TextPrompt<string>("Bandcamp password:").Secret())))
            {
                AnsiConsole.MarkupLine("[red]Connection failed. Try with other login details.[/]");
                return -1;
            }
            AnsiConsole.MarkupLine("[green]You are logged.[/]");
            return 0;
        }
    }
}
