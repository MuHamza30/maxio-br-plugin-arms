using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionInvoiceAccount;

/// <summary>
/// The inputs of the IssueServiceCredit operation.
/// </summary>
public sealed record IssueServiceCreditOperationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public IssueServiceCreditRequest? Body { get; init; }
}
