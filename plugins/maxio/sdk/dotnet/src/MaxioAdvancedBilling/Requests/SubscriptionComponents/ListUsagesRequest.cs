using System;
using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the ListUsages operation.
/// </summary>
public sealed record ListUsagesRequest
{
    /// <summary>
    /// Either the Advanced Billing subscription ID (integer) or the subscription reference (string). Important: In cases where a numeric string value matches both an existing subscription ID and an existing subscription reference, the system will prioritize the subscription ID lookup. For example, if both subscription ID 123 and subscription reference "123" exist, passing "123" will return the subscription with ID 123.
    /// </summary>
    public required SubscriptionIdOrReference SubscriptionIdOrReference { get; init; }

    /// <summary>
    /// Either the Advanced Billing id for the component or the component's handle prefixed by <c>handle:</c>
    /// </summary>
    public required ComponentIdModel ComponentId { get; init; }

    /// <summary>
    /// Returns usages with an id greater than or equal to the one specified.
    /// </summary>
    public long? SinceId { get; init; }

    /// <summary>
    /// Returns usages with an id less than or equal to the one specified.
    /// </summary>
    public long? MaxId { get; init; }

    /// <summary>
    /// Returns usages with a created_at date greater than or equal to midnight (12:00 AM) on the date specified.
    /// </summary>
    public DateTimeOffset? SinceDate { get; init; }

    /// <summary>
    /// Returns usages with a created_at date less than or equal to midnight (12:00 AM) on the date specified.
    /// </summary>
    public DateTimeOffset? UntilDate { get; init; }

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
