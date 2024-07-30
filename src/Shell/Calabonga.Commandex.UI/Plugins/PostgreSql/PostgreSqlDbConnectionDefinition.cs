using Calabonga.Commandex.Contracts;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.UI.Plugins.PostgreSql;

public class PostgreSqlDbConnectionDefinition : AppDefinition
{
    /// <summary>
    /// Configure services for current application
    /// </summary>
    /// <param name="services">instance of <see cref="IServiceCollection"/></param>
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IDbConnectionFactory, PostgreSqlDbConnectionFactory>();
    }
}