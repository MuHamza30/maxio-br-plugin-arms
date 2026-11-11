using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// Returned by an adjustment: the ledger row written, plus the wallet's new balance.
/// </summary>
public record WalletGrantResultResponse
{
    [JsonPropertyName("wallet_transactions")]
    public required IReadOnlyList<WalletTransaction> WalletTransactions { get; init; }

    /// <summary>
    /// The wallet's new balance, as a decimal string.
    /// </summary>
    [JsonPropertyName("balance")]
    public required string Balance { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
