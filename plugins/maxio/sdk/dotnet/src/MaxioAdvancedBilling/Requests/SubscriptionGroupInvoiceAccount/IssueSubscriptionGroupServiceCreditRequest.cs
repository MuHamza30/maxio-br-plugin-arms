using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionGroupInvoiceAccount;

/// <summary>
/// The inputs of the IssueSubscriptionGroupServiceCredit operation.
/// </summary>
public sealed record IssueSubscriptionGroupServiceCreditRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }

    public IssueServiceCreditRequest? Body { get; init; }
}
