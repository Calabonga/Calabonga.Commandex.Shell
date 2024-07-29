using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Actions;

namespace Calabonga.Commandex.QuizAction;

public class QuizCommandexAction : CommandexAction<QuizDialogView, QuizDialogResult>
{
    public QuizCommandexAction(IDialogService dialogService) : base(dialogService) { }

    public override string DisplayName => "Опросник";

    public override string Description => "Загрузчик вопросов без возможности ответить со стороннего сервиса, но с возможностью показать загруженные данные.";

    public override string Version => "v1.0.0-beta-5";

}