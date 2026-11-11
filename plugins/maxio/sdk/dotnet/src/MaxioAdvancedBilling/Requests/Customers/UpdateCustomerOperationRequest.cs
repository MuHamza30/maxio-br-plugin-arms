using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Customers;

/// <summary>
/// The inputs of the UpdateCustomer operation.
/// </summary>
public sealed record UpdateCustomerOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the customer
    /// </summary>
    public required int Id { get; init; }

    public UpdateCustomerRequest? Body { get; init; }
}
