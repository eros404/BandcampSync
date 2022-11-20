using System.Reflection;
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
using Eros404.BandcampSync.Mail.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var executingAssembly = Assembly.GetExecutingAssembly();

var services = new ServiceCollection()
    .AddScoped(_ => executingAssembly)
    .ConfigureWritable<BandcampOptions>(configuration.GetSection(BandcampOptions.Section))
    .ConfigureWritable<LocalCollectionOptions>(configuration.GetSection(LocalCollectionOptions.Section))
    .ConfigureWritable<EmailOptions>(configuration.GetSection(EmailOptions.Section))
    .Configure<DownloadOptions>(configuration.GetSection(DownloadOptions.Section))
    .AddScoped<ILogger, Logger>()
    .AddScoped<IBandcampApiService, BandcampApiService>()
    .AddScoped<IBandcampWebDriverFactory, BandcampWebDriverFactory>()
    .AddScoped<ILocalCollectionService, LocalCollectionService>()
    .AddScoped<IMailService, MailService>()
    .AddScoped<IDownloadService, DownloadService>();

var app = new CommandApp(new TypeRegistrar(services));
app.Configure(config =>
{
    config.SetApplicationName(executingAssembly.GetName().Name ?? "BandcampSync");
    config.AddCommand<CompareCollectionsCommand>("compare")
        .WithDescription("Display the items missing items of your Bandcamp collection.");
    config.AddCommand<SyncCommand>("sync")
        .WithDescription("Download the missing items of your Bandcamp collection.")
        .WithExample(new[] { "sync", "-f", "FLAC" });
    config.AddCommand<AddItemsCommand>("add")
        .WithAlias("add-item")
        .WithDescription("Download an item from your Bandcamp collection with a download link.")
        .WithExample(new[] { "add", "-f", "FLAC", "\"http://bandcamp.com/download?payment_id={...}&reauth_sig={...}&reauth_ts={...}&sig={...}\"" });
    config.AddBranch<SetConfigSettings>("set", set =>
    {
        set.SetDescription("Commands to change your configuration.");
        set.AddCommand<SetIdentityCookieCommand>("identity")
            .WithDescription("Change your Bandcamp identity cookie.");
        set.AddCommand<SetLocalCollectionPathCommand>("local")
            .WithDescription("Change the location of your local collection (do not move any file).");
        set.AddCommand<SetEmailAddressCommand>("email")
            .WithDescription("Change the email of your Bandcamp account.");
    });
    config.AddBranch<SeeCollectionSettings>("see", see =>
    {
        see.SetDescription("Cammands to display your collections.");
        see.AddCommand<SeeBandcampCollectionCommand>("bandcamp")
            .WithDescription("Display your Bandcamp collection.");
        see.AddCommand<SeeLocalCollectionCommand>("local")
            .WithDescription("Display your local collection.");
    });
});
return app.Run(args);