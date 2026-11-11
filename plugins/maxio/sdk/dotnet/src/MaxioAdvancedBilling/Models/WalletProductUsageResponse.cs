using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record WalletProductUsageResponse
{
    /// <summary>
    /// Wallet templates billing through this product at its default price point.
    /// </summary>
    [JsonPropertyName("wallet_templates")]
    public required IReadOnlyList<WalletProductUsageTemplate> WalletTemplates { get; init; }

    /// <summary>
    /// Wallet templates billing through this product at a price point other than the default.
    /// </summary>
    [JsonPropertyName("price_point_overrides")]
    public required IReadOnlyList<WalletProductUsageTemplate> PricePointOverrides { get; init; }

    [JsonPropertyName("active_wallets")]
    public required ActiveWallets ActiveWallets { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
