using Calabonga.Commandex.Shell.Models;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// // Calabonga: Summary required (DefaultGroupBuilder 2024-09-17 12:41)
/// </summary>
public class DefaultGroupBuilder : IGroupBuilder
{
    /// <summary>
    /// Returns group with no tags defined. It will be used for command without any tags.
    /// </summary>
    /// <returns>default <see cref="CommandGroup"/></returns>
    public CommandGroup GetDefault() => new() { Name = "Untagged", Description = "Group for untagged commandex command.", Tags = [] };

    /// <summary>
    /// Returns items for hierarchical view for <see cref="CommandItem"/>
    /// </summary>
    /// <returns></returns>
    public List<CommandGroup> GetGroups() =>
    [
        new() { Name = "Commands with HttpClient", Tags = ["http"], Description = "Commands with HttpClient using for request to remote services (API)" },
        new() { Name = "Empty commands type", Tags = ["empty"], Description = "Commands that is not result returned to Shell" },
        new() { Name = "Commands with result", Tags = ["result"], Description = "Commands with some result for the Shell" },
        new() { Name = "Commands open Dialog", Tags = ["dialog"], Description = "Commands that run into modal dialog window of the Shell" },
        new() { Name = "Command for Person entity", Tags = ["person"], Description = "Command for Person entity management" },
        new() { Name = "Wizard commands type", Tags = ["wizard"], Description = "Commands that in modal dialog shows a few steps in the wizard." },
        new() { Name = "Command with sharing the same generic Parameter", Tags = ["parameter"], Description = "Command for Person entity management" }
    ];
}