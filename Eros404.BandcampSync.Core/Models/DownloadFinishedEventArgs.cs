namespace Eros404.BandcampSync.Core.Models;

public class DownloadFinishedEventArgs
{
    public CollectionItem Item { get; set; }
    public Stream Stream { get; set; }
    public DownloadFinishedEventArgs(CollectionItem item, Stream stream)
    {
        Item = item;
        Stream = stream;
    }
}