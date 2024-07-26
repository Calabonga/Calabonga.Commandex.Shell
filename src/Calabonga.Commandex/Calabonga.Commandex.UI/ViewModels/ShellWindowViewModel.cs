using System.Collections.ObjectModel;
using Calabonga.Commandex.UI.Core;
using Calabonga.Commandex.UI.Core.Dialogs;
using Calabonga.Commandex.UI.Core.Dialogs.Base;
using Calabonga.Commandex.UI.Core.Helpers;
using Calabonga.Commandex.UI.Core.Services;
using Calabonga.Commandex.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Commandex.UI.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;

    public ShellWindowViewModel(
        IDialogService dialogService,
        IVersionService versionService)
    {
        Version = $"{versionService.Version} ({versionService.Branch}:{versionService.Commit})";
        Title = $"Command Executor {Version}";
        _dialogService = dialogService;
    }

    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    private ObservableCollection<GroupTreeItem> _tree;

    [RelayCommand]
    private void ShowModules()
    {
        _dialogService.ShowDialog<ModulesInfoDialog, ModulesInfoDialogViewModel>(result => { });
    }

    [RelayCommand]
    private void ShowAbout()
    {
        _dialogService.ShowNotification($"{Version}: Command Executor or Command Launcher. To run commands of any type for any purpose. For example, to execute a stored procedure or just to copy some files to some destination.");
    }

    [RelayCommand]
    private void LoadTree()
    {
        Tree = new ObservableCollection<GroupTreeItem>(TreeHelper.GetFakeTree());
    }
}