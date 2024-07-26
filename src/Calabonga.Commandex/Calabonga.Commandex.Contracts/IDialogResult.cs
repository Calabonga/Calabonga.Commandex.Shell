namespace Calabonga.Commandex.Contracts;

public interface IDialogResult
{
    string DialogTitle { get; }

    bool Ok { get; }
}