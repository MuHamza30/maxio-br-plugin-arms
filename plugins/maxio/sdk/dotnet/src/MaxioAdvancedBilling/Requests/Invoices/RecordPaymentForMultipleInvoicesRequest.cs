using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the RecordPaymentForMultipleInvoices operation.
/// </summary>
public sealed record RecordPaymentForMultipleInvoicesRequest
{
    public CreateMultiInvoicePaymentRequest? Body { get; init; }
}
