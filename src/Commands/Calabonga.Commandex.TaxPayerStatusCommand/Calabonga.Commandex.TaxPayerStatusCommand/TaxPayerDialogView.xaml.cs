using System.Windows.Controls;
using Calabonga.Commandex.Contracts;

namespace Calabonga.Commandex.TaxPayerStatusCommand;

/// <summary>
/// Interaction logic for TaxPayerDialogView.xaml
/// </summary>
public partial class TaxPayerDialogView : UserControl, IDialogView
{
    public TaxPayerDialogView()
    {
        InitializeComponent();
    }
}