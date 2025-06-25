using System.Text.Json.Serialization;

namespace Bitpin.Client.Models;

public class Match
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("price")]
    public string Price { get; set; } = null!;

    [JsonPropertyName("base_amount")]
    public string BaseAmount { get; set; } = null!;

    [JsonPropertyName("quote_amount")]
    public string QuoteAmount { get; set; } = null!;

    [JsonPropertyName("side")]
    public string Side { get; set; } = null!;
}
