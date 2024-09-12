using Calabonga.Commandex.Shell.Services;
using System.IO;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// // Calabonga: Summary required (FileService 2024-08-04 06:16)
/// </summary>
public sealed class FileService
{
    public long GetArtifactsSize()
    {
        var folder = new DirectoryInfo(ArtifactService.ArtifactsFolderPath);
        return folder.Exists ? folder.GetFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length) : 0;
    }
}