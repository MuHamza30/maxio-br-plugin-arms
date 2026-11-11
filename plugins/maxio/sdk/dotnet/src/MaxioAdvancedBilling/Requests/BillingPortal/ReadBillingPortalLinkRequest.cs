namespace MaxioAdvancedBilling.Requests.BillingPortal;

/// <summary>
/// The inputs of the ReadBillingPortalLink operation.
/// </summary>
public sealed record ReadBillingPortalLinkRequest
{
    /// <summary>
    /// The Chargify id of the customer
    /// </summary>
    public required int CustomerId { get; init; }
}
