namespace Calabonga.Commandex.Shell.Services;

public interface IVersionService
{
    public string Version { get; }

    public string Branch { get; }

    public string Commit { get; }
}

public class VersionService : IVersionService
{
    public VersionService()
    {
        Version = $"{ThisAssembly.Git.BaseVersion.Major}.{ThisAssembly.Git.BaseVersion.Minor}.{ThisAssembly.Git.Commits}";
        Branch = ThisAssembly.Git.Branch;
        Commit = ThisAssembly.Git.Commit;
    }

    public string Version { get; }

    public string Branch { get; }

    public string Commit { get; }
}