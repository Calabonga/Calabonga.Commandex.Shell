using System.Windows.Controls;
using Calabonga.Commandex.UI.Core.Dialogs.Base;

namespace Calabonga.Commandex.UI.Core.Dialogs
{
    /// <summary>
    /// Interaction logic for ModulesInfoDialog.xaml
    /// </summary>
    public partial class ModulesInfoDialog : UserControl, IDialogView
    {
        public ModulesInfoDialog()
        {
            InitializeComponent();
        }

        public IDialogResult Result { get; }
        public object ViewModel { get; }
    }
}
