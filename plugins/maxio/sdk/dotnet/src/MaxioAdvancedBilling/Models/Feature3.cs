using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

public record Feature3
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("value")]
    public string? Value { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("periodicity_interval")]
    public int? PeriodicityInterval { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("periodicity_unit")]
    public EntitlementPeriodicityUnit? PeriodicityUnit { get; init; }

    /// <summary>
    /// When <c>true</c>, the new <c>value</c>/periodicity is immediately applied to every existing entitlement created from this feature catalog item.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("propagate_to_subscriptions")]
    public bool? PropagateToSubscriptions { get; init; } = false;

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
