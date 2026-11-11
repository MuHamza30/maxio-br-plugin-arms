namespace MaxioAdvancedBilling.Requests.SubscriptionGroupStatus;

/// <summary>
/// The inputs of the InitiateDelayedCancellationForGroup operation.
/// </summary>
public sealed record InitiateDelayedCancellationForGroupRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }
}
