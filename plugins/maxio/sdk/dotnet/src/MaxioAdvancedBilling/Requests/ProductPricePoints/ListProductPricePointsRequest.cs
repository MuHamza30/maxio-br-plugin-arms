using System.Collections.Generic;
using MaxioAdvancedBilling.Core.Validation.Attributes;
using MaxioAdvancedBilling.Models.AnyOf;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.ProductPricePoints;

/// <summary>
/// The inputs of the ListProductPricePoints operation.
/// </summary>
public sealed record ListProductPricePointsRequest
{
    /// <summary>
    /// The id or handle of the product. When using the handle, it must be prefixed with <c>handle:</c>
    /// </summary>
    public required ProductIdModel ProductId { get; init; }

    /// <summary>
    /// Result records are organized in pages. By default, the first page of results is displayed. The page parameter specifies a page number of results to fetch. You can start navigating through the pages to consume the results. You do this by passing in a page parameter. Retrieve the next page by adding ?page=2 to the query string. If there are no results to return, then an empty result set will be returned.
    /// Use in query <c>page=1</c>.
    /// </summary>
    [Minimum(1)]
    public int Page { get; init; } = 1;

    /// <summary>
    /// This parameter indicates how many records to fetch in each request. Default value is 10. The maximum allowed values is 200; any per_page value over 200 will be changed to 200.
    /// </summary>
    [Maximum(200)]
    public int PerPage { get; init; } = 10;

    /// <summary>
    /// (Optional) If you have defined multiple currencies at the site level, you can pass ?currency_prices=true to include an array of currency price data in the response. If the product price point is set to use_site_exchange_rate: true, it will return pricing based on the current exchange rate. If the flag is set to false, it will return all of the defined prices for each currency.
    /// </summary>
    public bool? CurrencyPrices { get; init; }

    /// <summary>
    /// Use in query: <c>filter[type]=catalog,default</c>.
    /// </summary>
    public IReadOnlyList<PricePointType>? FilterType { get; init; }

    /// <summary>
    /// Set to include archived price points in the response.
    /// </summary>
    public bool? Archived { get; init; }
}
