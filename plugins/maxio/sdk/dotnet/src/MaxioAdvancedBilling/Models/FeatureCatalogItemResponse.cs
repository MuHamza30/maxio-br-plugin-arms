using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record FeatureCatalogItemResponse
{
    /// <summary>
    /// A feature template attached to a specific product or component (or one of their price points), with a concrete value. When a subscriber signs up for or is assigned this product/component, the feature catalog item is provisioned as an entitlement on their subscription.
    /// </summary>
    [JsonPropertyName("feature")]
    public required FeatureCatalogItem Feature { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
