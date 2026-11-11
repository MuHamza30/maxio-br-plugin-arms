using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

public record WalletTransaction1
{
    /// <summary>
    /// Determines the direction of the movement. The amount itself is always sent positive.
    /// </summary>
    [JsonPropertyName("kind")]
    public required Kind1 Kind { get; init; }

    /// <summary>
    /// Always positive; <c>kind</c> determines the sign written to the ledger. Encoded as a decimal string with at most 8 fractional digits. Send fractional values as strings: a JSON number with a decimal point is rejected with <c>422</c>, because a double-precision float may already have lost precision before the request arrives.
    /// </summary>
    [JsonPropertyName("amount")]
    public required Amount6 Amount { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("memo")]
    [MaxLength(255)]
    public string? Memo { get; init; }

    /// <summary>
    /// An expiration for the grant this creates. Only accepted with <c>kind: grant</c>; sending it with <c>kind: debit</c> is rejected. Omit it for an evergreen grant. Accepts either a bare calendar date (<c>YYYY-MM-DD</c>), resolved to the end of that day in the site's time zone, or a full timestamp. A calendar day that doesn't exist, such as <c>2027-02-30</c>, is rejected with <c>422</c> rather than rolled into the next month.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("expires_at")]
    public string? ExpiresAt { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
