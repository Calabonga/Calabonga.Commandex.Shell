using Calabonga.Commandex.Shell.Models;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// Catalog builder interface
/// </summary>
public interface IGroupBuilder
{
    /// <summary>
    /// Returns group with no tags defined. It will be used for command without any tags.
    /// </summary>
    /// <returns>default <see cref="CommandGroup"/></returns>
    CommandGroup GetDefault();

    /// <summary>
    /// Returns items for hierarchical view for <see cref="CommandItem"/>
    /// </summary>
    /// <returns></returns>
    List<CommandGroup> GetGroups();
}