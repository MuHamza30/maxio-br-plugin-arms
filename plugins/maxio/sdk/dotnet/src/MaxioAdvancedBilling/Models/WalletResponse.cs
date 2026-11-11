using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record WalletResponse
{
    /// <summary>
    /// A balance, denominated in the unit of a Wallet Product, owned by a subscription. Grants and recurring funding schedules are embedded so a single read shows the full balance breakdown.
    /// </summary>
    [JsonPropertyName("wallet")]
    public required Wallet Wallet { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
