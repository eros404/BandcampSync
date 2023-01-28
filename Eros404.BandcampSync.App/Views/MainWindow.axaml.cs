using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Eros404.BandcampSync.App.Models;
using Eros404.BandcampSync.App.ViewModels;
using ReactiveUI;

namespace Eros404.BandcampSync.App.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
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

        var result = await dialog.ShowDialog<UserSettingsModel>(this);
        interaction.SetOutput(result);
    }
}