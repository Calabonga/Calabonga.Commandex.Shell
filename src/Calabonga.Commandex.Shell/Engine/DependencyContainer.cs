using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Extensions;
using Calabonga.Commandex.Engine.Processors.Extensions;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Shell.Extensions;
using Calabonga.Commandex.Shell.Services;
using Calabonga.Commandex.Shell.ViewModels;
using Calabonga.Commandex.Shell.ViewModels.Dialogs;
using Calabonga.Commandex.Shell.ViewModels.UserControls;
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
        services.AddTransient<LoginControlViewModel>();

        services.AddTransient<AboutDialog>();
        services.AddTransient<AboutViewModel>();

        // defaults views for notifications
        services.AddTransient<DefaultDialogView>();

        // engine
        services.AddTransient<CommandExecutor>();
        services.AddTransient<ArtifactService>();
        services.AddTransient<FileService>();
        services.AddTransient<NugetLoader>();
        services.AddScoped<IConfigurationFinder, ConfigurationFinder>();
        services.AddSingleton<ISettingsReaderConfiguration, DefaultSettingsReaderConfiguration>();
        services.AddTransient<IVersionService, VersionService>();
        services.AddTransient<IGroupBuilder, DefaultGroupBuilder>();
        services.AddTransient<ICommandService, CommandService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddHttpClient(nameof(IAuthenticationService));

        // processor: to select default uncomment line below (nuget: Calabonga.Commandex.Engine) 
        // services.AddResultProcessor<DefaultResultProcessor>();

        // processor: advanced result (nuget: Calabonga.Commandex.Engine.Processors) 
        services.AddAdvancedResultProcessor();

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
