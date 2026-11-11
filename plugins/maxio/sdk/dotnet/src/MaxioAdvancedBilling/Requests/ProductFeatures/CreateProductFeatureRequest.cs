using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ProductFeatures;

/// <summary>
/// The inputs of the CreateProductFeature operation.
/// </summary>
public sealed record CreateProductFeatureRequest
{
    /// <summary>
    /// The Advanced Billing id of the product.
    /// </summary>
    public required int ProductId { get; init; }

    public CreateFeatureCatalogItemRequest? Body { get; init; }
}
