using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.ProformaInvoices;

/// <summary>
/// The inputs of the ListProformaInvoices operation.
/// </summary>
public sealed record ListProformaInvoicesRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The beginning date range for the invoice's Due Date, in the YYYY-MM-DD format.
    /// </summary>
    public string? StartDate { get; init; }

    /// <summary>
    /// The ending date range for the invoice's Due Date, in the YYYY-MM-DD format.
    /// </summary>
    public string? EndDate { get; init; }

    /// <summary>
    /// The current status of the invoice.  Allowed Values: draft, open, paid, pending, voided
    /// </summary>
    public ProformaInvoiceStatus? Status { get; init; }

    /// <summary>
    /// Result records are organized in pages. By default, the first page of results is displayed. The page parameter specifies a page number of results to fetch. You can start navigating through the pages to consume the results. You do this by passing in a page parameter. Retrieve the next page by adding ?page=2 to the query string. If there are no results to return, then an empty result set will be returned.
    /// Use in query <c>page=1</c>.
    /// </summary>
    [Minimum(1)]
    public int Page { get; init; } = 1;

    /// <summary>
    /// This parameter indicates how many records to fetch in each request. Default value is 20. The maximum allowed values is 200; any per_page value over 200 will be changed to 200.
    /// Use in query <c>per_page=200</c>.
    /// </summary>
    [Maximum(200)]
    public int PerPage { get; init; } = 20;

    /// <summary>
    /// The sort direction of the returned invoices.
    /// </summary>
    public Direction Direction { get; init; } = Direction.Desc;

    /// <summary>
    /// Include line items data.
    /// </summary>
    public bool LineItems { get; init; } = false;

    /// <summary>
    /// Include discounts data.
    /// </summary>
    public bool Discounts { get; init; } = false;

    /// <summary>
    /// Include taxes data.
    /// </summary>
    public bool Taxes { get; init; } = false;

    /// <summary>
    /// Include credits data.
    /// </summary>
    public bool Credits { get; init; } = false;

    /// <summary>
    /// Include payments data.
    /// </summary>
    public bool Payments { get; init; } = false;

    /// <summary>
    /// Include custom fields data.
    /// </summary>
    public bool CustomFields { get; init; } = false;
}
