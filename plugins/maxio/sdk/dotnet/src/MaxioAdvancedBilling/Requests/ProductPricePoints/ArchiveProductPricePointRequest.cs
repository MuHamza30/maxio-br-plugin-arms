using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Requests.ProductPricePoints;

/// <summary>
/// The inputs of the ArchiveProductPricePoint operation.
/// </summary>
public sealed record ArchiveProductPricePointRequest
{
    /// <summary>
    /// The id or handle of the product. When using the handle, it must be prefixed with <c>handle:</c>. Example: <c>123</c> for an integer ID, or <c>handle:example-product-handle</c> for a string handle.
    /// </summary>
    public required ProductIdModel ProductId { get; init; }

    /// <summary>
    /// The id or handle of the price point. When using the handle, it must be prefixed with <c>handle:</c>. Example: <c>123</c> for an integer ID, or <c>handle:example-product-price-point-handle</c> for a string handle.
    /// </summary>
    public required PricePointIdModel PricePointId { get; init; }
}
