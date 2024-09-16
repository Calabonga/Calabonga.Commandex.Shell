using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Extensions;
using System.Text;
using System.Text.Json;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// // Calabonga: Summary required (CommandReport 2024-07-30 10:13)
/// </summary>
public static class CommandReport
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
            if (res is null)
            {
                stringBuilder.Append("Result is NULL");
            }
            else
            {
                try
                {
                    var data = JsonSerializer.Serialize(res, JsonSerializerOptionsExt.Cyrillic);
                    stringBuilder.Append(data);

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    App.Current.LastException = exception;
                    throw;
                }
            }
        }

        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Выполнен успешно.");
        return stringBuilder.ToString();
    }
}