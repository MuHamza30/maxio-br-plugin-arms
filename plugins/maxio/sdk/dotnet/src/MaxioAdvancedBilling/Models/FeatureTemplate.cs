using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A feature that can be granted to subscribers, defined once at the site level and then attached to products or components.
/// </summary>
public record FeatureTemplate
{
    /// <summary>
    /// The Advanced Billing id of the feature template.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// A unique, lowercase, underscore-separated identifier for the feature. Immutable once set.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("key")]
    public string? Key { get; init; }

    /// <summary>
    /// The display name of the feature.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// The behavior of a feature:
    /// - <c>access_right</c>: a boolean entitlement. A subscriber either has access or does not.
    /// - <c>usage_limit</c>: a quantified allowance measured over a recurring period (for example, "10,000 API calls per month").
    /// - <c>service_right</c>: a free-form value (text, boolean, or number) that isn't a simple access flag or a metered limit.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("kind")]
    public FeatureKind? Kind { get; init; }

    /// <summary>
    /// The unit the feature is measured in (for example, <c>requests</c> or <c>GB</c>). Required when <c>kind</c> is <c>usage_limit</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }

    /// <summary>
    /// The data type of a feature's value. For <c>access_right</c> features this is always <c>boolean</c>, and for <c>usage_limit</c> features this is always <c>numeric</c>. For <c>service_right</c> features, you choose the value type explicitly.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("value_type")]
    public FeatureValueType? ValueType { get; init; }

    /// <summary>
    /// A default value used to pre-populate new feature catalog items created from this template.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("default_value")]
    public string? DefaultValue { get; init; }

    /// <summary>
    /// For <c>usage_limit</c> features, the default periodicity interval used to pre-populate new feature catalog items. Always <c>null</c> for other kinds.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("default_periodicity_interval")]
    public int? DefaultPeriodicityInterval { get; init; }

    /// <summary>
    /// For <c>usage_limit</c> features, the default periodicity unit used to pre-populate new feature catalog items. Always <c>null</c> for other kinds.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("default_periodicity_unit")]
    public EntitlementPeriodicityUnit? DefaultPeriodicityUnit { get; init; }

    /// <summary>
    /// The date and time the feature template was archived, or <c>null</c> if it is active.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("archived_at")]
    public DateTimeOffset? ArchivedAt { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }

    /// <summary>
    /// The number of <b>components</b> this feature template is currently attached to via an active feature catalog item. Despite the name, this counts components, not products. In the Advanced Billing UI, components are labeled "Products."
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("products_count")]
    public int? ProductsCount { get; init; }

    /// <summary>
    /// The number of <b>products</b> this feature template is currently attached to via an active feature catalog item. Despite the name, this counts products, not plans. In the Advanced Billing UI, products are labeled "Plans."
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("plans_count")]
    public int? PlansCount { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
