using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ComponentPricePoints;

/// <summary>
/// The inputs of the UpdateCurrencyPrices operation.
/// </summary>
public sealed record UpdateCurrencyPricesOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the price point
    /// </summary>
    public required int PricePointId { get; init; }

    public UpdateCurrencyPricesRequest? Body { get; init; }
}
