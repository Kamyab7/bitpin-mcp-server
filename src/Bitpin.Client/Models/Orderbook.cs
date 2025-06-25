using System.Text.Json.Serialization;

namespace Bitpin.Client.Models;

public class Orderbook
{
    [JsonPropertyName("asks")]
    public List<List<string>> Asks { get; set; } = new();

    [JsonPropertyName("bids")]
    public List<List<string>> Bids { get; set; } = new();
}