using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.Set
{
    internal class SetIdentityCookieCommand : Command<SetIdentityCookieSettings>
    {
        private readonly IWritableOptions<BandcampOptions> _bandcampOptions;

        public SetIdentityCookieCommand(IWritableOptions<BandcampOptions> bandcampOptions)
        {
            _bandcampOptions = bandcampOptions;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] SetIdentityCookieSettings settings)
        {
            _bandcampOptions.Update(options => options.IdentityCookie = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the cookie's value:").Secret()));
            AnsiConsole.MarkupLine("[green]Done[/]");
            return 0;
        }
    }
}
