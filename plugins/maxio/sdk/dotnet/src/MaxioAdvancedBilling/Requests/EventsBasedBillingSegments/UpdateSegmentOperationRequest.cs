using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.EventsBasedBillingSegments;

/// <summary>
/// The inputs of the UpdateSegment operation.
/// </summary>
public sealed record UpdateSegmentOperationRequest
{
    /// <summary>
    /// ID or Handle of the Component
    /// </summary>
    public required string ComponentId { get; init; }

    /// <summary>
    /// ID or Handle of the Price Point belonging to the Component
    /// </summary>
    public required string PricePointId { get; init; }

    /// <summary>
    /// The ID of the Segment
    /// </summary>
    public required double Id { get; init; }

    public UpdateSegmentRequest? Body { get; init; }
}
