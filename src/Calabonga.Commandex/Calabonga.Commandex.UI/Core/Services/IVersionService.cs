namespace Calabonga.Commandex.UI.Core.Services;

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
        Version = ThisAssembly.Git.Tag;
        Branch = ThisAssembly.Git.Branch;
        Commit = ThisAssembly.Git.Commit;
    }

    public string Version { get; }
    public string Branch { get; }
    public string Commit { get; }
}