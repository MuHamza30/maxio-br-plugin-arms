using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record AggregatedEntitlementsResponse
{
    [JsonPropertyName("subscription_id")]
    public required int SubscriptionId { get; init; }

    [JsonPropertyName("customer_id")]
    public required int CustomerId { get; init; }

    /// <summary>
    /// The subscription's current state, e.g. <c>active</c>, <c>trialing</c>, <c>canceled</c>.
    /// </summary>
    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("entitlements")]
    public required IReadOnlyList<AggregatedEntitlement> Entitlements { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
