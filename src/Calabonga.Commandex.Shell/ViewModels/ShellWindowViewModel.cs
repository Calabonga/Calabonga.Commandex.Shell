using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Infrastructure.Messaging;
using Calabonga.Commandex.Shell.Models;
using Calabonga.Commandex.Shell.ViewModels.Dialogs;
using Calabonga.Commandex.Shell.Views.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Calabonga.Commandex.Shell.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase, IRecipient<LoginSuccessMessage>
{
    private readonly IResultProcessor _resultProcessor;
    private readonly ICommandService _commandService;
    private readonly CommandExecutor _commandExecutor;
    private readonly IConfigurationFinder _configurationFinder;
    private readonly ILogger<ShellWindowViewModel> _logger;
    private readonly IDialogService _dialogService;

    public ShellWindowViewModel(
        IResultProcessor resultProcessor,
        ICommandService commandService,
        IAppSettings appSettings,
        CommandExecutor commandExecutor,
        IConfigurationFinder configurationFinder,
        ILogger<ShellWindowViewModel> logger,
        IDialogService dialogService)
    {
        Title = "Command Executor";
        _resultProcessor = resultProcessor;
        _commandService = commandService;
        _commandExecutor = commandExecutor;
        _configurationFinder = configurationFinder;
        _logger = logger;
        _dialogService = dialogService;

        ApplyViewTemplate(appSettings);

        Subscriptions();
    }

    #region Observable Properties

    #region property Username

    /// <summary>
    /// UserName logged in
    /// </summary>
    [ObservableProperty]
    private string _username;

    #endregion

    #region property ListViewName

    /// <summary>
    /// ListView Selected view
    /// </summary>
    [ObservableProperty]
    private string _listViewName;

    #endregion

    #region property IsAuthenticated

    /// <summary>
    /// Indicated that user login into OAuth2.0 successful
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotAuthenticated))]
    private bool _isAuthenticated;

    #endregion

    #region property CommandItemDataTemplate

    [ObservableProperty]
    private DataTemplate _commandItemDataTemplate;
    #endregion

    #region property SearchTerm
    [ObservableProperty]
    private string? _searchTerm;
    #endregion

    #region property IsFindEnabled
    [ObservableProperty]
    private bool _isFindEnabled = App.Current.Settings.ShowSearchPanelOnStartup;
    #endregion

    #region property CommandItems

    [ObservableProperty]
    private ObservableCollection<CommandItem> _commandItems = new();
    #endregion

    #region property SelectedCommand

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteActionCommand))]
    [NotifyCanExecuteChangedFor(nameof(OpenCommandConfigurationCommand))]
    [NotifyPropertyChangedFor(nameof(CanExecuteAction))]
    private CommandItem? _selectedCommand;

    #endregion

    #endregion

    #region Properties

    public bool IsNotAuthenticated => !IsAuthenticated;

    public bool CanExecuteAction => SelectedCommand is not null && SelectedCommand!.TypeName != nameof(CommandGroup);

    #endregion

    #region Commands

    #region command ToggleSearchBarVisibilityCommand
    [RelayCommand]
    private void ToggleSearchBarVisibility() => IsFindEnabled = !IsFindEnabled;

    #endregion

    #region command ExecuteActionCommand

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

            _resultProcessor.ProcessCommand(operation.Result);

            return;
        }

        _logger.LogError(operation.Error, operation.Error.Message);
        _dialogService.ShowError(operation.Error.Message);

    }
    #endregion

    #region command OpenCommandConfigurationCommand
    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private void OpenCommandConfiguration() => _configurationFinder.OpenOrCreateCommandConfigurationFile(SelectedCommand!.Scope);

    #endregion

    #region command ShowAboutCommand

    [RelayCommand]
    private void ShowAbout() => _dialogService.ShowDialog<AboutDialog, AboutViewModel>();
    #endregion

    #region command OpenLogsFolderCommand

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
    #endregion

    #region command SwitchViewCommand

    [RelayCommand]
    private void SwitchView(CommandViewType viewType)
    {
        ListViewName = viewType.ToString();
        CommandItems = new ObservableCollection<CommandItem>(_commandService.GetCommands(viewType, SearchTerm));
        var result = App.Current.TryFindResource(CurrentAppSettings.GetViewResourceName(viewType.ToString()));
        CommandItemDataTemplate = (DataTemplate)result;
    }
    #endregion

    #endregion

    #region Subscriptions

    public void Receive(LoginSuccessMessage message)
    {
        IsAuthenticated = true;
        Username = $"({message.Username})";
        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(Title));
        LoadData();
    }
    #endregion

    #region Privates

    partial void OnSearchTermChanged(string? _) => LoadData();

    private void LoadData()
    {
        if (!IsAuthenticated)
        {
            return;
        }

        IsBusy = true;
        var viewType = Enum.Parse<CommandViewType>(ListViewName);
        CommandItems = new ObservableCollection<CommandItem>(_commandService.GetCommands(viewType, SearchTerm));
        IsBusy = false;
    }

    #region Initializations

    private void ApplyViewTemplate(IAppSettings appSettings)
    {
        var view = ((CurrentAppSettings)appSettings).DefaultViewName;
        var result = App.Current.TryFindResource(CurrentAppSettings.GetViewResourceName(view));
        ListViewName = view;
        CommandItemDataTemplate = (DataTemplate)result;
    }

    private void Subscriptions()
    {
        _commandExecutor.CommandPreparedSuccess += (_, _) => { IsBusy = false; };
        _commandExecutor.CommandPrepareStart += (_, _) => { IsBusy = true; };
        _commandExecutor.CommandPreparationFailed += (_, _) => { IsBusy = false; };

        WeakReferenceMessenger.Default.Register(this);
    }

    #endregion

    #endregion
}
