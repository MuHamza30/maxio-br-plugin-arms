using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the ListCreditNotes operation.
/// </summary>
public sealed record ListCreditNotesRequest
{
    /// <summary>
    /// The subscription's Advanced Billing id
    /// </summary>
    public int? SubscriptionId { get; init; }

    /// <summary>
    /// The type of filter you would like to apply to your search. Use in query <c>date_field=issue_date</c>. If a date range is provided without an explicit <c>date_field</c>, it defaults to <c>issue_date</c>. If only <c>start_datetime</c>/<c>end_datetime</c> are provided without an explicit <c>date_field</c>, it defaults to <c>created_at</c> instead. An unrecognized <c>date_field</c> is ignored rather than raising an error.
    /// </summary>
    public CreditNoteDateField DateField { get; init; } = CreditNoteDateField.IssueDate;

    /// <summary>
    /// The start date (format YYYY-MM-DD) with which to filter the date_field. Returns credit notes with a timestamp at or after midnight (12:00:00 AM) in your site’s time zone on the date specified.
    /// </summary>
    public string? StartDate { get; init; }

    /// <summary>
    /// The end date (format YYYY-MM-DD) with which to filter the date_field. Returns credit notes with a timestamp up to and including 11:59:59PM in your site’s time zone on the date specified.
    /// </summary>
    public string? EndDate { get; init; }

    /// <summary>
    /// The start date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns credit notes with a timestamp at or after exact time provided in query. If provided, this parameter will be used instead of start_date. If no timezone offset is included in the value, it is interpreted as UTC. Allowed to be used only along with date_field set to created_at or updated_at.
    /// </summary>
    public string? StartDatetime { get; init; }

    /// <summary>
    /// The end date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns credit notes with a timestamp at or before exact time provided in query. If provided, this parameter will be used instead of end_date. If no timezone offset is included in the value, it is interpreted as UTC. Allowed to be used only along with date_field set to created_at or updated_at.
    /// </summary>
    public string? EndDatetime { get; init; }

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
    /// The sort direction of the returned credit notes, sorted by sequence_number.
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
    /// Include refunds data.
    /// </summary>
    public bool Refunds { get; init; } = false;

    /// <summary>
    /// Include applications data.
    /// </summary>
    public bool Applications { get; init; } = false;
}
