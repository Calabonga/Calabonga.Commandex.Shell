namespace Calabonga.Commandex.Contracts;

/// <summary>
/// // Calabonga: Summary required (IDialogService 2024-07-29 04:10)
/// </summary>
public interface IDialogService
{
    void ShowDialog<TView, TViewModel>(Action<TViewModel> onClosingDialogCallback)
        where TView : IDialogView
        where TViewModel : IDialogResult;

    /// <summary>
    /// Opens notification dialog
    /// </summary>
    /// <param name="message"></param>
    void ShowNotification(string message);

    /// <summary>
    /// Opens warning dialog
    /// </summary>
    /// <param name="message"></param>
    void ShowWarning(string message);

    /// <summary>
    /// Opens error dialog
    /// </summary>
    /// <param name="message"></param>
    void ShowError(string message);
}