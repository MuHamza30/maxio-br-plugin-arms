using System;
using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.FeatureTemplates;

/// <summary>
/// The inputs of the ListFeatureTemplates operation.
/// </summary>
public sealed record ListFeatureTemplatesRequest
{
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
    /// Filters by archived state. Defaults to <c>active</c> (non-archived templates only).
    /// </summary>
    public Status1 Status { get; init; } = Status1.Active;

    /// <summary>
    /// Filters to feature templates whose name contains this substring (case-insensitive).
    /// </summary>
    public string? Q { get; init; }

    /// <summary>
    /// Filters by feature kind.
    /// </summary>
    public Kind? Kind { get; init; }

    /// <summary>
    /// Returns feature templates updated on or after this date.
    /// </summary>
    public DateTimeOffset? UpdatedFrom { get; init; }

    /// <summary>
    /// Returns feature templates updated on or before this date.
    /// </summary>
    public DateTimeOffset? UpdatedTo { get; init; }

    /// <summary>
    /// The field to sort results by.
    /// </summary>
    public SortBy SortBy { get; init; } = SortBy.Name;

    /// <summary>
    /// The sort direction of the returned feature templates.
    /// </summary>
    public SortDirection SortDirection { get; init; } = SortDirection.Asc;
}
