using System.Collections.Generic;
using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.Events;

/// <summary>
/// The inputs of the ReadEventsCount operation.
/// </summary>
public sealed record ReadEventsCountRequest
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
    /// Returns events with an id greater than or equal to the one specified.
    /// </summary>
    public long? SinceId { get; init; }

    /// <summary>
    /// Returns events with an id less than or equal to the one specified.
    /// </summary>
    public long? MaxId { get; init; }

    /// <summary>
    /// The sort direction of the returned events.
    /// </summary>
    public Direction Direction { get; init; } = Direction.Desc;

    /// <summary>
    /// You can pass multiple event keys after comma.
    /// Use in query <c>filter=signup_success,payment_success</c>.
    /// </summary>
    public IReadOnlyList<EventKey>? Filter { get; init; }
}
