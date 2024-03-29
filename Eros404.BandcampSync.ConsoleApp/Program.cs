﻿using System.Reflection;
using Eros404.BandcampSync.AppSettings.Extensions;
using Eros404.BandcampSync.BandcampApi.Services;
using Eros404.BandcampSync.BandcampWebsite.Extensions;
using Eros404.BandcampSync.Comparator.Services;
using Eros404.BandcampSync.ConsoleApp;
using Eros404.BandcampSync.ConsoleApp.Cli.Commands;
using Eros404.BandcampSync.ConsoleApp.Cli.Commands.Phantom;
using Eros404.BandcampSync.ConsoleApp.Cli.Commands.See;
using Eros404.BandcampSync.ConsoleApp.Cli.Commands.Set;
using Eros404.BandcampSync.ConsoleApp.Cli.Infrastructure;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Phantom;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.See;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.Set;
using Eros404.BandcampSync.Core.Services;
using Eros404.BandcampSync.LocalCollection.Extensions;
using Eros404.BandcampSync.Mail.Services;
using Eros404.BandcampSync.Phantom.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var executingAssembly = Assembly.GetExecutingAssembly();
var currentDirectory = Environment.CurrentDirectory;
var userPersonalDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

var services = new ServiceCollection();
services.AddDataProtection();
services
    .RegisterUserSettingsService(Path.Combine(userPersonalDirectory, ".bandcampsync.usersettings.json"))
    .ConfigureAllOptions(configuration)
    .AddScoped<ILogger, Logger>()
    .AddScoped<IBandcampApiService, BandcampApiService>()
    .RegisterBandcampWebDriverFactory(Path.GetDirectoryName(executingAssembly.Location)!)
    .RegisterLocalCollectionService(currentDirectory)
    .RegisterPhantomService(currentDirectory)
    .AddScoped<IComparatorService, ComparatorService>()
    .AddScoped<IMailService, MailService>()
    .AddScoped<IDownloadService, DownloadService>();

var app = new CommandApp(new TypeRegistrar(services));
app.Configure(config =>
{
    config.SetApplicationName("bandcampsync");
    config.SetApplicationVersion(executingAssembly.GetName().Version!.ToString());
    config.AddCommand<CompareCollectionsCommand>("compare")
        .WithDescription("Displays all the missing items");
    config.AddCommand<SyncCommand>("sync")
        .WithDescription("Choose and download some missing items");
    config.AddCommand<AddItemsCommand>("add")
        .WithAlias("add-item")
        .WithDescription("Download some items with download links");
    config.AddBranch<SetConfigSettings>("set", set =>
    {
        set.SetDescription("Commands to change your configuration");
        set.AddCommand<SetIdentityCookieCommand>("identity")
            .WithDescription("Set your Bandcamp identity cookie");
        set.AddCommand<SetEmailAddressCommand>("email")
            .WithDescription("Set the email linked with your Bandcamp account");
    });
    config.AddBranch<SeeCollectionSettings>("see", see =>
    {
        see.SetDescription("Commands to display your collections");
        see.AddCommand<SeeBandcampCollectionCommand>("bandcamp")
            .WithDescription("Display your Bandcamp collection");
        see.AddCommand<SeeLocalCollectionCommand>("local")
            .WithDescription("Display the local collection");
        see.AddCommand<SeePhantomCollectionCommand>("phantoms")
            .WithDescription("Display the phantom collection");
    });
    config.AddBranch<PhantomSettings>("phantoms", phantoms =>
    {
        phantoms.SetDescription("Commands to manage the phantom collection");
        phantoms.AddCommand<AddPhantomCommand>("add")
            .WithDescription("Choose and phantomize some missing items");
        phantoms.AddCommand<RemovePhantomCommand>("remove")
            .WithDescription("Choose and remove some phantoms");
    });
});
return app.Run(args);