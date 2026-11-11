using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.AdvanceInvoice;

/// <summary>
/// The inputs of the IssueAdvanceInvoice operation.
/// </summary>
public sealed record IssueAdvanceInvoiceOperationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public IssueAdvanceInvoiceRequest? Body { get; init; }
}
