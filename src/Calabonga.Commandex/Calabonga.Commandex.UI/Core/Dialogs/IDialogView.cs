namespace Calabonga.Commandex.UI.Core.Dialogs;

public interface IDialogView
{
    IDialogResult Result { get; }

    object ViewModel { get; }
}