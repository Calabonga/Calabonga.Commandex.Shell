namespace Calabonga.Commandex.Shell.Models;

/// <summary>
/// DefaultHierarchical item for command presentation
/// </summary>
public sealed class CommandGroup
{
    private readonly List<CommandItem> _commandItems = [];

    public required string Name { get; set; } = null!;

    public required string Description { get; set; } = null!;

    public required List<string> Tags { get; init; } = [];

    public List<CommandItem> CommandItems => _commandItems;

    public List<CommandGroup> SubGroups { get; set; } = [];

    public void AddGroup(IEnumerable<CommandGroup> items) => SubGroups.AddRange(items);

    public void AddCommand(CommandItem item)
    {
        if (_commandItems.Contains(item))
        {
            return;
        }

        _commandItems.Add(item);
    }
}
