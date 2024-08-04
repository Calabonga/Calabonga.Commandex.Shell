using Calabonga.Commandex.Engine;
using Calabonga.Commandex.Engine.Commands;
using System.IO;

namespace Calabonga.Commandex.Shell.Core.Engine;

/// <summary>
/// // Calabonga: Summary required (ArtifactManager 2024-08-04 07:03)
/// </summary>
public sealed class ArtifactManager
{
    internal static readonly string ArtifactsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppSettings.Default.ArtifactsFolderName);
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly NuGetService _nuGetService;
    private readonly IEnumerable<INugetDependency> _dependencies;
    private string? _definitionArtifactFolder;

    public ArtifactManager(
        NuGetService nuGetService,
        IEnumerable<INugetDependency> dependencies)
    {
        _nuGetService = nuGetService;
        _dependencies = dependencies;

        CreateArtifactFolderInNotExists();
    }

    /// <summary>
    /// // Calabonga: Summary required (ArtifactManager 2024-08-04 07:26)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task CheckDependenciesReadyAsync(ICommandexCommand command)
    {
        _definitionArtifactFolder = Path.Combine(ArtifactsFolderPath, command.TypeName);

        var items = _dependencies.Where(x => x.Type.Name == command.TypeName).ToList();
        if (!items.Any())
        {
            return;
        }

        var definitionFolder = new DirectoryInfo(_definitionArtifactFolder);
        if (!definitionFolder.Exists)
        {
            await _nuGetService.LoadPackagesFromNugetAsync(command, items, NuGetSourceType.Remote, ArtifactsFolderPath, _cancellationTokenSource.Token);
        }

        else
        {
            await _nuGetService.LoadPackagesFromNugetAsync(command, items, NuGetSourceType.Local, ArtifactsFolderPath, _cancellationTokenSource.Token);
        }
    }

    /// <summary>
    /// // Calabonga: Summary required (ArtifactManager 2024-08-04 07:26)
    /// </summary>
    private static void CreateArtifactFolderInNotExists()
    {
        if (!Path.Exists(ArtifactsFolderPath))
        {
            Directory.CreateDirectory(ArtifactsFolderPath);
        }
    }
}