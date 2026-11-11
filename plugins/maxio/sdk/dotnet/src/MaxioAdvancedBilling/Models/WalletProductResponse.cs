using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A Wallet Product is a Component created with no product family, so it returns the full Component field set under a <c>wallet_product</c> root instead of <c>component</c>.
/// </summary>
public record WalletProductResponse
{
    [JsonPropertyName("wallet_product")]
    public required Component WalletProduct { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
