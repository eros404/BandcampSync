using Eros404.BandcampSync.AppSettings.Services;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Eros404.BandcampSync.AppSettings.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureWritable<T>(
            this IServiceCollection services,
            IConfigurationSection section,
            string file = "appsettings.json") where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var executingAssembly = provider.GetService<Assembly>();
                var options = provider.GetService<IOptionsMonitor<T>>();
                if (executingAssembly == null || options == null)
                    throw new Exception();
                return new WritableOptions<T>(executingAssembly, options, section.Key, file);
            });
            return services;
        }
    }
}
