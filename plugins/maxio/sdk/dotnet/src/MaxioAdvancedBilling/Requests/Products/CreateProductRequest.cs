using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Products;

/// <summary>
/// The inputs of the CreateProduct operation.
/// </summary>
public sealed record CreateProductRequest
{
    /// <summary>
    /// Either the product family's id or its handle prefixed with <c>handle:</c>
    /// </summary>
    public required string ProductFamilyId { get; init; }

    public CreateOrUpdateProductRequest? Body { get; init; }
}
