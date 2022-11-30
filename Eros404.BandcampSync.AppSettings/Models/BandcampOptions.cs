namespace Eros404.BandcampSync.AppSettings.Models
{
    public class BandcampOptions
    {
        public const string Section = "BandcampCollection";
        public string BaseUrl { get; set; } = "";
        public int GetItemsCount { get; set; }
    }
}
