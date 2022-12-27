using System.Web;

namespace Eros404.BandcampSync.Core.Models;

public abstract class CollectionItem
{
    public string? Title { get; init; }
    public string? BandName { get; init; }
    public string? RedownloadUrl { get; init; }
    public string? DownloadLink { get; set; }

    public override string ToString()
    {
        return this switch
        {
            Album album => album.ToString(),
            Track track => track.ToString(),
            _ => base.ToString() ?? ""
        };
    }

    public long? GetPaymentId()
    {
        if (RedownloadUrl == null)
            return null;
        return long.TryParse(
            HttpUtility.ParseQueryString(new Uri(RedownloadUrl).Query).Get("payment_id"), out var paymentId)
            ? paymentId
            : null;
    }
}