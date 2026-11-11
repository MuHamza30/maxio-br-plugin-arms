using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionRenewals;

/// <summary>
/// The inputs of the ScheduleScheduledRenewalLockIn operation.
/// </summary>
public sealed record ScheduleScheduledRenewalLockInRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The renewal id.
    /// </summary>
    public required int Id { get; init; }

    public ScheduledRenewalLockInRequest? Body { get; init; }
}
