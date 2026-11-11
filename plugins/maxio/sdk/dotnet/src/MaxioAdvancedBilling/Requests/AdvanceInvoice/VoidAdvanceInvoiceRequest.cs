using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.AdvanceInvoice;

/// <summary>
/// The inputs of the VoidAdvanceInvoice operation.
/// </summary>
public sealed record VoidAdvanceInvoiceRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public VoidInvoiceRequest? Body { get; init; }
}
