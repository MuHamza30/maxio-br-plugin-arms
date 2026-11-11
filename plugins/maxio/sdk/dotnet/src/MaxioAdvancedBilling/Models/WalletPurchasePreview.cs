using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A price quote for a purchase, without charging anything.
/// </summary>
public record WalletPurchasePreview
{
    [JsonPropertyName("price_point_id")]
    public required int PricePointId { get; init; }

    /// <summary>
    /// The three-letter currency code the <c>amount</c> is denominated in, matching the subscription's currency.
    /// </summary>
    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    /// <summary>
    /// The number of units the quote is for, as a decimal string.
    /// </summary>
    [JsonPropertyName("quantity")]
    public required string Quantity { get; init; }

    /// <summary>
    /// The price for <c>quantity</c> units at this price point, as a decimal string.
    /// </summary>
    [JsonPropertyName("amount")]
    public required string Amount { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
