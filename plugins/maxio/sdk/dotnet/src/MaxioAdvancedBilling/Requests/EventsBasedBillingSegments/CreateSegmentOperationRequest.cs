using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.EventsBasedBillingSegments;

/// <summary>
/// The inputs of the CreateSegment operation.
/// </summary>
public sealed record CreateSegmentOperationRequest
{
    /// <summary>
    /// ID or Handle for the Component
    /// </summary>
    public required string ComponentId { get; init; }

    /// <summary>
    /// ID or Handle for the Price Point belonging to the Component
    /// </summary>
    public required string PricePointId { get; init; }

    public CreateSegmentRequest? Body { get; init; }
}
