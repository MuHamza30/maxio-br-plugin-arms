using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionGroupStatus;

/// <summary>
/// The inputs of the ReactivateSubscriptionGroup operation.
/// </summary>
public sealed record ReactivateSubscriptionGroupOperationRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }

    public ReactivateSubscriptionGroupRequest? Body { get; init; }
}
