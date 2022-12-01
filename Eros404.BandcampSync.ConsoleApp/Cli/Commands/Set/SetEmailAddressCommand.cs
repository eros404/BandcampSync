using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.Set;

internal class SetEmailAddressCommand : Command<SetEmailAddressSettings>
{
    private readonly IUserSettingsService _userSettingsService;

    public SetEmailAddressCommand(IUserSettingsService userSettingsService)
    {
        _userSettingsService = userSettingsService;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] SetEmailAddressSettings settings)
    {
        _userSettingsService.UpdateValue(UserSettings.EmailAddress, settings.NewEmailAddress);
        AnsiConsole.MarkupLine("[green]Done[/]");
        return 0;
    }
}