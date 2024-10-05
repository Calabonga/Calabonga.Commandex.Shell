using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Shell.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Calabonga.Commandex.Shell.ViewModels.UserControls;

public sealed partial class LoginControlViewModel : ViewModelWithValidatorBase, IDisposable
{
    private readonly IDialogService _dialogService;
    private readonly IAuthenticationService _authenticationService;
    public LoginControlViewModel(
        IDialogService dialogService,
        IAuthenticationService authenticationService)
    {
        ErrorsChanged += OnErrorsChanged;
        _dialogService = dialogService;
        _authenticationService = authenticationService;
        Title = "Log in";
    }
    #region properties

    #region property ErrorList
    /// <summary>
    /// Errors list from validation
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string>? _errorList;
    #endregion

    #region property Username
    /// <summary>
    /// Login property
    /// </summary>
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [NotifyPropertyChangedFor(nameof(CanUserLogin))]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string? _username;
    #endregion

    #region property Password
    /// <summary>
    /// Password property
    /// </summary>
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [NotifyPropertyChangedFor(nameof(CanUserLogin))]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string? _password;
    #endregion

    #region property CanLogin
    /// <summary>
    /// Can login flag
    /// </summary>
    public bool CanUserLogin => ErrorList is null || !ErrorList.Any();
    #endregion

    #endregion

    #region commands

    #region LoginCommand
    [RelayCommand(CanExecute = nameof(CanUserLogin))]
    private async Task Login()
    {
        IsBusy = true;
        var password = new NetworkCredential(string.Empty, Password).Password;
        var loginResult = await _authenticationService.AuthenticateUser(Username!, password, "commandex");
        if (loginResult.Ok)
        {
            IsBusy = false;
            return;
        }
        IsBusy = false;
        _dialogService.ShowError(loginResult.Error);
    }
    #endregion

    #endregion
    private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        var errors = GetErrors().Select(x => x.ErrorMessage).ToList();
        ErrorList = new ObservableCollection<string>(errors!);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose() => ErrorsChanged -= OnErrorsChanged;
}
