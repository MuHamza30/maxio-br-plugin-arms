using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionGroups;

/// <summary>
/// The inputs of the UpdateSubscriptionGroupMembers operation.
/// </summary>
public sealed record UpdateSubscriptionGroupMembersRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }

    public UpdateSubscriptionGroupRequest? Body { get; init; }
}
