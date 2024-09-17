using Calabonga.Commandex.Shell.Models;

namespace Calabonga.Commandex.Shell.Engine;

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
    public List<CommandGroup> GetGroups()
    {
        var http = new CommandGroup { Name = "HttpClient Requests", Description = "Commands with HttpClient using for request to remote services (API)", Tags = ["http", "client"] };
        var empty = new CommandGroup { Name = "Empty", Description = "Commands that is not result returned to Shell", Tags = ["empty"] };
        var calabonga = new CommandGroup { Name = "Simplest commands", Description = "The simplest command type for demo purposes", Tags = ["demo"] };
        var subGroup = new CommandGroup { Name = "Samples", Description = "", Tags = [] };
        subGroup.AddGroup([calabonga]);
        return [http, empty, subGroup];
    }
}