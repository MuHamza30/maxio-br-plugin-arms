using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record WalletPurchaseResultResponse
{
    [JsonPropertyName("wallet_purchases")]
    public required IReadOnlyList<WalletPurchase> WalletPurchases { get; init; }

    /// <summary>
    /// The wallet's new balance, as a decimal string.
    /// </summary>
    [JsonPropertyName("balance")]
    public required string Balance { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
