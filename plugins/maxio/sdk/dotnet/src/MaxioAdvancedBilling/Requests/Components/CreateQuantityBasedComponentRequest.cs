using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Components;

/// <summary>
/// The inputs of the CreateQuantityBasedComponent operation.
/// </summary>
public sealed record CreateQuantityBasedComponentRequest
{
    /// <summary>
    /// Either the product family's id or its handle prefixed with <c>handle:</c>
    /// </summary>
    public required string ProductFamilyId { get; init; }

    public CreateQuantityBasedComponent? Body { get; init; }
}
