using System.Text.Json.Serialization;

namespace BitpinClient.Models;

public class Currency
{
    [JsonPropertyName("currency")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("tradable")]
    public bool Tradebale { get; set; }

    [JsonPropertyName("precision")]
    public string Precision { get; set; } = null!;
}
