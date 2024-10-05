using Calabonga.Commandex.Shell.Infrastructure.Security;

namespace Calabonga.Commandex.Shell.Infrastructure.Identity;

/// <summary>
/// This is a ApplicationUser
/// </summary>
public class ApplicationUser : CommandexIdentity
{
    public ApplicationUser(string name, SecureData secureData) : base(name, secureData) { }
}
