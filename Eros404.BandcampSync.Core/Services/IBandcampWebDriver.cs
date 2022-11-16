using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.Core.Services
{
    public interface IBandcampWebDriver : IDisposable
    {
        bool Login(string userName, string password);
        DownloadLinkResult GetDownloadLink(string url, AudioFormat format, string email);
    }
}