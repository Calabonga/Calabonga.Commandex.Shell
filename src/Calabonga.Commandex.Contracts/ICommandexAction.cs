namespace Calabonga.Commandex.Contracts;

public interface ICommandexAction : IAction
{
    Task ExecuteAsync();
}

public interface IAction
{
    string Name { get; }

    string DisplayName { get; }

    string Description { get; }
}