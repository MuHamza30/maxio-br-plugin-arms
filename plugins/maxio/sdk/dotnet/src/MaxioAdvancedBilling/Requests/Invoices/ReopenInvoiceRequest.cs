namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the ReopenInvoice operation.
/// </summary>
public sealed record ReopenInvoiceRequest
{
    /// <summary>
    /// The unique identifier for the invoice, this does not refer to the public facing invoice number.
    /// </summary>
    public required string Uid { get; init; }
}
