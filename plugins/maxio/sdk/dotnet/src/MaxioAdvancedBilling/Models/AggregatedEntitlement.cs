using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// One entitlement in a subscriber's aggregated entitlements list. Entries are aggregated per feature key and periodicity window, not per feature key alone. A <c>usage_limit</c> feature granted with two different periodicities yields two entries sharing one <c>feature_key</c>. Use <c>periodicity_key</c> to identify an entry uniquely.
/// </summary>
public record AggregatedEntitlement
{
    /// <summary>
    /// The feature's key, prefixed by kind: <c>feature.*</c> for <c>access_right</c>, <c>usage.*</c> for <c>usage_limit</c>, <c>service.*</c> for <c>service_right</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("feature_key")]
    public string? FeatureKey { get; init; }

    /// <summary>
    /// Uniquely identifies this aggregated entry: the prefixed feature key, suffixed with <c>:{interval}:{unit}</c> when the entitlement has a periodicity window. Equal to <c>feature_key</c> when <c>periodicity</c> is <c>null</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("periodicity_key")]
    public string? PeriodicityKey { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// The behavior of a feature:
    /// - <c>access_right</c>: a boolean entitlement. A subscriber either has access or does not.
    /// - <c>usage_limit</c>: a quantified allowance measured over a recurring period (for example, "10,000 API calls per month").
    /// - <c>service_right</c>: a free-form value (text, boolean, or number) that isn't a simple access flag or a metered limit.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("type")]
    public FeatureKind? Type { get; init; }

    /// <summary>
    /// The aggregated value, coerced according to <c>type</c>: a boolean for <c>access_right</c> (and boolean <c>service_right</c>), a number for <c>usage_limit</c> (and numeric <c>service_right</c>), or a string for text <c>service_right</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("value")]
    public Value? Value { get; init; }

    /// <summary>
    /// <c>true</c> only when the aggregated value is truthy for this feature's kind, and the subscription is in a live state (<c>active</c>, <c>trialing</c>, <c>assessing</c>, <c>past_due</c>, or <c>soft_failure</c>). <c>false</c> otherwise, including for <c>awaiting_signup</c>, canceled, expired, and on-hold subscriptions. Entitlements deliberately stay enabled through dunning.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("periodicity")]
    public AggregatedEntitlementPeriodicity? Periodicity { get; init; }

    /// <summary>
    /// The names of the products/components contributing to this entitlement. For <c>access_right</c> features, only contributors that granted <c>true</c> are listed.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("source_products")]
    public IReadOnlyList<string>? SourceProducts { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
