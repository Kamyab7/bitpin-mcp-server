using System.Text.Json.Serialization;

namespace BitpinClient.Models;

public class RefreshTokenResponse
{
    [JsonPropertyName("access")]
    public string AccessToken { get; set; } = null!;
}
