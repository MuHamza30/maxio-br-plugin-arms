namespace MaxioAdvancedBilling.Requests.SubscriptionRenewals;

/// <summary>
/// The inputs of the CancelScheduledRenewalConfiguration operation.
/// </summary>
public sealed record CancelScheduledRenewalConfigurationRequest
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
