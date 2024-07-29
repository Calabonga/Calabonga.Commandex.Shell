namespace Calabonga.Commandex.Contracts.Actions;

public interface ICommandexAction
{
    string TypeName { get; }

    string DisplayName { get; }

    string Description { get; }

    string Version { get; }

    Task ShowDialogAsync();

    bool HasResult { get; }
}