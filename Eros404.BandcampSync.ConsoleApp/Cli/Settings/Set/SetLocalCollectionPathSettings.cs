using System.ComponentModel;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set
{
    internal class SetLocalCollectionPathSettings : SetConfigSettings
    {
        [CommandArgument(0, "<newPath>")]
        [Description("The directory's path containing your collection.")]
        public string NewPath { get; init; } = "";
    }
}
