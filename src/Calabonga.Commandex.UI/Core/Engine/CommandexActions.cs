using System.IO;
using System.Reflection;
using Calabonga.Wpf.AppDefinitions;

namespace Calabonga.Commandex.UI.Core.Engine
{
    internal static class CommandexActions
    {
        internal static Type[] FindActions(string actionsFolderPath)
        {
            try
            {
                if (!Directory.Exists(actionsFolderPath))
                {
                    //if (logger.IsEnabled(LogLevel.Debug))
                    //{
                    //    logger.LogDebug("[Error]: Directory not exists {ModuleName}", actionsFolderPath);
                    //}
                    throw new DirectoryNotFoundException(actionsFolderPath);
                }

                var types = new List<Type>();

                var modulesDirectory = new DirectoryInfo(actionsFolderPath);
                var files = modulesDirectory.GetFiles("*.dll");
                if (!files.Any())
                {
                    //if (logger.IsEnabled(LogLevel.Debug))
                    //{
                    //    logger.LogDebug("[Warning]: No modules found in folder {ModuleName}", actionsFolderPath);
                    //}
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
}
