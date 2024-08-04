using System.IO;
using System.Reflection;
using Calabonga.Wpf.AppDefinitions;
using Serilog;

namespace Calabonga.Commandex.Shell.Core.Engine;

/// <summary>
/// // Calabonga: Summary required (CommandFinder 2024-07-29 04:06)
/// </summary>
internal static class CommandFinder
{
    /// <summary>
    /// // Calabonga: Summary required (CommandFinder 2024-08-03 08:37)
    /// </summary>
    /// <param name="commandexFolderPath"></param>
    /// <exception cref="AppDefinitionsNotFoundException"></exception>
    internal static Type[] Find(string commandexFolderPath)
    {
        try
        {
            if (!Directory.Exists(commandexFolderPath))
            {
                return [];
            }

            var types = new List<Type>();

            var modulesDirectory = new DirectoryInfo(commandexFolderPath);
            var files = modulesDirectory.GetFiles("*.dll");
            if (!files.Any())
            {
                return [];
            }

            foreach (var fileInfo in files)
            {
                var assembly = Assembly.LoadFile(fileInfo.FullName) ?? throw new ArgumentNullException(nameof(commandexFolderPath));

                var referencedAssemblies = assembly.GetReferencedAssemblies();

                var modulesTypes = assembly.ExportedTypes.Where(Predicate).ToList();

                if (!modulesTypes.Any())
                {
                    throw new AppDefinitionsNotFoundException($"There are no any AppDefinition found in {fileInfo.FullName}");
                }

                // Calabonga: Summary required (CommandFinder 2024-08-04 06:47)
                //foreach (var modulesType in modulesTypes)
                //{
                //    var instance = Activator.CreateInstance(modulesType);
                //    if (instance is not null)
                //    {
                //        if (instance is AppDefinition commandex)
                //        {
                //            var nugetDependencies = commandex.RequiredDependencies();
                //            foreach (var nugetDependency in nugetDependencies)
                //            {
                //                if (!referencedAssemblies.Any())
                //                {
                //                    throw new AppDefinitionsNotFoundException($"{nameof(NugetDependency)} found, but there are no any referenced assemblies");
                //                }

                //                if (!referencedAssemblies.Select(x => x.Name).Contains(nugetDependency.Name))
                //                {
                //                    throw new AppDefinitionsNotFoundException($"Required {nameof(NugetDependency)} does not contains in referenced assemblies");
                //                }

                //            }
                //        }
                //    }
                //}

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

