using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record WalletsListResponse
{
    [JsonPropertyName("wallets")]
    public required IReadOnlyList<Wallet> Wallets { get; init; }

    [JsonPropertyName("meta")]
    public required ListPublicKeysMeta Meta { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
