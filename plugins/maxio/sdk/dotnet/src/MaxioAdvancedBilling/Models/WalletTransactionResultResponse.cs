using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// Returned by <see href="$e/Wallet%20Transactions/createWalletTransaction">Create Wallet Transaction</see>. <c>wallet_transactions</c> and <c>balance</c> are always present. <c>requested_amount</c>, <c>drained_amount</c>, and <c>rejected_amount</c> are present only when the request's <c>kind</c> was <c>debit</c>.
/// </summary>
public record WalletTransactionResultResponse
{
    [JsonPropertyName("wallet_transactions")]
    public required IReadOnlyList<WalletTransaction> WalletTransactions { get; init; }

    /// <summary>
    /// <c>kind: debit</c> only. The amount that was requested, as a decimal string.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("requested_amount")]
    public string? RequestedAmount { get; init; }

    /// <summary>
    /// <c>kind: debit</c> only. The amount actually drained from the wallet's grants, as a decimal string.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("drained_amount")]
    public string? DrainedAmount { get; init; }

    /// <summary>
    /// <c>kind: debit</c> only, as a decimal string. Under <c>hard_stop_overflow: discard</c>, this is the requested amount minus what the wallet could cover (<c>"0.0"</c> on a full drain). Always <c>"0.0"</c> under <c>allow_negative</c>; nothing is rejected, and the balance goes negative instead.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("rejected_amount")]
    public string? RejectedAmount { get; init; }

    /// <summary>
    /// The wallet's new balance, as a decimal string.
    /// </summary>
    [JsonPropertyName("balance")]
    public required string Balance { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
