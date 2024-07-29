namespace Calabonga.Commandex.Contracts;

//public interface ICommandexDialog<TView, TViewModel>
//    where TView : IDialogView
//    where TViewModel : IDialogResult
//{
//    string TypeName { get; }

//    string DisplayName { get; }

//    string Description { get; }
//}


public interface ICommandexAction
{
    string TypeName { get; }

    string DisplayName { get; }

    string Description { get; }

    string Version { get; }
}

public interface ICommandexAction<TResult> : ICommandexAction
{
    Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
}