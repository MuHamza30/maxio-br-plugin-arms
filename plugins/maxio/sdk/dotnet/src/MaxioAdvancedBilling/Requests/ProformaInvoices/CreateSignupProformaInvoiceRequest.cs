using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.ProformaInvoices;

/// <summary>
/// The inputs of the CreateSignupProformaInvoice operation.
/// </summary>
public sealed record CreateSignupProformaInvoiceRequest
{
    public CreateSubscriptionRequest? Body { get; init; }
}
