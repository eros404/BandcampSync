using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Eros404.BandcampSync.App.ViewModels;
using ReactiveUI;
using System;

namespace Eros404.BandcampSync.App.Views;

public partial class UserSettingsWindow : ReactiveWindow<UserSettingsWindowViewModel>
{
    public UserSettingsWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel.SaveCommand.Subscribe(Close)));
    }
}