using System.Collections.ObjectModel;
using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Commands;
using Calabonga.Commandex.UI.Core.Dialogs;
using Calabonga.Commandex.UI.Core.Engine;

using Calabonga.Commandex.UI.Core.Services;
using Calabonga.Commandex.UI.Models;
using Calabonga.PredicatesBuilder;
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
    private string? _searchTerm;

    [ObservableProperty]
    private bool _isFindEnabled = AppSettings.Default.ShowSearchPanelOnStartup;

    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    private ObservableCollection<CommandItem> _actionList;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteActionCommand))]
    private CommandItem? _selectedCommand;

    private bool CanExecuteAction => SelectedCommand is not null;

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task ExecuteActionAsync()
    {
        var command = _commands.FirstOrDefault(x => x.TypeName == SelectedCommand!.TypeName);
        if (command is null)
        {
            _logger.LogError("Action not found");
            return;
        }

        _logger.LogDebug("Executing {CommandType}", command.TypeName);

        await command.ShowDialogAsync();

        if (command.HasResult)
        {
            var message = ExecutionReport.CreateReport(command);
            _logger.LogInformation("{CommandType} executed with result: {Result}", command.TypeName, message);
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

        FindCommands();
        IsBusy = false;
    }

    private void FindCommands()
    {
        var predicate = PredicateBuilder.True<ICommandexCommand>().And(x => !string.IsNullOrEmpty(x.Version));

        if (!string.IsNullOrEmpty(SearchTerm))
        {
            var term = SearchTerm.ToLower();
            predicate = predicate
                .And(x => x.DisplayName.ToLower().Contains(term))
                .Or(x => x.Description.ToLower().Contains(term))
                .Or(x => x.CopyrightInfo.ToLower().Contains(term))
                .Or(x => x.TypeName.ToLower().Contains(term))
                .Or(x => x.Version.ToLower().Contains(term));
        }

        var actionsList = _commands
            .Where(predicate.Compile())
            .Select(x => new CommandItem(x.TypeName, x.Version, x.DisplayName, x.Description))
            .ToList();

        ActionList = new ObservableCollection<CommandItem>(actionsList);

        _logger.LogInformation($"Total commands were found: {actionsList.Count}");
    }

    partial void OnSearchTermChanged(string? value)
    {
        FindCommands();
    }

    partial void OnIsFindEnabledChanged(bool value)
    {
        _logger.LogInformation("IsFindEnabled {Value}", value);
    }

    partial void OnSelectedCommandChanged(CommandItem? value)
    {
        _logger.LogInformation("SelectedCommand {Name}", value?.TypeName ?? "N/A");
    }
}