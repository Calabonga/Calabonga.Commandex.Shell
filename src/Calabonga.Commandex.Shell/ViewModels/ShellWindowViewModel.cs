using System.Collections.ObjectModel;
using Calabonga.Commandex.Engine;
using Calabonga.Commandex.Engine.Commands;
using Calabonga.Commandex.Shell.Core.Dialogs;
using Calabonga.Commandex.Shell.Core.Engine;
using Calabonga.Commandex.Shell.Core.Services;
using Calabonga.Commandex.Shell.Models;
using Calabonga.PredicatesBuilder;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace Calabonga.Commandex.Shell.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase
{
    private readonly CommandExecutor _commandExecutor;
    private readonly IEnumerable<ICommandexCommand> _commands;
    private readonly IEnumerable<INugetDependency> _dependencies;
    private readonly ILogger<ShellWindowViewModel> _logger;
    private readonly IDialogService _dialogService;

    public ShellWindowViewModel(
        CommandExecutor commandExecutor,
        IEnumerable<ICommandexCommand> commands,
        IEnumerable<INugetDependency> dependencies,
        ILogger<ShellWindowViewModel> logger,
        IDialogService dialogService,
        IVersionService versionService)
    {
        Version = $"{versionService.Version} ({versionService.Branch}:{versionService.Commit})";
        Title = $"Command Executor {Version}";
        _commandExecutor = commandExecutor;
        _commands = commands;
        _dependencies = dependencies;
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
    private ObservableCollection<CommandItem> _commandItems = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteActionCommand))]
    private CommandItem? _selectedCommand;

    private bool CanExecuteAction => SelectedCommand is not null;

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task ExecuteActionAsync()
    {
        var operation = await _commandExecutor.ExecuteAsync(SelectedCommand!);
        if (operation.Ok)
        {
            var command = operation.Result;
            var message = CommandReport.CreateReport(command);
            _logger.LogInformation("{CommandType} executed with result: {Result}", command.TypeName, message);
            _dialogService.ShowNotification(message);
            return;
        }

        _logger.LogError(operation.Error, operation.Error.Message);
        _dialogService.ShowError(operation.Error.Message);
    }

    [RelayCommand]
    private void ShowAbout()
    {
        _dialogService.ShowDialog<AboutDialog, AboutDialogResult>();
        //_dialogService.ShowNotification($"{Version}: Command Executor or Command Launcher. To run commands of any type for any purpose. For example, to execute a stored procedure or just to copy some files to some destination.");
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
                .And(x => x.DisplayName.ToLower().Contains((string)term))
                .Or(x => x.Description.ToLower().Contains((string)term))
                .Or(x => x.CopyrightInfo.ToLower().Contains((string)term))
                .Or(x => x.TypeName.ToLower().Contains((string)term))
                .Or(x => x.Version.ToLower().Contains((string)term));
        }

        var actionsList = _commands
            .Where(predicate.Compile())
            .Select(x => new CommandItem(x.TypeName, x.Version, x.DisplayName, x.Description))
            .ToList();

        CommandItems = new ObservableCollection<CommandItem>(actionsList);

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