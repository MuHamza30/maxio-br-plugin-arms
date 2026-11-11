using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Models;

public record Purchase
{
    /// <summary>
    /// The number of wallet units to purchase, as a decimal string with at most 8 fractional digits. Send fractional values as strings: a JSON number with a decimal point is rejected with <c>422</c>, because a double-precision float may already have lost precision before the request arrives.
    /// </summary>
    [JsonPropertyName("quantity")]
    public required Quantity4 Quantity { get; init; }

    /// <summary>
    /// The price point to purchase at. Must belong to the wallet's billing product. Defaults to the <c>price_point_id</c> set on the wallet.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point_id")]
    public int? PricePointId { get; init; }

    /// <summary>
    /// (Optional) A note recorded on the resulting grant's ledger row.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("memo")]
    [MaxLength(255)]
    public string? Memo { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
