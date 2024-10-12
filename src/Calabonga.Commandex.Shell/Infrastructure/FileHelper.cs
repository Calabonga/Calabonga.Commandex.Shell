using Calabonga.Commandex.Engine.Extensions;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Calabonga.Commandex.Shell.Infrastructure;

/// <summary>
/// File I/O helper
/// </summary>
internal static class FileHelper
{
    /// <summary>
    /// Load data from storage
    /// </summary>
    /// <param name="folderPath"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal static T? GetData<T>(string folderPath) where T : class
    {
        var file = typeof(T).Name.PascalToKebabCase() + ".prm";
        var path = Path.Combine(folderPath, file);
        if (!File.Exists(path))
        {
            return default;
        }
        var parameter = File.ReadAllText(path);
        var fromBase64String = Convert.FromBase64String(parameter);
        return JsonSerializer.Deserialize<T>(fromBase64String);
    }
    /// <summary>
    /// Saves data to storage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal static void SetData<T>(T parameter, string folderPath) where T : class
    {
        var file = typeof(T).Name.PascalToKebabCase() + ".prm";
        if (!Path.Exists(folderPath))
        {
            return;
        }
        var path = Path.Combine(folderPath, file);
        var data = JsonSerializer.SerializeToUtf8Bytes(parameter);
        var base64 = Convert.ToBase64String(data);
        File.WriteAllText(path, base64, Encoding.UTF8);
    }

    /// <summary>
    /// Deletes data from storage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="folderPath"></param>
    public static void ClearData<T>(string folderPath)
    {
        var file = typeof(T).Name.PascalToKebabCase() + ".prm";
        var path = Path.Combine(folderPath, file);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
