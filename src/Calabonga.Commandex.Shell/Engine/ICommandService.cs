using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Shell.Models;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// // Calabonga: Summary required (ICommandService 2024-09-17 06:29)
/// </summary>
public interface ICommandService
{
    /// <summary>
    /// // Calabonga: Summary required (ICommandService 2024-09-17 06:29)
    /// </summary>
    /// <param name="viewType"></param>
    /// <param name="searchTerm"></param>
    /// <returns></returns>
    IEnumerable<CommandItem> GetCommands(CommandViewType viewType, string? searchTerm);
}

/// <summary>
/// // Calabonga: Summary required (ICommandService 2024-09-17 06:30)
/// </summary>
public class CommandService : ICommandService
{
    private readonly IGroupBuilder _groupBuilder;
    private readonly IEnumerable<ICommandexCommand> _commands;
    private readonly ISettingsReaderConfiguration _settingsReader;

    public CommandService(
        IGroupBuilder groupBuilder,
        IEnumerable<ICommandexCommand> commands,
        ISettingsReaderConfiguration settingsReader)
    {
        _groupBuilder = groupBuilder;
        _commands = commands;
        _settingsReader = settingsReader;
    }
    public IEnumerable<CommandItem> GetCommands(CommandViewType viewType, string? searchTerm)
        => viewType switch
        {
            CommandViewType.DefaultList => CommandFinder.ConvertToItems(_commands, _settingsReader, searchTerm),
            CommandViewType.BriefList => CommandFinder.ConvertToItems(_commands, _settingsReader, searchTerm),
            CommandViewType.ExtendedList => CommandFinder.ConvertToItems(_commands, _settingsReader, searchTerm),
            CommandViewType.Hierarchical => CommandFinder.ConvertToGroupedItems(_groupBuilder, _commands, _settingsReader, searchTerm),
            _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null)
        };
}