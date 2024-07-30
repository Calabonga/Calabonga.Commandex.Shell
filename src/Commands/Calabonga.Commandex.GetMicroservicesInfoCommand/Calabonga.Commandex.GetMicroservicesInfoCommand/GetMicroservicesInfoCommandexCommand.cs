using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Commands;

namespace Calabonga.Commandex.GetMicroservicesInfoCommand;

public class GetMicroservicesInfoCommandexCommand : CommandexCommand<GetMicroserviceInfoDialogView, GetMicroserviceInfoDialogResult>
{
    public GetMicroservicesInfoCommandexCommand(IDialogService dialogService) : base(dialogService)
    {
    }

    public override string CopyrightInfo => "Calabonga SOFT © 2024";

    public override string DisplayName => "Список микросервисов";

    public override string Description => "Команда получает список микросервисов c PostgreSQL (Localhost), которые зарегистрированы в OpenIddict Server.";

    public override string Version => "v1.0.0-beta.4";

}