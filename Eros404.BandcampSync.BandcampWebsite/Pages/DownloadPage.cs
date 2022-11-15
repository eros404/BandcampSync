using OpenQA.Selenium;

namespace Eros404.BandcampSync.BandcampWebsite.Pages
{
    internal class DownloadPage : Page<DownloadPage>
    {
        public DownloadPage(IWebDriver driver, string baseUrl) : base(driver, baseUrl)
        {
        }

        protected override void ExecuteLoad()
        {
            throw new NotImplementedException();
        }

        protected override bool EvaluateLoadedStatus()
        {
            throw new NotImplementedException();
        }
    }
}