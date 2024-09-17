using Calabonga.Commandex.Engine.Base;

namespace Calabonga.Commandex.Shell.Models;

/// <summary>
/// Represents <see cref="ICommandexCommand" /> as the item to show on the UI.
/// </summary>
public sealed class CommandItem : ItemBase
{
    public CommandItem(string scope, string type, string version, string name, string description, string[]? tags, List<CommandItem>? items)
    {
        Scope = scope;
        TypeName = type;
        Version = version;
        Name = name;
        Description = description;
        Tags = tags;
        if (items is not null)
        {
            SetItems(items);
        }
    }

    public List<CommandItem> Items => _items;

    void SetItems(List<CommandItem> items) => _items = items;

    private List<CommandItem> _items = [];

    public void AddCommand(CommandItem item) => _items.Add(item);
}