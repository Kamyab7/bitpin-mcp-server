using System.Text.Json.Serialization;

namespace Bitpin.Client.Models;

public class CreateLimitOrderRequest
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("side")]
    public string Side { get; set; } = null!;

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("base_amount")]
    public decimal BaseAmount { get; set; }
} 
