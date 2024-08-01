using Calabonga.Commandex.Engine.Commands;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.TaxPayerStatusCommand;

public class TaxPayerStatusActionDefinition : AppDefinition
{
    /// <summary>
    /// Configure services for current application
    /// </summary>
    /// <param name="services">instance of <see cref="IServiceCollection"/></param>
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<TaxPayerDialogView>();
        services.AddScoped<TaxPayerDialogResult>();
        services.AddScoped<ICommandexCommand, TaxPayerCommand>();
    }
}