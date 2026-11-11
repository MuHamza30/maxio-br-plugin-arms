using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Products;

/// <summary>
/// The inputs of the UpdateProduct operation.
/// </summary>
public sealed record UpdateProductRequest
{
    /// <summary>
    /// The Advanced Billing id of the product
    /// </summary>
    public required int ProductId { get; init; }

    public CreateOrUpdateProductRequest? Body { get; init; }
}
