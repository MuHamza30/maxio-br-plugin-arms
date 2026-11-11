using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.Insights;

/// <summary>
/// The inputs of the ListMrrPerSubscription operation.
/// </summary>
public sealed record ListMrrPerSubscriptionRequest
{
    /// <summary>
    /// Filter to use for List MRR per subscription operation
    /// </summary>
    public ListMrrFilter? Filter { get; init; }

    /// <summary>
    /// Submit a timestamp in ISO8601 format to request MRR for a historic time. Use in query: <c>at_time=2022-01-10T10:00:00-05:00</c>.
    /// </summary>
    public string? AtTime { get; init; }

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
    /// Controls the order in which results are returned. Records are ordered by subscription_id in ascending order by default. Use in query <c>direction=desc</c>.
    /// </summary>
    public Direction? Direction { get; init; }
}
