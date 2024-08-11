using Calabonga.Commandex.Engine.Exceptions;
using NuGet.Packaging;
using Serilog;
using System.Reflection;
using System.Runtime.Versioning;

namespace Calabonga.Commandex.Shell.Extensions;

/// <summary>
/// Nuget helper
/// </summary>
internal static class FrameworkSpecificGroupExtensions
{
    /// <summary>
    /// Returns filtered by <see cref="FrameworkSpecificGroup"/> items.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ExtractCommandexNugetException"></exception>
    internal static List<FrameworkSpecificGroup> FindCompatible(this List<FrameworkSpecificGroup> source)
    {
        const string netStandardName21 = ".NETStandard,Version=v2.1";
        const string netStandardName20 = ".NETStandard,Version=v2.0";

        var found = new List<FrameworkSpecificGroup>();

        var targetFrameworkAttribute = Assembly.GetExecutingAssembly()
            .GetCustomAttributes(typeof(TargetFrameworkAttribute), false)
            .SingleOrDefault();

        if (targetFrameworkAttribute is not TargetFrameworkAttribute attribute)
        {
            throw new ExtractCommandexNugetException("TargetFramework not found");
        }

        var termList = new string[] { attribute.FrameworkName, netStandardName21, netStandardName20 };

        Log.Logger.Debug(string.Join(Environment.NewLine, source.Select(x => x.TargetFramework.DotNetFrameworkName)));

        foreach (var term in termList)
        {
            Log.Logger.Information("Try to find TargetFramework {NetStandardName} in the nuget-package.", term);
            var filtered = source.FirstOrDefault(x => x.TargetFramework.DotNetFrameworkName == term);

            if (filtered is not null)
            {
                found.Add(filtered);
                return found;
            }
        }

        throw new ExtractCommandexNugetException($"TargetFrameworks {string.Join(",", termList)}  were not found. Sorry.");
    }
}