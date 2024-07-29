namespace Calabonga.Commandex.Contracts.Actions;

/// <summary>
/// // Calabonga: Summary required (EmptyCommandexAction 2024-07-29 09:38)
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract class EmptyCommandexAction<TResult> : ICommandexAction
{
    public bool HasResult => Result is not null;

    public abstract string TypeName { get; }

    public abstract string DisplayName { get; }

    public abstract string Description { get; }

    public abstract string Version { get; }

    public abstract Task ShowDialogAsync();

    protected abstract TResult? Result { get; set; }

    protected virtual void OnResult(TResult result)
    {
        SetResult(result);
    }

    private void SetResult(TResult result)
    {
        Result = result;
    }
}