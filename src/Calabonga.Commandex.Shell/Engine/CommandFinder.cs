using Calabonga.Commandex.Engine.Commands;
using Calabonga.Commandex.Engine.NugetDependencies;
using Calabonga.OperationResults;
using Calabonga.Wpf.AppDefinitions;
using Serilog;
using System.IO;
using System.Reflection;

namespace Calabonga.Commandex.Shell.Engine;

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
    internal static Operation<Type[], Exception> Find(string commandexFolderPath)
    {
        try
        {
            if (!Directory.Exists(commandexFolderPath))
            {
                Directory.CreateDirectory(commandexFolderPath);
                Log.Information("Folder for modules [{FolderName}] not found. It has been created.", commandexFolderPath);
                return Operation.Result(new Type[] { });
            }

            var types = new List<Type>();

            var modulesDirectory = new DirectoryInfo(commandexFolderPath);
            var files = modulesDirectory.GetFiles("*.dll");
            if (!files.Any())
            {
                Log.Information("No modules were found in folder {FolderName}", commandexFolderPath);
                return Operation.Result(new Type[] { });
            }

            foreach (var fileInfo in files)
            {
                var assembly = Assembly.LoadFrom(fileInfo.FullName) ?? throw new ArgumentNullException(nameof(commandexFolderPath));

                var exportedTypes = assembly.GetExportedTypes();
                var modulesTypes = exportedTypes.Where(AppDefinitionFindPredicate).ToList();
                if (!modulesTypes.Any())
                {
                    var error = new AppDefinitionsNotFoundException($"There are no any AppDefinition found in {fileInfo.FullName}");
                    Log.Logger.Error(error, error.Message);
                }

                var commands = exportedTypes.Where(CommandexPredicate).ToList();
                if (!commands.Any())
                {
                    var error = new AppDefinitionsNotFoundException($"AppDefinition found in {fileInfo.FullName}, but there are no ICommandexCommand implementation were found");
                    Log.Logger.Error(error, error.Message);
                }

                var nugetDependencies = exportedTypes.Where(NugetDependencyFindPredicate).ToList();
                Log.Logger.Information("NugetDependencies for {File} found {Count}", fileInfo.Name, nugetDependencies.Count);


                types.AddRange(modulesTypes);
            }

            return types.ToArray();
        }
        catch (Exception exception)
        {
            Log.Error(exception, exception.Message);
            App.Current.LastException = exception;
            return Operation.Error(exception);
        }
    }

    private static bool AppDefinitionFindPredicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(AppDefinition).IsAssignableFrom(type);

    private static bool NugetDependencyFindPredicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(INugetDependency).IsAssignableFrom(type);

    private static bool CommandexPredicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(ICommandexCommand).IsAssignableFrom(type);
}

