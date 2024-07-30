namespace Calabonga.Commandex.UI.Models;

/// <summary>
/// Calabonga: Summary required (CommandItem 2024-07-30 08:36)
/// </summary>
public sealed class CommandItem : ItemBase
{
    public CommandItem(string type, string version, string name, string description)
    {
        TypeName = type;
        Version = version;
        Name = name;
        Description = description;
    }
}