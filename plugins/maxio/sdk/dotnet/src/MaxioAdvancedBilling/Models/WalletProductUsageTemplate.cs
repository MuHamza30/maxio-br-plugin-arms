using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// One wallet template billing through this Wallet Product, joined to the Feature and plan/component that assigns it, when there is one.
/// </summary>
public record WalletProductUsageTemplate
{
    /// <summary>
    /// The name of the product or component the Feature is assigned to. <c>null</c> when the wallet template has no assigning Feature.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("plan_name")]
    public string? PlanName { get; init; }

    /// <summary>
    /// The label set in <c>wallet_behavior.name</c> on the Feature, or the standalone wallet's own <c>name</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("wallet_template_name")]
    public string? WalletTemplateName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("unit_name")]
    public string? UnitName { get; init; }

    /// <summary>
    /// A best-effort label for how this wallet is expected to be replenished. Currently reports <c>"Overage"</c> when the wallet template allows a negative balance, and <c>null</c> otherwise; there's no persisted field yet distinguishing, for example, an intended "Auto top-up" from "Purchase more" setup. Treat this as informational, not a stable enum.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("purchase_mechanism")]
    public string? PurchaseMechanism { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point_name")]
    public string? PricePointName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }

    /// <summary>
    /// <c>false</c> when the assigning Feature is archived.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("active")]
    public bool? Active { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("feature_template_id")]
    public int? FeatureTemplateId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("feature_template_name")]
    public string? FeatureTemplateName { get; init; }

    /// <summary>
    /// <c>true</c> when this wallet template is priced at a price point other than the Wallet Product's default.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point_override")]
    public bool? PricePointOverride { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
