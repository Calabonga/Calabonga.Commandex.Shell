namespace Calabonga.Commandex.Shell.Services;

public interface IVersionService
{
    public string Version { get; }
    public string Branch { get; }
    public string Commit { get; }
    public string Tag { get; }
}

public class VersionService : IVersionService
{
    public VersionService()
    {
        Version = $"{ThisAssembly.Git.SemVer.Major}.{ThisAssembly.Git.SemVer.Minor}.{ThisAssembly.Git.SemVer.Patch}";
        Branch = ThisAssembly.Git.Branch;
        Commit = ThisAssembly.Git.Commit;
        Tag = ThisAssembly.Git.SemVer.Label;
    }

    public string Version { get; }

    public string Branch { get; }

    public string Commit { get; }

    public string Tag { get; }
}