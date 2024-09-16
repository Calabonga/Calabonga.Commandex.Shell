using Calabonga.Commandex.Engine.Settings;

namespace Calabonga.Commandex.Shell.Models;

public class CurrentAppSettings : AppSettings
{
    /// <summary>
    /// There are modes available: Default, Brief, Extended
    /// </summary>
    public string DefaultViewName { get; set; } = null!;
}