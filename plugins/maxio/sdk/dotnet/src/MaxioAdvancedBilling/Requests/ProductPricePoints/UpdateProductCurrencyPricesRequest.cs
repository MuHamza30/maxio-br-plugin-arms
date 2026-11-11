using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ProductPricePoints;

/// <summary>
/// The inputs of the UpdateProductCurrencyPrices operation.
/// </summary>
public sealed record UpdateProductCurrencyPricesRequest
{
    /// <summary>
    /// The Advanced Billing id of the product price point
    /// </summary>
    public required int ProductPricePointId { get; init; }

    public UpdateCurrencyPricesRequest? Body { get; init; }
}
