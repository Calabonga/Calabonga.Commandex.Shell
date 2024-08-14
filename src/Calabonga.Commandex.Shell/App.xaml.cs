using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Shell.Core;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.ViewModels;
using Calabonga.Commandex.Shell.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Windows;

namespace Calabonga.Commandex.Shell;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File("logs/commandex-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, shared: true)
            .CreateLogger();

        Settings = SettingsFinder.Configure();

        Services = DependencyContainer.ConfigureServices();
    }

    /// <summary>
    /// Gets the current <see cref="App"/> instance in use
    /// </summary>
    public static new App Current => (App)Application.Current;

    /// <summary>
    /// Current Application settings from env-file.
    /// </summary>
    public AppSettings Settings { get; }

    /// <summary>
    /// Last exception that was occurred in application.
    /// If not null it will show as soon as possible and then will  be reset to null
    /// </summary>
    public Exception? LastException { get; set; }

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider Services { get; }

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

        if (LastException is not null)
        {
            Services.GetRequiredService<IDialogService>().ShowError(LastException.Message);
            LastException = null;
        }

        shell.Show();

        Log.Logger.Information("Application started.");
    }

    /// <summary>
    ///     Required method to load Application Resources
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
        Log.Information("Commandex Executor successfully shutdown");
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}