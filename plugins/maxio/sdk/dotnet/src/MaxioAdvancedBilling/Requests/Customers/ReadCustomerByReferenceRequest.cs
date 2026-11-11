namespace MaxioAdvancedBilling.Requests.Customers;

/// <summary>
/// The inputs of the ReadCustomerByReference operation.
/// </summary>
public sealed record ReadCustomerByReferenceRequest
{
    /// <summary>
    /// Customer reference
    /// </summary>
    public required string Reference { get; init; }
}
