using System.Text.Json.Serialization;

namespace BitpinClient.Models;

public class RefreshTokenRequest
{
    [JsonPropertyName("refresh")]
    public string RefreshToken { get; set; } = null!;
}
