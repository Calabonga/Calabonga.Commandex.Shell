using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Settings;
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
using System.Windows;

namespace Calabonga.Commandex.Shell.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase
{
    private readonly ICommandService _commandService;
    private readonly CommandExecutor _commandExecutor;
    private readonly IConfigurationFinder _configurationFinder;
    private readonly ILogger<ShellWindowViewModel> _logger;
    private readonly IDialogService _dialogService;

    public ShellWindowViewModel(
        ICommandService commandService,
        IAppSettings appSettings,
        CommandExecutor commandExecutor,
        IConfigurationFinder configurationFinder,
        ILogger<ShellWindowViewModel> logger,
        IDialogService dialogService)
    {
        Title = "Command Executor";
        _commandService = commandService;
        _commandExecutor = commandExecutor;
        _configurationFinder = configurationFinder;
        _logger = logger;
        _dialogService = dialogService;

        var view = ((CurrentAppSettings)appSettings).DefaultViewName;
        var result = App.Current.TryFindResource(CurrentAppSettings.GetViewResourceName(view));
        ListViewName = view;
        CommandItemDataTemplate = (DataTemplate)result;

        _commandExecutor.CommandPreparedSuccess += (_, _) => { IsBusy = false; };
        _commandExecutor.CommandPrepareStart += (_, _) => { IsBusy = true; };
        _commandExecutor.CommandPreparationFailed += (_, _) => { IsBusy = false; };
    }

    #region Observable Properties

    #region property ListViewName

    /// <summary>
    /// ListView Selected view
    /// </summary>
    [ObservableProperty]
    private string _listViewName;

    #endregion

    [ObservableProperty]
    private DataTemplate _commandItemDataTemplate;

    [ObservableProperty]
    private string? _searchTerm;

    [ObservableProperty]
    private bool _isFindEnabled = App.Current.Settings.ShowSearchPanelOnStartup;

    [ObservableProperty]
    private ObservableCollection<CommandItem> _commandItems = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteActionCommand))]
    [NotifyCanExecuteChangedFor(nameof(OpenCommandConfigurationCommand))]
    [NotifyPropertyChangedFor(nameof(CanExecuteAction))]
    private CommandItem? _selectedCommand;

    #endregion

    #region Properties
    public bool CanExecuteAction => SelectedCommand is not null && SelectedCommand!.TypeName != nameof(CommandGroup);
    #endregion

    #region Commands

    [RelayCommand]
    private void ToggleSearchBarVisibility() => IsFindEnabled = !IsFindEnabled;

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task ExecuteActionAsync()
    {
        var operation = await _commandExecutor.ExecuteAsync(SelectedCommand!);

        if (operation.Ok)
        {
            if (!operation.Result.IsPushToShellEnabled)
            {
                IsBusy = false;
                return;
            }

            var command = operation.Result;
            var message = CommandReport.CreateReport(command);
            _logger.LogInformation("{CommandType} executed with result: {Result}", command.TypeName, message);
            IsBusy = false;
            _dialogService.ShowNotification(message);
            return;
        }

        _logger.LogError(operation.Error, operation.Error.Message);
        _dialogService.ShowError(operation.Error.Message);

    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private void OpenCommandConfiguration() => _configurationFinder.OpenOrCreateCommandConfigurationFile(SelectedCommand!.Scope);

    [RelayCommand]
    private void ShowAbout() => _dialogService.ShowDialog<AboutDialog, AboutViewModel>();

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
        var viewType = Enum.Parse<CommandViewType>(ListViewName);
        CommandItems = new ObservableCollection<CommandItem>(_commandService.GetCommands(viewType, SearchTerm));
        IsBusy = false;
    }

    [RelayCommand]
    private void SwitchView(CommandViewType viewType)
    {
        ListViewName = viewType.ToString();
        CommandItems = new ObservableCollection<CommandItem>(_commandService.GetCommands(viewType, SearchTerm));
        var result = App.Current.TryFindResource(CurrentAppSettings.GetViewResourceName(viewType.ToString()));
        CommandItemDataTemplate = (DataTemplate)result;
    }

    #endregion

    partial void OnSearchTermChanged(string? value) => LoadData();
}