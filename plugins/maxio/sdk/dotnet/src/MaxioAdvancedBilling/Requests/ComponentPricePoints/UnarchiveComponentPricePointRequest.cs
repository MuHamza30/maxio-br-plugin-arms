namespace MaxioAdvancedBilling.Requests.ComponentPricePoints;

/// <summary>
/// The inputs of the UnarchiveComponentPricePoint operation.
/// </summary>
public sealed record UnarchiveComponentPricePointRequest
{
    /// <summary>
    /// The Advanced Billing id of the component to which the price point belongs
    /// </summary>
    public required int ComponentId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the price point
    /// </summary>
    public required int PricePointId { get; init; }
}
