namespace Calabonga.Commandex.Contracts;

public interface ICommandexAction
{
    string TypeName { get; }

    string DisplayName { get; }

    string Description { get; }

    string Version { get; }

    Task ShowDialogAsync();

    bool HasResult { get; }
}