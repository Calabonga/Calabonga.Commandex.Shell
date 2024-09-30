using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Shell.Models;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// <see cref="CommandItem"/> service
/// </summary>
public interface ICommandService
{
    /// <summary>
    /// Returns a collection of the <see cref="CommandItem"/> created from <see cref="ICommandexCommand"/> items.
    /// </summary>
    /// <param name="viewType">View type selected</param>
    /// <param name="searchTerm">filter term</param>
    /// <returns></returns>
    IEnumerable<CommandItem> GetCommands(CommandViewType viewType, string? searchTerm);
}
/// <summary>
/// <see cref="CommandItem"/> service
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

    /// <summary>
    /// Returns a collection of the <see cref="CommandItem"/> created from <see cref="ICommandexCommand"/> items.
    /// </summary>
    /// <param name="viewType">View type selected</param>
    /// <param name="searchTerm">filter term</param>
    /// <returns></returns>
    public IEnumerable<CommandItem> GetCommands(CommandViewType viewType, string? searchTerm)
        => viewType switch
        {
            CommandViewType.DefaultList
                or CommandViewType.BriefList
                or CommandViewType.ExtendedList => CommandFinder.ConvertToItems(_commands, _settingsReader, searchTerm),

            CommandViewType.DefaultHierarchical
                or CommandViewType.BriefHierarchical
                or CommandViewType.ExtendedHierarchical => CommandFinder.ConvertToGroupedItems(_groupBuilder, _commands, _settingsReader, searchTerm),

            _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null)
        };
}