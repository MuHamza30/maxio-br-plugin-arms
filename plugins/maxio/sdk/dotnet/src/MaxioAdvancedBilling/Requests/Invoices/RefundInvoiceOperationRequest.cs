using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the RefundInvoice operation.
/// </summary>
public sealed record RefundInvoiceOperationRequest
{
    /// <summary>
    /// The unique identifier for the invoice, this does not refer to the public facing invoice number.
    /// </summary>
    public required string Uid { get; init; }

    public RefundInvoiceRequest? Body { get; init; }
}
