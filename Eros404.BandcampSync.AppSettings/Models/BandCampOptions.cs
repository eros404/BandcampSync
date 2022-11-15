namespace Eros404.BandcampSync.AppSettings.Models
{
    public class BandcampOptions
    {
        public const string Section = "BandcampCollection";
        public string IdentityCookie { get; set; } = "";
        public string BaseUrl { get; set; } = "";
    }
}
