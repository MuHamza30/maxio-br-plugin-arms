using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A summary of the Wallet Product's default price point, for rendering a catalog listing without a separate price points call.
/// </summary>
public record WalletProductPricePointSummary
{
    /// <summary>
    /// Number of archived price points on this Wallet Product.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("archived_count")]
    public int? ArchivedCount { get; init; }

    /// <summary>
    /// The default price point's pricing scheme. <c>null</c> if the Wallet Product has no default price point.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("pricing_scheme")]
    public string? PricingScheme { get; init; }

    /// <summary>
    /// Number of price brackets on the default price point. A <c>per_unit</c> scheme always stores exactly one synthesized bracket, so this is <c>1</c> for <c>per_unit</c>, not <c>0</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("tier_count")]
    public int? TierCount { get; init; }

    /// <summary>
    /// The unit price of the default price point's first bracket, as a decimal string. <c>null</c> if there are no brackets.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("entry_price")]
    public string? EntryPrice { get; init; }

    /// <summary>
    /// The unit price of the default price point's last bracket, as a decimal string. <c>null</c> if there are no brackets.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("final_price")]
    public string? FinalPrice { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
