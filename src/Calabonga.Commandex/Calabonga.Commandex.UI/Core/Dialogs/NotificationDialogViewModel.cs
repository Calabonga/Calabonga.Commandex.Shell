using Calabonga.Commandex.Contracts;

namespace Calabonga.Commandex.UI.Core.Dialogs
{
    public partial class NotificationDialogViewModel : ViewModelBase, IDialogResult
    {
        public string DialogTitle => Title;

        public bool Ok => true;
    }
}
