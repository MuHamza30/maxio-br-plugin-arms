namespace MaxioAdvancedBilling.Requests.Products;

/// <summary>
/// The inputs of the ReadProductByHandle operation.
/// </summary>
public sealed record ReadProductByHandleRequest
{
    /// <summary>
    /// The handle of the product
    /// </summary>
    public required string ApiHandle { get; init; }
}
