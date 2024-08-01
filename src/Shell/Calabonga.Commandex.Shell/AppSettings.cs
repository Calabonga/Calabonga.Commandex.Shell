using DotNetEnv;

namespace Calabonga.Commandex.Shell;

/// <summary>
/// Application settings imported from .env-file with parameters.
/// </summary>
public class AppSettings
{
    private static readonly Lazy<AppSettings> Lazy = new(new AppSettings());

    private AppSettings()
    {
        Env.Load(".env", LoadOptions.TraversePath());

        CommandsPath = Environment.GetEnvironmentVariable("COMMANDS_FOLDER") ?? throw new ArgumentNullException($"COMMANDS_FOLDER");
        ShowSearchPanelOnStartup = bool.Parse(Environment.GetEnvironmentVariable("SHOW_SEARCH_PANEL_ONSTARTUP") ?? "false");
    }

    /// <summary>
    /// Instance of the <see cref="AppSettings"/>
    /// </summary>
    public static AppSettings Default => Lazy.Value;

    /// <summary>
    /// Where Commandex will search the commands
    /// </summary>
    public string CommandsPath { get; }

    /// <summary>
    /// If True then search bar on the top of the commands list will be visible by default.
    /// </summary>
    public bool ShowSearchPanelOnStartup { get; }
}