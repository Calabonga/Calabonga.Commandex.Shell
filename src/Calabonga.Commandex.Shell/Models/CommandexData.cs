using Calabonga.Commandex.Shell.Infrastructure.Security;

namespace Calabonga.Commandex.Shell.Models;

/// <summary>
/// Commandex Storage Data
/// </summary>
public class CommandexData
{
    /// <summary>
    /// Who is data for
    /// </summary>
    public string Username { get; set; } = null!;
    /// <summary>
    /// Personal data
    /// </summary>
    public SecureData Data { get; set; } = null!;
}
