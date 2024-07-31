using CommunityToolkit.Mvvm.ComponentModel;

namespace Calabonga.Commandex.Contracts;

public abstract partial class ViewModelWithValidatorBase : ObservableValidator
{
    [ObservableProperty]
    private string _title = null!;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    public bool IsNotBusy => !IsBusy;
}