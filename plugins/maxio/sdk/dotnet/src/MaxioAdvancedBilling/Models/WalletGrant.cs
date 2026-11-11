using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A funding tranche embedded on the wallet. <c>original</c> is the amount the grant was created with; <c>posted_remaining</c> is what debits and adjustments written to the ledger have left; <c>remaining</c> is what can still be spent. Debits drain grants earliest-expiration-first (grants with no expiration are drained last).
/// </summary>
public record WalletGrant
{
    /// <summary>
    /// The Advanced Billing id of the grant.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// The memo the grant was created with, or `Grant
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// The amount the grant was created with, encoded as a decimal string. Never changes after creation.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("original")]
    public string? Original { get; init; }

    /// <summary>
    /// What the grant can still spend, encoded as a decimal string. On <see href="$e/Wallets/readWallet">Read Wallet</see> for a metered wallet whose <c>balance</c> is the live figure, this is <c>posted_remaining</c> less the pending usage drawn from this grant in drain order: usage past a grant spills to the next one, and usage past every grant is an overdraft that belongs to no grant. It never goes below zero, and the grants' <c>remaining</c> add up to the wallet's <c>balance</c> whenever <c>balance</c> is zero or more. Everywhere else it equals <c>posted_remaining</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("remaining")]
    public string? Remaining { get; init; }

    /// <summary>
    /// The grant's remainder from written ledger entries alone, encoded as a decimal string. Always present. This is the figure an adjustment checks <c>expected_remaining</c> and its bounds against, so send it back, not <c>remaining</c>, when correcting a grant.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("posted_remaining")]
    public string? PostedRemaining { get; init; }

    /// <summary>
    /// When this grant's unconsumed remainder is automatically written off. <c>null</c> for an evergreen grant.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("expires_at")]
    public DateTimeOffset? ExpiresAt { get; init; }

    /// <summary>
    /// The calendar day <c>expires_at</c> falls on, already resolved in the site's time zone.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("expires_on")]
    public DateTimeOffset? ExpiresOn { get; init; }

    /// <summary>
    /// When the expiry sweep actually wrote this grant's remainder off. Usually falls shortly after <c>expires_at</c>; use <c>expires_on</c> for the date the grant expired, not this field. <c>null</c> if the grant was never written off (including a grant simply spent to zero before its <c>expires_at</c>).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("expired_at")]
    public DateTimeOffset? ExpiredAt { get; init; }

    /// <summary>
    /// The calendar day <c>expired_at</c> falls on, resolved in the site's time zone.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("expired_on")]
    public DateTimeOffset? ExpiredOn { get; init; }

    /// <summary>
    /// How the grant was created. <c>recurring</c> comes from the wallet's automatic funding schedule (see the wallet's <c>recurring_funding</c> array). <c>manual</c> comes from a one-time grant posted to <see href="$e/Wallet%20Transactions/createWalletTransaction">Create Wallet Transaction</see>. <c>initial_balance</c> comes from a non-zero <c>initial_balance</c> at wallet creation. <c>allowance_increase</c> comes from raising a funding entitlement's allowance while it's already funding a wallet. <c>purchase</c> comes from <see href="$e/Wallet%20Purchases/createWalletPurchase">Create Wallet Purchase</see>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("origin")]
    public Origin? Origin { get; init; }

    /// <summary>
    /// For a <c>recurring</c> grant, the start of the funding cadence period this grant covers. <c>null</c> for every other origin. After a gap in funding (see <c>skipped_periods</c>), this is the period currently in progress, not the oldest period that was missed.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("granted_for_period_start")]
    public DateTimeOffset? GrantedForPeriodStart { get; init; }

    /// <summary>
    /// The uid of the invoice that paid for this grant, for a grant with <c>origin</c> of <c>purchase</c>. <c>null</c> for every other origin.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("invoice_uid")]
    public string? InvoiceUid { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
