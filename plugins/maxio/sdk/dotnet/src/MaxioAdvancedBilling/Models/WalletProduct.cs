using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

public record WalletProduct
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// The name of the unit the Wallet Product's usage is measured in, for example <c>tokens</c>.
    /// </summary>
    [JsonPropertyName("unit_name")]
    public required string UnitName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("handle")]
    public string? Handle { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("taxable")]
    public bool? Taxable { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("tax_code")]
    public string? TaxCode { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("accounting_code")]
    public string? AccountingCode { get; init; }

    /// <summary>
    /// One of the following: Business Software, Consumer Software, Digital Services, Physical Goods, Other
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("item_category")]
    public ItemCategory? ItemCategory { get; init; }

    /// <summary>
    /// Creates one price point for the Wallet Product in the same call. Ignored if <c>price_points</c> is also sent.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point")]
    public WalletProductPricePointAttributes? PricePoint { get; init; }

    /// <summary>
    /// Creates several price points for the Wallet Product in the same call, winning over <c>price_point</c> if both are sent. The first price point created becomes the Wallet Product's default. The whole call is one transaction: if any price point in the array fails validation, nothing is created, including the Wallet Product itself.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_points")]
    public IReadOnlyList<WalletProductPricePointAttributes>? PricePoints { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
