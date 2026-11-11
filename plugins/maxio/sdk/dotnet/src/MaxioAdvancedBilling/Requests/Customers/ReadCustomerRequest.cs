namespace MaxioAdvancedBilling.Requests.Customers;

/// <summary>
/// The inputs of the ReadCustomer operation.
/// </summary>
public sealed record ReadCustomerRequest
{
    /// <summary>
    /// The Advanced Billing id of the customer
    /// </summary>
    public required int Id { get; init; }
}
