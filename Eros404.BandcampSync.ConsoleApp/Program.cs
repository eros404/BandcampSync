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
using Eros404.BandcampSync.LocalCollection.Extensions;
using Eros404.BandcampSync.Mail.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var executingAssembly = Assembly.GetExecutingAssembly();
var currentDirectory = Environment.CurrentDirectory;

var services = new ServiceCollection();
services.AddDataProtection();
services
    .RegisterUserSettingsService(Path.Combine(Path.GetDirectoryName(executingAssembly.Location)!,
        "usersettings.json"))
    .Configure<BandcampOptions>(configuration.GetSection(BandcampOptions.Section))
    .Configure<DownloadOptions>(configuration.GetSection(DownloadOptions.Section))
    .AddScoped<ILogger, Logger>()
    .AddScoped<IBandcampApiService, BandcampApiService>()
    .AddScoped<IBandcampWebDriverFactory, BandcampWebDriverFactory>()
    .RegisterLocalCollectionService(currentDirectory)
    .AddScoped<IMailService, MailService>()
    .AddScoped<IDownloadService, DownloadService>();

var app = new CommandApp(new TypeRegistrar(services));
app.Configure(config =>
{
    config.SetApplicationName("bandcampsync");
    config.SetApplicationVersion(executingAssembly.GetName().Version!.ToString());
    config.AddCommand<CompareCollectionsCommand>("compare")
        .WithDescription("Displays all the items that are missing in the local collection.");
    config.AddCommand<SyncCommand>("sync")
        .WithDescription("Download the items that are missing in the local collection.");
    config.AddCommand<AddItemsCommand>("add")
        .WithAlias("add-item")
        .WithDescription("Download an item from your Bandcamp collection with a download link.");
    config.AddBranch<SetConfigSettings>("set", set =>
    {
        set.SetDescription("Commands to change your configuration.");
        set.AddCommand<SetIdentityCookieCommand>("identity")
            .WithDescription("Set your Bandcamp identity cookie.");
        set.AddCommand<SetEmailAddressCommand>("email")
            .WithDescription("Set the email linked with your Bandcamp account.");
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