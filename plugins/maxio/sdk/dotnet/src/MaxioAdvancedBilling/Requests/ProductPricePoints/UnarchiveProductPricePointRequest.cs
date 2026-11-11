namespace MaxioAdvancedBilling.Requests.ProductPricePoints;

/// <summary>
/// The inputs of the UnarchiveProductPricePoint operation.
/// </summary>
public sealed record UnarchiveProductPricePointRequest
{
    /// <summary>
    /// The Advanced Billing id of the product to which the price point belongs
    /// </summary>
    public required int ProductId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the product price point
    /// </summary>
    public required int PricePointId { get; init; }
}
