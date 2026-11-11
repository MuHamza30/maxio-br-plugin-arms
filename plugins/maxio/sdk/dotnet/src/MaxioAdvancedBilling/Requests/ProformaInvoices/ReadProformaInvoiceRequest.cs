namespace MaxioAdvancedBilling.Requests.ProformaInvoices;

/// <summary>
/// The inputs of the ReadProformaInvoice operation.
/// </summary>
public sealed record ReadProformaInvoiceRequest
{
    /// <summary>
    /// The uid of the proforma invoice
    /// </summary>
    public required string ProformaInvoiceUid { get; init; }
}
