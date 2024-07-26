using System.IO;
using System.Windows;
using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.MicrosoftSqlDbConnection;
using Calabonga.Commandex.PostgreSqlDbConnection;
using Calabonga.Commandex.UI.Core.Dialogs;
using Calabonga.Commandex.UI.Core.Dialogs.Base;
using Calabonga.Commandex.UI.Core.Engine;
using Calabonga.Commandex.UI.Core.Services;
using Calabonga.Commandex.UI.ViewModels;
using Calabonga.Commandex.UI.Views;
using Calabonga.Wpf.AppDefinitions;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Env.Load(".env", LoadOptions.TraversePath());

            Services = ConfigureServices();
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

            services.AddLogging();
            services.AddSingleton<ShellWindowViewModel>();
            services.AddSingleton<ShellWindow>();

            services.AddSingleton<ModulesInfoDialog>();
            services.AddSingleton<ModulesInfoDialogViewModel>();

            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IVersionService, VersionService>();

            services.AddDefinitions(typeof(PostgreSqlDbConnectionEntry), typeof(MicrosoftSqlDbConnectionEntry));


            CommandexActionManager.FindActions(services, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Actions"));


            return services.BuildServiceProvider();
        }

        /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var shell = Services.GetService<ShellWindow>();
            var shellViewModel = Services.GetService<ShellWindowViewModel>();

            if (shellViewModel is null || shell is null)
            {
                return;
            }

            shell.DataContext = shellViewModel;
            shell.Show();
        }
    }

}
