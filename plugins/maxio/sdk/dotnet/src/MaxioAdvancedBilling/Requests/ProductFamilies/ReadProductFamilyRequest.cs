namespace MaxioAdvancedBilling.Requests.ProductFamilies;

/// <summary>
/// The inputs of the ReadProductFamily operation.
/// </summary>
public sealed record ReadProductFamilyRequest
{
    /// <summary>
    /// The Advanced Billing id of the product family
    /// </summary>
    public required int Id { get; init; }
}
