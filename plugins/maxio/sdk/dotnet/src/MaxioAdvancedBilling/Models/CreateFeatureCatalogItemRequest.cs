using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// The owning product or component is taken from the URL and must not be included in the request body.
/// </summary>
public record CreateFeatureCatalogItemRequest
{
    [JsonPropertyName("feature")]
    public required Feature2 Feature { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
