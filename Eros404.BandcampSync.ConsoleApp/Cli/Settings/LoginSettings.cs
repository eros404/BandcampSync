using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings;

internal class LoginSettings : CommandSettings
{
    [CommandArgument(0, "<userName>")] public string UserName { get; init; } = "";
}