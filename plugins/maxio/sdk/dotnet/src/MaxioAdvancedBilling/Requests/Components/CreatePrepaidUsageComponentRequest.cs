using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Components;

/// <summary>
/// The inputs of the CreatePrepaidUsageComponent operation.
/// </summary>
public sealed record CreatePrepaidUsageComponentRequest
{
    /// <summary>
    /// Either the product family's id or its handle prefixed with <c>handle:</c>
    /// </summary>
    public required string ProductFamilyId { get; init; }

    public CreatePrepaidComponent? Body { get; init; }
}
