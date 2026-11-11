using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ComponentPricePoints;

/// <summary>
/// The inputs of the BulkCreateComponentPricePoints operation.
/// </summary>
public sealed record BulkCreateComponentPricePointsRequest
{
    /// <summary>
    /// The Advanced Billing id of the component for which you want to fetch price points.
    /// </summary>
    public required string ComponentId { get; init; }

    public CreateComponentPricePointsRequest? Body { get; init; }
}
