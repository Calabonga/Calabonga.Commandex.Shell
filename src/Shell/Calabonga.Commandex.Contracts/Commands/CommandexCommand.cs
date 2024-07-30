namespace Calabonga.Commandex.Contracts.Commands;

/// <summary>
/// // Calabonga: Summary required (CommandexCommand 2024-07-30 07:14)
/// </summary>
/// <typeparam name="TDialogView"></typeparam>
/// <typeparam name="TDialogResult"></typeparam>
public abstract class CommandexCommand<TDialogView, TDialogResult> : ICommandexCommand
    where TDialogView : IDialogView
    where TDialogResult : IDialogResult
{
    private readonly IDialogService _dialogService;

    protected CommandexCommand(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public IDialogResult? Result { get; private set; }

    public bool HasResult => Result is not null;

    public string TypeName => GetType().Name;

    public abstract string DisplayName { get; }

    public abstract string Description { get; }

    public abstract string Version { get; }

    public Task ShowDialogAsync()
    {
        _dialogService.ShowDialog<TDialogView, TDialogResult>(OnResult);

        return Task.CompletedTask;
    }

    public virtual void OnResult(TDialogResult result)
    {
        SetResult(result);
    }

    private void SetResult(TDialogResult result)
    {
        Result = result;
    }

    public object GetResult()
    {
        return Result!;
    }
}