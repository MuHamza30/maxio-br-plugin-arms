using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A price point to create for the Wallet Product, in the same call that creates or updates it.
/// </summary>
public record WalletProductPricePointAttributes
{
    /// <summary>
    /// The price point's name. Required. Omitting it fails validation with a message that reads <c>Name: cannot be blank.</c>, not naming the price point.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("handle")]
    public string? Handle { get; init; }

    /// <summary>
    /// The identifier for the pricing scheme. See <see href="https://help.chargify.com/products/product-components.html">Product Components</see> for an overview of pricing schemes.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("pricing_scheme")]
    public PricingScheme? PricingScheme { get; init; }

    /// <summary>
    /// Required when <c>pricing_scheme</c> is <c>per_unit</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("unit_price")]
    public string? UnitPrice { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("tax_included")]
    public bool? TaxIncluded { get; init; }

    /// <summary>
    /// Requires Multifrequency to be enabled on the site.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("interval")]
    public int? Interval { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("interval_unit")]
    public IntervalUnit? IntervalUnit { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("use_site_exchange_rate")]
    public bool? UseSiteExchangeRate { get; init; }

    /// <summary>
    /// Required for every <c>pricing_scheme</c> other than <c>per_unit</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("prices")]
    public IReadOnlyList<Price1>? Prices { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
