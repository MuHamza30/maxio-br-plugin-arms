namespace MaxioAdvancedBilling.Requests.Customers;

/// <summary>
/// The inputs of the ListCustomerSubscriptions operation.
/// </summary>
public sealed record ListCustomerSubscriptionsRequest
{
    /// <summary>
    /// The Chargify id of the customer
    /// </summary>
    public required int CustomerId { get; init; }
}
