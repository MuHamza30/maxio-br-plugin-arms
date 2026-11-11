using System.ComponentModel.DataAnnotations;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Components;

/// <summary>
/// The inputs of the UpdateProductFamilyComponent operation.
/// </summary>
public sealed record UpdateProductFamilyComponentRequest
{
    /// <summary>
    /// The Advanced Billing id of the product family to which the component belongs
    /// </summary>
    public required int ProductFamilyId { get; init; }

    /// <summary>
    /// Either the Advanced Billing id of the component or the handle for the component prefixed with <c>handle:</c>
    /// </summary>
    [RegularExpression("/\\A(?:\\d+|handle:(?:uuid:|[a-z])(?:\\w|-)+)\\z/")]
    public required string ComponentId { get; init; }

    public UpdateComponentRequest? Body { get; init; }
}
