using System.Windows;
using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.UI.Core.Dialogs;
using Calabonga.Commandex.UI.Core.Dialogs.Base;
using Calabonga.Commandex.UI.Core.Engine;
using Calabonga.Commandex.UI.Core.Services;
using Calabonga.Commandex.UI.ViewModels;
using Calabonga.Commandex.UI.Views;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Calabonga.Commandex.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    public App()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("/logs/log-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, shared: true)
            .CreateLogger();

        try
        {
            Services = ConfigureServices();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Something went wrong");
        }
        finally
        {
            Log.CloseAndFlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

    }



    /// <summary>
    /// Gets the current <see cref="App"/> instance in use
    /// </summary>
    public new static App Current => (App)Application.Current;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(Log.Logger);
        });

        services.AddSingleton(typeof(DefaultDialogResult<>));
        services.AddSingleton<DefaultDialogView>();

        services.AddSingleton<ShellWindowViewModel>();
        services.AddSingleton<ShellWindow>();

        services.AddSingleton<ModulesInfoDialog>();
        services.AddSingleton<ModulesInfoDialogViewModel>();

        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IVersionService, VersionService>();

        var types = new List<Type>() { typeof(App) };
        types.AddRange(CommandsFinder.Find(AppSettings.Default.CommandsPath).ToList());
        services.AddDefinitions(types.ToArray());

        return services.BuildServiceProvider();
    }

    protected override void OnActivated(EventArgs e)
    {
        base.OnActivated(e);
        InitializeComponent();
    }

    /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
    /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        SetupExceptionHandling();

        Log.Logger.Information("Application stating...");
        var shell = Services.GetService<ShellWindow>();
        var shellViewModel = Services.GetService<ShellWindowViewModel>();

        if (shellViewModel is null || shell is null)
        {
            return;
        }

        shell.DataContext = shellViewModel;
        shell.Show();
    }

    /// <summary>
    ///     Required method to load App.Resources
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        InitializeComponent();
    }

    private void SetupExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
        };

        DispatcherUnhandledException += (_, e) =>
        {
            LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
            e.Handled = true;
        };

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
            e.SetObserved();
        };
    }

    private void LogUnhandledException(Exception exception, string source)
    {
        var message = $"Unhandled exception ({source})";
        try
        {
            var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            message = $"Unhandled exception in {assemblyName.Name} v{assemblyName.Version}";
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Exception in LogUnhandledException");
        }
        finally
        {
            Log.Logger.Error(exception, message);
        }
    }
}