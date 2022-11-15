using Eros404.BandcampSync.Core.Models;
using Spectre.Console;

namespace Eros404.BandcampSync.ConsoleApp.Extensions
{
    public static class MissingAlbumExtensions
    {
        public static Table ToTable(this IEnumerable<MissingAlbum> albums, string title = "Missing Albums")
        {
            var albumTable = new Table()
            {
                Title = new TableTitle(title, new Style(Color.Blue))
            };
            albumTable.Expand();
            albumTable.AddColumn("[green]Title[/]");
            albumTable.AddColumn("[green]Band[/]");
            albumTable.AddColumn("[green]Number of Missing Tracks[/]");
            albums.ToList().ForEach(album =>
                albumTable.AddRow(new[]
                {
                    album.Title?.EscapeMarkup() ?? "",
                    album.BandName?.EscapeMarkup() ?? "",
                    album.NumberOfMissingTracks.ToString()
                }));
            return albumTable;
        }
    }
}
