﻿using System.Text.Json.Serialization;

namespace BitpinClient.Models;

public class GetTokenRequest
{
    [JsonPropertyName("api_key")]
    public string Key { get; set; } = null!;

    [JsonPropertyName("secret_key")]
    public string Secret { get; set; } = null!;
}
