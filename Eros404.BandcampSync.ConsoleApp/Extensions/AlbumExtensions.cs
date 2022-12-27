using Eros404.BandcampSync.Core.Models;
using Spectre.Console;

namespace Eros404.BandcampSync.ConsoleApp.Extensions;

public static class AlbumExtensions
{
    public static Table ToTable(this IEnumerable<Album> albums, string title = "Albums")
    {
        var albumTable = new Table
        {
            Title = new TableTitle(title, new Style(Color.Blue))
        };
        albumTable.Expand();
        albumTable.AddColumn("[green]Title[/]");
        albumTable.AddColumn("[green]Band[/]");
        albumTable.AddColumn("[green]Number of Tracks[/]");
        albums.ToList().ForEach(album =>
            albumTable.AddRow(album.Title?.EscapeMarkup() ?? "", album.BandName?.EscapeMarkup() ?? "",
                album.NumberOfTracks.ToString()));
        return albumTable;
    }
}