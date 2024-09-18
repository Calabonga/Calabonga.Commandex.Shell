using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Models;

namespace Calabonga.Commandex.Shell.Tests;

public class TestGroupBuilder : IGroupBuilder
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
        new() { Name = "One", Tags = ["one"], Description = "Description" },
        new() { Name = "Two", Tags = ["two"], Description = "Description" },
        new() { Name = "Three", Tags = ["three"], Description = "Description" },
        new() { Name = "Four", Tags = ["one","three"], Description = "Description" },
        new() { Name = "Five", Tags = ["five"], Description = "Description" },
        new() { Name = "Six", Tags = ["six"], Description = "Description",
            SubGroups =
                [
                    new() {Name = "Seven", Tags = ["seven"], Description = "Description"}
                ]
        },
    ];
}