
using Calabonga.Commandex.Engine;
using Calabonga.Commandex.Engine.Commands;

namespace Calabonga.Commandex.QuizCommand;

public class QuizCommand : CommandexCommand<QuizDialogView, QuizDialogResult>
{
    public QuizCommand(IDialogService dialogService) : base(dialogService) { }

    public override string CopyrightInfo => "Calabonga SOFT © 2024";

    public override string DisplayName => "Генератор вопросов";

    public override string Description => "Загрузчик вопросов без возможности ответить со стороннего сервиса, но с возможностью показать загруженные данные.";

    public override string Version => "v1.0.0-beta-6";

}