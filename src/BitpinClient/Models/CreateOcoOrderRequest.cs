using System.Text.Json.Serialization;

namespace BitpinClient.Models;

public class CreateOcoOrderRequest
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("side")]
    public string Side { get; set; } = null!;

    [JsonPropertyName("base_amount")]
    public decimal BaseAmount { get; set; }

    [JsonPropertyName("oco_target_price")]
    public decimal OcoTargetPrice { get; set; }

    [JsonPropertyName("stop_price")]
    public decimal StopPrice { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
} 