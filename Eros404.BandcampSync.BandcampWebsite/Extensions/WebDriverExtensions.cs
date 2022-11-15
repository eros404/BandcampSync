using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Eros404.BandcampSync.BandcampWebsite.Extensions;

public static class WebDriverExtensions
{
    public static TResult WaitUntil<TResult>(this IWebDriver driver, Func<IWebDriver, TResult> condition,
        int secondsTimeout = 5)
    {
        return new WebDriverWait(driver, TimeSpan.FromSeconds(secondsTimeout)).Until(condition);
    }
}