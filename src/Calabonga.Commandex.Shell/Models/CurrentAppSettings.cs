using Calabonga.Commandex.Engine.Settings;

namespace Calabonga.Commandex.Shell.Models;

public class CurrentAppSettings : AppSettings
{
    /// <summary>
    /// There are modes available: Default, Brief, Extended
    /// </summary>
    public string DefaultViewName { get; set; } = null!;

    public static string GetViewResourceName(string settingsName)
    {
        Enum.TryParse<CommandViewType>(settingsName, true, out var defaultList);

        return $"ListView{defaultList}DataTemplate";
    }
}