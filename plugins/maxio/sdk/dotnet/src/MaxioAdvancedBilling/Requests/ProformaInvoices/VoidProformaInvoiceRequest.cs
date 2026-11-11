using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ProformaInvoices;

/// <summary>
/// The inputs of the VoidProformaInvoice operation.
/// </summary>
public sealed record VoidProformaInvoiceRequest
{
    /// <summary>
    /// The uid of the proforma invoice
    /// </summary>
    public required string ProformaInvoiceUid { get; init; }

    public VoidInvoiceRequest? Body { get; init; }
}
