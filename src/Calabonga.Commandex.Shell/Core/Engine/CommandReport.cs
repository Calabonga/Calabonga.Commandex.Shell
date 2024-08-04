using System.Text;
using Calabonga.Commandex.Engine.Commands;
using NuGet.Protocol;

namespace Calabonga.Commandex.Shell.Core.Engine;

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
            try
            {
                var data = res.ToJson();
                stringBuilder.Append(data);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Выполнен успешно.");
        return stringBuilder.ToString();
    }
}