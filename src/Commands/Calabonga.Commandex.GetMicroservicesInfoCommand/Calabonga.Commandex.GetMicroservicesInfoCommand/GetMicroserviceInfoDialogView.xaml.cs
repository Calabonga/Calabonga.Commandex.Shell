using System.Windows;
using System.Windows.Controls;
using Calabonga.Commandex.Contracts;

namespace Calabonga.Commandex.GetMicroservicesInfoCommand
{
    /// <summary>
    /// Interaction logic for GetMicroserviceInfoDialogView.xaml
    /// </summary>
    public partial class GetMicroserviceInfoDialogView : UserControl, IDialogView
    {
        public GetMicroserviceInfoDialogView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((GetMicroserviceInfoDialogResult)DataContext).Password = ((PasswordBox)sender).SecurePassword; }
        }
    }
}
