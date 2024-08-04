using Calabonga.Commandex.Engine;
using Calabonga.Commandex.Engine.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Calabonga.Commandex.Shell.Core.Engine;

/// <summary>
/// // Calabonga: Summary required (NuGetService 2024-08-04 06:50)
/// </summary>
public sealed class NuGetService
{
    /// <summary>
    /// // Calabonga: Summary required (NuGetService 2024-08-04 07:09)
    /// </summary>
    /// <param name="items"></param>
    /// <param name="sourceType"></param>
    /// <param name="artifactsFolderPath"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task LoadPackagesFromNugetAsync(ICommandexCommand command, List<INugetDependency> items, NuGetSourceType sourceType, string artifactsFolderPath, CancellationToken cancellationToken)
    {
        foreach (var nugetDependency in items.Select(x => x.Dependencies))
        {
            var packageId = nugetDependency[0].Name;
            var version = nugetDependency[0].Version;

            var list = await LoadPackageByIdFromNugetAsync(command, packageId, version, sourceType, artifactsFolderPath, cancellationToken);

            list.ForEach(x => Assembly.LoadFrom(x));
        }
    }

    /// <summary>
    /// // Calabonga: Summary required (NuGetService 2024-08-04 07:09)
    /// </summary>
    /// <param name="isRemote"></param>
    /// <param name="artifactsFolderPath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private SourceRepository GetRepository(NuGetSourceType isRemote, string artifactsFolderPath)
        => isRemote switch
        {
            NuGetSourceType.Local => Repository.Factory.GetCoreV3(new PackageSource(artifactsFolderPath, "AppDefinitionsRepository", true, false, false)),
            NuGetSourceType.Remote => Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json"),
            _ => throw new ArgumentOutOfRangeException(nameof(isRemote), isRemote, null)
        };

    /// <summary>
    /// Loads from nuget package local storage (artifacts)
    /// </summary>
    /// <param name="command"></param>
    /// <param name="packageId"></param>
    /// <param name="version"></param>
    /// <param name="sourceType"></param>
    /// <param name="artifactsFolderPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The list of loaded assemblies</returns>
    /// <exception cref="Exception"></exception>
    private async Task<List<string>> LoadPackageByIdFromNugetAsync(ICommandexCommand command, string packageId, string version, NuGetSourceType sourceType, string artifactsFolderPath, CancellationToken cancellationToken)
    {
        var globalNuget = Path.Combine(artifactsFolderPath, "packages");
        var repository = GetRepository(sourceType, globalNuget);
        var downloadResource = await repository.GetResourceAsync<DownloadResource>(cancellationToken);
        if (!NuGetVersion.TryParse(version, out var nuGetVersion))
        {
            throw new Exception($"Invalid version {version} for nuget package {packageId} ({sourceType})");
        }


        using var downloadResourceResult = await downloadResource.GetDownloadResourceResultAsync(
            new PackageIdentity(packageId, nuGetVersion),
            new PackageDownloadContext(new SourceCacheContext()),
            globalPackagesFolder: globalNuget,
            logger: NullLogger.Instance,
            token: cancellationToken);

        if (downloadResourceResult.Status != DownloadResourceResultStatus.Available)
        {
            throw new Exception($"DownloadAsync of NuGet package failed. DownloadResult Status: {downloadResourceResult.Status}");
        }

        var reader = downloadResourceResult.PackageReader;

        var archive = new ZipArchive(downloadResourceResult.PackageStream);

        var libItems = await reader.GetLibItemsAsync(cancellationToken);

        var loadedDllPaths = new List<string>();
        var definitionArtifactFolder = Path.Combine(artifactsFolderPath, command.TypeName);
        foreach (var libItem in libItems)
        {
            foreach (var libItemItem in libItem.Items)
            {
                var entry = archive.GetEntry(libItemItem);
                using var memoryStream = new MemoryStream();
                await entry!.Open().CopyToAsync(memoryStream, cancellationToken);
                var assemblyLoadContext = new System.Runtime.Loader.AssemblyLoadContext(null, isCollectible: true);
                memoryStream.Position = 0;
                assemblyLoadContext.LoadFromStream(memoryStream);
                var dllFile = Path.Combine(definitionArtifactFolder!, entry!.Name);
                entry.SaveAsFile(dllFile, NullLogger.Instance);
                loadedDllPaths.Add(dllFile);
            }
        }

        return loadedDllPaths;
    }
}