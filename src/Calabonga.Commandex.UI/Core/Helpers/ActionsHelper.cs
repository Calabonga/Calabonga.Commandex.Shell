using Calabonga.Commandex.UI.Models;

namespace Calabonga.Commandex.UI.Core.Helpers;

public static class ActionsHelper
{
    internal static List<ActionItem> GetFakeActions()
    {
        return new List<ActionItem>
        {
            new("Тип1", "v1.0.0", "Отключить валидацию документов на DocumentValidationService" ,"Нет описания. Нечего показывать"),
            new("Тип1", "v1.0.0", "Включить валидацию документов на DocumentValidationService" ,"Нет описания. Нечего показывать"),
            new("Тип1", "v1.0.0", "Запуск синхронизации" ,"Нет описания. Нечего показывать"),
            new("Тип2", "v1.0.0", "Получение списка отчетов" ,"Нет описания. Нечего показывать"),
            new("Тип2", "v1.0.0", "Обновить название шаблона для печати","Нет описания. Нечего показывать"),
        };
    }
}