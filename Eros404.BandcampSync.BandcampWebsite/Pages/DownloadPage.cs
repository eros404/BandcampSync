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

        protected override void ExecuteLoad()
        {
            Driver.Navigate().GoToUrl(_url);
        }

        protected override bool EvaluateLoadedStatus()
        {
            return Driver.Url.StartsWith(_url);
        }
    }
}