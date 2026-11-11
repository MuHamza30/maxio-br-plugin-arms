namespace MaxioAdvancedBilling.Requests.ProformaInvoices;

/// <summary>
/// The inputs of the PreviewProformaInvoice operation.
/// </summary>
public sealed record PreviewProformaInvoiceRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }
}
