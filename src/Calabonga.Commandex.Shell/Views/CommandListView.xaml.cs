using Calabonga.Commandex.Engine.Zones;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calabonga.Commandex.Shell.Views;
/// <summary>
/// Interaction logic for CommandListView.xaml
/// </summary>
public partial class CommandListView : UserControl, IZoneView
{
    public CommandListView()
    {
        InitializeComponent();
        Focusable = true;
        Loaded += (_, _) => Keyboard.Focus(this);
    }
}
