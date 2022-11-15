﻿using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands
{
    internal class LoginCommand : Command<LoginSettings>
    {
        private readonly ILogger _logger;
        private readonly IBandcampWebDriverFactory _webDriverFactory;

        public LoginCommand(ILogger logger, IBandcampWebDriverFactory webDriverFactory)
        {
            _logger = logger;
            _webDriverFactory = webDriverFactory;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] LoginSettings settings)
        {
            try
            {
                using var webDriver = _webDriverFactory.Create();
                if (!webDriver.Login(settings.UserName, AnsiConsole.Prompt(
                    new TextPrompt<string>("Bandcamp password:").Secret())))
                {
                    AnsiConsole.MarkupLine("[red]Connection failed. Try with other login details.[/]");
                    return 1;
                }
                AnsiConsole.MarkupLine("[green]You are logged.[/]");
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