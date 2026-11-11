namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the DeleteInvoice operation.
/// </summary>
public sealed record DeleteInvoiceRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The unique identifier for the invoice, this does not refer to the public facing invoice number.
    /// </summary>
    public required string Uid { get; init; }
}
