using Calabonga.Commandex.Engine;
using Calabonga.Commandex.Shell.Core;
using Calabonga.Commandex.Shell.Core.Dialogs;
using Calabonga.Commandex.Shell.Services;
using Calabonga.Commandex.Shell.ViewModels;
using Calabonga.Commandex.Shell.Views;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Calabonga.Commandex.Shell.Engine;

internal static class DependencyContainer
{
    internal static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(options =>
        {
            options.AddSerilog(dispose: true);
            options.AddDebug();
        });

        services.AddSingleton(typeof(DefaultDialogResult<>));
        services.AddSingleton<DefaultDialogView>();

        services.AddSingleton<ShellWindowViewModel>();
        services.AddSingleton<ShellWindow>();
        services.AddSingleton<AboutDialog>();
        services.AddSingleton<AboutDialogResult>();

        services.AddTransient<CommandExecutor>();
        services.AddTransient<ArtifactService>();
        services.AddTransient<FileService>();
        services.AddTransient<NuGetService>();

        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IVersionService, VersionService>();

        var types = new List<Type>() { typeof(App) };
        types.AddRange(CommandFinder.Find(AppSettings.Default.CommandsPath));
        services.AddDefinitions(types.ToArray());

        return services.BuildServiceProvider();
    }
}