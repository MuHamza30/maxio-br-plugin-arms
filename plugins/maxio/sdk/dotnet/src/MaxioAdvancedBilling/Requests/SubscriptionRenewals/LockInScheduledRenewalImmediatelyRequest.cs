namespace MaxioAdvancedBilling.Requests.SubscriptionRenewals;

/// <summary>
/// The inputs of the LockInScheduledRenewalImmediately operation.
/// </summary>
public sealed record LockInScheduledRenewalImmediatelyRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The renewal id.
    /// </summary>
    public required int Id { get; init; }
}
