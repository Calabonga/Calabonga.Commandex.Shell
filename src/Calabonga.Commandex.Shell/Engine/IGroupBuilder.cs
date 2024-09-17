using Calabonga.Commandex.Shell.Models;

namespace Calabonga.Commandex.Shell.Engine;

public interface IGroupBuilder
{
    /// <summary>
    /// Returns group with no tags defined. It will be used for command without any tags.
    /// </summary>
    /// <returns>default <see cref="CommandGroup"/></returns>
    CommandGroup? GetDefault();

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
    public CommandGroup? GetDefault() => GetGroups().FirstOrDefault(x => !x.Tags.Any());

    /// <summary>
    /// Returns items for hierarchical view for <see cref="CommandItem"/>
    /// </summary>
    /// <returns></returns>
    public List<CommandGroup> GetGroups()
    {
        var defaultGroup = new CommandGroup { Name = "Default", Tags = [] };

        var http = new CommandGroup { Name = "HttpClient Requests", Tags = ["http", "client"] };

        var noResult = new CommandGroup { Name = "No results", Tags = ["empty"] };
        var calabonga = new CommandGroup { Name = "No results", Tags = ["calabonga"] };

        return [defaultGroup, http, noResult, calabonga];
    }
}