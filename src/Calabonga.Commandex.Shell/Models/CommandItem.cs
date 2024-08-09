namespace Calabonga.Commandex.Shell.Models;

/// <summary>
/// Calabonga: Summary required (CommandItem 2024-07-30 08:36)
/// </summary>
public sealed class CommandItem : ItemBase
{
    public CommandItem(string scope, string type, string version, string name, string description)
    {
        Scope = scope;
        TypeName = type;
        Version = version;
        Name = name;
        Description = description;
    }
}