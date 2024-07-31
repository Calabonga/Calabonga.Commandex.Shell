using Calabonga.Commandex.Contracts.Commands;

namespace Calabonga.Commandex.Welcome;

public class WelcomeCommandexCommand : EmptyCommandexCommand<string>
{
    public override Task ShowDialogAsync()
    {
        Result = "Welcome";
        return Task.CompletedTask;
    }
    public override bool IsPushToShellEnabled => true;

    public override string CopyrightInfo => "Calabonga SOFT © 2024";

    public override string DisplayName => "Добро пожаловать в модульность";

    public override string Description => "Это демонстрация по реализации команды для Commandex.";

    public override string Version => "1.0.0-beta.1";


    protected override string? Result { get; set; }
}