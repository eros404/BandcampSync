using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.App.Models;

public class SelectableAlbum : Album
{
    public SelectableAlbum(Album album)
    {
        Title = album.Title;
        BandName = album.BandName;
        NumberOfTracks = album.NumberOfTracks;
        RedownloadUrl = album.RedownloadUrl;
    }
    public bool IsChecked { get; set; }
}