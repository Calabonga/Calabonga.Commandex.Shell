using System.Collections.ObjectModel;
using Calabonga.Commandex.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Calabonga.Commandex.UI.Core.Dialogs;

public partial class ModulesInfoDialogViewModel : ViewModelBase, IDialogResult
{
    private readonly IEnumerable<IDbConnectionFactory> _factories;

    public ModulesInfoDialogViewModel(IEnumerable<IDbConnectionFactory> factories)
    {
        Title = "Available Modules";
        _factories = factories;
        GetModules();
    }

    [ObservableProperty]
    private ObservableCollection<string> _modules;

    public string DialogTitle => Title;

    private void GetModules()
    {
        IsBusy = true;

        var modules = _factories.Select(factory => factory.Description).ToList();
        Modules = new ObservableCollection<string>(modules);

        IsBusy = false;
    }
}