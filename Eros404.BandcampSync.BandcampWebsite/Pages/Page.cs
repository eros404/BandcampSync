using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Eros404.BandcampSync.BandcampWebsite.Pages;

internal abstract class Page<T> : LoadableComponent<T>
    where T : LoadableComponent<T>
{
    protected readonly string BaseUrl;
    protected readonly IWebDriver Driver;

    protected Page(IWebDriver driver, string baseUrl)
    {
        Driver = driver;
        BaseUrl = baseUrl;
    }

    public bool Loaded => IsLoaded;
}