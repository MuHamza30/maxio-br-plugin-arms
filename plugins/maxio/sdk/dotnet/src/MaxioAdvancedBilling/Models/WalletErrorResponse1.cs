using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// The JSON error body returned by every wallet, transaction, adjustment, purchase, and Wallet Product endpoint for <c>404</c>, <c>409</c>, <c>422</c>, <c>500</c>, and <c>503</c> responses. A <c>403</c> is not JSON; it is the plain text <c>403 Forbidden</c>.
/// </summary>
public record WalletErrorResponse1
{
    /// <summary>
    /// Human-readable messages. These may be reworded, so match on <c>code</c> instead.
    /// </summary>
    [JsonPropertyName("errors")]
    public required IReadOnlyList<string> Errors { get; init; }

    /// <summary>
    /// A stable, machine-readable reason, for example <c>wallet_depleted</c>, <c>wallet_not_found</c>, <c>wallet_product_not_found</c>, <c>price_point_archived</c>, <c>kind_invalid</c>, or <c>parameter_missing</c>. Failures that come from record validation, such as a field that fails a model rule, report <c>validation_failed</c>.
    /// </summary>
    [JsonPropertyName("code")]
    public required string Code { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
