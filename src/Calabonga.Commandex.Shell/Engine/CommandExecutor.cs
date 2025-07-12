using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Exceptions;
using Calabonga.Commandex.Engine.Extensions;
using Calabonga.Commandex.Engine.ToastNotifications;
using Calabonga.Commandex.Engine.ToastNotifications.Controls;
using Calabonga.Commandex.Shell.Models;
using Calabonga.Commandex.Shell.Services;
using Calabonga.OperationResults;
using Serilog;
using System.Text.Json;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// Command Executor helper
/// </summary>
public sealed class CommandExecutor
{
    private readonly INotificationManager _notificationManager;
    private readonly IEnumerable<ICommandexCommand> _commands;
    private readonly ArtifactService _artifactService;

    public CommandExecutor(
        INotificationManager notificationManager,
        IEnumerable<ICommandexCommand> commands,
        ArtifactService artifactService)
    {
        _notificationManager = notificationManager;
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
    /// Returns a status of the command executing operation without a result.
    /// Pipeline: Find command -> Prepare -> Execute -> Return status.
    /// </summary>
    /// <param name="commandItem"></param>
    /// <returns>Nothing when Success and Error <see cref="ExecuteCommandexCommandException"/> when error occurred.</returns>
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

        var result = command.GetResult();
        if (result is null)
        {
            _notificationManager.Show(NotificationManager.CreateSuccessToast("Command executed but Result is NULL"), nameof(NotificationZone));
        }
        else
        {
            var data = JsonSerializer.Serialize(result, JsonSerializerOptionsExt.Cyrillic);
            _notificationManager.Show(NotificationManager.CreateSuccessToast($"Command executed and the Result is {data}"), nameof(NotificationZone));
        }

        command.Dispose();

        return operation.Ok
            ? Operation.Result(command)
            : Operation.Error(new ExecuteCommandexCommandException(operation.Error.Message, operation.Error));
    }

    private void OnCommandPrepared()
    {
        CommandPreparedSuccess?.Invoke(this, EventArgs.Empty);
    }

    private void OnCommandPreparing()
    {
        CommandPrepareStart?.Invoke(this, EventArgs.Empty);
    }

    private void OnCommandPreparationFailed()
    {
        CommandPreparationFailed?.Invoke(this, EventArgs.Empty);
    }
}
