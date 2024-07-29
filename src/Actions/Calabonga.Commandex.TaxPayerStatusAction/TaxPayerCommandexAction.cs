using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Actions;

namespace Calabonga.Commandex.TaxPayerStatusAction;

public class TaxPayerCommandexAction : CommandexAction<TaxPayerDialogView, TaxPayerDialogResult>
{
    public TaxPayerCommandexAction(IDialogService dialogService) : base(dialogService)
    {

    }

    public override string DisplayName => "Проверка статуса налогоплательщика";

    public override string Description => "Публичный сервиса ФНС России «Проверка статуса налогоплательщика налога на профессиональный доход (самозанятого)»";

    public override string Version => "v1.0.0-beta.3";

    public override void OnResult(TaxPayerDialogResult result)
    {

    }
}