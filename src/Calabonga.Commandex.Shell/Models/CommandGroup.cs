namespace Calabonga.Commandex.Shell.Models;

/// <summary>
/// Hierarchical item for command presentation
/// </summary>
public sealed class CommandGroup : ICommandItem
{
    public required string Name { get; set; } = null!;

    public required List<string> Tags { get; init; } = [];

    public CommandGroup? Parent { get; set; }

    public bool HasItems => _items.Any();

    private List<CommandItem> _items = [];

    public List<CommandItem> Items => _items;

    void SetItems(List<CommandItem> items) => _items = items;

    public void AddCommand(CommandItem item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
        }
    }

    public string Scope => Name;

    public string TypeName => Name;
}

public interface ICommandItem
{
    string Scope { get; }
    string TypeName { get; }
}