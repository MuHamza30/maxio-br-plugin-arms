namespace MaxioAdvancedBilling.Requests.Customers;

/// <summary>
/// The inputs of the DeleteCustomer operation.
/// </summary>
public sealed record DeleteCustomerRequest
{
    /// <summary>
    /// The Advanced Billing id of the customer
    /// </summary>
    public required int Id { get; init; }
}
