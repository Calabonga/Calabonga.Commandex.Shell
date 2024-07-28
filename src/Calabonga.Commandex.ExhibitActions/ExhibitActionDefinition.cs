using Calabonga.Commandex.Contracts;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.ExhibitActions;

public class ExhibitActionDefinition : AppDefinition
{
    /// <summary>
    /// Configure services for current application
    /// </summary>
    /// <param name="services">instance of <see cref="IServiceCollection"/></param>
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_ => new ExhibitRequest() { IsActive = true });
        services.AddScoped<ICommandexAction, LoadExhibitCommandexAction>();
    }
}