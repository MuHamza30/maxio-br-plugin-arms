namespace MaxioAdvancedBilling.Requests.Products;

/// <summary>
/// The inputs of the ArchiveProduct operation.
/// </summary>
public sealed record ArchiveProductRequest
{
    /// <summary>
    /// The Advanced Billing id of the product
    /// </summary>
    public required int ProductId { get; init; }
}
