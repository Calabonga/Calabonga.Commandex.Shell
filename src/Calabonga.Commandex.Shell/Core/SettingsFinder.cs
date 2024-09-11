using DotNetEnv;

namespace Calabonga.Commandex.Shell.Core;

/// <summary>
/// Environment file settings reader for current application (Commandex)
/// </summary>
internal class SettingsFinder
{
    public static AppSettings Configure()
    {
        Env.Load("commandex.env", LoadOptions.TraversePath());

        var appSettings = new AppSettings
        {
            CommandsPath = Environment.GetEnvironmentVariable("COMMANDS_FOLDER") ?? throw new ArgumentNullException($"COMMANDS_FOLDER"),
            ShowSearchPanelOnStartup = bool.Parse(Environment.GetEnvironmentVariable("SHOW_SEARCH_PANEL_ONSTARTUP") ?? "false"),
            ArtifactsFolderName = Environment.GetEnvironmentVariable("ARTIFACTS_FOLDER_NAME") ?? "Artifacts",
            NugetFeedUrl = Environment.GetEnvironmentVariable("NUGET_FEED_URL") ?? "https://api.nuget.org/v3/index.json"
        };

        return appSettings;
    }
}