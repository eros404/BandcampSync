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
    
    public static void ExecuteScript(this IWebDriver driver, string script)
    {
        ((IJavaScriptExecutor)driver).ExecuteScript(script);
    }
    public static T ExecuteScript<T>(this IWebDriver driver, string script)
    {
        return (T)((IJavaScriptExecutor)driver).ExecuteScript(script);
    }
    
    public static void WaitForJsToLoad(this IWebDriver driver, int secondsTimeout = 10)
    {
        driver.WaitUntil(d => d.ExecuteScript<bool>(
            "return document.readyState == 'complete' && jQuery.active == 0"), secondsTimeout);
    }
}