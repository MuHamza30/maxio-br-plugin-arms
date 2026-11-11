using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.FeatureTemplates;

/// <summary>
/// The inputs of the CreateFeatureTemplate operation.
/// </summary>
public sealed record CreateFeatureTemplateOperationRequest
{
    public CreateFeatureTemplateRequest? Body { get; init; }
}
