using Eros404.BandcampSync.BandcampWebsite.Extensions;
using Eros404.BandcampSync.Core.Extensions;
using Eros404.BandcampSync.Core.Models;
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
        private IWebElement ItemFormatButton => Driver.FindElement(By.ClassName("item-format"));

        private IEnumerable<IWebElement> ReauthErrorMessages =>
            Driver.FindElements(By.CssSelector(".email-reauth-error .error-msg"));

        private IEnumerable<IWebElement> ItemFormatListElements =>
            Driver.FindElements(By.CssSelector("ul.formats > li"));

        protected override void ExecuteLoad()
        {
            Driver.Navigate().GoToUrl(_url);
            Driver.WaitForJsToLoad(30);
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

        private void WaitUntilDownloadIsReady() =>
            Driver.WaitUntil(driver => driver.FindElement(DownloadButtonBy).Displayed);

        public string GetDownloadLink(AudioFormat audioFormat)
        {
            ItemFormatButton.Click();
            var itemFormatElement = ItemFormatListElements.ToList().Find(listEl =>
                listEl.FindElement(By.ClassName("description")).Text == audioFormat.GetDisplayName());
            if (itemFormatElement != null)
            {
                Driver.WaitUntil(_ => itemFormatElement.Displayed);
                itemFormatElement.Click();
                Driver.WaitForJsToLoad(30);
            }

            WaitUntilDownloadIsReady();
            return DownloadButton.GetAttribute("href");
        }

        public bool SendReauthMail(string email)
        {
            ReauthEmailInput.SendKeys(email);
            ReauthSubmit.Click();
            Driver.WaitForJsToLoad();
            return !ReauthEmailInput.Displayed;
        }
    }
}