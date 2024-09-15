using Calabonga.Commandex.Engine.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Calabonga.Commandex.Shell.ViewModels;

public partial class MenuItemViewModel : ViewModelBase
{
    public MenuItemViewModel()
    {

    }

    public string Text { get; set; }

    [ObservableProperty]
    private ObservableCollection<MenuItemViewModel> _menuItems;
}