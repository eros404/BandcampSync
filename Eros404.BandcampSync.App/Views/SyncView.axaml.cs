using Avalonia.ReactiveUI;
using Eros404.BandcampSync.App.ViewModels;

namespace Eros404.BandcampSync.App.Views;

public partial class SyncView : ReactiveUserControl<SyncViewModel>
{
    public SyncView()
    {
        InitializeComponent();
    }
}