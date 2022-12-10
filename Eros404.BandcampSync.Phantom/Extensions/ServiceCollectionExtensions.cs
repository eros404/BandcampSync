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
}