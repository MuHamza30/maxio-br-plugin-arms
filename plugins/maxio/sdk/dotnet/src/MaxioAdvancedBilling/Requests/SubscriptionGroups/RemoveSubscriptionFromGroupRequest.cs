namespace MaxioAdvancedBilling.Requests.SubscriptionGroups;

/// <summary>
/// The inputs of the RemoveSubscriptionFromGroup operation.
/// </summary>
public sealed record RemoveSubscriptionFromGroupRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }
}
