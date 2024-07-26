using CommunityToolkit.Mvvm.ComponentModel;

namespace Calabonga.Commandex.UI.Core;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private string _title = null!;

    [ObservableProperty]
    private bool _isBusy;
}