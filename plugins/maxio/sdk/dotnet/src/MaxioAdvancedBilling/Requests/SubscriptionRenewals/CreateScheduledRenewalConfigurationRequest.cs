using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionRenewals;

/// <summary>
/// The inputs of the CreateScheduledRenewalConfiguration operation.
/// </summary>
public sealed record CreateScheduledRenewalConfigurationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public ScheduledRenewalConfigurationRequest? Body { get; init; }
}
