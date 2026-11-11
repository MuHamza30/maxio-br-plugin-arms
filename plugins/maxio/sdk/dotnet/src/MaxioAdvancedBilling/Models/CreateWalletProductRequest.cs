using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// Does not accept <c>product_family_id</c>: a Wallet Product never belongs to a product family. The underlying endpoint accepts the param, but only to reject it; sending one returns <c>422</c> ("Product family cannot be set on a wallet product") rather than silently ignoring it.
/// </summary>
public record CreateWalletProductRequest
{
    [JsonPropertyName("wallet_product")]
    public required WalletProduct WalletProduct { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
