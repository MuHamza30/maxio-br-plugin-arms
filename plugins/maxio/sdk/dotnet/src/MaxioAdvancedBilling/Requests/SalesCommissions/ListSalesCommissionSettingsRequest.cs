using MaxioAdvancedBilling.Core.Validation.Attributes;

namespace MaxioAdvancedBilling.Requests.SalesCommissions;

/// <summary>
/// The inputs of the ListSalesCommissionSettings operation.
/// </summary>
public sealed record ListSalesCommissionSettingsRequest
{
    /// <summary>
    /// The Chargify id of your seller account
    /// </summary>
    public required string SellerId { get; init; }

    /// <summary>
    /// This parameter indicates if records should be fetched from live mode sites. Default value is true.
    /// </summary>
    public bool? LiveMode { get; init; }

    /// <summary>
    /// Result records are organized in pages. By default, the first page of results is displayed. The page parameter specifies a page number of results to fetch. You can start navigating through the pages to consume the results. You do this by passing in a page parameter. Retrieve the next page by adding ?page=2 to the query string. If there are no results to return, then an empty result set will be returned.
    /// Use in query <c>page=1</c>.
    /// </summary>
    [Minimum(1)]
    public int Page { get; init; } = 1;

    /// <summary>
    /// This parameter indicates how many records to fetch in each request. Default value is 100.
    /// </summary>
    public int PerPage { get; init; } = 100;

    /// <summary>
    /// For authorization use user API key. See details <see href="https://developers.chargify.com/docs/developer-docs/ZG9jOjMyNzk5NTg0-2020-04-20-new-api-authentication">here</see>.
    /// </summary>
    public string Authorization { get; init; } = "Bearer <<apiKey>>";
}
