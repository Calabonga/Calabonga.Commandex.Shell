namespace Calabonga.Commandex.Contracts;

public abstract class CommandexAction<TDialogView, TDialogResult> : ICommandexAction
    where TDialogView : IDialogView
    where TDialogResult : IDialogResult
{
    private readonly IDialogService _dialogService;

    protected CommandexAction(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public void ShowDialog()
    {
        _dialogService.ShowDialog<TDialogView, TDialogResult>(OnResult);
    }

    protected abstract void OnResult(TDialogResult result);

    public abstract string TypeName { get; }

    public abstract string DisplayName { get; }

    public abstract string Description { get; }

    public abstract string Version { get; }
}


public interface ICommandexAction
{
    string TypeName { get; }

    string DisplayName { get; }

    string Description { get; }

    string Version { get; }

    void ShowDialog();
}

public interface ICommandexAction<TResult> : ICommandexAction
{
    Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
}