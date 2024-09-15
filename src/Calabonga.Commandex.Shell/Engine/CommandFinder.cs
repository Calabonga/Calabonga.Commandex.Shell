using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.NugetDependencies;
using Calabonga.Commandex.Shell.Models;
using Calabonga.OperationResults;
using Calabonga.PredicatesBuilder;
using Calabonga.Wpf.AppDefinitions;
using Serilog;
using System.Collections.Immutable;
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
        // Calabonga: Refactoring required (CommandFinder 2024-09-15 08:10)
        var commandBaseTypes = FindAllAbstractCommandTypes().ToList();

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

                foreach (var commandType in commands)
                {
                    var typeName = "";
                    foreach (var type in commandBaseTypes.Where(type => IsAssignableToGenericType(commandType, type)))
                    {
                        typeName = GetNameWithoutGenericArity(type);
                        break;
                    }

                    Log.Logger.Debug("[{Command}] is type of {Type}", commandType.Name, typeName);
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

    /// <summary>
    /// Converts <see cref="ICommandexCommand"/>  to <see cref="CommandItem"/> for UI.
    /// Also, it can filter collection of found commands by search term.
    /// </summary>
    /// <param name="commands"></param>
    /// <param name="settingsReader"></param>
    /// <param name="searchTerm"></param>
    /// <returns><see cref="IReadOnlyList{T}"/> of the command items for UI</returns>
    internal static IReadOnlyList<CommandItem> ConvertToItems(IEnumerable<ICommandexCommand> commands, ISettingsReaderConfiguration settingsReader, string? searchTerm)
    {
        var predicate = PredicateBuilder.True<ICommandexCommand>().And(x => !string.IsNullOrEmpty(x.Version));

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var term = searchTerm.ToLower();
            predicate = predicate
                .And(x => x.DisplayName.ToLower().Contains(term))
                .Or(x => x.Description.ToLower().Contains(term))
                .Or(x => x.CopyrightInfo.ToLower().Contains(term))
                .Or(x => x.TypeName.ToLower().Contains(term))
                .Or(x => x.Version.ToLower().Contains(term));
        }

        var actionsList = commands
            .Where(predicate.Compile())
            .Select(x => new CommandItem(settingsReader.GetEnvironmentFileName(x.GetType()), x.TypeName, x.Version, x.DisplayName, x.Description, x.Tags))
            .ToList();

        return actionsList.ToImmutableList();
    }

    private static IEnumerable<Type> FindAllAbstractCommandTypes()
    {
        var typeCommand = typeof(ICommandexCommand);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => typeCommand.IsAssignableFrom(x) && x is { IsAbstract: true, IsClass: true });

        foreach (var type in types)
        {
            //do stuff
            yield return type;
        }
    }

    private static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = givenType.BaseType;
        return baseType != null && IsAssignableToGenericType(baseType, genericType);
    }

    private static bool AppDefinitionFindPredicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(AppDefinition).IsAssignableFrom(type);

    private static bool NugetDependencyFindPredicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(INugetDependency).IsAssignableFrom(type);

    private static bool CommandexPredicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(ICommandexCommand).IsAssignableFrom(type);

    private static string GetNameWithoutGenericArity(this Type t)
    {
        var name = t.Name;
        var index = name.IndexOf('`');
        return index == -1 ? name : name[..index];
    }
}

