using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Subscriptions;

/// <summary>
/// The inputs of the CreateSubscription operation.
/// </summary>
public sealed record CreateSubscriptionOperationRequest
{
    public CreateSubscriptionRequest? Body { get; init; }
}
