using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.BandcampWebsite.Services
{
    public class BandcampWebDriverFactory : IBandcampWebDriverFactory
    {
        private readonly string _baseUrl;
        private readonly string _identityCookie;

        public BandcampWebDriverFactory(IOptions<BandcampOptions> options, IUserSettingsService userSettingsService)
        {
            _baseUrl = options.Value.BaseUrl;
            _identityCookie = userSettingsService.GetValue(UserSettings.BandcampIdentityCookie);
        }

        public IBandcampWebDriver Create() => new BandcampWebDriver(_baseUrl);

        public IBandcampWebDriver CreateWithIdentity() => new BandcampWebDriver(_baseUrl)
            .SetIdentityCookie(_identityCookie);
    }
}
