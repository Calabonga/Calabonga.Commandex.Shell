using System.ComponentModel.DataAnnotations;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// The types of the nuget repositories feed
/// </summary>
public enum NuGetSourceType
{
    /// <summary>
    /// Local artifacts folder
    /// </summary>
    [Display(Name = "Local artifacts folder")]
    Local,

    /// <summary>
    /// Remote feed from settings
    /// </summary>
    [Display(Name = "Remote feed from settings")]
    Remote
}