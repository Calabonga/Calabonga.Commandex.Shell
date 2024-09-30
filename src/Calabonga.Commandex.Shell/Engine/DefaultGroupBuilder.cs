using Calabonga.Commandex.Shell.Models;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// Implementation of the group builder for demo purposes only. This is something like a catalog for commands.
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
        new() { Name = "Empty commands type", Tags = ["empty"], Description = "Commands that is not result returned to Shell" },
        new()
        {
            Name = "Commands with Result", Tags = ["result"], Description = "Commands with some result for the Shell",
            SubGroups = [new() { Name = "Commands with HttpClient Result", Tags = ["http"], Description = "Commands with HttpClient using for request to remote services (API)" }]
        },
        new()
        {
            Name = "Commands open Dialog", Tags = ["dialog"], Description = "Commands that run into modal dialog window of the Shell",
            SubGroups = [new() { Name = "Wizard commands type", Tags = ["wizard"], Description = "Commands that in modal dialog shows a few steps in the wizard." }]
        },
        new() { Name = "Command for Person entity", Tags = ["person"], Description = "Command for Person entity management" },
        new() { Name = "Command with sharing the same generic Parameter", Tags = ["parameter"], Description = "Command for Person entity management" }
    ];
}