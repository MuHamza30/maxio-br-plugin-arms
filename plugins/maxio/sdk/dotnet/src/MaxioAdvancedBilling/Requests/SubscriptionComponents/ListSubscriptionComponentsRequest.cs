using System.Collections.Generic;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the ListSubscriptionComponents operation.
/// </summary>
public sealed record ListSubscriptionComponentsRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The type of filter you'd like to apply to your search. Use in query <c>date_field=updated_at</c>.
    /// </summary>
    public SubscriptionListDateField? DateField { get; init; }

    /// <summary>
    /// Controls the order in which results are returned.
    /// Use in query <c>direction=asc</c>.
    /// </summary>
    public SortingDirection? Direction { get; init; }

    /// <summary>
    /// Filter to use for List Subscription Components operation
    /// </summary>
    public ListSubscriptionComponentsFilter? Filter { get; init; }

    /// <summary>
    /// The end date (format YYYY-MM-DD) with which to filter the date_field. Returns components with a timestamp up to and including 11:59:59PM in your site’s time zone on the date specified.
    /// </summary>
    public string? EndDate { get; init; }

    /// <summary>
    /// The end date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns components with a timestamp at or before exact time provided in query. You can specify timezone in query - otherwise your site''s time zone will be used. If provided, this parameter will be used instead of end_date.
    /// </summary>
    public string? EndDatetime { get; init; }

    /// <summary>
    /// Allows fetching components allocation only if price point id is present. Use in query <c>price_point_ids=not_null</c>.
    /// </summary>
    public IncludeNotNull? PricePointIds { get; init; }

    /// <summary>
    /// Allows fetching components allocation with matching product family id based on provided ids. Use in query <c>product_family_ids=1,2,3</c>.
    /// </summary>
    public IReadOnlyList<int>? ProductFamilyIds { get; init; }

    /// <summary>
    /// The attribute by which to sort. Use in query <c>sort=updated_at</c>.
    /// </summary>
    public ListSubscriptionComponentsSort? Sort { get; init; }

    /// <summary>
    /// The start date (format YYYY-MM-DD) with which to filter the date_field. Returns components with a timestamp at or after midnight (12:00:00 AM) in your site’s time zone on the date specified.
    /// </summary>
    public string? StartDate { get; init; }

    /// <summary>
    /// The start date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns components with a timestamp at or after exact time provided in query. You can specify timezone in query - otherwise your site''s time zone will be used. If provided, this parameter will be used instead of start_date.
    /// </summary>
    public string? StartDatetime { get; init; }

    /// <summary>
    /// Allows including additional data in the response. Use in query <c>include=subscription,historic_usages</c>.
    /// </summary>
    public IReadOnlyList<ListSubscriptionComponentsInclude>? Include { get; init; }

    /// <summary>
    /// If in_use is set to true, it returns only components that are currently in use. However, if it's set to false or not provided, it returns all components connected with the subscription.
    /// </summary>
    public bool? InUse { get; init; }
}
