namespace Calabonga.Commandex.Contracts.Commands;

/// <summary>
/// // Calabonga: Summary required (EmptyCommandexCommand 2024-07-29 09:38)
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract class EmptyCommandexCommand<TResult> : ICommandexCommand
{
    public string TypeName => GetType().Name;

    public abstract string CopyrightInfo { get; }

    public virtual bool IsPushToShellEnabled => false;

    public abstract string DisplayName { get; }

    public abstract string Description { get; }

    public abstract string Version { get; }

    public abstract Task ShowDialogAsync();

    protected abstract TResult? Result { get; set; }

    private void SetResult(TResult result)
    {
        Result = result;
    }

    public object GetResult()
    {
        return Result!;
    }
}