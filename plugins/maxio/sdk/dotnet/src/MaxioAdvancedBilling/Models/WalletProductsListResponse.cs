using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record WalletProductsListResponse
{
    [JsonPropertyName("wallet_products")]
    public required IReadOnlyList<WalletProductListItem> WalletProducts { get; init; }

    /// <summary>
    /// Total number of Wallet Products matching the filters, across all pages.
    /// </summary>
    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }

    /// <summary>
    /// Not a simple count of archived products matching the filters. With <c>status=archived</c>, equals <c>total_count</c> (there's nothing else to report). With any other <c>status</c>, it's <c>0</c> whenever <c>total_count</c> is non-zero, and only when <c>total_count</c> is <c>0</c> does it become the number of archived Wallet Products matching the same <c>q</c> search term (ignoring <c>status</c>): an empty-state hint, not a running total.
    /// </summary>
    [JsonPropertyName("archived_count")]
    public required int ArchivedCount { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
