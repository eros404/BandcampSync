using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.BandcampWebsite.Services;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.BandcampWebsite.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBandcampWebDriverFactory(this IServiceCollection services,
        string driverDownloadDirectory)
    {
        return services.AddTransient<IBandcampWebDriverFactory>(provider =>
        {
            var bandcampOptions = provider.GetService<IOptions<BandcampOptions>>();
            var seleniumOptions = provider.GetService<IOptions<SeleniumOptions>>();
            var userSettingsService = provider.GetService<IUserSettingsService>();
            if (bandcampOptions == null || seleniumOptions == null || userSettingsService == null)
                throw new Exception("Could not register BandcampWebDriverFactory");
            return new BandcampWebDriverFactory(bandcampOptions, seleniumOptions, userSettingsService,
                driverDownloadDirectory);
        });
    }
}