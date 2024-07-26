using System.Collections.ObjectModel;
using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.UI.Core;
using Calabonga.Commandex.UI.Core.Dialogs;
using Calabonga.Commandex.UI.Core.Helpers;
using Calabonga.Commandex.UI.Core.Services;
using Calabonga.Commandex.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calabonga.Commandex.UI.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase
{
    private readonly ILogger<ShellWindowViewModel> _logger;
    private readonly IDialogService _dialogService;

    public ShellWindowViewModel(
        ILogger<ShellWindowViewModel> logger,
        IDialogService dialogService,
        IVersionService versionService)
    {
        Version = $"{versionService.Version} ({versionService.Branch}:{versionService.Commit})";
        Title = $"Command Executor {Version}";
        _logger = logger;
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
        var actions = App.Current.Services.GetServices<IAction>();
        var enumerable = actions as IAction[] ?? actions.ToArray();

        var group = new GroupTreeItem()
        {
            Name = "Чужие API",
            Items = enumerable.Select(x => new TreeItem() { Name = x.DisplayName }).ToList()
        };


        Tree = enumerable.Any() ? new ObservableCollection<GroupTreeItem>([group])
            : new ObservableCollection<GroupTreeItem>(TreeHelper.GetFakeTree());
    }
}