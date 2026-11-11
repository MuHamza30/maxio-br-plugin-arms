using System.ComponentModel.DataAnnotations;

namespace MaxioAdvancedBilling.Requests.Components;

/// <summary>
/// The inputs of the ReadComponent operation.
/// </summary>
public sealed record ReadComponentRequest
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

    /// <summary>
    /// When <c>true</c>, embeds the active feature catalog items for each result in a <c>features</c> array. Default value is <c>false</c>.
    /// </summary>
    public bool IncludeFeatures { get; init; } = false;
}
