using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Components;

/// <summary>
/// The inputs of the CreateOnOffComponent operation.
/// </summary>
public sealed record CreateOnOffComponentRequest
{
    /// <summary>
    /// Either the product family's id or its handle prefixed with <c>handle:</c>
    /// </summary>
    public required string ProductFamilyId { get; init; }

    public CreateOnOffComponent? Body { get; init; }
}
