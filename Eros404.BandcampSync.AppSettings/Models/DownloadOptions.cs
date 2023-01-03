namespace Eros404.BandcampSync.AppSettings.Models;

public class DownloadOptions
{
    public const string Section = "Download";
    public int AlbumBatchSize { get; set; }
    public int TrackBatchSize { get; set; }
    public int Timeout { get; set; }
}