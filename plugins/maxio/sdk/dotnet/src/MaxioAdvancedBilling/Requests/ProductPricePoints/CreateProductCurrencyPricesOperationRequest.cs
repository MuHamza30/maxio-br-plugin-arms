using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ProductPricePoints;

/// <summary>
/// The inputs of the CreateProductCurrencyPrices operation.
/// </summary>
public sealed record CreateProductCurrencyPricesOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the product price point
    /// </summary>
    public required int ProductPricePointId { get; init; }

    public CreateProductCurrencyPricesRequest? Body { get; init; }
}
