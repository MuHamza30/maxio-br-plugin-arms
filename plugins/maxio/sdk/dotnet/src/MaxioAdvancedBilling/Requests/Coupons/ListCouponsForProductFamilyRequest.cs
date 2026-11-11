using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Coupons;

/// <summary>
/// The inputs of the ListCouponsForProductFamily operation.
/// </summary>
public sealed record ListCouponsForProductFamilyRequest
{
    /// <summary>
    /// The Advanced Billing id of the product family to which the coupon belongs
    /// </summary>
    public required int ProductFamilyId { get; init; }

    /// <summary>
    /// Result records are organized in pages. By default, the first page of results is displayed. The page parameter specifies a page number of results to fetch. You can start navigating through the pages to consume the results. You do this by passing in a page parameter. Retrieve the next page by adding ?page=2 to the query string. If there are no results to return, then an empty result set will be returned.
    /// Use in query <c>page=1</c>.
    /// </summary>
    [Minimum(1)]
    public int Page { get; init; } = 1;

    /// <summary>
    /// This parameter indicates how many records to fetch in each request. Default value is 30. The maximum allowed values is 200; any per_page value over 200 will be changed to 200.
    /// Use in query <c>per_page=200</c>.
    /// </summary>
    [Maximum(200)]
    public int PerPage { get; init; } = 30;

    /// <summary>
    /// Filter to use for List Coupons operations
    /// </summary>
    public ListCouponsFilter? Filter { get; init; }

    /// <summary>
    /// (Optional) If you have defined multiple currencies at the site level, you can pass <c>?currency_prices=true</c> to include an array of currency price data in the response. Use in query <c>currency_prices=true</c>.
    /// </summary>
    public bool? CurrencyPrices { get; init; }
}
