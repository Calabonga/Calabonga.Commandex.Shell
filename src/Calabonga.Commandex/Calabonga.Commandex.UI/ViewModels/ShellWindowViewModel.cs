using Calabonga.Commandex.UI.Core;
using Calabonga.Commandex.UI.Core.Dialogs;
using Calabonga.Commandex.UI.Core.Services;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Commandex.UI.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly IVersionService _versionService;

    public ShellWindowViewModel(IDialogService dialogService, IVersionService versionService)
    {
        Title = "Command Executor";
        _dialogService = dialogService;
        _versionService = versionService;
    }

    [RelayCommand]
    private void ShowAbout()
    {
        var version = $"{_versionService.Branch}:{_versionService.Version}:{_versionService.Commit}";
        _dialogService.ShowDialog($"{version}:Command Executor or Command Launcher. To run commands of any type for any purpose. For example, to execute a stored procedure or just to copy some files to some destination.", LogLevel.Notification);
    }
}