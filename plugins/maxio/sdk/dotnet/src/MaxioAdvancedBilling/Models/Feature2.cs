using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

public record Feature2
{
    /// <summary>
    /// The id of the feature template to attach.
    /// </summary>
    [JsonPropertyName("feature_template_id")]
    public required int FeatureTemplateId { get; init; }

    [JsonPropertyName("value")]
    public required string Value { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("periodicity_interval")]
    public int? PeriodicityInterval { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("periodicity_unit")]
    public EntitlementPeriodicityUnit? PeriodicityUnit { get; init; }

    /// <summary>
    /// Omit to have this feature catalog item apply to every price point of the product/component. Set together with <c>price_point_id</c> to scope the feature catalog item to a single price point.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point_type")]
    public FeatureOwnerPricePointType? PricePointType { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point_id")]
    public int? PricePointId { get; init; }

    /// <summary>
    /// When <c>true</c>, existing subscriptions on this product/component are immediately granted an entitlement for this feature, instead of waiting for their next subscription change.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("propagate_to_subscriptions")]
    public bool? PropagateToSubscriptions { get; init; } = false;

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
