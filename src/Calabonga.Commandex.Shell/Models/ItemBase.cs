using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Shell.Engine;

namespace Calabonga.Commandex.Shell.Models;

/// <summary>
/// Represents <see cref="ICommandexCommand"/> item in UI for current application.
/// </summary>
public abstract class ItemBase
{
    /// <summary>
    /// Base scope for command
    /// </summary>
    public string Scope { get; set; } = null!;

    /// <summary>
    /// Command type name
    /// </summary>
    public string TypeName { get; set; } = null!;

    /// <summary>
    /// Small description
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// The name of the command
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Actual version
    /// </summary>
    public string Version { get; set; } = null!;

    /// <summary>
    /// Tags that should be mapped with <see cref="IGroupBuilder"/> catalog presentation
    /// </summary>
    public string[]? Tags { get; set; }
}