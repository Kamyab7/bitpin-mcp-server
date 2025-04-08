using System.Text.Json.Serialization;

namespace BitpinClient.Models;

public class Orderbook
{
    [JsonPropertyName("asks")]
    public List<List<string>> Asks { get; set; } = new();

    [JsonPropertyName("bids")]
    public List<List<string>> Bids { get; set; } = new();
}