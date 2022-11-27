using Eros404.BandcampSync.Core.Models;
using Spectre.Console;

namespace Eros404.BandcampSync.ConsoleApp.Extensions
{
    public static class TrackExtensions
    {
        public static Table ToTable(this IEnumerable<Track> tracks, string title = "Tracks")
        {
            var trackTable = new Table()
            {
                Title = new TableTitle(title, new Style(Color.Blue))
            };
            trackTable.Expand();
            trackTable.AddColumn("[green]Number[/]");
            trackTable.AddColumn("[green]Title[/]");
            trackTable.AddColumn("[green]Album[/]");
            trackTable.AddColumn("[green]Band[/]");
            tracks.ToList().ForEach(track =>
                trackTable.AddRow(track.Number.ToString(), track.Title?.EscapeMarkup() ?? "",
                    track.AlbumTitle?.EscapeMarkup() ?? "", track.BandName?.EscapeMarkup() ?? ""));
            return trackTable;
        }
    }
}
