using Calabonga.Commandex.Engine;
using Calabonga.Commandex.Engine.Commands;
using Calabonga.Commandex.Shell.Engine;
using System.IO;

namespace Calabonga.Commandex.Shell.Services;

/// <summary>
/// // Calabonga: Summary required (ArtifactService 2024-08-04 07:03)
/// </summary>
public sealed class ArtifactService
{
    internal static readonly string ArtifactsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppSettings.Default.ArtifactsFolderName);
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly NuGetService _nuGetService;
    private readonly IEnumerable<INugetDependency> _dependencies;
    private string? _definitionArtifactFolder;

    public ArtifactService(
        NuGetService nuGetService,
        IEnumerable<INugetDependency> dependencies)
    {
        _nuGetService = nuGetService;
        _dependencies = dependencies;

        CreateArtifactFolderInNotExists();
    }

    /// <summary>
    /// // Calabonga: Summary required (ArtifactService 2024-08-04 07:26)
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
        var source = definitionFolder.Exists switch
        {
            true => NuGetSourceType.Local,
            false => NuGetSourceType.Remote
        };

        await _nuGetService.LoadPackagesFromNugetAsync(command, items, source, ArtifactsFolderPath, _cancellationTokenSource.Token);
    }

    /// <summary>
    /// // Calabonga: Summary required (ArtifactService 2024-08-04 07:26)
    /// </summary>
    private static void CreateArtifactFolderInNotExists()
    {
        if (!Path.Exists(ArtifactsFolderPath))
        {
            Directory.CreateDirectory(ArtifactsFolderPath);
        }
    }
}