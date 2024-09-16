using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Exceptions;
using Calabonga.Commandex.Shell.Models;
using Calabonga.Commandex.Shell.Services;
using Calabonga.OperationResults;
using Serilog;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// // Calabonga: Summary required (CommandExecutor 2024-08-03 09:54)
/// </summary>
public sealed class CommandExecutor
{
    private readonly IEnumerable<ICommandexCommand> _commands;
    private readonly ArtifactService _artifactService;

    public CommandExecutor(
        IEnumerable<ICommandexCommand> commands,
        ArtifactService artifactService)
    {
        _commands = commands;
        _artifactService = artifactService;
    }

    /// <summary>
    /// Fires when everything is prepared for command execution.
    /// </summary>
    public event EventHandler? CommandPreparedSuccess;

    /// <summary>
    /// Fires when everything is prepared for command execution.
    /// </summary>
    public event EventHandler? CommandPreparationFailed;

    /// <summary>
    /// Fires before command be executed and prepare command dependencies is started.
    /// </summary>
    public event EventHandler? CommandPrepareStart;

    /// <summary>
    /// // Calabonga: Summary required (CommandExecutor 2024-08-03 09:54)
    /// </summary>
    /// <param name="commandItem"></param>
    public async Task<Operation<ICommandexCommand, ExecuteCommandexCommandException>> ExecuteAsync(CommandItem commandItem)
    {
        var command = _commands.FirstOrDefault(x => x.TypeName == commandItem.TypeName);
        if (command is null)
        {
            const string errorMessage = "Command not found in the available commands list.";
            Log.Logger.Error(errorMessage);
            return Operation.Error(new ExecuteCommandexCommandException(errorMessage));
        }

        Log.Logger.Debug("Executing {CommandType}", command.TypeName);

        OnCommandPreparing();

        var checkOperation = await _artifactService.CheckDependenciesReadyAsync(command);
        if (!checkOperation.Ok)
        {
            OnCommandPreparationFailed();
            return Operation.Error(checkOperation.Error);
        }

        OnCommandPrepared();

        var operation = await command.ExecuteCommandAsync();

        command.Dispose();

        return operation.Ok
            ? Operation.Result(command)
            : Operation.Error(new ExecuteCommandexCommandException(operation.Error.Message, operation.Error));
    }

    private void OnCommandPrepared() => CommandPreparedSuccess?.Invoke(this, EventArgs.Empty);

    private void OnCommandPreparing() => CommandPrepareStart?.Invoke(this, EventArgs.Empty);

    private void OnCommandPreparationFailed() => CommandPreparationFailed?.Invoke(this, EventArgs.Empty);
}
