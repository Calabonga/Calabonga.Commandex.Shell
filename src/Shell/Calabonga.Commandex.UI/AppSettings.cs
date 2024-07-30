using DotNetEnv;

namespace Calabonga.Commandex.UI;

public class AppSettings
{
    private static readonly Lazy<AppSettings> Lazy = new(new AppSettings());

    private AppSettings()
    {
        Env.Load(".env", LoadOptions.TraversePath());

        CommandsPath = Environment.GetEnvironmentVariable("COMMANDS_FOLDER") ?? throw new ArgumentNullException($"COMMANDS_FOLDER");
    }

    public static AppSettings Default => Lazy.Value;

    public string CommandsPath { get; }
}