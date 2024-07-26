namespace Calabonga.Commandex.UI.Core.Dialogs.Base;

public interface IDialogView
{
    IDialogResult Result { get; }

    object ViewModel { get; }
}