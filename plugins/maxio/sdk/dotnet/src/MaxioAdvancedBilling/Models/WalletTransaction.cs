using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// One append-only row on the wallet's ledger. Always appears bare inside a <c>wallet_transactions</c> array, never wrapped in its own key.
/// </summary>
public record WalletTransaction
{
    /// <summary>
    /// The Advanced Billing id of the ledger row.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("wallet_id")]
    public int? WalletId { get; init; }

    /// <summary>
    /// The kind of ledger row. Known values: <c>grant</c>, <c>debit</c>, <c>adjustment</c>, <c>initial_balance</c>, and <c>expiration</c>. More kinds are expected over time, so treat this as an open string and don't fail on one you don't recognize. Only <c>grant</c> and <c>debit</c> are accepted on <see href="$e/Wallet%20Transactions/createWalletTransaction">Create Wallet Transaction</see>. <c>adjustment</c> rows are written by <see href="$e/Wallet%20Adjustments/createWalletAdjustment">Create Wallet Adjustment</see>, <c>initial_balance</c> rows only by a seeded <see href="$e/Wallets/createWallet">Create Wallet</see>, and <c>expiration</c> rows only by the automatic expiry sweep that writes off an unconsumed grant once its <c>expires_at</c> passes.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("kind")]
    public string? Kind { get; init; }

    /// <summary>
    /// A decimal string, <b>signed</b> on this resource. Non-negative for grants, negative for debits and expirations, either sign for adjustments. A grant row written after an overdraft repayment carries only what was left after the repayment (possibly <c>"0.0"</c>), not the grant's <c>original</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>
    /// The grant this row is attributed to. <c>null</c> for an overdraft row (a <c>kind: debit</c> row draining the below-zero portion of a balance, attributed to no grant), for an overdraft-repayment row (the <c>kind: grant</c> row that pays an existing negative balance back to zero, written before any remainder becomes a new grant), and for an overdraft correction (a <c>kind: adjustment</c> row posted against an overdraft entry, which credits no grant).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("grant_id")]
    public int? GrantId { get; init; }

    /// <summary>
    /// For an <c>adjustment</c> row posted against a specific usage entry, the id of the <c>debit</c> row it corrects. <c>null</c> for every other kind, and for adjustments made against a grant's balance directly.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("source_transaction_id")]
    public int? SourceTransactionId { get; init; }

    /// <summary>
    /// For a <c>debit</c> row only, the signed sum (as a decimal string) of every adjustment posted against it so far (<c>"0.0"</c> when none). <c>null</c> for every other kind. The row's own <c>amount</c> is never rewritten, so a caller deriving the entry's corrected quantity nets this out of <c>amount</c>, and sends this value back as <c>expected_source_corrections</c> on a further correction.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("source_corrections_total")]
    public string? SourceCorrectionsTotal { get; init; }

    /// <summary>
    /// The wallet's posted balance immediately after this row was applied, as a decimal string. Compare it with the wallet's <c>posted_balance</c>, not its <c>balance</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("balance_after")]
    public string? BalanceAfter { get; init; }

    /// <summary>
    /// For a row that closed a metered usage window (an hourly close, a depletion, or an expiry write-off), the start of that window. <c>null</c> on every other row.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("metered_from_at")]
    public DateTimeOffset? MeteredFromAt { get; init; }

    /// <summary>
    /// The end of the usage window this row closed. <c>null</c> on every other row.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("metered_through_at")]
    public DateTimeOffset? MeteredThroughAt { get; init; }

    /// <summary>
    /// The meter-to-wallet-unit conversion rate applied to compute this row's <c>amount</c>, as a decimal string. <c>null</c> on every other row.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("conversion_rate_applied")]
    public string? ConversionRateApplied { get; init; }

    /// <summary>
    /// The uid of the invoice behind a purchase's rows: the grant-creating row and, when the purchase repaid an overdraft, the repayment row. <c>null</c> on every other row.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("invoice_uid")]
    public string? InvoiceUid { get; init; }

    /// <summary>
    /// Free-text note. For an <c>adjustment</c> row, carries the required <c>reason</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("memo")]
    public string? Memo { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
