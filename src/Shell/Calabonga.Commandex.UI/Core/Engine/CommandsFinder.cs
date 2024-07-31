using System.IO;
using System.Reflection;
using Calabonga.Wpf.AppDefinitions;
using Serilog;

namespace Calabonga.Commandex.Shell.Core.Engine;

/// <summary>
/// // Calabonga: Summary required (CommandsFinder 2024-07-29 04:06)
/// </summary>
internal static class CommandsFinder
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
                var s = Assembly.LoadFile(fileInfo.FullName);
                var modulesTypes = s.ExportedTypes.Where(Predicate).ToList();

                if (!modulesTypes.Any())
                {
                    throw new AppDefinitionsNotFoundException($"There are no any AppDefinition found in {fileInfo.FullName}");
                }

                types.AddRange(modulesTypes);
            }

            return types.ToArray();
        }
        catch (Exception exception)
        {
            Log.Error(exception, exception.Message);
            throw;
        }
    }

    private static bool Predicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(AppDefinition).IsAssignableFrom(type);
}