using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Requests.ProductPricePoints;

/// <summary>
/// The inputs of the ReadProductPricePoint operation.
/// </summary>
public sealed record ReadProductPricePointRequest
{
    /// <summary>
    /// The id or handle of the product. When using the handle, it must be prefixed with <c>handle:</c>. Example: <c>123</c> for an integer ID, or <c>handle:example-product-handle</c> for a string handle.
    /// </summary>
    public required ProductIdModel ProductId { get; init; }

    /// <summary>
    /// The id or handle of the price point. When using the handle, it must be prefixed with <c>handle:</c>. Example: <c>123</c> for an integer ID, or <c>handle:example-product-price-point-handle</c> for a string handle.
    /// </summary>
    public required PricePointIdModel PricePointId { get; init; }

    /// <summary>
    /// (Optional) If you have defined multiple currencies at the site level, you can pass ?currency_prices=true to include an array of currency price data in the response. If the product price point is set to use_site_exchange_rate: true, it will return pricing based on the current exchange rate. If the flag is set to false, it will return all of the defined prices for each currency.
    /// </summary>
    public bool? CurrencyPrices { get; init; }
}
