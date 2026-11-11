namespace MaxioAdvancedBilling.Requests.Products;

/// <summary>
/// The inputs of the ReadProduct operation.
/// </summary>
public sealed record ReadProductRequest
{
    /// <summary>
    /// The Advanced Billing id of the product
    /// </summary>
    public required int ProductId { get; init; }

    /// <summary>
    /// When <c>true</c>, embeds the active feature catalog items for each result in a <c>features</c> array. Default value is <c>false</c>.
    /// </summary>
    public bool IncludeFeatures { get; init; } = false;
}
