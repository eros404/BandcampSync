using System.Text.Encodings.Web;
using Eros404.BandcampSync.BandcampWebsite.Pages;
using Eros404.BandcampSync.Core.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Eros404.BandcampSync.BandcampWebsite.Services
{
    public sealed class BandcampWebDriver : IBandcampWebDriver
    {
        private readonly string _baseAddress;
        private readonly IWebDriver _driver;
        public BandcampWebDriver(string baseAddress)
        {
            _baseAddress = baseAddress;
            _driver = BuildDriver(baseAddress);

            static IWebDriver BuildDriver(string baseAddress)
            {
                var options = new ChromeOptions();
#if !DEBUG
                options.AddArguments("headless");
#endif
                options.AddArguments("window-size=1920,1080");
                return new ChromeDriver(options)
                {
                    Url = baseAddress
                };
            }
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _driver.Dispose();
            }
            _disposed = true;
        }

        public bool Login(string userName, string password)
        {
            return new LoginPage(_driver, _baseAddress).Load().Login(userName, password).Loaded;
        }

        public void OpenDownloadPage(string url)
        {
            new DownloadPage(_driver, _baseAddress, new Uri(url).Query).Load();
        }

        public IBandcampWebDriver SetIdentityCookie(string value)
        {
            _driver.Manage().Cookies.AddCookie(new Cookie("identity", UrlEncoder.Create().Encode(value),
                new Uri(_baseAddress).Host, "/", null));
            return this;
        }
    }
}
