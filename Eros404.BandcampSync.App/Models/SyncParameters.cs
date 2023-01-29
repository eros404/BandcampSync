using System.Collections.Generic;
using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.App.Models;

public class SyncParameters
{
    public SyncParameters(AudioFormat audioFormat, List<Album> albumsToDownload, List<Track> tracksToDownload)
    {
        AudioFormat = audioFormat;
        AlbumsToDownload = albumsToDownload;
        TracksToDownload = tracksToDownload;
    }

    public AudioFormat AudioFormat { get; set; }
    public List<Album> AlbumsToDownload { get; set; }
    public List<Track> TracksToDownload { get; set; }
}