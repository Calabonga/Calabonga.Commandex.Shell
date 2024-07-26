using Calabonga.Commandex.UI.Models;

namespace Calabonga.Commandex.UI.Core.Helpers;

public static class TreeHelper
{
    internal static List<GroupTreeItem> GetFakeTree()
    {
        return new List<GroupTreeItem>
        {
            new GroupTreeItem
            {
                Name = "Микросервисы",
                Items = [
                    new TreeItem(){ Name = "Отключить валидацию документов на DocumentValidationService" },
                    new TreeItem(){ Name = "Включить валидацию документов на DocumentValidationService" }
                ]
            },
            new GroupTreeItem
            {
                Name = "ЕЦБД",
                Items = [
                    new TreeItem() { Name = "Запуск синхронизации" },
                    new TreeItem() { Name = "Получение списка отчетов" },
                    new TreeItem() { Name = "Обновить название шаблона для печати" }
                ]
            }
        };
    }
}