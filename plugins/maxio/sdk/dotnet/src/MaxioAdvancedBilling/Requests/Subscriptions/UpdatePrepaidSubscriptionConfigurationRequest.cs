using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Subscriptions;

/// <summary>
/// The inputs of the UpdatePrepaidSubscriptionConfiguration operation.
/// </summary>
public sealed record UpdatePrepaidSubscriptionConfigurationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public UpsertPrepaidConfigurationRequest? Body { get; init; }
}
