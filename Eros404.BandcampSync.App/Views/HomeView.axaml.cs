using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Eros404.BandcampSync.App.Models;
using Eros404.BandcampSync.App.ViewModels;
using ReactiveUI;

namespace Eros404.BandcampSync.App.Views;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.ShowUserSettingsDialog.RegisterHandler(DoShowUserSettingsDialogAsync)));
    }

    private async Task DoShowUserSettingsDialogAsync(InteractionContext<UserSettingsWindowViewModel, UserSettingsModel> interaction)
    {
        var dialog = new UserSettingsWindow
        {
            DataContext = interaction.Input
        };

        var result = await dialog.ShowDialog<UserSettingsModel>((Window)Parent);
        interaction.SetOutput(result);
    }
}