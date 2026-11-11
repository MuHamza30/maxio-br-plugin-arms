namespace MaxioAdvancedBilling.Requests.BillingPortal;

/// <summary>
/// The inputs of the ResendBillingPortalInvitation operation.
/// </summary>
public sealed record ResendBillingPortalInvitationRequest
{
    /// <summary>
    /// The Chargify id of the customer
    /// </summary>
    public required int CustomerId { get; init; }
}
