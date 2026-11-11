using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ProductPricePoints;

/// <summary>
/// The inputs of the BulkCreateProductPricePoints operation.
/// </summary>
public sealed record BulkCreateProductPricePointsOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the product to which the price points belong
    /// </summary>
    public required int ProductId { get; init; }

    public BulkCreateProductPricePointsRequest? Body { get; init; }
}
