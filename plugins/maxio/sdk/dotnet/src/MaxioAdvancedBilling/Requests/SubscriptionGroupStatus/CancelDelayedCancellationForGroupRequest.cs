namespace MaxioAdvancedBilling.Requests.SubscriptionGroupStatus;

/// <summary>
/// The inputs of the CancelDelayedCancellationForGroup operation.
/// </summary>
public sealed record CancelDelayedCancellationForGroupRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }
}
