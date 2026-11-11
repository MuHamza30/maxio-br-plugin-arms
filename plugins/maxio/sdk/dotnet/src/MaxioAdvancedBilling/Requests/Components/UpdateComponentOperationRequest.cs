using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Components;

/// <summary>
/// The inputs of the UpdateComponent operation.
/// </summary>
public sealed record UpdateComponentOperationRequest
{
    /// <summary>
    /// The id or handle of the component
    /// </summary>
    public required string ComponentId { get; init; }

    public UpdateComponentRequest? Body { get; init; }
}
