using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.Shell.Extensions;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Finds and Adds *.dll-modules (plugins) with <see cref="ICommandexCommand"/> command implementations
    /// </summary>
    /// <param name="source"></param>
    internal static void AddModulesDefinitions(this IServiceCollection source)
    {
        var types = new List<Type>() { typeof(App) };
        var findOperation = CommandFinder.Find(App.Current.Settings.CommandsPath);
        if (findOperation.Ok)
        {
            types.AddRange(findOperation.Result);
        }

        source.AddDefinitions(types.ToArray());
    }
}