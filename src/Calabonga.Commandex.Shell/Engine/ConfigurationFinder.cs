using Calabonga.Commandex.Engine.Settings;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// // Calabonga: Summary required (ConfigurationFinder 2024-08-09 07:16)
/// </summary>
public interface IConfigurationFinder
{
    /// <summary>
    /// // Calabonga: Summary required (ConfigurationFinder 2024-08-09 07:16)
    /// </summary>
    /// <param name="scope"></param>
    void CommandConfiguration(string scope);
}

public class ConfigurationFinder : IConfigurationFinder
{
    private readonly IAppSettings _shellSettings;
    private const string _configurationFileDefaultExtension = ".env";

    public ConfigurationFinder(IAppSettings shellSettings) => _shellSettings = shellSettings;

    public void CommandConfiguration(string scope)
    {
        var configurationPath = Path.Combine(_shellSettings.CommandsPath, scope + _configurationFileDefaultExtension);

        if (TryGetRegisteredApplication(".env", out var defaultApp))
        {
            if (!string.IsNullOrEmpty(defaultApp))
            {
                Process.Start(new ProcessStartInfo(configurationPath)
                {
                    FileName = defaultApp,
                    Arguments = configurationPath
                });
            }
        }
        else
        {

        }
        //Process.Start(process);
    }

    public static bool TryGetRegisteredApplication(
        string extension, out string registeredApp)
    {
        string extensionId = GetClassesRootKeyDefaultValue(extension);
        if (extensionId == null)
        {
            registeredApp = null;
            return false;
        }

        string openCommand = GetClassesRootKeyDefaultValue(
            Path.Combine(new[] { extensionId, "shell", "open", "command" }));

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

    private static string GetClassesRootKeyDefaultValue(string keyPath)
    {
        using (var key = Registry.ClassesRoot.OpenSubKey(keyPath))
        {
            if (key == null)
            {
                return null;
            }

            var defaultValue = key.GetValue(null);
            if (defaultValue == null)
            {
                return null;
            }

            return defaultValue.ToString();
        }
    }
}