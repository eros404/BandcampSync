using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.Core.Services;

public interface IDownloadService
{
    event EventHandler<DownloadStartedEventArgs> DownloadStarted;
    event EventHandler<DownloadFinishedEventArgs> DownloadFinished;
    Task Download(IReadOnlyCollection<Album> albums);
    Task Download(IReadOnlyCollection<Track> tracks);
}