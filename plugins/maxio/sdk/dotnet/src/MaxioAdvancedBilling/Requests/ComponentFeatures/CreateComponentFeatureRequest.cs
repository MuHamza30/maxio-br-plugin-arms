using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ComponentFeatures;

/// <summary>
/// The inputs of the CreateComponentFeature operation.
/// </summary>
public sealed record CreateComponentFeatureRequest
{
    /// <summary>
    /// The Advanced Billing id of the component.
    /// </summary>
    public required int ComponentId { get; init; }

    public CreateFeatureCatalogItemRequest? Body { get; init; }
}
