using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Exceptions;
using Calabonga.OperationResults;

namespace Calabonga.Commandex.Shell.Tests.Commands;

internal class FakeCommandexCommand5 : ICommandexCommand
{
    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {

    }

    public Task<OperationEmpty<ExecuteCommandexCommandException>> ExecuteCommandAsync() => throw new NotImplementedException();

    public object? GetResult() => throw new NotImplementedException();

    public string CopyrightInfo => "";
    public bool IsPushToShellEnabled => false;
    public string DisplayName => "Five: one,two";
    public string Description => "";
    public string Version => "4";
    public string[]? Tags => ["one", "two"];

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => Tags != null ? string.Join(" ", Tags) : string.Empty;
}