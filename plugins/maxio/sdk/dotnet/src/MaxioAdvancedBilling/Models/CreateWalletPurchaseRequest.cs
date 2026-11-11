using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// Purchases prepaid units for the wallet at a price point, charging the subscription's payment method.
/// </summary>
public record CreateWalletPurchaseRequest
{
    [JsonPropertyName("purchase")]
    public required Purchase Purchase { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
