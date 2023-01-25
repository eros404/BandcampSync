using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Eros404.BandcampSync.App.Views;

public partial class UserSettingsView : UserControl
{
    public UserSettingsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}