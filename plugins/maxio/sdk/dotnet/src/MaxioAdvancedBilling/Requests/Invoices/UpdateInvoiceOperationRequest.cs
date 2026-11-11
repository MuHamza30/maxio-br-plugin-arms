using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the UpdateInvoice operation.
/// </summary>
public sealed record UpdateInvoiceOperationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The unique identifier for the invoice, this does not refer to the public facing invoice number.
    /// </summary>
    public required string Uid { get; init; }

    public UpdateInvoiceRequest? Body { get; init; }
}
