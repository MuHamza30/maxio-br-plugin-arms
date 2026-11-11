using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Requests.ProductPricePoints;

/// <summary>
/// The inputs of the CreateProductPricePoint operation.
/// </summary>
public sealed record CreateProductPricePointOperationRequest
{
    /// <summary>
    /// The id or handle of the product. When using the handle, it must be prefixed with <c>handle:</c>
    /// </summary>
    public required ProductIdModel ProductId { get; init; }

    public CreateProductPricePointRequest? Body { get; init; }
}
