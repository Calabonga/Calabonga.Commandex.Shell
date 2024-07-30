using DotNetEnv;

namespace Calabonga.Commandex.UI;

public class AppSettings
{
    private static readonly Lazy<AppSettings> Lazy = new(new AppSettings());

    private AppSettings()
    {
        Env.Load(".env", LoadOptions.TraversePath());

        CommandsPath = Environment.GetEnvironmentVariable("COMMANDS_FOLDER") ?? throw new ArgumentNullException($"COMMANDS_FOLDER");
        ShowSearchPanelOnStartup = bool.Parse(Environment.GetEnvironmentVariable("SHOW_SEARCH_PANEL_ONSTARTUP") ?? "false");
    }

    public static AppSettings Default => Lazy.Value;

    public string CommandsPath { get; }
    public bool ShowSearchPanelOnStartup { get; }
}