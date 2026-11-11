using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.Sites;

/// <summary>
/// The inputs of the ClearSite operation.
/// </summary>
public sealed record ClearSiteRequest
{
    /// <summary>
    /// <c>all</c>: Will clear all products, customers, and related subscriptions from the site.
    /// <c>customers</c>: Will clear only customers and related subscriptions (leaving the products untouched) for the site.
    /// Revenue will also be reset to 0.
    /// Use in query <c>cleanup_scope=all</c>.
    /// </summary>
    public CleanupScope CleanupScope { get; init; } = CleanupScope.All;
}
