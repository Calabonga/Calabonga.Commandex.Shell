using Calabonga.Commandex.Contracts.Actions;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.TaxPayerStatusAction;

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
        services.AddScoped<ICommandexAction, TaxPayerCommandexAction>();
    }
}