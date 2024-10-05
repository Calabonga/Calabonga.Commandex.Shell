using Calabonga.Commandex.Shell.Models;
using DotNetEnv;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// Environment file settings reader for current application (Commandex)
/// </summary>
internal static class SettingsFinder
{
    internal static CurrentAppSettings Configure()
    {
        Env.Load("commandex.env", LoadOptions.TraversePath());

        var appSettings = new CurrentAppSettings
        {
            CommandsPath = Environment.GetEnvironmentVariable("COMMANDS_FOLDER") ?? throw new ArgumentNullException($"COMMANDS_FOLDER"),
            ShowSearchPanelOnStartup = bool.Parse(Environment.GetEnvironmentVariable("SHOW_SEARCH_PANEL_ONSTARTUP") ?? "false"),
            ArtifactsFolderName = Environment.GetEnvironmentVariable("ARTIFACTS_FOLDER_NAME") ?? "Artifacts",
            NugetFeedUrl = Environment.GetEnvironmentVariable("NUGET_FEED_URL") ?? "https://api.nuget.org/v3/index.json",
            DefaultViewName = Environment.GetEnvironmentVariable("DEFAULT_VIEW_NAME") ?? "DefaultList",
            AuthorizationServerUrl = Environment.GetEnvironmentVariable("AUTHORIZATION_SERVER_URL"),
            ClientId = Environment.GetEnvironmentVariable("AUTHORIZATION_CLIENT_ID"),
            ClientSecret = Environment.GetEnvironmentVariable("AUTHORIZATION_CLIENT_SECRET"),
            GrantType = Environment.GetEnvironmentVariable("AUTHORIZATION_GRANT_TYPE")
        };

        return appSettings;
    }


}
