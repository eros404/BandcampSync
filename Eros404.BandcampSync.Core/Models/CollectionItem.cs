using System.Web;

namespace Eros404.BandcampSync.Core.Models
{
    public abstract class CollectionItem
    {
        public string? Title { get; set; }
        public string? BandName { get; set; }
        public string? RedownloadUrl { get; set; }

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
}
