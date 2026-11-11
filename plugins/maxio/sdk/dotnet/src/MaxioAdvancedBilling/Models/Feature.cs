using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

public record Feature
{
    /// <summary>
    /// A unique, lowercase, underscore-separated identifier for the feature. Immutable once set.
    /// </summary>
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    /// <summary>
    /// The display name of the feature.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// The behavior of a feature:
    /// - <c>access_right</c>: a boolean entitlement. A subscriber either has access or does not.
    /// - <c>usage_limit</c>: a quantified allowance measured over a recurring period (for example, "10,000 API calls per month").
    /// - <c>service_right</c>: a free-form value (text, boolean, or number) that isn't a simple access flag or a metered limit.
    /// </summary>
    [JsonPropertyName("kind")]
    public required FeatureKind Kind { get; init; }

    /// <summary>
    /// Required when <c>kind</c> is <c>usage_limit</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }

    /// <summary>
    /// Required when <c>kind</c> is <c>service_right</c>. Ignored for other kinds, where it is inferred automatically.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("value_type")]
    public FeatureValueType? ValueType { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("default_value")]
    public string? DefaultValue { get; init; }

    /// <summary>
    /// Only valid when <c>kind</c> is <c>usage_limit</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("default_periodicity_interval")]
    public int? DefaultPeriodicityInterval { get; init; }

    /// <summary>
    /// Only valid when <c>kind</c> is <c>usage_limit</c>. Must be set together with <c>default_periodicity_interval</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("default_periodicity_unit")]
    public EntitlementPeriodicityUnit? DefaultPeriodicityUnit { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
