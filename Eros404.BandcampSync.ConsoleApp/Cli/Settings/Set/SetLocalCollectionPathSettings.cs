using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set
{
    internal class SetLocalCollectionPathSettings : SetConfigSettings
    {
        [CommandArgument(0, "<newPath>")]
        public string NewPath { get; init; } = "";
    }
}
