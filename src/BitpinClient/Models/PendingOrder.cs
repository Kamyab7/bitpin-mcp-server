using System.Text.Json.Serialization;

namespace BitpinClient.Models
{
    public class PendingOrder
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("side")]
        public string Side { get; set; } = null!;

        [JsonPropertyName("base_amount")]
        public string BaseAmount { get; set; } = null!;

        [JsonPropertyName("quote_amount")]
        public string QuoteAmount { get; set; } = null!;

        [JsonPropertyName("price")]
        public string Price { get; set; } = null!;

        [JsonPropertyName("stop_price")]
        public string? StopPrice { get; set; }

        [JsonPropertyName("oco_target_price")]
        public string? OcoTargetPrice { get; set; }

        [JsonPropertyName("identifier")]
        public string? Identifier { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; } = null!;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("closed_at")]
        public DateTime? ClosedAt { get; set; }

        [JsonPropertyName("dealed_base_amount")]
        public string DealedBaseAmount { get; set; } = null!;

        [JsonPropertyName("dealed_quote_amount")]
        public string DealedQuoteAmount { get; set; } = null!;

        [JsonPropertyName("req_to_cancel")]
        public bool ReqToCancel { get; set; }

        [JsonPropertyName("commission")]
        public string Commission { get; set; } = null!;
    }
}