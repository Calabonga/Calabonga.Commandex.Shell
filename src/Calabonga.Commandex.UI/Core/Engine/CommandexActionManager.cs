using System.IO;
using System.Reflection;
using Calabonga.Commandex.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.UI.Core.Engine
{
    internal static class CommandexActionManager
    {
        internal static void FindActions(IServiceCollection services, string actionsFolderPath)
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
                var modules = modulesDirectory.GetFiles("*.dll");
                if (!modules.Any())
                {
                    //if (logger.IsEnabled(LogLevel.Debug))
                    //{
                    //    logger.LogDebug("[Warning]: No modules found in folder {ModuleName}", actionsFolderPath);
                    //}
                    return;
                }

                foreach (var fileInfo in modules)
                {
                    var module = Assembly.LoadFile(fileInfo.FullName);
                    var typesAll = module.GetExportedTypes().Where(x => x.IsAssignableTo(typeof(IAction)));
                    var typesDefinition = typesAll.ToList();

                    var instances = typesDefinition.Select(Activator.CreateInstance)
                        .Cast<IAction>()
                        .Select(x => x.GetType())
                        .ToList();

                    types.AddRange(instances);
                }

                if (types.Any())
                {

                    foreach (var type in types)
                    {
                        services.AddSingleton(typeof(IAction), type);
                    }

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
