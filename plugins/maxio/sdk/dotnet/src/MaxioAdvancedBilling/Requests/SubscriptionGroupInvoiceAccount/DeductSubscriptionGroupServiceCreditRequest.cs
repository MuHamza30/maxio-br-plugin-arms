using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionGroupInvoiceAccount;

/// <summary>
/// The inputs of the DeductSubscriptionGroupServiceCredit operation.
/// </summary>
public sealed record DeductSubscriptionGroupServiceCreditRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }

    public DeductServiceCreditRequest? Body { get; init; }
}
