using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.BandcampWebsite.Models;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.BandcampWebsite.Services;

public class BandcampWebDriverFactory : IBandcampWebDriverFactory
{
    private readonly string _baseUrl;
    private readonly string _driverDownloadDirectory;
    private readonly string _identityCookie;
    private readonly SeleniumWebDriverType _webDriverType;

    public BandcampWebDriverFactory(IOptions<BandcampOptions> bandcampOptions,
        IOptions<SeleniumOptions> seleniumOptions, IUserSettingsService userSettingsService,
        string driverDownloadDirectory)
    {
        _baseUrl = bandcampOptions.Value.BaseUrl;
        _webDriverType = GetWebDriverType(seleniumOptions.Value.WebDriver);
        _identityCookie = userSettingsService.GetValue(UserSettings.BandcampIdentityCookie);
        _driverDownloadDirectory = driverDownloadDirectory;
    }

    public IBandcampWebDriver Create()
    {
        return new BandcampWebDriver(_baseUrl, _webDriverType, _driverDownloadDirectory);
    }

    public IBandcampWebDriver CreateWithIdentity()
    {
        return new BandcampWebDriver(_baseUrl, _webDriverType, _driverDownloadDirectory)
            .SetIdentityCookie(_identityCookie);
    }

    private static SeleniumWebDriverType GetWebDriverType(string content)
    {
        return content switch
        {
            "firefox" => SeleniumWebDriverType.Firefox,
            _ => SeleniumWebDriverType.Chrome
        };
    }
}