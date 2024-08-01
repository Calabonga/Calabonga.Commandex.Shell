using Calabonga.Commandex.Engine;
using Calabonga.Commandex.Engine.Commands;

namespace Calabonga.Commandex.TaxPayerStatusCommand;

public class TaxPayerCommand : CommandexCommand<TaxPayerDialogView, TaxPayerDialogResult>
{
    public TaxPayerCommand(IDialogService dialogService) : base(dialogService) { }

    public override string CopyrightInfo => "Calabonga SOFT © 2024";

    public override string DisplayName => "Проверка статуса налогоплательщика";

    public override string Description => "Публичный сервиса ФНС России «Проверка статуса налогоплательщика налога на профессиональный доход (самозанятого)»";

    public override string Version => "v1.0.0-beta.4";

    public override bool IsPushToShellEnabled => true;
}