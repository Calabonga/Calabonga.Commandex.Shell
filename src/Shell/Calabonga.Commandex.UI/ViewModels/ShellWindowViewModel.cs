using System.Collections.ObjectModel;
using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Commands;
using Calabonga.Commandex.UI.Core.Dialogs;
using Calabonga.Commandex.UI.Core.Engine;

using Calabonga.Commandex.UI.Core.Services;
using Calabonga.Commandex.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace Calabonga.Commandex.UI.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase
{
    private readonly IEnumerable<ICommandexCommand> _commands;
    private readonly ILogger<ShellWindowViewModel> _logger;
    private readonly IDialogService _dialogService;

    public ShellWindowViewModel(
        IEnumerable<ICommandexCommand> commands,
        ILogger<ShellWindowViewModel> logger,
        IDialogService dialogService,
        IVersionService versionService)
    {
        Version = $"{versionService.Version} ({versionService.Branch}:{versionService.Commit})";
        Title = $"Command Executor {Version}";
        _commands = commands;
        _logger = logger;
        _dialogService = dialogService;
    }

    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteActionCommand))]
    private CommandItem? _selectedAction;

    [ObservableProperty]
    private ObservableCollection<CommandItem> _actionList;

    private bool CanExecuteAction => SelectedAction is not null;

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task ExecuteActionAsync()
    {
        var action = _commands.FirstOrDefault(x => x.TypeName == SelectedAction!.TypeName);
        if (action is null)
        {
            _logger.LogError("Action not found");
            return;
        }

        await action.ShowDialogAsync();

        if (action.HasResult)
        {
            var message = ActionsReport.CreateReport(action);
            _dialogService.ShowNotification(message);
        }
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
    private void LoadData()
    {
        IsBusy = true;

        var actionsList = _commands.Select(x => new CommandItem(x.TypeName, x.Version, x.DisplayName, x.Description)).ToList();
        ActionList = new ObservableCollection<CommandItem>(actionsList);

        IsBusy = false;
    }
}