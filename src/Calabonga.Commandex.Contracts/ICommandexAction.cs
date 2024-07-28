namespace Calabonga.Commandex.Contracts;

public interface ICommandexAction
{
    string TypeName { get; }

    string DisplayName { get; }

    string Description { get; }
}

public interface ICommandexAction<TResult> : ICommandexAction
{
    Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
}