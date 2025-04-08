using System.Text.Json.Serialization;

namespace BitpinClient.Models;

public class GetTokenResponse
{
    [JsonPropertyName("access")]
    public string AccessToken { get; set; } = null!;

    [JsonPropertyName("refresh")]
    public string RefreshToken { get; set; } = null!;
}
