using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Shell.Models;

namespace Calabonga.Commandex.Shell.Engine;

public interface ICommandService
{
    IEnumerable<CommandItem> GetCommands(CommandViewType viewType, string? searchTerm);
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
    public IEnumerable<CommandItem> GetCommands(CommandViewType viewType, string? searchTerm)
        => viewType switch
        {
            CommandViewType.DefaultList => CommandFinder.ConvertToItems(_commands, _settingsReader, searchTerm),
            CommandViewType.BriefList => CommandFinder.ConvertToItems(_commands, _settingsReader, searchTerm),
            CommandViewType.ExtendedList => CommandFinder.ConvertToItems(_commands, _settingsReader, searchTerm),
            CommandViewType.Hierarchical => CommandFinder.ConvertToItems(_commands, _settingsReader, searchTerm),
            _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null)
        };
}