using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Extensions;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Shell.Extensions;
using Calabonga.Commandex.Shell.Services;
using Calabonga.Commandex.Shell.ViewModels;
using Calabonga.Commandex.Shell.ViewModels.Dialogs;
using Calabonga.Commandex.Shell.Views;
using Calabonga.Commandex.Shell.Views.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Calabonga.Commandex.Shell.Engine;

internal static class DependencyContainer
{
    /// <summary>
    /// Configure services
    /// </summary>
    /// <returns></returns>
    internal static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(options =>
        {
            options.AddSerilog(dispose: true);
            options.AddDebug();
        });

        // views and models for views
        services.AddTransient<ShellWindow>();
        services.AddTransient<ShellWindowViewModel>();
        services.AddTransient<AboutDialog>();
        services.AddTransient<AboutDialogResult>();
        services.AddTransient<DefaultDialogView>();

        // engine
        services.AddTransient<CommandExecutor>();
        services.AddTransient<ArtifactService>();
        services.AddTransient<FileService>();
        services.AddTransient<NugetLoader>();
        services.AddScoped<IConfigurationFinder, ConfigurationFinder>();
        services.AddSingleton<ISettingsReaderConfiguration, DefaultSettingsReaderConfiguration>();
        services.AddTransient<IVersionService, VersionService>();

        // components
        services.AddDialogComponent();
        services.AddWizardComponent();

        // settings
        services.AddSingleton<IAppSettings>(_ => App.Current.Settings);

        // definitions
        services.AddModulesDefinitions();

        return services.BuildServiceProvider();
    }
}