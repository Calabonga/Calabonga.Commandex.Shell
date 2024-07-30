using System.Windows;
using Calabonga.Commandex.UI.Core.Engine;
using Calabonga.Commandex.UI.ViewModels;
using Calabonga.Commandex.UI.Views;
using Microsoft.Extensions.DependencyInjection;
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
            .MinimumLevel.Verbose()
            .WriteTo.File("/logs/commandex-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, shared: true)
            .CreateLogger();

        Services = DependencyContainer.ConfigureServices();
    }

    /// <summary>
    /// Gets the current <see cref="App"/> instance in use
    /// </summary>
    public new static App Current => (App)Application.Current;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider Services { get; private set; }

    /// <summary>
    /// Configures the services for the application.
    /// </summary>

    /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
    /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        SetupExceptionHandling();

        Log.Logger.Information("Application starting...");

        var shell = Services.GetService<ShellWindow>();
        var shellViewModel = Services.GetService<ShellWindowViewModel>();

        if (shellViewModel is null || shell is null)
        {
            return;
        }

        shell.DataContext = shellViewModel;

        Log.Logger.Information("Application started.");

        shell.Show();

        Log.Logger.Information("Application startup completed.");
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

    protected override void OnExit(ExitEventArgs e)
    {
        Log.Information("Application shutdown successful");
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}