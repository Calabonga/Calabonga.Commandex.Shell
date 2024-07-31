using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Commands;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Developer.Shell.UI
{
    public partial class MainWindowsViewModel : ViewModelBase
    {
        private readonly ICommandexCommand _command;
        private readonly IDialogService _dialogService;

        public MainWindowsViewModel(ICommandexCommand command, IDialogService dialogService)
        {
            _command = command;
            _dialogService = dialogService;
        }

        [RelayCommand]
        private async Task ExecuteAsync()
        {
            await _command.ShowDialogAsync();

            var message = _command.GetResult();

            _dialogService.ShowNotification(message?.ToString() ?? "Error");
        }
    }
}
