using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Models;

public record Adjustment
{
    /// <summary>
    /// Signed: positive adds units to the grant, negative removes them. Bounded below by the grant's <c>posted_remaining</c> (a correction can't drive it negative) and above by the grant's <c>original</c> (a correction can't credit past what was originally granted). An out-of-bounds value is rejected rather than clamped. Zero is rejected. Without a <c>grant_id</c> (an overdraft correction), the amount must be positive, can't exceed the usage the overdraft entry still records, and can't exceed the overdraft the wallet still owes. Encoded as a decimal string with at most 8 fractional digits. Send fractional values as strings: a JSON number with a decimal point is rejected with <c>422</c>, because a double-precision float may already have lost precision before the request arrives.
    /// </summary>
    [JsonPropertyName("amount")]
    public required Amount7 Amount { get; init; }

    /// <summary>
    /// Required. Lands on the resulting ledger row's <c>memo</c>.
    /// </summary>
    [JsonPropertyName("reason")]
    [MaxLength(255)]
    public required string Reason { get; init; }

    /// <summary>
    /// The grant to correct. Must belong to this wallet. Required unless <c>source_transaction_id</c> names an overdraft entry (a <c>debit</c> with no <c>grant_id</c>, written when an <c>allow_negative</c> wallet consumes past its grants). For an overdraft entry, omit it: sending one returns <c>422</c>, and the correction credits the wallet's outstanding overdraft instead of any grant.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("grant_id")]
    public int? GrantId { get; init; }

    /// <summary>
    /// (Optional) The <c>debit</c> row this correction is fixing. Must belong to this wallet, and must name a row of kind <c>debit</c>; grant and adjustment rows are append-only history, corrected by posting a further adjustment rather than by pointing at them. For a grant-attributed debit, it's recorded on the resulting ledger row for audit only: it does not change how <c>amount</c> is applied, and more than one adjustment may name the same row. For an overdraft entry, it decides the path: omit <c>grant_id</c>, and the correction is credit-only, bounded by the usage the entry still records and by the overdraft the wallet still owes.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("source_transaction_id")]
    public int? SourceTransactionId { get; init; }

    /// <summary>
    /// (Optional) An optimistic-concurrency check for a caller whose <c>amount</c> was derived from the grant's own remainder. Send the grant's <c>posted_remaining</c>, not its <c>remaining</c>: on a metered wallet read through <see href="$e/Wallets/readWallet">Read Wallet</see>, <c>remaining</c> also subtracts usage not yet posted, so it can differ from what the adjustment checks. If given and the grant's current <c>posted_remaining</c> no longer matches, the request is rejected with <c>409</c> and nothing is written. Compared numerically, so <c>"100"</c> and <c>"100.00000000"</c> both match a <c>posted_remaining</c> of <c>100</c>. Omit the field entirely to skip the check; sending it as an empty string is treated as a malformed value (<c>422</c>), not as an omission.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("expected_remaining")]
    public string? ExpectedRemaining { get; init; }

    /// <summary>
    /// (Optional) An optimistic-concurrency check for a caller whose <c>amount</c> was derived from a usage entry, sent together with <c>source_transaction_id</c>: the entry's <c>source_corrections_total</c> as the caller last read it (<c>"0"</c> if none). If the sum the server computes no longer matches, the request is rejected with <c>409</c> and nothing is written. This is what catches a correction posted between that read and this request. Ignored unless <c>source_transaction_id</c> is also given.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("expected_source_corrections")]
    public string? ExpectedSourceCorrections { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
