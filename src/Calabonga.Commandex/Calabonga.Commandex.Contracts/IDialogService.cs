namespace Calabonga.Commandex.Contracts;

public interface IDialogService
{
    void ShowDialog<TView, TViewModel>(Action<TViewModel> onClosingDialogCallback) where TView : IDialogView where TViewModel : IDialogResult;

    void ShowNotification(string message);

    void ShowWarning(string message);

    void ShowError(string message);
}