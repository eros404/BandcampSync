using System.ComponentModel;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set;

internal class SetEmailAddressSettings : SetConfigSettings
{
    [CommandArgument(0, "<newEmailAddress>")]
    [Description("The email address linked with your Bandcamp account.")]
    public string NewEmailAddress { get; init; } = "";
}