using System.Security.Principal;

namespace Calabonga.Commandex.Shell.Infrastructure.Identity;

/// <summary>
/// This is a CommandexPrincipal
/// </summary>
public class CommandexPrincipal : IPrincipal
{
    #region property Identity

    /// <summary>
    /// The Identity property
    /// </summary>
    public CommandexIdentity Identity
    {
        get => _identity ?? new AnonymousIdentity();
        set => _identity = value;
    }

    /// <summary>
    /// The backing field for Identity property 
    /// </summary>
    private CommandexIdentity? _identity;

    #endregion

    #region IPrincipal

    /// <summary>Determines whether the current principal belongs to the specified role.</summary>
    /// <param name="role">The name of the role for which to check membership.</param>
    /// <returns>
    /// <see langword="true" /> if the current principal is a member of the specified role; otherwise, <see langword="false" />.</returns>
    public bool IsInRole(string role) => false;

    /// <summary>
    /// Gets the identity of the current principal.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.
    /// </returns>
    IIdentity IPrincipal.Identity => Identity;

    #endregion
}