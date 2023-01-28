using Eros404.BandcampSync.Core.Services;
using Eros404.BandcampSync.Phantom.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Eros404.BandcampSync.Phantom.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterPhantomService(this IServiceCollection services, string collectionPath)
    {
        return services.AddTransient<IPhantomService>(_ => new PhantomService(collectionPath));
    }
    public static IServiceCollection RegisterPhantomService(this IServiceCollection services)
    {
        return services.AddTransient<IPhantomService>(provider =>
        {
            var userSettingsService = provider.GetService<IUserSettingsService>();
            if (userSettingsService is null)
                throw new Exception("Could not register PhantomService.");
            return new PhantomService(userSettingsService);
        });
    }
}