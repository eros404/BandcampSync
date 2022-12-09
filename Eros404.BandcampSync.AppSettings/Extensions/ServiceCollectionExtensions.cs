using Eros404.BandcampSync.AppSettings.Services;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;

namespace Eros404.BandcampSync.AppSettings.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterUserSettingsService(this IServiceCollection services, string filePath)
        {
            return services.AddTransient<IUserSettingsService>(provider =>
            {
                var dataProtectionProviider = provider.GetService<IDataProtectionProvider>();
                if (dataProtectionProviider == null)
                    throw new Exception();
                return new UserSettingsService(dataProtectionProviider, filePath);
            });
        }
    }
}
