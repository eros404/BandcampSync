using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.Core.Services;

namespace Eros404.BandcampSync.BandcampWebsite.Services
{
    public class BandcampWebDriverFactory : IBandcampWebDriverFactory
    {
        private readonly IWritableOptions<BandcampOptions> _options;

        public BandcampWebDriverFactory(IWritableOptions<BandcampOptions> options)
        {
            _options = options;
        }

        public IBandcampWebDriver Create() => new BandcampWebDriver(_options.Value.BaseUrl);

        public IBandcampWebDriver CreateWithIdentity() => new BandcampWebDriver(_options.Value.BaseUrl)
            .SetIdentityCookie(_options.Value.IdentityCookie);
    }
}
