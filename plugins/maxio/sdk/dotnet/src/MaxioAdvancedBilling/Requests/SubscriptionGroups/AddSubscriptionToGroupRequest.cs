using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionGroups;

/// <summary>
/// The inputs of the AddSubscriptionToGroup operation.
/// </summary>
public sealed record AddSubscriptionToGroupRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public AddSubscriptionToAGroup? Body { get; init; }
}
