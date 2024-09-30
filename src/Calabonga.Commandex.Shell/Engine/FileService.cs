using Calabonga.Commandex.Shell.Services;
using System.IO;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// Artifacts items service
/// </summary>
public sealed class FileService
{
    /// <summary>
    /// Returns a size of the Artifacts folder
    /// </summary>
    /// <returns></returns>
    public long GetArtifactsSize()
    {
        var folder = new DirectoryInfo(ArtifactService.ArtifactsFolderPath);
        return folder.Exists ? folder.GetFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length) : 0;
    }
}