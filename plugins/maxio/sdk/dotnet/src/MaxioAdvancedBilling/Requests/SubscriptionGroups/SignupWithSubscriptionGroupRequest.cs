using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionGroups;

/// <summary>
/// The inputs of the SignupWithSubscriptionGroup operation.
/// </summary>
public sealed record SignupWithSubscriptionGroupRequest
{
    public SubscriptionGroupSignupRequest? Body { get; init; }
}
