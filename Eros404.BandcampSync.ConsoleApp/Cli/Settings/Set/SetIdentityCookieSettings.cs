using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set
{
    internal class SetIdentityCookieSettings : SetConfigSettings
    {
        [CommandArgument(0, "<newIdentityCookie>")]
        public string NewIdentityCookie { get; init; } = "";
    }
}
