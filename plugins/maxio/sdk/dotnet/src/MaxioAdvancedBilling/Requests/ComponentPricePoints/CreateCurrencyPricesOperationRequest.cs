using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ComponentPricePoints;

/// <summary>
/// The inputs of the CreateCurrencyPrices operation.
/// </summary>
public sealed record CreateCurrencyPricesOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the price point
    /// </summary>
    public required int PricePointId { get; init; }

    public CreateCurrencyPricesRequest? Body { get; init; }
}
