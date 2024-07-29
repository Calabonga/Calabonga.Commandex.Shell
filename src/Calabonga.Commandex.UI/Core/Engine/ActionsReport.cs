using System.Text;
using Calabonga.Commandex.Contracts;

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

        stringBuilder.AppendLine("Выполнен успешно.");
        return stringBuilder.ToString();
    }
}