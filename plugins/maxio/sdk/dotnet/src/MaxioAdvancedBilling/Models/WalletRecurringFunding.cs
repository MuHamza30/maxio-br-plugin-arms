using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// Describes automatic funding of the wallet from a usage-limit entitlement. Set up by configuring <c>wallet_behavior</c> on the Feature behind that entitlement, not by any parameter on the wallet itself. This field is an array as of its first release. The array currently holds zero or one element, and is expected to hold several once a wallet can be funded by more than one entitlement.
/// </summary>
public record WalletRecurringFunding
{
    /// <summary>
    /// The Advanced Billing id of the recurring funding schedule.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// The usage-limit entitlement this schedule reads its allowance and cadence from.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("entitlement_id")]
    public int? EntitlementId { get; init; }

    /// <summary>
    /// The entitlement's current allowance, read fresh at request time rather than stored on the wallet. Editing the entitlement changes this immediately, with no separate sync step. <c>"0"</c> means the entitlement is configured to grant nothing, a valid, working configuration and not an error. <c>null</c> when <c>status</c> is <c>stopped</c>, since there is no longer an entitlement to read.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("allowance")]
    public string? Allowance { get; init; }

    /// <summary>
    /// When the next automatic grant is due. Grants are applied by a background process shortly after this moment passes, not exactly on it, so a wallet read taken in that window still shows the pre-grant balance. <c>null</c> when <c>status</c> is <c>stopped</c>, and also <c>null</c> once the owning subscription has terminated, even while <c>status</c> is still <c>active</c>: at its next due moment the schedule is stopped with reason <c>subscription_terminated</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("next_grant_at")]
    public DateTimeOffset? NextGrantAt { get; init; }

    /// <summary>
    /// The calendar day <c>next_grant_at</c> falls on, resolved in the site's time zone. <c>null</c> under the same conditions as <c>next_grant_at</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("next_grant_on")]
    public DateTimeOffset? NextGrantOn { get; init; }

    /// <summary>
    /// Cumulative count of due moments that passed without a grant being made (for example, during an outage). A gap in funding is visible here without cross-referencing the ledger.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("skipped_periods")]
    public int? SkippedPeriods { get; init; }

    /// <summary>
    /// <c>active</c>: refilling normally. <c>erroring</c>: the schedule is still due and retrying, but the last attempt couldn't be written (see <c>reason</c>); correcting the underlying entitlement resumes funding on the next run with no action needed on the wallet itself. <c>stopped</c>: funding has ended for good (see <c>reason</c>); units already granted stay on the wallet and remain spendable, but no further automatic grants will be made.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("status")]
    public WalletRecurringFundingStatus? Status { get; init; }

    /// <summary>
    /// Set whenever <c>status</c> isn't <c>active</c>. Accompanying <c>stopped</c>: <c>entitlement_missing</c> (an ordinary product or component change removed the entitlement), <c>wallet_archived</c> (the wallet was archived), <c>subscription_terminated</c> (the subscription reached a terminal state), <c>feature_removed</c> (the subscription no longer has this Feature), <c>wallet_behavior_disabled</c> (the Feature no longer has <c>wallet_behavior</c> configured), <c>superseded_by_replacement</c> (a later entitlement now funds this wallet instead), or <c>subscription_prepaid</c> (the subscription switched to prepaid, which cannot carry an active wallet). Accompanying <c>erroring</c>: <c>allowance_not_representable</c> (the entitlement's current allowance doesn't fit the wallet's amount encoding) or <c>balance_at_capacity</c> (granting it would push the balance past the maximum supported value).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("reason")]
    public Reason? Reason { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
