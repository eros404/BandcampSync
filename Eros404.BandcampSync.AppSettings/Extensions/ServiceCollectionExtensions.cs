using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.AppSettings.Services;
using Eros404.BandcampSync.Core.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eros404.BandcampSync.AppSettings.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterUserSettingsService(this IServiceCollection services, string filePath)
    {
        return services.AddTransient<IUserSettingsService>(provider =>
        {
            var dataProtectionProviider = provider.GetService<IDataProtectionProvider>();
            if (dataProtectionProviider == null)
                throw new Exception("Could not register UserSettingsService");
            return new UserSettingsService(dataProtectionProviider, filePath);
        });
    }

    public static IServiceCollection ConfigureAllOptions(this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        return services
            .Configure<BandcampOptions>(configuration.GetSection(BandcampOptions.Section))
            .Configure<DownloadOptions>(configuration.GetSection(DownloadOptions.Section))
            .Configure<SeleniumOptions>(configuration.GetSection(SeleniumOptions.Section));
    }
}