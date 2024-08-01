using System.Text.Json.Serialization;

namespace Calabonga.Commandex.TaxPayerStatusCommand;

/// <summary>
/// Response from nalog.ru
/// </summary>
public class NalogResponse
{
    /// <summary>
    /// Status of employee
    /// </summary>
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    /// <summary>
    /// Message from nalog.ru
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}