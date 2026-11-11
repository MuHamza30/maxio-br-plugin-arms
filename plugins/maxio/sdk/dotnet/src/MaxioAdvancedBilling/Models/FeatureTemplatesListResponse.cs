using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record FeatureTemplatesListResponse
{
    [JsonPropertyName("items")]
    public required IReadOnlyList<FeatureTemplate> Items { get; init; }

    /// <summary>
    /// Total number of feature templates matching the filters, across all pages.
    /// </summary>
    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }

    /// <summary>
    /// Number of archived feature templates matching the filters. Returned as <c>0</c> unless the active result set is empty or <c>status=archived</c> was requested.
    /// </summary>
    [JsonPropertyName("archived_count")]
    public required int ArchivedCount { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
