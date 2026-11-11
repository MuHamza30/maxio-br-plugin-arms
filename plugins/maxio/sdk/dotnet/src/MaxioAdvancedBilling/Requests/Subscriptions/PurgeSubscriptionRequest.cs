using System.Collections.Generic;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.Subscriptions;

/// <summary>
/// The inputs of the PurgeSubscription operation.
/// </summary>
public sealed record PurgeSubscriptionRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// id of the customer.
    /// </summary>
    public required int Ack { get; init; }

    /// <summary>
    /// Options are "customer" or "payment_profile".
    /// Use in query: <c>cascade[]=customer&amp;cascade[]=payment_profile</c>.
    /// </summary>
    public IReadOnlyList<SubscriptionPurgeType>? Cascade { get; init; }
}
