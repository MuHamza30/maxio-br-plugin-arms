using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.BillingPortal;

/// <summary>
/// The inputs of the EnableBillingPortalForCustomer operation.
/// </summary>
public sealed record EnableBillingPortalForCustomerRequest
{
    /// <summary>
    /// The Chargify id of the customer
    /// </summary>
    public required int CustomerId { get; init; }

    /// <summary>
    /// When set to 1, an Invitation email will be sent to the Customer.
    /// When set to 0, or not sent, an email will not be sent.
    /// Use in query: <c>auto_invite=1</c>.
    /// </summary>
    public AutoInvite? AutoInvite { get; init; }
}
