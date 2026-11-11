using System.Collections.Generic;
using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.SubscriptionGroups;

/// <summary>
/// The inputs of the ListSubscriptionGroups operation.
/// </summary>
public sealed record ListSubscriptionGroupsRequest
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
    /// A list of additional information to include in the response. The following values are supported:
    /// <list type="bullet">
    ///   <item><description><c>account_balances</c>: Account balance information for the subscription groups. Use in query: <c>include[]=account_balances</c></description></item>
    /// </list>
    /// </summary>
    public IReadOnlyList<SubscriptionGroupsListInclude>? Include { get; init; }
}
