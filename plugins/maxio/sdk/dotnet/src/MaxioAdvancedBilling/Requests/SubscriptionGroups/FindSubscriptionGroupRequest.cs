namespace MaxioAdvancedBilling.Requests.SubscriptionGroups;

/// <summary>
/// The inputs of the FindSubscriptionGroup operation.
/// </summary>
public sealed record FindSubscriptionGroupRequest
{
    /// <summary>
    /// The Advanced Billing id of the subscription associated with the subscription group
    /// </summary>
    public required string SubscriptionId { get; init; }
}
