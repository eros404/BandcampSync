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
    }
    
}