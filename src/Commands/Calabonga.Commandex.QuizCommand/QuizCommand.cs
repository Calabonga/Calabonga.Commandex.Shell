using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Commands;

namespace Calabonga.Commandex.QuizCommand;

public class QuizCommand : CommandexCommand<QuizDialogView, QuizDialogResult>
{
    public QuizCommand(IDialogService dialogService) : base(dialogService) { }

    public override string DisplayName => "Опросник";

    public override string Description => "Загрузчик вопросов без возможности ответить со стороннего сервиса, но с возможностью показать загруженные данные.";

    public override string Version => "v1.0.0-beta-5";

}