using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the CreateInvoice operation.
/// </summary>
public sealed record CreateInvoiceOperationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public CreateInvoiceRequest? Body { get; init; }
}
