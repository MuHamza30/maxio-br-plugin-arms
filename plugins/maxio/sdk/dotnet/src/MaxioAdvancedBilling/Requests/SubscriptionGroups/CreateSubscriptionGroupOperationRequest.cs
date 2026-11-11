using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionGroups;

/// <summary>
/// The inputs of the CreateSubscriptionGroup operation.
/// </summary>
public sealed record CreateSubscriptionGroupOperationRequest
{
    public CreateSubscriptionGroupRequest? Body { get; init; }
}
