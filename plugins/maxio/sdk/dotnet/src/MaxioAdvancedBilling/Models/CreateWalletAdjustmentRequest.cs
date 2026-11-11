using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A signed correction to a specific grant's balance, or to an overdraft entry's usage, intended for support and ops use.
/// </summary>
public record CreateWalletAdjustmentRequest
{
    [JsonPropertyName("adjustment")]
    public required Adjustment Adjustment { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
