using MaxioAdvancedBilling.Core.Validation.Attributes;

namespace MaxioAdvancedBilling.Requests.ApiExports;

/// <summary>
/// The inputs of the ListExportedSubscriptions operation.
/// </summary>
public sealed record ListExportedSubscriptionsRequest
{
    /// <summary>
    /// Id of a Batch Job.
    /// </summary>
    public required string BatchId { get; init; }

    /// <summary>
    /// This parameter indicates how many records to fetch in each request.
    /// Default value is 100.
    /// The maximum allowed values is 10000; any per_page value over 10000 will be changed to 10000.
    /// </summary>
    [Minimum(1)]
    [Maximum(10000)]
    public int PerPage { get; init; } = 100;

    /// <summary>
    /// Result records are organized in pages. By default, the first page of results is displayed. The page parameter specifies a page number of results to fetch. You can start navigating through the pages to consume the results. You do this by passing in a page parameter. Retrieve the next page by adding ?page=2 to the query string. If there are no results to return, then an empty result set will be returned.
    /// Use in query <c>page=1</c>.
    /// </summary>
    [Minimum(1)]
    public int Page { get; init; } = 1;
}
