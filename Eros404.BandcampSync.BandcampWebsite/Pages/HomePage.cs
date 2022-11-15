using OpenQA.Selenium;

namespace Eros404.BandcampSync.BandcampWebsite.Pages
{
    internal class HomePage : Page<HomePage>
    {
        private readonly string _url;
        public HomePage(IWebDriver driver, string baseUrl) : base(driver, baseUrl)
        {
            _url = baseUrl;
        }

        protected override bool EvaluateLoadedStatus()
        {
            return Driver.Url.StartsWith(_url);
        }

        protected override void ExecuteLoad()
        {
            Driver.Navigate().GoToUrl(_url);
        }
    }
}
