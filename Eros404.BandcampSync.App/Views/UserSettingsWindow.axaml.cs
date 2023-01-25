using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Eros404.BandcampSync.App.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Eros404.BandcampSync.App.Views;

public partial class UserSettingsWindow : ReactiveWindow<UserSettingsWindowViewModel>
{
    public UserSettingsWindow()
    {
        InitializeComponent();
        this.WhenActivated(d =>
            d(ViewModel.SelectLocalCollectionPathDialog.RegisterHandler(DoSelectLocalCollectionPathAsync)));
        this.WhenActivated(d => d(ViewModel.SaveCommand.Subscribe(Close)));
    }

    private async Task DoSelectLocalCollectionPathAsync(InteractionContext<Unit, string?> interaction)
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Locate the local collection"
        };
        var result = await dialog.ShowAsync(this);
        interaction.SetOutput(result);
    }
}