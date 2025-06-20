namespace Calabonga.Commandex.Shell.Infrastructure.Messaging;

/// <summary>
/// Search Term changed notification message
/// </summary>
/// <param name="SearchTerm"></param>
public record SearchTermChangedMessage(string? SearchTerm);
