using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ProductFeatures;

/// <summary>
/// The inputs of the UpdateProductFeature operation.
/// </summary>
public sealed record UpdateProductFeatureRequest
{
    /// <summary>
    /// The Advanced Billing id of the product.
    /// </summary>
    public required int ProductId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the feature catalog item.
    /// </summary>
    public required int Id { get; init; }

    public UpdateFeatureCatalogItemRequest? Body { get; init; }
}
