namespace Calabonga.Commandex.Shell.Models;

/// <summary>
/// // Calabonga: Summary required (Catalog 2024-07-30 08:36) 
/// </summary>
public abstract class ItemBase
{
    public string Scope { get; set; } = null!;

    public string TypeName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Version { get; set; } = null!;

    public string[]? Tags { get; set; }
}