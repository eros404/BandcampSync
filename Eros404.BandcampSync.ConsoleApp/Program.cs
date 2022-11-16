using Eros404.BandcampSync.AppSettings.Extensions;
using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.BandcampApi.Services;
using Eros404.BandcampSync.BandcampWebsite.Services;
using Eros404.BandcampSync.ConsoleApp;
using Eros404.BandcampSync.ConsoleApp.Cli.Commands;
using Eros404.BandcampSync.ConsoleApp.Cli.Commands.See;
using Eros404.BandcampSync.ConsoleApp.Cli.Commands.Set;
using Eros404.BandcampSync.ConsoleApp.Cli.Infrastructure;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.See;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set;
using Eros404.BandcampSync.Core.Services;
using Eros404.BandcampSync.LocalCollection.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection()
    .AddScoped(_ => System.Reflection.Assembly.GetExecutingAssembly())
    .ConfigureWritable<BandcampOptions>(configuration.GetSection(BandcampOptions.Section))
    .ConfigureWritable<LocalCollectionOptions>(configuration.GetSection(LocalCollectionOptions.Section))
    .AddScoped<ILogger, Logger>()
    .AddScoped<IBandcampApiService, BandcampApiService>()
    .AddScoped<IBandcampWebDriverFactory, BandcampWebDriverFactory>()
    .AddScoped<ILocalCollectionService, LocalCollectionService>();

var app = new CommandApp(new TypeRegistrar(services));
app.Configure(config =>
{
    config.AddCommand<CompareCollectionsCommand>("compare");
    config.AddCommand<SyncCommand>("sync");
    config.AddBranch<SetConfigSettings>("set", set =>
    {
        set.AddCommand<SetIdentityCookieCommand>("identity");
        set.AddCommand<SetLocalCollectionPathCommand>("local");
    });
    config.AddBranch<SeeCollectionSettings>("see", view =>
    {
        view.AddCommand<SeeBandcampCollectionCommand>("bandcamp");
        view.AddCommand<SeeLocalCollectionCommand>("local");
    });
});
return app.Run(args);