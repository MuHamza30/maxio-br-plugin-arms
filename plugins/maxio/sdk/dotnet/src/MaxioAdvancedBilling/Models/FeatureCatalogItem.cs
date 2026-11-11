using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A feature template attached to a specific product or component (or one of their price points), with a concrete value. When a subscriber signs up for or is assigned this product/component, the feature catalog item is provisioned as an entitlement on their subscription.
/// </summary>
public record FeatureCatalogItem
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// The id of the feature template this item was created from.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("feature_template_id")]
    public int? FeatureTemplateId { get; init; }

    /// <summary>
    /// The <c>key</c> of the parent feature template.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("feature_key")]
    public string? FeatureKey { get; init; }

    /// <summary>
    /// The <c>name</c> of the parent feature template.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("feature_name")]
    public string? FeatureName { get; init; }

    /// <summary>
    /// The behavior of a feature:
    /// - <c>access_right</c>: a boolean entitlement. A subscriber either has access or does not.
    /// - <c>usage_limit</c>: a quantified allowance measured over a recurring period (for example, "10,000 API calls per month").
    /// - <c>service_right</c>: a free-form value (text, boolean, or number) that isn't a simple access flag or a metered limit.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("feature_kind")]
    public FeatureKind? FeatureKind { get; init; }

    /// <summary>
    /// The value granted by this feature catalog item. Interpreted according to <c>feature_kind</c>: <c>"true"</c>/<c>"false"</c> for <c>access_right</c>, a numeric string for <c>usage_limit</c>, or any string for <c>service_right</c> (shaped by the feature template's <c>value_type</c>).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("value")]
    public string? Value { get; init; }

    /// <summary>
    /// Set when <c>feature_kind</c> is <c>usage_limit</c>; <c>null</c> otherwise.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("periodicity_interval")]
    public int? PeriodicityInterval { get; init; }

    /// <summary>
    /// Set when <c>feature_kind</c> is <c>usage_limit</c>; <c>null</c> otherwise.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("periodicity_unit")]
    public EntitlementPeriodicityUnit? PeriodicityUnit { get; init; }

    /// <summary>
    /// <c>null</c> when this feature catalog item applies to every price point of its owning product/component. Set when the feature catalog item is an override for one specific price point.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point_type")]
    public FeatureOwnerPricePointType? PricePointType { get; init; }

    /// <summary>
    /// Set together with <c>price_point_type</c> for price-point-specific overrides.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point_id")]
    public int? PricePointId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("archived_at")]
    public DateTimeOffset? ArchivedAt { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
