using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Models;
using Calabonga.Commandex.Shell.ViewModels.Dialogs;
using Calabonga.Commandex.Shell.Views.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace Calabonga.Commandex.Shell.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase
{
    private readonly CommandExecutor _commandExecutor;
    private readonly IConfigurationFinder _configurationFinder;
    private readonly IEnumerable<ICommandexCommand> _commands;
    private readonly ILogger<ShellWindowViewModel> _logger;
    private readonly IDialogService _dialogService;
    private readonly ISettingsReaderConfiguration _settingsReader;

    public ShellWindowViewModel(
        CommandExecutor commandExecutor,
        IConfigurationFinder configurationFinder,
        IEnumerable<ICommandexCommand> commands,
        ILogger<ShellWindowViewModel> logger,
        IDialogService dialogService,
        ISettingsReaderConfiguration settingsReader)
    {
        Title = "CommandEx - Command Executor";
        _commandExecutor = commandExecutor;
        _configurationFinder = configurationFinder;
        _commands = commands;
        _logger = logger;
        _dialogService = dialogService;
        _settingsReader = settingsReader;

        _commandExecutor.CommandPreparedSuccess += (_, _) => { IsBusy = false; };
        _commandExecutor.CommandPrepareStart += (_, _) => { IsBusy = true; };
        _commandExecutor.CommandPreparationFailed += (_, _) => { IsBusy = false; };
    }

    [ObservableProperty]
    private string? _searchTerm;

    [ObservableProperty]
    private bool _isFindEnabled = App.Current.Settings.ShowSearchPanelOnStartup;

    [ObservableProperty]
    private ObservableCollection<CommandItem> _commandItems = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteActionCommand))]
    [NotifyCanExecuteChangedFor(nameof(OpenCommandConfigurationCommand))]
    private CommandItem? _selectedCommand;

    private bool CanExecuteAction => SelectedCommand is not null;

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task ExecuteActionAsync()
    {
        var operation = await _commandExecutor.ExecuteAsync(SelectedCommand!);

        if (operation.Ok)
        {
            if (!operation.Result.IsPushToShellEnabled)
            {
                return;
            }

            var command = operation.Result;
            var message = CommandReport.CreateReport(command);
            _logger.LogInformation("{CommandType} executed with result: {Result}", command.TypeName, message);
            _dialogService.ShowNotification(message);
            return;
        }

        _logger.LogError(operation.Error, operation.Error.Message);
        _dialogService.ShowError(operation.Error.Message);
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private void OpenCommandConfiguration() => _configurationFinder.CommandConfiguration(SelectedCommand!.Scope);

    [RelayCommand]
    private void ShowAbout() => _dialogService.ShowDialog<AboutDialog, AboutDialogResult>();

    [RelayCommand]
    private void OpenLogsFolder()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        if (!Path.Exists(path))
        {
            _dialogService.ShowNotification($"Folder not found: {path}");
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true,
            Verb = "open"
        });
    }

    [RelayCommand]
    private void LoadData()
    {
        IsBusy = true;

        FindCommandsInContainer();

        IsBusy = false;
    }

    private void FindCommandsInContainer()
        => CommandItems = new ObservableCollection<CommandItem>(CommandFinder.ConvertToItems(_commands, _settingsReader, SearchTerm));

    partial void OnSearchTermChanged(string? value) => FindCommandsInContainer();

}