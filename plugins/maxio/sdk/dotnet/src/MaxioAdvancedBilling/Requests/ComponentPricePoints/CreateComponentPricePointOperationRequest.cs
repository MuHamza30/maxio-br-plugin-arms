using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ComponentPricePoints;

/// <summary>
/// The inputs of the CreateComponentPricePoint operation.
/// </summary>
public sealed record CreateComponentPricePointOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the component
    /// </summary>
    public required int ComponentId { get; init; }

    public CreateComponentPricePointRequest? Body { get; init; }
}
