namespace Calabonga.Commandex.UI.Models;

/// <summary>
/// // Calabonga: Summary required (Catalog 2024-07-28 05:25)
/// </summary>
public sealed class ActionItem : ItemBase
{
    public ActionItem(string type, string version, string name, string description)
    {
        TypeName = type;
        Version = version;
        Name = name;
        Description = description;
    }
}

/// <summary>
/// // Calabonga: Summary required (Catalog 2024-07-28 05:25)
/// </summary>
public abstract class ItemBase
{
    public string Description { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string TypeName { get; set; } = null!;

    public string Version { get; set; } = null!;
}