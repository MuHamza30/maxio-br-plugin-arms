using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Components;

/// <summary>
/// The inputs of the CreateEventBasedComponent operation.
/// </summary>
public sealed record CreateEventBasedComponentRequest
{
    /// <summary>
    /// Either the product family's id or its handle prefixed with <c>handle:</c>
    /// </summary>
    public required string ProductFamilyId { get; init; }

    public CreateEbbComponent? Body { get; init; }
}
