using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record UpdateFeatureTemplateRequest
{
    /// <summary>
    /// <c>key</c> cannot be changed once set. <c>kind</c> cannot be changed once any feature catalog item has been created from this template.
    /// </summary>
    [JsonPropertyName("feature")]
    public required Feature1 Feature { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
