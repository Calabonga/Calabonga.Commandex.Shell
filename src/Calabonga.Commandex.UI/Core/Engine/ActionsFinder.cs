using System.IO;
using System.Reflection;
using Calabonga.Wpf.AppDefinitions;

namespace Calabonga.Commandex.UI.Core.Engine;

/// <summary>
/// // Calabonga: Summary required (ActionsFinder 2024-07-29 04:06)
/// </summary>
internal static class ActionsFinder
{
    internal static Type[] Find(string actionsFolderPath)
    {
        try
        {
            if (!Directory.Exists(actionsFolderPath))
            {
                return [];
            }

            var types = new List<Type>();

            var modulesDirectory = new DirectoryInfo(actionsFolderPath);
            var files = modulesDirectory.GetFiles("*.dll");
            if (!files.Any())
            {

                return [];
            }

            foreach (var fileInfo in files)
            {
                var modulesTypes = Assembly.LoadFile(fileInfo.FullName).ExportedTypes.Where(Predicate);
                types.AddRange(modulesTypes);
            }

            return types.ToArray();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    private static bool Predicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(AppDefinition).IsAssignableFrom(type);
}