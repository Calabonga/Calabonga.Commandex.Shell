using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Zones;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Infrastructure.Messaging;
using Calabonga.Commandex.Shell.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Calabonga.Commandex.Shell.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase,
    IRecipient<LoginSuccessMessage>

{
    private readonly IZoneManager _zoneManager;
    private readonly CommandExecutor _commandExecutor;

    public ShellWindowViewModel(
        IZoneManager zoneManager,
        CommandExecutor commandExecutor)
    {
        Title = "Command Executor";
        _zoneManager = zoneManager;
        _commandExecutor = commandExecutor;

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

    #region property IsAuthenticated

    /// <summary>
    /// Indicated that user login into OAuth2.0 successful
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotAuthenticated))]
    private bool _isAuthenticated;

    #endregion

    #endregion

    #region Properties

    public bool IsNotAuthenticated => !IsAuthenticated;

    #endregion

    #region handlers

    public void Receive(LoginSuccessMessage message)
    {
        IsAuthenticated = true;
        Username = $"({message.Username})";
        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(Title));
        //LoadData();

        _zoneManager.ActivateZone<CommandListView, CommandListViewModel>("MainZone");
    }

    #endregion

    #region privates

    #region Initializations
    private void Subscriptions()
    {
        _commandExecutor.CommandPreparedSuccess += (_, _) => { IsBusy = false; };
        _commandExecutor.CommandPrepareStart += (_, _) => { IsBusy = true; };
        _commandExecutor.CommandPreparationFailed += (_, _) => { IsBusy = false; };

        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    #endregion

    #endregion
}
