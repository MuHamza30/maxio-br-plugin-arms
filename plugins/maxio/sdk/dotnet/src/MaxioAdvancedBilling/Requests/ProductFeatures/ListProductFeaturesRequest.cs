namespace MaxioAdvancedBilling.Requests.ProductFeatures;

/// <summary>
/// The inputs of the ListProductFeatures operation.
/// </summary>
public sealed record ListProductFeaturesRequest
{
    /// <summary>
    /// The Advanced Billing id of the product.
    /// </summary>
    public required int ProductId { get; init; }
}
