using System.Collections.Generic;
using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the ListInvoiceEvents operation.
/// </summary>
public sealed record ListInvoiceEventsRequest
{
    /// <summary>
    /// The timestamp in a format <c>YYYY-MM-DD T HH:MM:SS Z</c>, or <c>YYYY-MM-DD</c>(in this case, it returns data from the beginning of the day). of the event from which you want to start the search. All the events before the <c>since_date</c> timestamp are not returned in the response.
    /// </summary>
    public string? SinceDate { get; init; }

    /// <summary>
    /// The ID of the event from which you want to start the search(ID is not included. e.g. if ID is set to 2, then all events with ID 3 and more will be shown) This parameter is not used if since_date is defined.
    /// </summary>
    public long? SinceId { get; init; }

    /// <summary>
    /// Result records are organized in pages. By default, the first page of results is displayed. The page parameter specifies a page number of results to fetch. You can start navigating through the pages to consume the results. You do this by passing in a page parameter. Retrieve the next page by adding ?page=2 to the query string. If there are no results to return, then an empty result set will be returned.
    /// Use in query <c>page=1</c>.
    /// </summary>
    [Minimum(1)]
    public int Page { get; init; } = 1;

    /// <summary>
    /// This parameter indicates how many records to fetch in each request. Default value is 100. The maximum allowed values is 200; any per_page value over 200 will be changed to 200.
    /// </summary>
    public int PerPage { get; init; } = 100;

    /// <summary>
    /// Providing an invoice_uid allows for scoping of the invoice events to a single invoice or credit note.
    /// </summary>
    public string? InvoiceUid { get; init; }

    /// <summary>
    /// Use this parameter if you want to fetch also invoice events with change_invoice_status type.
    /// </summary>
    public string? WithChangeInvoiceStatus { get; init; }

    /// <summary>
    /// Filter results by event_type. Supply a comma separated list of event types (listed above). Use in query: <c>event_types=void_invoice,void_remainder</c>.
    /// </summary>
    public IReadOnlyList<InvoiceEventType>? EventTypes { get; init; }
}
