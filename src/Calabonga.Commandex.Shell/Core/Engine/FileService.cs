using System.IO;

namespace Calabonga.Commandex.Shell.Core.Engine;

/// <summary>
/// // Calabonga: Summary required (FileService 2024-08-04 06:16)
/// </summary>
public sealed class FileService
{
    public long GetArtifactsSize()
    {
        var folder = new DirectoryInfo(ArtifactManager.ArtifactsFolderPath);
        return folder.GetFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
    }
}