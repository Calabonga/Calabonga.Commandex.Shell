using CommunityToolkit.Mvvm.ComponentModel;

namespace Calabonga.Commandex.Contracts;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private string _title = null!;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    public bool IsNotBusy => !IsBusy;
}