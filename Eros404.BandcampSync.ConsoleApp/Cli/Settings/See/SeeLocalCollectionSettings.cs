using Spectre.Console.Cli;
using System.ComponentModel;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings.See
{
    internal class SeeLocalCollectionSettings : SeeCollectionSettings
    {
        [CommandOption("-a|--as-albums")]
        [DefaultValue(false)]
        public bool AsAlbums { get; init; }
    }
}
