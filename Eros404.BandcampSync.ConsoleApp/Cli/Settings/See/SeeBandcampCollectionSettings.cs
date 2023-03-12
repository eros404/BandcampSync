using System.ComponentModel;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings.See;

internal class SeeBandcampCollectionSettings : SeeCollectionSettings
{
    [CommandOption("-s|--search")]
    [DefaultValue(null)]
    [Description("Search into your Bandcamp collection")]
    public string? Search { get; init; }
}