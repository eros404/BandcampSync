using OpenQA.Selenium;

namespace Eros404.BandcampSync.BandcampWebsite.Pages;

internal class ProfilePage : Page<ProfilePage>
{
    public ProfilePage(IWebDriver driver, string baseUrl) : base(driver, baseUrl)
    {
    }

    private IWebElement FanNameContainer =>
        Driver.FindElement(By.CssSelector("div.name > h1:nth-child(1) > span:nth-child(1)"));

    protected override bool EvaluateLoadedStatus()
    {
        try
        {
            return FanNameContainer.Displayed &&
                   Driver.Url.StartsWith(new Uri(new Uri(BaseUrl), $"/{FanNameContainer.Text}").ToString());
        }
        catch (InvalidElementStateException)
        {
            return false;
        }
    }

    protected override void ExecuteLoad()
    {
    }
}