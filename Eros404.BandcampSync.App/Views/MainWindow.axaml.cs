using Avalonia.ReactiveUI;
using Eros404.BandcampSync.App.ViewModels;

namespace Eros404.BandcampSync.App.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
}