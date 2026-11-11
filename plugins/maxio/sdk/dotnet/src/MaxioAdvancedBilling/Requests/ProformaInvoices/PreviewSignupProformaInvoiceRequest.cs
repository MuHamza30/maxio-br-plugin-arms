using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.ProformaInvoices;

/// <summary>
/// The inputs of the PreviewSignupProformaInvoice operation.
/// </summary>
public sealed record PreviewSignupProformaInvoiceRequest
{
    /// <summary>
    /// Choose to include a proforma invoice preview for the first renewal. Use in query <c>include=next_proforma_invoice</c>.
    /// </summary>
    public CreateSignupProformaPreviewInclude? Include { get; init; }

    public CreateSubscriptionRequest? Body { get; init; }
}
