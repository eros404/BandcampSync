using Eros404.BandcampSync.BandcampWebsite.Models;
using Eros404.BandcampSync.BandcampWebsite.Pages;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Eros404.BandcampSync.BandcampWebsite.Services
{
    public sealed class BandcampWebDriver : IBandcampWebDriver
    {
        private readonly string _baseAddress;
        private readonly IWebDriver _driver;
        public BandcampWebDriver(string baseAddress, SeleniumWebDriverType webDriverType, string driverDownloadDirectory)
        {
            _baseAddress = baseAddress;
            _driver = BuildDriver(baseAddress, webDriverType, driverDownloadDirectory);
        }
        private static IWebDriver BuildDriver(string baseAddress, SeleniumWebDriverType webDriverType, string driverDownloadDirectory)
        {
            var driverManager = new DriverManager(driverDownloadDirectory);
            return webDriverType switch
            {
                SeleniumWebDriverType.Chrome => BuildChromeDriver(driverManager, baseAddress),
                SeleniumWebDriverType.Firefox => BuildFirefoxDriver(driverManager, baseAddress),
                _ => throw new ArgumentOutOfRangeException(nameof(webDriverType), webDriverType, null)
            };
            
            static IWebDriver BuildFirefoxDriver(DriverManager driverManager, string baseAddress)
            {
                driverManager.SetUpDriver(new FirefoxConfig());
                var options = new FirefoxOptions();
#if !DEBUG
                options.AddArguments("headless");
                options.AddArgument("log-level=3");
#endif
                options.AddArguments("window-size=1920,1080");
                var driverService = FirefoxDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                driverService.SuppressInitialDiagnosticInformation = true;
                return new FirefoxDriver(driverService, options)
                {
                    Url = baseAddress
                };   
            }
            static IWebDriver BuildChromeDriver(DriverManager driverManager, string baseAddress)
            {
                driverManager.SetUpDriver(new ChromeConfig());
                var options = new ChromeOptions();
#if !DEBUG
                options.AddArguments("headless");
                options.AddArgument("log-level=3");
#endif
                options.AddArguments("window-size=1920,1080");
                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                driverService.SuppressInitialDiagnosticInformation = true;
                return new ChromeDriver(driverService, options)
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
        
        ~BandcampWebDriver()
        {
            Dispose(disposing: false);
        }

        public bool Login(string userName, string password)
        {
            return new LoginPage(_driver, _baseAddress).Load().Login(userName, password).Loaded;
        }

        public DownloadLinkResult GetDownloadLink(string url, AudioFormat format, string email)
        {
            var page = new DownloadPage(_driver, _baseAddress, new Uri(url).Query).Load();
            if (page.DownloadIsExpired())
            {
                return page.SendReauthMail(email) ? DownloadLinkResult.Expired : DownloadLinkResult.WrongEmail;
            }
            return DownloadLinkResult.Success(page.GetDownloadLink(format));
        }

        public IBandcampWebDriver SetIdentityCookie(string value)
        {
            _driver.Manage().Cookies.AddCookie(new Cookie("identity", value, new Uri(_baseAddress).Host, "/", null));
            return this;
        }
    }
}