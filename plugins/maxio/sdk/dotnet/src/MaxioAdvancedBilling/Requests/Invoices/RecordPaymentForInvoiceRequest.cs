using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the RecordPaymentForInvoice operation.
/// </summary>
public sealed record RecordPaymentForInvoiceRequest
{
    /// <summary>
    /// The unique identifier for the invoice, this does not refer to the public facing invoice number.
    /// </summary>
    public required string Uid { get; init; }

    public CreateInvoicePaymentRequest? Body { get; init; }
}
