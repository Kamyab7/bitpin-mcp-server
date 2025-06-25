using System.Text.Json.Serialization;

namespace Bitpin.Client.Models;

public class Ticker
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("price")]
    public string Price { get; set; } = null!;

    [JsonPropertyName("daily_change_price")]
    public decimal? DailyChangePrice { get; set; }

    [JsonPropertyName("low")]
    public string? Low { get; set; }

    [JsonPropertyName("high")]
    public string? High { get; set; }

    [JsonPropertyName("timestamp")]
    public double? Timestamp { get; set; }
}
