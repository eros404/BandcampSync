using System;
using System.IO;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Eros404.BandcampSync.App.Managers;
using Eros404.BandcampSync.App.Views;
using Eros404.BandcampSync.AppSettings.Extensions;
using Eros404.BandcampSync.BandcampApi.Services;
using Eros404.BandcampSync.BandcampWebsite.Extensions;
using Eros404.BandcampSync.Comparator.Services;
using Eros404.BandcampSync.Core.Services;
using Eros404.BandcampSync.LocalCollection.Extensions;
using Eros404.BandcampSync.Mail.Services;
using Eros404.BandcampSync.Phantom.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eros404.BandcampSync.App;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ConfigureServiceProvider();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _serviceProvider!.GetService<IViewModelsManager>()!.MainWindowViewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServiceProvider()
    {
        _serviceProvider = ConfigureServices().BuildServiceProvider();
    }

    private static IServiceCollection ConfigureServices()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        var executingAssembly = Assembly.GetExecutingAssembly();
        var userPersonalDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        var services = new ServiceCollection();
        services.AddDataProtection();
        services
            .RegisterUserSettingsService(Path.Combine(userPersonalDirectory, ".bandcampsync.usersettings.json"))
            .ConfigureAllOptions(configuration)
            .AddTransient<IBandcampApiService, BandcampApiService>()
            .RegisterBandcampWebDriverFactory(Path.GetDirectoryName(executingAssembly.Location)!)
            .RegisterLocalCollectionService()
            .RegisterPhantomService()
            .AddTransient<IComparatorService, ComparatorService>()
            .AddTransient<IMailService, MailService>()
            .AddTransient<IDownloadService, DownloadService>()
            .AddTransient<IViewModelsManager, ViewModelsManager>();
        return services;
    }
}