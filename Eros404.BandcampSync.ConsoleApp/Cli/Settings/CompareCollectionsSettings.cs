using System.ComponentModel;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings;

internal class CompareCollectionsSettings : CommandSettings
{
    [CommandOption("-s|--search")]
    [DefaultValue(null)]
    [Description("Search into your Bandcamp collection")]
    public string? Search { get; init; }
}