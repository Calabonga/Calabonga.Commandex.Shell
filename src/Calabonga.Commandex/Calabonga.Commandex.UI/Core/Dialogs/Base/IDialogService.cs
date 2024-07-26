namespace Calabonga.Commandex.UI.Core.Dialogs.Base;

public interface IDialogService
{
    void ShowDialog<TView, TViewModel>(Action<TViewModel> onClosingDialogCallback) where TView : IDialogView where TViewModel : IDialogResult;

    void ShowNotification(string message);

    void ShowWarning(string message);

    void ShowError(string message);
}