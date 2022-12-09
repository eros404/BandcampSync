using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.BandcampWebsite.Models;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.BandcampWebsite.Services
{
    public class BandcampWebDriverFactory : IBandcampWebDriverFactory
    {
        private readonly string _baseUrl;
        private readonly SeleniumWebDriverType _webDriverType;
        private readonly string _identityCookie;

        public BandcampWebDriverFactory(IOptions<BandcampOptions> bandcampOptions,
            IOptions<SeleniumOptions> seleniumOptions, IUserSettingsService userSettingsService)
        {
            _baseUrl = bandcampOptions.Value.BaseUrl;
            _webDriverType = GetWebDriverType(seleniumOptions.Value.WebDriver);
            _identityCookie = userSettingsService.GetValue(UserSettings.BandcampIdentityCookie);
        }

        public IBandcampWebDriver Create() => new BandcampWebDriver(_baseUrl, _webDriverType);

        public IBandcampWebDriver CreateWithIdentity() => new BandcampWebDriver(_baseUrl, _webDriverType)
            .SetIdentityCookie(_identityCookie);

        private static SeleniumWebDriverType GetWebDriverType(string content)
        {
            return content switch
            {
                "firefox" => SeleniumWebDriverType.Firefox,
                _ => SeleniumWebDriverType.Chrome
            };
        }
    }
}
