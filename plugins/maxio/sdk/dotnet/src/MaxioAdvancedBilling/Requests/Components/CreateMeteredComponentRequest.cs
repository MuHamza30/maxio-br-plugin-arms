using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Components;

/// <summary>
/// The inputs of the CreateMeteredComponent operation.
/// </summary>
public sealed record CreateMeteredComponentRequest
{
    /// <summary>
    /// Either the product family's id or its handle prefixed with <c>handle:</c>
    /// </summary>
    public required string ProductFamilyId { get; init; }

    public CreateMeteredComponent? Body { get; init; }
}
