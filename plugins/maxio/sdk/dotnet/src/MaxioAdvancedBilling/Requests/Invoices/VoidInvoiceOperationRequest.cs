using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the VoidInvoice operation.
/// </summary>
public sealed record VoidInvoiceOperationRequest
{
    /// <summary>
    /// The unique identifier for the invoice, this does not refer to the public facing invoice number.
    /// </summary>
    public required string Uid { get; init; }

    public VoidInvoiceRequest? Body { get; init; }
}
