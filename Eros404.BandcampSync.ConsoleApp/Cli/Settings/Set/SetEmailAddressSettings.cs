using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set;

internal class SetEmailAddressSettings : SetConfigSettings
{
    [CommandArgument(0, "<newEmailAddress>")]
    public string NewEmailAddress { get; init; } = "";
}