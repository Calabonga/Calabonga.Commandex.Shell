using Calabonga.Commandex.Engine.Commands;
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
    public event EventHandler? CommandPrepared;

    /// <summary>
    /// Fires before command be executed and prepare command dependencies is started.
    /// </summary>
    public event EventHandler? CommandPreparing;

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

        await _artifactService.CheckDependenciesReadyAsync(command);
        var checkOperation = await _artifactService.CheckDependenciesReadyAsync(command);

        if (!checkOperation.Ok)
        {
            return Operation.Error(checkOperation.Error);
        }

        OnCommandPrepared();

        var operation = await command.ExecuteCommandAsync();

        return operation.Ok
            ? Operation.Result(command)
            : Operation.Error(new ExecuteCommandexCommandException(operation.Error.Message, operation.Error));
    }

    private void OnCommandPrepared() => CommandPrepared?.Invoke(this, EventArgs.Empty);

    private void OnCommandPreparing() => CommandPreparing?.Invoke(this, EventArgs.Empty);
}