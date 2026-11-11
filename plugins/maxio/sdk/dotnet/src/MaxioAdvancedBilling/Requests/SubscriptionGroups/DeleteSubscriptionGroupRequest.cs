namespace MaxioAdvancedBilling.Requests.SubscriptionGroups;

/// <summary>
/// The inputs of the DeleteSubscriptionGroup operation.
/// </summary>
public sealed record DeleteSubscriptionGroupRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }
}
