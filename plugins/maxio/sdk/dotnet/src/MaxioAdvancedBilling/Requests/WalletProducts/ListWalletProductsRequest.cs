using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.WalletProducts;

/// <summary>
/// The inputs of the ListWalletProducts operation.
/// </summary>
public sealed record ListWalletProductsRequest
{
    /// <summary>
    /// Includes archived Wallet Products in the result. Defaults to <c>false</c>. Ignored when <c>status</c> is also sent.
    /// </summary>
    public bool? IncludeArchived { get; init; }

    /// <summary>
    /// Restricts the result by status, and overrides <c>include_archived</c> when sent. An unrecognized value falls back to <c>active</c> rather than returning <c>422</c>.
    /// </summary>
    public Status1? Status { get; init; }

    /// <summary>
    /// Restricts the result to Wallet Products whose name, handle, or unit name contains this substring (case-insensitive).
    /// </summary>
    public string? Q { get; init; }

    /// <summary>
    /// The field to sort results by. An unrecognized value falls back to <c>name</c>.
    /// </summary>
    public SortBy1 SortBy { get; init; } = SortBy1.Name;

    /// <summary>
    /// The sort direction. Anything other than <c>desc</c> is treated as <c>asc</c>.
    /// </summary>
    public SortDirection SortDirection { get; init; } = SortDirection.Asc;

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
}
