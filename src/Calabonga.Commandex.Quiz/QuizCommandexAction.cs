using Calabonga.Commandex.Contracts;

namespace Calabonga.Commandex.QuizActions;

public class QuizCommandexAction : CommandexAction<QuizDialogView, QuizDialogResult>
{
    public QuizCommandexAction(IDialogService dialogService) : base(dialogService) { }

    public override string TypeName => GetType().Name;

    public override string DisplayName => "Опросник";

    public override string Description => "Загрузчик вопросов для опроса со стороннего сервиса.";

    public override string Version => "v1.0.0-beta-4";

}