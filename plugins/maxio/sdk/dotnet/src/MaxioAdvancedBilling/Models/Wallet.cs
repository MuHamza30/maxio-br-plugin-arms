using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A balance, denominated in the unit of a Wallet Product, owned by a subscription. Grants and recurring funding schedules are embedded so a single read shows the full balance breakdown.
/// </summary>
public record Wallet
{
    /// <summary>
    /// The Advanced Billing id of the wallet.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// The polymorphic owner type. <c>subscription</c> is the only value currently supported.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("owner_type")]
    public OwnerType? OwnerType { get; init; }

    /// <summary>
    /// The id of the owning subscription.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("owner_id")]
    public int? OwnerId { get; init; }

    /// <summary>
    /// (Optional) A merchant-facing label for the wallet, set on create and changeable with <see href="$e/Wallets/updateWallet">Update Wallet</see>. Surrounding spaces are stripped; a blank value is stored as <c>null</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// The id of the <see href="$e/Wallet%20Products/createWalletProduct">Wallet Product</see> that denominates the wallet. Immutable for the life of the wallet.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("wallet_product_id")]
    public int? WalletProductId { get; init; }

    /// <summary>
    /// The name of the Wallet Product that denominates the wallet.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("wallet_product_name")]
    public string? WalletProductName { get; init; }

    /// <summary>
    /// The billing product's unit name, immutable for the life of the wallet.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }

    /// <summary>
    /// The price point that priced this wallet's <see href="$e/Wallet%20Purchases/createWalletPurchase">purchases</see>, set on create and immutable afterward. Defaults to the Wallet Product's default price point; <c>null</c> if the Wallet Product has none and none was given on create.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point_id")]
    public int? PricePointId { get; init; }

    /// <summary>
    /// What the wallet can spend now, encoded as a decimal string to avoid floating-point precision loss. This is the number to enforce against. For a metered wallet, it includes usage not yet posted to the ledger, through <c>metered_through_at</c>, but only in the response from <see href="$e/Wallets/readWallet">Read Wallet</see>; every other response (list, create, update, archive) reports the balance as of the last ledger write, equal to <c>posted_balance</c>, even for a metered wallet. The grants' <c>remaining</c> follow this figure, so they add up to it whenever it is zero or more.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("balance")]
    public string? Balance { get; init; }

    /// <summary>
    /// The balance from written ledger entries alone: grants funded, minus debits written to the ledger, as a decimal string. Always present, also for a wallet with no meter. It equals <c>balance</c> wherever no live metered balance is computed: list, create, update, and archive responses, an unlinked or archived wallet, and a read where <c>metered_stale</c> is <c>true</c>. Only an active, metered wallet read through <see href="$e/Wallets/readWallet">Read Wallet</see>, with <c>metered_stale</c> not <c>true</c>, can have a <c>balance</c> below its <c>posted_balance</c>. It stays exact when <c>metered_stale</c> is <c>true</c>. Reconcile the transactions list (<c>balance_after</c>) against this figure, not against <c>balance</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("posted_balance")]
    public string? PostedBalance { get; init; }

    /// <summary>
    /// The metering meter this wallet is paired with, or <c>null</c> for a wallet with no meter. Set on create and immutable afterward.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("meter")]
    public Meter? Meter { get; init; }

    /// <summary>
    /// The point up to which live meter usage has been factored into <c>balance</c>. Populated only in the response from <see href="$e/Wallets/readWallet">Read Wallet</see>, for an active, metered wallet on a site with metered consumption enabled; <c>null</c> everywhere else, including list, create, update, and archive responses for the same wallet.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("metered_through_at")]
    public DateTimeOffset? MeteredThroughAt { get; init; }

    /// <summary>
    /// How many seconds to wait before re-reading <c>balance</c> to see usage past <c>metered_through_at</c>. Same availability as <c>metered_through_at</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("metered_recheck_after_seconds")]
    public int? MeteredRecheckAfterSeconds { get; init; }

    /// <summary>
    /// <c>true</c> when live usage could not be read fresh and <c>balance</c> reflects an earlier snapshot. Same availability as <c>metered_through_at</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("metered_stale")]
    public bool? MeteredStale { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("status")]
    public WalletStatus? Status { get; init; }

    /// <summary>
    /// What happens when a debit would take the wallet below zero. Known values: <c>hard_stop</c> (the only value you can set today). More values are expected over time, so treat this as an open string and don't fail on one you don't recognize. At the zero boundary, <c>hard_stop</c> behaves as set by <c>hard_stop_overflow</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("depletion_policy")]
    public string? DepletionPolicy { get; init; }

    /// <summary>
    /// Behavior at the hard-stop boundary. <c>discard</c> drains the wallet to zero and rejects the excess (the default). <c>allow_negative</c> deducts the full amount, letting the balance go negative. The excess past the wallet's grants is booked as an overdraft ledger row with <c>grant_id</c> set to <c>null</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("hard_stop_overflow")]
    public WalletHardStopOverflow? HardStopOverflow { get; init; }

    /// <summary>
    /// How the wallet came to exist. <c>manual</c> for a wallet created directly with <see href="$e/Wallets/createWallet">Create Wallet</see>. <c>feature</c> for a wallet auto-provisioned because a subscription resolved a usage-limit entitlement whose Feature has <c>wallet_behavior</c> configured.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("origin")]
    public WalletOrigin? Origin { get; init; }

    /// <summary>
    /// The key of the Feature this wallet was provisioned for, when <c>origin</c> is <c>feature</c>. <c>null</c> for a <c>manual</c> wallet.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("feature_key")]
    public string? FeatureKey { get; init; }

    /// <summary>
    /// Every grant on the wallet, including ones already spent or expired, in drain order (the order debits would consume the still-open ones in): the soonest-expiring grant first, evergreen grants last.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("grants")]
    public IReadOnlyList<WalletGrant>? Grants { get; init; }

    /// <summary>
    /// The wallet's automatic funding schedules, if any. Empty for a <c>manual</c> wallet with no funding entitlement. A wallet funds automatically when it's provisioned from a Feature with <c>wallet_behavior</c> configured; see <see href="page:introduction/basic-concepts/wallets#provisioning-a-wallet-from-a-feature">the Wallets concept guide</see>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("recurring_funding")]
    public IReadOnlyList<WalletRecurringFunding>? RecurringFunding { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
