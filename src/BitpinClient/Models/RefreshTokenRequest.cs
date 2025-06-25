using System.Text.Json.Serialization;

namespace Bitpin.Client.Models;

public class RefreshTokenRequest
{
    [JsonPropertyName("refresh")]
    public string RefreshToken { get; set; } = null!;
}
