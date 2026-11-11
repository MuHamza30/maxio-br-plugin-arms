using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ProformaInvoices;

/// <summary>
/// The inputs of the DeliverProformaInvoice operation.
/// </summary>
public sealed record DeliverProformaInvoiceOperationRequest
{
    /// <summary>
    /// The uid of the proforma invoice
    /// </summary>
    public required string ProformaInvoiceUid { get; init; }

    public DeliverProformaInvoiceRequest? Body { get; init; }
}
