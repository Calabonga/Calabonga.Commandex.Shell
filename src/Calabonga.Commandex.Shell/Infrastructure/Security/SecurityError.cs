using System.Text.Json.Serialization;

namespace Calabonga.Commandex.Shell.Infrastructure.Security;

/// <summary>
/// Server authentication error
/// </summary>
public class SecurityError
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }
    [JsonPropertyName("error_description")]
    public string? Description { get; set; }
}
