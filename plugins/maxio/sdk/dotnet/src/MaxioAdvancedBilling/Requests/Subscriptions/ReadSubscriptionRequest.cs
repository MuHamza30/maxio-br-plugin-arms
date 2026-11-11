using System.Collections.Generic;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.Subscriptions;

/// <summary>
/// The inputs of the ReadSubscription operation.
/// </summary>
public sealed record ReadSubscriptionRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// Allows including additional data in the response. Use in query: <c>include[]=coupons&amp;include[]=self_service_page_token</c>.
    /// </summary>
    public IReadOnlyList<SubscriptionInclude>? Include { get; init; }
}
