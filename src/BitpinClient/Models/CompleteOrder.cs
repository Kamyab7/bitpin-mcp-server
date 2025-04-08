using System.Text.Json.Serialization;

namespace BitpinClient.Models;

public class CompleteOrder
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("base_amount")]
    public string BaseAmount { get; set; } = null!;

    [JsonPropertyName("quote_amount")]
    public string QuoteAmount { get; set; } = null!;

    [JsonPropertyName("price")]
    public string Price { get; set; } = null!;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("commission")]
    public string Commission { get; set; } = null!;

    [JsonPropertyName("side")]
    public string Side { get; set; } = null!;

    [JsonPropertyName("commission_currency")]
    public string CommissionCurrency { get; set; } = null!;

    [JsonPropertyName("order_id")]
    public long OrderId { get; set; }

    [JsonPropertyName("identifier")]
    public string? Identifier { get; set; }
}