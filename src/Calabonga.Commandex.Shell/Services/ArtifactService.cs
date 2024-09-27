using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Exceptions;
using Calabonga.Commandex.Engine.NugetDependencies;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Models;
using Calabonga.OperationResults;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
    private readonly ILogger<ArtifactService> _logger;
    private string? _definitionArtifactFolder;
    private readonly CurrentAppSettings _currentAppSettings;

    public ArtifactService(
        IAppSettings appSettings,
        NugetLoader nugetLoader,
        IEnumerable<INugetDependency> dependencies,
        ILogger<ArtifactService> logger)
    {
        _nugetLoader = nugetLoader;
        _dependencies = dependencies;
        _logger = logger;
        _currentAppSettings = (CurrentAppSettings)appSettings;

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

    /// <summary>
    /// Tries to delete loaded artifacts (nuget-packages)
    /// </summary>
    /// <returns></returns>
    public OperationEmpty<string> CleanArtifactsFolder()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _currentAppSettings.ArtifactsFolderName);

        if (!Path.Exists(path))
        {
            var message = $"{path} not exists";
            _logger.LogWarning(message);
            return Operation.Error(message);
        }

        try
        {
            var directory = new DirectoryInfo(path);
            directory.Delete(true);
            return Operation.Result();
        }
        catch (Exception exception)
        {
            var message = exception.Message;
            _logger.LogError(exception, message);

            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true,
                Verb = "open"
            });

            return Operation.Error(message);
        }
    }
}