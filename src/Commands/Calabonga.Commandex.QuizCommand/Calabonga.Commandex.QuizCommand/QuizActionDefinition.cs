using Calabonga.Commandex.Engine.Commands;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.QuizCommand;

public class QuizActionDefinition : AppDefinition
{
    /// <summary>
    /// Configure services for current application
    /// </summary>
    /// <param name="services">instance of <see cref="IServiceCollection"/></param>
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<QuizDialogView>();
        services.AddScoped<QuizDialogResult>();
        services.AddScoped<ICommandexCommand, QuizCommand>();
    }
}