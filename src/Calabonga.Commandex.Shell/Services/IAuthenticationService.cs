using Calabonga.Commandex.Shell.Infrastructure.Identity;
using Calabonga.OperationResults;

namespace Calabonga.Commandex.Shell.Services;

/// <summary>
/// This is a IAuthenticationService
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Returns the authenticated ApplicationUser if authentication on the server was successful
    /// or returns an error message about why authentication failed
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="clientType"></param>
    /// <returns></returns>
    Task<Operation<ApplicationUser, string>> AuthenticateUser(string username, string password, string clientType);
}
