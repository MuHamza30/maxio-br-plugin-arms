using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.Insights;

/// <summary>
/// The inputs of the ListMrrMovements operation.
/// </summary>
public sealed record ListMrrMovementsRequest
{
    /// <summary>
    /// (Optional) Filter results by subscription.
    /// </summary>
    public int? SubscriptionId { get; init; }

    /// <summary>
    /// Result records are organized in pages. By default, the first page of results is displayed. The page parameter specifies a page number of results to fetch. You can start navigating through the pages to consume the results. You do this by passing in a page parameter. Retrieve the next page by adding ?page=2 to the query string. If there are no results to return, then an empty result set will be returned.
    /// Use in query <c>page=1</c>.
    /// </summary>
    [Minimum(1)]
    public int Page { get; init; } = 1;

    /// <summary>
    /// This parameter indicates how many records to fetch in each request. Default value is 10. The maximum allowed values is 50; any per_page value over 50 will be changed to 50.
    /// Use in query <c>per_page=20</c>.
    /// </summary>
    [Maximum(50)]
    public int PerPage { get; init; } = 10;

    /// <summary>
    /// Controls the order in which results are returned.
    /// Use in query <c>direction=asc</c>.
    /// </summary>
    public SortingDirection? Direction { get; init; }
}
