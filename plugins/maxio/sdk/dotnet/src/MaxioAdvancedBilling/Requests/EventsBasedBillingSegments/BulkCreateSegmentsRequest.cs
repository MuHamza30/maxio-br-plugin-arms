using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.EventsBasedBillingSegments;

/// <summary>
/// The inputs of the BulkCreateSegments operation.
/// </summary>
public sealed record BulkCreateSegmentsRequest
{
    /// <summary>
    /// ID or Handle for the Component
    /// </summary>
    public required string ComponentId { get; init; }

    /// <summary>
    /// ID or Handle for the Price Point belonging to the Component
    /// </summary>
    public required string PricePointId { get; init; }

    public BulkCreateSegments? Body { get; init; }
}
