using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Requests.ComponentPricePoints;

/// <summary>
/// The inputs of the CloneComponentPricePoint operation.
/// </summary>
public sealed record CloneComponentPricePointOperationRequest
{
    /// <summary>
    /// The id or handle of the component. When using the handle, it must be prefixed with <c>handle:</c>. Example: <c>123</c> for an integer ID, or <c>handle:example-product-handle</c> for a string handle.
    /// </summary>
    public required ComponentIdModel ComponentId { get; init; }

    /// <summary>
    /// The id or handle of the price point. When using the handle, it must be prefixed with <c>handle:</c>. Example: <c>123</c> for an integer ID, or <c>handle:example-price_point-handle</c> for a string handle.
    /// </summary>
    public required PricePointIdModel PricePointId { get; init; }

    public CloneComponentPricePointRequest? Body { get; init; }
}
