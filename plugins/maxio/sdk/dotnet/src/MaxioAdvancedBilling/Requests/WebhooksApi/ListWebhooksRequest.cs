using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.WebhooksApi;

/// <summary>
/// The inputs of the ListWebhooks operation.
/// </summary>
public sealed record ListWebhooksRequest
{
    /// <summary>
    /// Webhooks with matching status would be returned.
    /// </summary>
    public WebhookStatus? Status { get; init; }

    /// <summary>
    /// Format YYYY-MM-DD. Returns Webhooks with the created_at date greater than or equal to the one specified.
    /// </summary>
    public string? SinceDate { get; init; }

    /// <summary>
    /// Format YYYY-MM-DD. Returns Webhooks with the created_at date less than or equal to the one specified.
    /// </summary>
    public string? UntilDate { get; init; }

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
    /// The order in which the Webhooks are returned.
    /// </summary>
    public WebhookOrder? Order { get; init; }

    /// <summary>
    /// The Advanced Billing id of a subscription you'd like to filter for
    /// </summary>
    public int? Subscription { get; init; }
}
