using Calabonga.Commandex.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Calabonga.Developer.Shell.UI.Engine;

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
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowsViewModel>();
        services.AddSingleton<IDialogService, DialogService>();

        // Attach definition from your library
        // services.AddDefinitions(typeof(YourCommandDefinition));

        return services.BuildServiceProvider();
    }
}

