using Spectre.Console.Cli;
using System.ComponentModel;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings.See
{
    internal class SeeLocalCollectionSettings : SeeCollectionSettings
    {
        [CommandOption("-a|--as-albums")]
        [DefaultValue(false)]
        [Description("Display your local collection as a list of albums (default is a list of tracks).")]
        public bool AsAlbums { get; init; }
    }
}
