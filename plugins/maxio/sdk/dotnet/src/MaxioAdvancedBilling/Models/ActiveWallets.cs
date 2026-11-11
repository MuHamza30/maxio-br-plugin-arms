using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record ActiveWallets
{
    /// <summary>
    /// Total count of active wallets drawing on this product.
    /// </summary>
    [JsonPropertyName("total")]
    public required int Total { get; init; }

    /// <summary>
    /// Up to 50 of the most recently created active wallets, newest first.
    /// </summary>
    [JsonPropertyName("sample")]
    public required IReadOnlyList<WalletProductActiveWallet> Sample { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
