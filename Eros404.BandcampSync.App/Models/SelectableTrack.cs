using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.App.Models;

public class SelectableTrack : Track
{
    public SelectableTrack(Track track)
    {
        Title = track.Title;
        BandName = track.BandName;
        AlbumTitle = track.AlbumTitle;
        Number = track.Number;
        RedownloadUrl = track.RedownloadUrl;
    }
    public bool IsChecked { get; set; }
}