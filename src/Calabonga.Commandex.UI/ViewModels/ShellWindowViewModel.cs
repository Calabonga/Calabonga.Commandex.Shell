using System.Collections.ObjectModel;
using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.UI.Core;
using Calabonga.Commandex.UI.Core.Dialogs;
using Calabonga.Commandex.UI.Core.Helpers;
using Calabonga.Commandex.UI.Core.Services;
using Calabonga.Commandex.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace Calabonga.Commandex.UI.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase
{
    private readonly IEnumerable<ICommandexAction> _actions;
    private readonly ILogger<ShellWindowViewModel> _logger;
    private readonly IDialogService _dialogService;

    public ShellWindowViewModel(
        IEnumerable<ICommandexAction> actions,
        ILogger<ShellWindowViewModel> logger,
        IDialogService dialogService,
        IVersionService versionService)
    {
        Version = $"{versionService.Version} ({versionService.Branch}:{versionService.Commit})";
        Title = $"Command Executor {Version}";
        _actions = actions;
        _logger = logger;
        _dialogService = dialogService;
    }

    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteActionCommand))]
    private ActionItem? _selectedAction;

    [ObservableProperty]
    private ObservableCollection<ActionItem> _actionList;

    private bool CanExecuteAction => SelectedAction is not null;

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task ExecuteActionAsync()
    {
        var cancellationTokenSource = new CancellationTokenSource();

        // _dialogService.ShowDialog<ActionExecuteDialog, ActionExecuteDialogViewModel>(result => { });

        var action = _actions.FirstOrDefault(x => x.TypeName == SelectedAction!.TypeName);


        if (action is null)
        {
            _logger.LogError("Action not found");
            return;
        }

        _logger.LogDebug("Action found: {ActionName}", action.TypeName);


        await action.ExecuteAsync(cancellationTokenSource.Token);

    }

    [RelayCommand]
    private void ShowModules()
    {
        _dialogService.ShowDialog<ModulesInfoDialog, ModulesInfoDialogViewModel>(_ => { });
    }

    [RelayCommand]
    private void ShowAbout()
    {
        _dialogService.ShowNotification($"{Version}: Command Executor or Command Launcher. To run commands of any type for any purpose. For example, to execute a stored procedure or just to copy some files to some destination.");
    }

    [RelayCommand]
    private void LoadTree()
    {
        IsBusy = true;

        var actionsList = _actions.Select(x => new ActionItem(x.TypeName, x.DisplayName, x.Description)).ToList();
        actionsList.AddRange(ActionsHelper.GetFakeActions());
        ActionList = new ObservableCollection<ActionItem>(actionsList);

        IsBusy = false;
    }
}