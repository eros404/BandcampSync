using Eros404.BandcampSync.BandcampWebsite.Pages;
using Eros404.BandcampSync.Core.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Eros404.BandcampSync.BandcampWebsite.Services
{
    public class BandcampWebDriver : IBandcampWebDriver
    {
        private readonly string _baseAddress;
        private readonly IWebDriver _driver;
        public BandcampWebDriver(string baseAddress)
        {
            _baseAddress = baseAddress;
            _driver = BuildDriver();

            static IWebDriver BuildDriver()
            {
                var options = new ChromeOptions();
#if !DEBUG
                options.AddArguments("headless");
#endif
                options.AddArguments("window-size=1920,1080");
                return new ChromeDriver(options);
            }
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
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
    }
}
