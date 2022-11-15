namespace Eros404.BandcampSync.Core.Services
{
    public interface IBandcampWebDriver : IDisposable
    {
        bool Login(string userName, string password);
    }
}