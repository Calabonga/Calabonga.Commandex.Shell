using Calabonga.Commandex.Shell.Infrastructure.Security;
using System.Security.Principal;

namespace Calabonga.Commandex.Shell.Infrastructure.Identity;

/// <summary>
/// This is a CommandexIdentity
/// </summary>
public class CommandexIdentity : IIdentity
{
    #region constructors

    protected CommandexIdentity()
    {
        Name = "Anonymous";
        SecureData = new SecureData();
    }

    protected CommandexIdentity(string name, SecureData secureData)
    {
        Name = name;
        SecureData = secureData;
        AuthenticationType = "OAuth2.0";
    }

    #endregion

    /// <summary>
    /// Gets the name of the current user.
    /// </summary>
    /// <returns>
    /// The name of the user on whose behalf the code is running.
    /// </returns>
    public string Name { get; }

    /// <summary>
    /// Gets the type of authentication used.
    /// </summary>
    /// <returns>
    /// The type of authentication used to identify the user.
    /// </returns>
    public string? AuthenticationType { get; }

    /// <summary>
    /// Gets a value that indicates whether the user has been authenticated.
    /// </summary>
    /// <returns>
    /// true if the user was authenticated; otherwise, false.
    /// </returns>
    public bool IsAuthenticated => !string.IsNullOrEmpty(SecureData.AccessToken);

    /// <summary>
    /// AuthenticationToken for access to server
    /// </summary>
    public SecureData SecureData { get; }
}
