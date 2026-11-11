using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the ListSubscriptionComponentsForSite operation.
/// </summary>
public sealed record ListSubscriptionComponentsForSiteRequest
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
    /// The attribute by which to sort. Use in query: <c>sort=updated_at</c>.
    /// </summary>
    public ListSubscriptionComponentsSort? Sort { get; init; }

    /// <summary>
    /// Controls the order in which results are returned.
    /// Use in query <c>direction=asc</c>.
    /// </summary>
    public SortingDirection? Direction { get; init; }

    /// <summary>
    /// Filter to use for List Subscription Components For Site operation
    /// </summary>
    public ListSubscriptionComponentsForSiteFilter? Filter { get; init; }

    /// <summary>
    /// The type of filter you'd like to apply to your search. Use in query: <c>date_field=updated_at</c>.
    /// </summary>
    public SubscriptionListDateField? DateField { get; init; }

    /// <summary>
    /// The start date (format YYYY-MM-DD) with which to filter the date_field. Returns components with a timestamp at or after midnight (12:00:00 AM) in your site’s time zone on the date specified. Use in query <c>start_date=2011-12-15</c>.
    /// </summary>
    public string? StartDate { get; init; }

    /// <summary>
    /// The start date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns components with a timestamp at or after exact time provided in query. You can specify timezone in query - otherwise your site''s time zone will be used. If provided, this parameter will be used instead of start_date. Use in query <c>start_datetime=2022-07-01 09:00:05</c>.
    /// </summary>
    public string? StartDatetime { get; init; }

    /// <summary>
    /// The end date (format YYYY-MM-DD) with which to filter the date_field. Returns components with a timestamp up to and including 11:59:59PM in your site’s time zone on the date specified. Use in query <c>end_date=2011-12-16</c>.
    /// </summary>
    public string? EndDate { get; init; }

    /// <summary>
    /// The end date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns components with a timestamp at or before exact time provided in query. You can specify timezone in query - otherwise your site''s time zone will be used. If provided, this parameter will be used instead of end_date. Use in query <c>end_datetime=2022-07-01 09:00:05</c>.
    /// </summary>
    public string? EndDatetime { get; init; }

    /// <summary>
    /// Allows fetching components allocation with matching subscription id based on provided ids. Use in query <c>subscription_ids=1,2,3</c>.
    /// </summary>
    [MinLength(1)]
    [MaxLength(200)]
    public IReadOnlyList<int>? SubscriptionIds { get; init; }

    /// <summary>
    /// Allows fetching components allocation only if price point id is present. Use in query <c>price_point_ids=not_null</c>.
    /// </summary>
    public IncludeNotNull? PricePointIds { get; init; }

    /// <summary>
    /// Allows fetching components allocation with matching product family id based on provided ids. Use in query <c>product_family_ids=1,2,3</c>.
    /// </summary>
    public IReadOnlyList<int>? ProductFamilyIds { get; init; }

    /// <summary>
    /// Allows including additional data in the response. Use in query <c>include=subscription,historic_usages</c>.
    /// </summary>
    public ListSubscriptionComponentsInclude? Include { get; init; }
}
