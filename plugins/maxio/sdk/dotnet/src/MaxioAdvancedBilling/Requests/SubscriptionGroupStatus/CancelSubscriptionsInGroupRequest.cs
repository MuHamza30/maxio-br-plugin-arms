using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionGroupStatus;

/// <summary>
/// The inputs of the CancelSubscriptionsInGroup operation.
/// </summary>
public sealed record CancelSubscriptionsInGroupRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }

    public CancelGroupedSubscriptionsRequest? Body { get; init; }
}
