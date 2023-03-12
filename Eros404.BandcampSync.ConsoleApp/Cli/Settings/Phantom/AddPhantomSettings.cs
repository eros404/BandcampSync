using System.ComponentModel;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings.Phantom;

internal class AddPhantomSettings : PhantomSettings
{
    [CommandOption("-s|--search")]
    [DefaultValue(null)]
    [Description("Search into your Bandcamp collection")]
    public string? Search { get; init; }
}