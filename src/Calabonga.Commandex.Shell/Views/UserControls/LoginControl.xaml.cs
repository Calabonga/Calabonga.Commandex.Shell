using Calabonga.Commandex.Shell.ViewModels.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Calabonga.Commandex.Shell.Views.UserControls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<LoginControlViewModel>();
        }
    }
}
