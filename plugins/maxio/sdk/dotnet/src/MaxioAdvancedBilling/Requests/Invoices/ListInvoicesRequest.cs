using System.Collections.Generic;
using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the ListInvoices operation.
/// </summary>
public sealed record ListInvoicesRequest
{
    /// <summary>
    /// The start date (format YYYY-MM-DD) with which to filter the date_field. Returns invoices with a timestamp at or after midnight (12:00:00 AM) in your site’s time zone on the date specified.
    /// </summary>
    public string? StartDate { get; init; }

    /// <summary>
    /// The end date (format YYYY-MM-DD) with which to filter the date_field. Returns invoices with a timestamp up to and including 11:59:59PM in your site’s time zone on the date specified.
    /// </summary>
    public string? EndDate { get; init; }

    /// <summary>
    /// The current status of the invoice.  Allowed Values: draft, open, paid, pending, voided
    /// </summary>
    public InvoiceStatus? Status { get; init; }

    /// <summary>
    /// The subscription's ID.
    /// </summary>
    public int? SubscriptionId { get; init; }

    /// <summary>
    /// The UID of the subscription group you want to fetch consolidated invoices for. This will return a paginated list of consolidated invoices for the specified group.
    /// </summary>
    public string? SubscriptionGroupUid { get; init; }

    /// <summary>
    /// The consolidation level of the invoice. Allowed Values: none, parent, child or comma-separated lists of thereof, e.g. none,parent.
    /// </summary>
    public string? ConsolidationLevel { get; init; }

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

    /// <summary>
    /// Include refunds data.
    /// </summary>
    public bool Refunds { get; init; } = false;

    /// <summary>
    /// The type of filter you would like to apply to your search. Use in query <c>date_field=issue_date</c>.
    /// </summary>
    public InvoiceDateField DateField { get; init; } = InvoiceDateField.DueDate;

    /// <summary>
    /// The start date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns invoices with a timestamp at or after exact time provided in query. You can specify timezone in query - otherwise your site's time zone will be used. If provided, this parameter will be used instead of start_date. Allowed to be used only along with date_field set to created_at or updated_at.
    /// </summary>
    public string? StartDatetime { get; init; }

    /// <summary>
    /// The end date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns invoices with a timestamp at or before exact time provided in query. You can specify timezone in query - otherwise your site's time zone will be used. If provided, this parameter will be used instead of end_date. Allowed to be used only along with date_field set to created_at or updated_at.
    /// </summary>
    public string? EndDatetime { get; init; }

    /// <summary>
    /// Allows fetching invoices with matching customer id based on provided values. Use in query <c>customer_ids=1,2,3</c>.
    /// </summary>
    public IReadOnlyList<int>? CustomerIds { get; init; }

    /// <summary>
    /// Allows fetching invoices with matching invoice number based on provided values. Use in query <c>number=1234,1235</c>.
    /// </summary>
    public IReadOnlyList<string>? Number { get; init; }

    /// <summary>
    /// Allows fetching invoices with matching line items product ids based on provided values. Use in query <c>product_ids=23,34</c>.
    /// </summary>
    public IReadOnlyList<int>? ProductIds { get; init; }

    /// <summary>
    /// Allows specification of the order of the returned list. Use in query <c>sort=total_amount</c>.
    /// </summary>
    public InvoiceSortField Sort { get; init; } = InvoiceSortField.Number;
}
