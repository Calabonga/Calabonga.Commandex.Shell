using Calabonga.Commandex.Engine.Settings;

namespace Calabonga.Commandex.Shell.Models;

public class CurrentAppSettings : AppSettings
{
    /// <summary>
    /// There are modes available: Default, Brief, Extended. <see cref="CommandViewType"/>
    /// </summary>
    public string DefaultViewName { get; init; } = null!;

    /// <summary>
    /// Authorization Server URL (OAuth2.0)
    /// </summary>
    public string? AuthorizationServerUrl { get; set; }

    /// <summary>
    /// OAuth2.0 Client Identity
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// OAuth2.0 Client Secret
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// OAuth2.0 Grand Type
    /// </summary>
    public string? GrantType { get; set; }

    public static string GetViewResourceName(string settingsName)
    {
        Enum.TryParse<CommandViewType>(settingsName, true, out var defaultList);

        return $"ListView{defaultList}DataTemplate";
    }
}
