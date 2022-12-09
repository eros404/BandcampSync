using Eros404.BandcampSync.Core.Services;
using Eros404.BandcampSync.LocalCollection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Eros404.BandcampSync.LocalCollection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLocalCollectionService(this IServiceCollection services, string collectionPath)
    {
        return services.AddTransient<ILocalCollectionService>(_ => new LocalCollectionService(collectionPath));
    }
}