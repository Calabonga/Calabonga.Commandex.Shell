using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.GetMicroservicesInfoCommand;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Develop.Command.Engine;

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

        services.AddDefinitions(typeof(App), typeof(GetMicroserviceInfoDefinition));

        return services.BuildServiceProvider();
    }
}

