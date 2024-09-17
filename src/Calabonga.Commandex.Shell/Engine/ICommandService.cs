using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Shell.Models;

namespace Calabonga.Commandex.Shell.Engine;

public interface ICommandService
{
    IEnumerable<CommandItem> GetCommands(string? searchTerm);
}

public class CommandService : ICommandService
{
    private readonly IEnumerable<ICommandexCommand> _commands;
    private readonly ISettingsReaderConfiguration _settingsReader;

    public CommandService(IEnumerable<ICommandexCommand> commands,
        ISettingsReaderConfiguration settingsReader)
    {
        _commands = commands;
        _settingsReader = settingsReader;
    }
    public IEnumerable<CommandItem> GetCommands(string? searchTerm) => CommandFinder.ConvertToItems(_commands, _settingsReader, searchTerm);
}