using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A rollup of what's using this Wallet Product, for the catalog listing.
/// </summary>
public record WalletProductUsageCount
{
    /// <summary>
    /// Number of wallet templates billing through this product, whether or not any wallet has been created from them yet.
    /// </summary>
    [JsonPropertyName("wallet_configurations")]
    public required int WalletConfigurations { get; init; }

    /// <summary>
    /// Number of active (non-archived) wallets currently denominated by this product.
    /// </summary>
    [JsonPropertyName("active_wallets")]
    public required int ActiveWallets { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
