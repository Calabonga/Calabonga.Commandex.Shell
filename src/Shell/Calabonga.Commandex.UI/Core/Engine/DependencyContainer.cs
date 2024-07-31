using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.UI.Core.Dialogs.Base;
using Calabonga.Commandex.UI.Core.Services;
using Calabonga.Commandex.UI.ViewModels;
using Calabonga.Commandex.UI.Views;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Serilog;

namespace Calabonga.Commandex.UI.Core.Engine;

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

        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IVersionService, VersionService>();

        var types = new List<Type>() { typeof(App) };
        types.AddRange(CommandsFinder.Find(AppSettings.Default.CommandsPath).ToList());
        services.AddDefinitions(types.ToArray());

        return services.BuildServiceProvider();
    }
}

public class PostgreSqlDbConnectionFactory : IDbConnectionFactory<NpgsqlConnection>
{
    private readonly ILogger<PostgreSqlDbConnectionFactory> _logger;

    public PostgreSqlDbConnectionFactory(ILogger<PostgreSqlDbConnectionFactory> logger)
    {
        _logger = logger;
    }

    public NpgsqlConnection CreateConnection(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            _logger.LogError(new ArgumentNullException(connectionString), "Connection string is required");
        }

        return new NpgsqlConnection(connectionString);
    }

    public string Description => "PostgreSQL Connection Factory";
}