using System;
using System.Collections.Generic;
using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.AnyOf;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.Subscriptions;

/// <summary>
/// The inputs of the ListSubscriptions operation.
/// </summary>
public sealed record ListSubscriptionsRequest
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
    /// The attribute by which to sort
    /// </summary>
    public SubscriptionSort Sort { get; init; } = SubscriptionSort.SignupDate;

    /// <summary>
    /// Controls the order in which results are returned.
    /// Use in query <c>direction=asc</c>.
    /// </summary>
    public SortingDirection? Direction { get; init; }

    /// <summary>
    /// The current state of the subscription
    /// </summary>
    public SubscriptionStateFilter? State { get; init; }

    /// <summary>
    /// Filter subscriptions by product. Accepts product ID or exact product name. Product handle is not supported.
    /// </summary>
    public Product1? Product { get; init; }

    /// <summary>
    /// Search string.
    /// </summary>
    public string? Q { get; init; }

    /// <summary>
    /// Scope of fields used by the q search.
    /// </summary>
    public QScope? QScope { get; init; }

    /// <summary>
    /// The Advanced Billing id of the customer.
    /// </summary>
    public int? CustomerId { get; init; }

    /// <summary>
    /// The ID of the product price point. If supplied, product is required.
    /// </summary>
    public int? ProductPricePointId { get; init; }

    /// <summary>
    /// The numeric id of the coupon currently applied to the subscription. (This can be found in the URL when editing a coupon. Note that the coupon code cannot be used.)
    /// </summary>
    public int? Coupon { get; init; }

    /// <summary>
    /// The coupon code currently applied to the subscription
    /// </summary>
    public string? CouponCode { get; init; }

    /// <summary>
    /// The collection method for the subscription.
    /// </summary>
    public CollectionMethod1? CollectionMethod { get; init; }

    /// <summary>
    /// Filter subscriptions by the ID of an assigned Branding Theme. Branding Themes is a beta feature. See <see href="https://docs.maxio.com/hc/en-us/articles/43796895662093-Understand-Branding-Themes#understand-branding-themes-0-0">Understand Branding Themes</see> for more information.
    /// </summary>
    public int? BrandingThemeId { get; init; }

    /// <summary>
    /// The type of filter you'd like to apply to your search.  Allowed Values: , current_period_ends_at, current_period_starts_at, created_at, activated_at, canceled_at, expires_at, trial_started_at, trial_ended_at, updated_at
    /// </summary>
    public SubscriptionDateField? DateField { get; init; }

    /// <summary>
    /// The start date (format YYYY-MM-DD) with which to filter the date_field. Returns subscriptions with a timestamp at or after midnight (12:00:00 AM) in your site’s time zone on the date specified. Use in query <c>start_date=2022-07-01</c>.
    /// </summary>
    public DateTimeOffset? StartDate { get; init; }

    /// <summary>
    /// The end date (format YYYY-MM-DD) with which to filter the date_field. Returns subscriptions with a timestamp up to and including 11:59:59PM in your site’s time zone on the date specified. Use in query <c>end_date=2022-08-01</c>.
    /// </summary>
    public DateTimeOffset? EndDate { get; init; }

    /// <summary>
    /// The start date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns subscriptions with a timestamp at or after exact time provided in query. You can specify timezone in query - otherwise your site's time zone will be used. If provided, this parameter will be used instead of start_date. Use in query <c>start_datetime=2022-07-01 09:00:05</c>.
    /// </summary>
    public DateTimeOffset? StartDatetime { get; init; }

    /// <summary>
    /// The end date and time (format YYYY-MM-DD HH:MM:SS) with which to filter the date_field. Returns subscriptions with a timestamp at or before exact time provided in query. You can specify timezone in query - otherwise your site's time zone will be used. If provided, this parameter will be used instead of end_date. Use in query <c>end_datetime=2022-08-01 10:00:05</c>.
    /// </summary>
    public DateTimeOffset? EndDatetime { get; init; }

    /// <summary>
    /// The value of the metadata field specified in the parameter. Use in query <c>metadata[my-field]=value&amp;metadata[other-field]=another_value</c>.
    /// </summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }

    /// <summary>
    /// Filter by whether a subscription is in a group.
    /// </summary>
    public GroupStatus? GroupStatus { get; init; }

    /// <summary>
    /// Filter by dunning exemption status.
    /// </summary>
    public bool? DunningExemption { get; init; }

    /// <summary>
    /// Comma-separated payment gateway identifiers.
    /// </summary>
    public string? PaymentGateways { get; init; }

    /// <summary>
    /// Comma-separated currency codes.
    /// </summary>
    public string? Currencies { get; init; }

    /// <summary>
    /// Allows including additional data in the response. Use in query: <c>include[]=self_service_page_token</c>.
    /// </summary>
    public IReadOnlyList<SubscriptionListInclude>? Include { get; init; }
}
