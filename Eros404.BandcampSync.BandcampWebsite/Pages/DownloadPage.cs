using Eros404.BandcampSync.BandcampWebsite.Extensions;
using OpenQA.Selenium;

namespace Eros404.BandcampSync.BandcampWebsite.Pages
{
    internal class DownloadPage : Page<DownloadPage>
    {
        private readonly string _url;
        public DownloadPage(IWebDriver driver, string baseUrl) : base(driver, baseUrl)
        {
            _url = new Uri(new Uri(baseUrl), "/download").ToString();
        }
        public DownloadPage(IWebDriver driver, string baseUrl, string query) : base(driver, baseUrl)
        {
            _url = new Uri(new Uri(baseUrl), $"/download{query}").ToString();
        }

        private IWebElement PreparingTitle => Driver.FindElement(By.ClassName("preparing-title"));
        private static By DownloadButtonBy => By.CssSelector(".download-title > a.item-button");
        private IWebElement DownloadButton => Driver.FindElement(DownloadButtonBy);
        private IWebElement ReauthEmailInput => Driver.FindElement(By.CssSelector(".reauth-form > input.reauth-email"));
        private IWebElement ReauthSubmit => Driver.FindElement(By.CssSelector(".reauth-form > input.submit"));

        protected override void ExecuteLoad()
        {
            Driver.Navigate().GoToUrl(_url);
        }

        protected override bool EvaluateLoadedStatus()
        {
            try
            {
                return Driver.Url.StartsWith(_url) &&
                       (PreparingTitle.Displayed || DownloadButton.Displayed || ReauthSubmit.Displayed);
            }
            catch (InvalidElementStateException)
            {
                return false;
            }
        }

        public bool DownloadIsExpired()
        {
            try
            {
                return ReauthSubmit.Displayed;
            }
            catch (InvalidElementStateException)
            {
                return false;
            }
        }

        private void WaitUntilDownloadIsReady() => Driver.WaitUntil(driver => driver.FindElement(DownloadButtonBy).Displayed);

        public void Download()
        {
            WaitUntilDownloadIsReady();
            DownloadButton.Click();
        }
    }
}