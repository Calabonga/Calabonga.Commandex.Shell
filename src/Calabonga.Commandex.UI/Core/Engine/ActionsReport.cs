using System.Text;
using System.Text.Json;
using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Actions;

namespace Calabonga.Commandex.UI.Core.Engine;

public class ActionsReport
{
    public static string CreateReport(ICommandexAction action)
    {
        var stringBuilder = new StringBuilder($"{action.DisplayName} v.{action.Version}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(action.Description);
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(action.HasResult
            ? "Есть объект в результате."
            : "Нет объекта в результате.");

        if (action.HasResult)
        {
            var res = action.GetResult();
            var data = JsonSerializer.Serialize(res, JsonSerializerOptionsExt.Cyrillic);
            stringBuilder.Append(data);
        }

        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Выполнен успешно.");
        return stringBuilder.ToString();
    }
}