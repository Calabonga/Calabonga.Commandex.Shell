using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Commands;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Calabonga.Commandex.GetMicroservicesInfoCommand
{
    public class GetMicroserviceInfoDefinition : AppDefinition
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICommandexCommand, GetMicroservicesInfoCommandexCommand>();
            services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, PostgreSqlDbConnectionFactory>();
            services.AddScoped<GetMicroserviceInfoDialogView>();
            services.AddScoped<GetMicroserviceInfoDialogResult>();
        }
    }
}
