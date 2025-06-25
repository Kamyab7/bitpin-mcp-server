using System.Text.Json.Serialization;

namespace Bitpin.Client.Models;

public class Market
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("base")]
    public string Base { get; set; } = null!;

    [JsonPropertyName("quote")]
    public string Quote { get; set; } = null!;

    [JsonPropertyName("tradable")]
    public bool Tradable { get; set; }

    [JsonPropertyName("price_precision")]
    public int PricePrecision { get; set; }

    [JsonPropertyName("base_amount_precision")]
    public int BaseAmountPrecision { get; set; } 

    [JsonPropertyName("quote_amount_precision")]
    public int QuoteAmountPrecision { get; set; }
}

