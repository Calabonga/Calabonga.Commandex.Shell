using Calabonga.Commandex.Engine;
using Calabonga.Commandex.Engine.Commands;
using Calabonga.Commandex.Engine.Exceptions;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.OperationResults;
using System.IO;

namespace Calabonga.Commandex.Shell.Services;

/// <summary>
/// // Calabonga: Summary required (ArtifactService 2024-08-04 07:03)
/// </summary>
public sealed class ArtifactService
{
    internal static readonly string ArtifactsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, App.Current.Settings.ArtifactsFolderName);
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly NugetLoader _nugetLoader;
    private readonly IEnumerable<INugetDependency> _dependencies;
    private string? _definitionArtifactFolder;

    public ArtifactService(
        NugetLoader nugetLoader,
        IEnumerable<INugetDependency> dependencies)
    {
        _nugetLoader = nugetLoader;
        _dependencies = dependencies;

        CreateArtifactFolderInNotExists();
    }

    /// <summary>
    /// // Calabonga: Summary required (ArtifactService 2024-08-04 07:26)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<OperationEmpty<ExecuteCommandexCommandException>> CheckDependenciesReadyAsync(ICommandexCommand command)
    {
        _definitionArtifactFolder = Path.Combine(ArtifactsFolderPath, command.TypeName);

        var items = _dependencies.Where(x => x.Type.Name == command.TypeName).ToList();
        if (!items.Any())
        {
            return Operation.Result();
        }

        var definitionFolder = new DirectoryInfo(_definitionArtifactFolder);
        var source = definitionFolder.Exists switch
        {
            true => NuGetSourceType.Local,
            false => NuGetSourceType.Remote
        };

        return await _nugetLoader.LoadPackagesFromNugetAsync(command, items, source, ArtifactsFolderPath, _cancellationTokenSource.Token);

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