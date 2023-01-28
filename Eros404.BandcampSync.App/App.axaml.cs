using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Eros404.BandcampSync.App.Managers;
using Eros404.BandcampSync.App.Views;
using Eros404.BandcampSync.AppSettings.Extensions;
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
        var userPersonalDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        var services = new ServiceCollection();
        services.AddDataProtection();
        services.RegisterUserSettingsService(Path.Combine(userPersonalDirectory, ".bandcampsync.usersettings.json"))
            .AddTransient<IViewModelsManager, ViewModelsManager>();
        return services;
    }
}