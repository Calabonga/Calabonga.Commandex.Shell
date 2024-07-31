using System.Text;
using System.Text.Json;
using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Commands;

namespace Calabonga.Commandex.UI.Core.Engine;

/// <summary>
/// // Calabonga: Summary required (ExecutionReport 2024-07-30 10:13)
/// </summary>
public static class ExecutionReport
{
    public static string CreateReport(ICommandexCommand command)
    {
        var stringBuilder = new StringBuilder($"{command.DisplayName} v.{command.Version}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(command.Description);
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(command.IsPushToShellEnabled
            ? "Есть объект в результате."
            : "Нет объекта в результате.");

        if (command.IsPushToShellEnabled)
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