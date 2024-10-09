using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Extensions;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Calabonga.Commandex.Shell.Processing;

/// <summary>
/// Default Commandex command result  processor.
/// </summary>
public sealed class DefaultResultProcessor : IResultProcessor
{
    private readonly IDialogService _dialogService;
    private readonly ILogger<DefaultResultProcessor> _logger;

    public DefaultResultProcessor(
        IDialogService dialogService,
        ILogger<DefaultResultProcessor> logger)
    {
        _dialogService = dialogService;
        _logger = logger;
    }

    public void ProcessCommand(ICommandexCommand command)
    {
        var pushState = command.IsPushToShellEnabled ? "Enabled" : "Disabled";
        var stringBuilder = new StringBuilder($"{command.DisplayName} v.{command.Version}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(command.Description);
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($"{nameof(command.IsPushToShellEnabled)} is {pushState}");
        stringBuilder.AppendLine();

        if (command.IsPushToShellEnabled)
        {
            var result = command.GetResult();
            if (result is null)
            {
                stringBuilder.Append("Result is null.");
            }
            else
            {
                try
                {
                    var data = JsonSerializer.Serialize(result, JsonSerializerOptionsExt.Cyrillic);
                    stringBuilder.Append(data);

                }
                catch (Exception exception)
                {
                    _dialogService.ShowError(exception.Message);

                    App.Current.LastException = exception;
                    throw;
                }
            }
        }

        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Command execution successfully completed.");

        var message = stringBuilder.ToString();

        _logger.LogInformation("{CommandType} executed with result: {Result}", command.TypeName, message);
        _dialogService.ShowNotification(message);
    }
}
