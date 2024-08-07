using Calabonga.Commandex.Shell.Engine;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.Shell.Extensions;

/// <summary>
/// // Calabonga: Summary required (DependencyContainer 2024-08-07 06:40) 
/// </summary>
internal static class ServiceCollectionExtensions
{
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