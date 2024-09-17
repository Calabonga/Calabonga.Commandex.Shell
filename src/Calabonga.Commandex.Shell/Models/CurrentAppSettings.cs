using Calabonga.Commandex.Engine.Settings;

namespace Calabonga.Commandex.Shell.Models;

public class CurrentAppSettings : AppSettings
{
    /// <summary>
    /// There are modes available: Default, Brief, Extended. <see cref="CommandViewType"/>
    /// </summary>
    public string DefaultViewName { get; init; } = null!;

    public static string GetViewResourceName(string settingsName)
    {
        Enum.TryParse<CommandViewType>(settingsName, true, out var defaultList);

        return $"ListView{defaultList}DataTemplate";
    }
}