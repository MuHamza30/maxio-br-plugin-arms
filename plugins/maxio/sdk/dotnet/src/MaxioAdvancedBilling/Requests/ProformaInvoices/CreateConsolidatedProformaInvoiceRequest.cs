namespace MaxioAdvancedBilling.Requests.ProformaInvoices;

/// <summary>
/// The inputs of the CreateConsolidatedProformaInvoice operation.
/// </summary>
public sealed record CreateConsolidatedProformaInvoiceRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }
}
