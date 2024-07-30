using System.Data.SqlClient;
using Calabonga.Commandex.Contracts;
using Microsoft.Extensions.Logging;

namespace Calabonga.Commandex.UI.Plugins.MsSql
{
    public class MsSqlDbConnectionFactory : IDbConnectionFactory<SqlConnection, SqlCredential>
    {
        private readonly ILogger<MsSqlDbConnectionFactory> _logger;

        public MsSqlDbConnectionFactory(ILogger<MsSqlDbConnectionFactory> logger)
        {
            _logger = logger;
        }

        public SqlConnection CreateConnection(string connectionString, SqlCredential? credential)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError(new ArgumentNullException(connectionString), "Connection string is required");
            }

            return credential is null
                ? new SqlConnection(connectionString)
                : new SqlConnection(connectionString, credential);
        }

        public string Description => "Microsoft Sql Server Connection Factory";
    }
}
