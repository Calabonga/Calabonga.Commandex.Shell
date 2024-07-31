using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Commands;
using CommunityToolkit.Mvvm.Input;

namespace Develop.Command
{
    public partial class MainWindowsViewModel : ViewModelBase
    {
        private readonly ICommandexCommand _command;

        public MainWindowsViewModel(ICommandexCommand command)
        {
            _command = command;
        }

        [RelayCommand]
        private async Task ExecuteAsync()
        {
            await _command.ShowDialogAsync();
        }
    }
}
