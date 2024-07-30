namespace Calabonga.Commandex.Contracts.Commands;

public interface ICommandexCommand
{
    string TypeName { get; }

    string DisplayName { get; }

    string Description { get; }

    string Version { get; }

    Task ShowDialogAsync();

    bool HasResult { get; }

    object GetResult();
}