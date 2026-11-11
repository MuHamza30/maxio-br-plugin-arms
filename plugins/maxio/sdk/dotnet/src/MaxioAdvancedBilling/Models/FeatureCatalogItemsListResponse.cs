using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record FeatureCatalogItemsListResponse
{
    [JsonPropertyName("features")]
    public required IReadOnlyList<FeatureCatalogItem> Features { get; init; }

    /// <summary>
    /// The number of subscriptions on this product/component that would be affected if a feature catalog item change were propagated with <c>propagate_to_subscriptions=true</c>.
    /// </summary>
    [JsonPropertyName("subscriptions_count")]
    public required int SubscriptionsCount { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
