using Calabonga.Commandex.Engine.Settings;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// Configuration for commands finder
/// </summary>
public interface IConfigurationFinder
{
    /// <summary>
    /// Opens or Creates a command configuration file.
    /// </summary>
    /// <param name="scope"></param>
    void OpenOrCreateCommandConfigurationFile(string scope);
}

/// <summary>
/// Configuration for commands finder
/// </summary>
public class ConfigurationFinder : IConfigurationFinder
{
    private readonly IAppSettings _shellSettings;
    private const string _configurationFileDefaultExtension = ".env";

    public ConfigurationFinder(IAppSettings shellSettings)
    {
        _shellSettings = shellSettings;
    }

    /// <summary>
    /// Opens or Creates a command configuration file.
    /// </summary>
    /// <param name="scope"></param>
    public void OpenOrCreateCommandConfigurationFile(string scope)
    {
        var configurationPath = Path.Combine(_shellSettings.SettingsPath, scope + _configurationFileDefaultExtension);

        if (!TryGetRegisteredApplication(".env", out var defaultApp))
        {
            return;
        }

        if (!string.IsNullOrEmpty(defaultApp))
        {
            Process.Start(new ProcessStartInfo(configurationPath)
            {
                FileName = defaultApp,
                Arguments = configurationPath
            });
        }
    }

    private static bool TryGetRegisteredApplication(string extension, out string? registeredApp)
    {
        var extensionId = GetClassesRootKeyDefaultValue(extension);
        if (extensionId == null)
        {
            registeredApp = null;
            return false;
        }

        var openCommand = GetClassesRootKeyDefaultValue(Path.Combine(new[] { extensionId, "shell", "open", "command" }));

        if (openCommand == null)
        {
            registeredApp = null;
            return false;
        }

        registeredApp = openCommand
            .Replace("%1", string.Empty)
            .Replace("\"", string.Empty)
            .Trim();
        return true;
    }

    private static string? GetClassesRootKeyDefaultValue(string keyPath)
    {
        using var key = Registry.ClassesRoot.OpenSubKey(keyPath);
        var defaultValue = key?.GetValue(null);
        return defaultValue?.ToString();
    }
}
