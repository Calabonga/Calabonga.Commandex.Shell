using System.Text.Json.Serialization;

namespace Calabonga.Commandex.TaxPayerStatusCommand;

public class NalogResponse
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }


    [JsonPropertyName("message")]
    public string? Message { get; set; }
}