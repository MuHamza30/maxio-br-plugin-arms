namespace MaxioAdvancedBilling.Requests.BillingPortal;

/// <summary>
/// The inputs of the RevokeBillingPortalAccess operation.
/// </summary>
public sealed record RevokeBillingPortalAccessRequest
{
    /// <summary>
    /// The Chargify id of the customer
    /// </summary>
    public required int CustomerId { get; init; }
}
