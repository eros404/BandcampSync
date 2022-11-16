using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.Set;

internal class SetEmailAddressCommand : Command<SetEmailAddressSettings>
{
    private readonly ILogger _logger;
    private readonly IWritableOptions<EmailOptions> _emailOptions;

    public SetEmailAddressCommand(ILogger logger, IWritableOptions<EmailOptions> emailOptions)
    {
        _logger = logger;
        _emailOptions = emailOptions;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] SetEmailAddressSettings settings)
    {
        _emailOptions.Update(options => options.Address = settings.NewEmailAddress);
        AnsiConsole.MarkupLine("[green]Done[/]");
        return 0;
    }
}