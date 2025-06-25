using System.Text.Json.Serialization;

namespace Bitpin.Client.Models;

public class RefreshTokenResponse
{
    [JsonPropertyName("access")]
    public string AccessToken { get; set; } = null!;
}
