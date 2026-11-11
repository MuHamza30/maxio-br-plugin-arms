namespace MaxioAdvancedBilling.Requests.Components;

/// <summary>
/// The inputs of the FindComponent operation.
/// </summary>
public sealed record FindComponentRequest
{
    /// <summary>
    /// The handle of the component to find
    /// </summary>
    public required string Handle { get; init; }
}
