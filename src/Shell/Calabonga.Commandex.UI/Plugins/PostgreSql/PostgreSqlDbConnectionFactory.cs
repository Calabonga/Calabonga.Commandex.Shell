using Calabonga.Commandex.Contracts;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Calabonga.Commandex.UI.Plugins.PostgreSql;

public class PostgreSqlDbConnectionFactory : IDbConnectionFactory<NpgsqlConnection>
{
    private readonly ILogger<PostgreSqlDbConnectionFactory> _logger;

    public PostgreSqlDbConnectionFactory(ILogger<PostgreSqlDbConnectionFactory> logger)
    {
        _logger = logger;
        _logger.LogInformation("[{Name}] registered", GetType().Namespace);
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