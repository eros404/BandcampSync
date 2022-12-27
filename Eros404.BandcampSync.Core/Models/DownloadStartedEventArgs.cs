namespace Eros404.BandcampSync.Core.Models;

public class DownloadStartedEventArgs : EventArgs
{
    public DownloadStartedEventArgs(CollectionItem item)
    {
        Item = item;
    }

    public CollectionItem Item { get; set; }
}