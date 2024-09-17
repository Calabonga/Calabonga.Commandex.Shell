namespace Calabonga.Commandex.Shell.Models;

/// <summary>
/// Hierarchical item for command presentation
/// </summary>
public sealed class CommandGroup
{
    public required string Name { get; set; } = null!;

    public required List<string> Tags { get; init; } = [];

    public CommandGroup? Parent { get; set; }

    public bool HasItems => Items.Any();

    public List<CommandItem> Items { get; private set; } = [];

    void SetItems(List<CommandItem> items) => Items = items;
}