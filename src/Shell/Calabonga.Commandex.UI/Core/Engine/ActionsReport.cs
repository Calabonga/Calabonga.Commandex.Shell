using System.Text;
using System.Text.Json;
using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Commands;

namespace Calabonga.Commandex.UI.Core.Engine;

public class ActionsReport
{
    public static string CreateReport(ICommandexCommand command)
    {
        var stringBuilder = new StringBuilder($"{command.DisplayName} v.{command.Version}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(command.Description);
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(command.HasResult
            ? "Есть объект в результате."
            : "Нет объекта в результате.");

        if (command.HasResult)
        {
            var res = command.GetResult();
            var data = JsonSerializer.Serialize(res, JsonSerializerOptionsExt.Cyrillic);
            stringBuilder.Append(data);
        }

        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Выполнен успешно.");
        return stringBuilder.ToString();
    }
}