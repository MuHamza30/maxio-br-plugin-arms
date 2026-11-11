using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.ProductPricePoints;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Product Price Point object defines a specific pricing configuration for a Product, including price, billing interval, and trial terms.
/// <para>
/// A single Product can have multiple price points, letting you offer different rates or terms for the same product without creating separate products. For more information, see <see href="https://docs.maxio.com/billing/catalog/plans/setting-up-plan-price-points">Setting Up Plan Price Points</see>.
/// </para>
/// </summary>
public sealed class ProductPricePoints
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal ProductPricePoints(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Archive Product Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ArchiveProductPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Archives a product price point.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The price point to archive is the product's default price point → Maxio answers 422 with "Cannot archive the default price point.". Promote another price point to default before archiving this one.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    /// </list>
    /// </remarks>
    public Task<ProductPricePointResponse> ArchiveProductPricePoint(ArchiveProductPricePointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/price_points/{price_point_id}.json"),
            [
                new TemplateParam("product_id", request.ProductId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<ProductPricePointResponse>(),
            ArchiveProductPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Bulk Create Product Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="BulkCreateProductPricePointsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="BulkCreateProductPricePointsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates multiple product price points in one request.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>interval is less than 1 (including fractions such as 0.5) → Maxio answers 422 with "Recurring Interval: must be greater than or equal to 1.". Send interval as a whole number of at least 1.</description></item>
    ///   <item><description>The price is below 0 → Maxio answers 422 with "Price: must be greater than or equal to 0.". Send prices of 0 or more.</description></item>
    ///   <item><description>trial_type is no_obligation and a subscription's trial ends with no credit card on file → The subscription moves to Trial Ended and Maxio sends no emails or statements. Choose no_obligation only when trials should end quietly without dunning.</description></item>
    ///   <item><description>trial_type is payment_expected and a subscription's trial ends with no credit card on file → The subscription moves to Past Due and Maxio sends normal dunning emails and statements. Choose payment_expected only when trials without a card on file should go into dunning.</description></item>
    /// </list>
    /// </remarks>
    public Task<BulkCreateProductPricePointsResponse> BulkCreateProductPricePoints(BulkCreateProductPricePointsOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/price_points/bulk.json"),
            [new TemplateParam("product_id", request.ProductId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<BulkCreateProductPricePointsResponse>(),
            BulkCreateProductPricePointsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Product Currency Prices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CurrencyPricesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateProductCurrencyPricesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates currency prices for a given currency that has been defined on the site level in your settings.
    /// <para>
    /// When creating currency prices, they need to mirror the structure of your primary pricing. If the product price point defines a trial and/or setup fee, each currency must also define a trial and/or setup fee.
    /// </para>
    /// <para>
    /// Note: Currency Prices are not able to be created for custom product price points.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A currency price is sent for a currency not defined in the site's settings → the outcome is not stated. Define the currency at the site level before adding product currency prices in it.</description></item>
    ///   <item><description>The currency prices do not cover every price role the product price point defines (baseline, plus trial and/or initial setup fee when defined) → Maxio answers 422 with "This product price point requires that prices are defined for: [baseline].". Send a currency price for each role the price point uses: baseline, and trial and initial when it has a trial or setup fee.</description></item>
    ///   <item><description>The product price point is a custom price point → the outcome is not stated. Create currency prices only on non-custom product price points.</description></item>
    /// </list>
    /// </remarks>
    public Task<CurrencyPricesResponse> CreateProductCurrencyPrices(CreateProductCurrencyPricesOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_price_points/{product_price_point_id}/currency_prices.json"),
            [new TemplateParam("product_price_point_id", request.ProductPricePointId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<CurrencyPricesResponse>(),
            CreateProductCurrencyPricesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Product Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateProductPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a Product Price Point. See the <see href="https://maxio.zendesk.com/hc/en-us/articles/24261111947789-Product-Price-Points">Product Price Point</see> documentation for details.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>interval is less than 1 (including fractions such as 0.5) → Maxio answers 422 with "Recurring Interval: must be greater than or equal to 1.". Send interval as a whole number of at least 1.</description></item>
    ///   <item><description>The price is below 0 → Maxio answers 422 with "Price: must be greater than or equal to 0.". Send prices of 0 or more.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>trial_type is no_obligation and a subscription's trial ends with no credit card on file → The subscription moves to Trial Ended and Maxio sends no emails or statements. Choose no_obligation only when trials should end quietly without dunning.</description></item>
    ///   <item><description>trial_type is payment_expected and a subscription's trial ends with no credit card on file → The subscription moves to Past Due and Maxio sends normal dunning emails and statements. Choose payment_expected only when trials without a card on file should go into dunning.</description></item>
    /// </list>
    /// </remarks>
    public Task<ProductPricePointResponse> CreateProductPricePoint(CreateProductPricePointOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/price_points.json"),
            [new TemplateParam("product_id", request.ProductId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ProductPricePointResponse>(),
            CreateProductPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List All Products Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListProductPricePointsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListAllProductPricePointsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists Product Price Points belonging to a site.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>filter[start_date] (or another date filter) is not a valid ISO 8601 value → Maxio answers 422 with "start_date supplied value is invalid, expected ISO 8601 format". Send date filters in ISO 8601 format (YYYY-MM-DD, or a full timestamp for the datetime filters).</description></item>
    ///   <item><description>filter[date_field] is a value other than created_at or updated_at → Maxio answers 422 with "date_field must be one of: created_at, updated_at". Send filter[date_field] as created_at or updated_at only.</description></item>
    ///   <item><description>filter[type] is not sent → Only catalog and default price points are returned; custom price points are left out. Pass filter[type] including custom when custom price points are needed.</description></item>
    ///   <item><description>A price point is read or listed without the currency prices query parameter (currency_prices=true or include=currency_prices) → currency_prices is left out of the response; it appears only on read endpoints when requested. Pass the currency prices query parameter when you need currency pricing.</description></item>
    ///   <item><description>Currency prices are requested for a price point → With use_site_exchange_rate true the prices are computed from the current exchange rate; with false the defined prices for each currency are returned. Expect converted prices to change with the exchange rate when use_site_exchange_rate is true.</description></item>
    ///   <item><description>Currency prices are requested on a site without multiple currencies defined → Currency pricing data is available only when the site defines multiple currencies. Define extra currencies at the site level before relying on currency_prices.</description></item>
    ///   <item><description>A list is filtered with start_date or end_date → start_date matches from midnight and end_date up to and including 11:59:59 PM, both in the site's time zone. Convert date filters to the site's time zone, or use start_datetime/end_datetime with an explicit offset.</description></item>
    ///   <item><description>Both start_datetime and start_date (or end_datetime and end_date) are sent → The datetime parameter is used and the date parameter is ignored. Send either the date or the datetime form of each bound, not both.</description></item>
    ///   <item><description>per_page is omitted or set above 200 → 20 records are returned by default; a per_page above 200 is changed to 200. Page through results with per_page of at most 200 and do not assume a larger page was honoured.</description></item>
    ///   <item><description>A page number past the last page of results is requested → An empty result set is returned, not an error. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include an explicit time zone in start_datetime and end_datetime.</description></item>
    /// </list>
    /// </remarks>
    public Task<ListProductPricePointsResponse> ListAllProductPricePoints(ListAllProductPricePointsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products_price_points.json"),
            [],
            [
                new Param("direction", request.Direction),
                new Param("filter", request.Filter),
                new Param("include", request.Include),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListProductPricePointsResponse>(),
            ListAllProductPricePointsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Product Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListProductPricePointsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves a list of product price points.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A price point is read or listed without the currency prices query parameter (currency_prices=true or include=currency_prices) → currency_prices is left out of the response; it appears only on read endpoints when requested. Pass the currency prices query parameter when you need currency pricing.</description></item>
    ///   <item><description>Currency prices are requested for a price point → With use_site_exchange_rate true the prices are computed from the current exchange rate; with false the defined prices for each currency are returned. Expect converted prices to change with the exchange rate when use_site_exchange_rate is true.</description></item>
    ///   <item><description>Currency prices are requested on a site without multiple currencies defined → Currency pricing data is available only when the site defines multiple currencies. Define extra currencies at the site level before relying on currency_prices.</description></item>
    ///   <item><description>A page number past the last page of results is requested → An empty result set is returned, not an error. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A product's price points are listed → Results are always in descending created_at order; the default price point is not put first. Find the default price point by its type or the product's default id, not by its position in the list.</description></item>
    ///   <item><description>per_page is omitted or set above 200 → 10 records are returned by default; a per_page above 200 is changed to 200. Page through product price points with per_page of at most 200, expecting 10 by default.</description></item>
    ///   <item><description>archived is not set → Archived price points are left out of the response. Pass archived=true when archived price points are needed.</description></item>
    /// </list>
    /// </remarks>
    public Task<ListProductPricePointsResponse> ListProductPricePoints(ListProductPricePointsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/price_points.json"),
            [new TemplateParam("product_id", request.ProductId)],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("currency_prices", request.CurrencyPrices),
                new Param("filter[type]", request.FilterType),
                new Param("archived", request.Archived),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListProductPricePointsResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Promote Product Price Point to Default
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Sets a product price point as the default for the product.
    /// <para>
    /// Note: Custom product price points cannot be set as the default for a product.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The price point to promote is a custom product price point → the outcome is not stated. Promote only non-custom product price points to default.</description></item>
    /// </list>
    /// </remarks>
    public Task<ProductResponse> PromoteProductPricePointToDefault(PromoteProductPricePointToDefaultRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/price_points/{price_point_id}/default.json"),
            [
                new TemplateParam("product_id", request.ProductId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            new HttpMethod("PATCH"),
            EmptyBody.Instance,
            JsonResponse.Create<ProductResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Product Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns details for a specific product price point. You can achieve this by using either the product price point ID or handle.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A price point is read or listed without the currency prices query parameter (currency_prices=true or include=currency_prices) → currency_prices is left out of the response; it appears only on read endpoints when requested. Pass the currency prices query parameter when you need currency pricing.</description></item>
    ///   <item><description>Currency prices are requested for a price point → With use_site_exchange_rate true the prices are computed from the current exchange rate; with false the defined prices for each currency are returned. Expect converted prices to change with the exchange rate when use_site_exchange_rate is true.</description></item>
    ///   <item><description>Currency prices are requested on a site without multiple currencies defined → Currency pricing data is available only when the site defines multiple currencies. Define extra currencies at the site level before relying on currency_prices.</description></item>
    /// </list>
    /// </remarks>
    public Task<ProductPricePointResponse> ReadProductPricePoint(ReadProductPricePointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/price_points/{price_point_id}.json"),
            [
                new TemplateParam("product_id", request.ProductId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [new Param("currency_prices", request.CurrencyPrices)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ProductPricePointResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Unarchive Product Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Unarchives an archived product price point.
    /// </remarks>
    public Task<ProductPricePointResponse> UnarchiveProductPricePoint(UnarchiveProductPricePointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/price_points/{price_point_id}/unarchive.json"),
            [
                new TemplateParam("product_id", request.ProductId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            new HttpMethod("PATCH"),
            EmptyBody.Instance,
            JsonResponse.Create<ProductPricePointResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Product Currency Prices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CurrencyPricesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateProductCurrencyPricesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the <c>price</c>s of currency prices for a given currency that exists on the product price point.
    /// <para>
    /// When updating the pricing, it needs to mirror the structure of your primary pricing. If the product price point defines a trial and/or setup fee, each currency must also define a trial and/or setup fee.
    /// </para>
    /// <para>
    /// Note: Currency Prices cannot be updated for custom product price points.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The currency prices do not cover every price role the product price point defines (baseline, plus trial and/or initial setup fee when defined) → Maxio answers 422 with "This product price point requires that prices are defined for: [baseline].". Send a currency price for each role the price point uses: baseline, and trial and initial when it has a trial or setup fee.</description></item>
    ///   <item><description>The product price point is a custom price point → the outcome is not stated. Update currency prices only on non-custom product price points.</description></item>
    ///   <item><description>Prices are updated for a currency that has no currency prices on the product price point yet → the outcome is not stated. Create the currency prices with Create Product Currency Prices before updating them.</description></item>
    /// </list>
    /// </remarks>
    public Task<CurrencyPricesResponse> UpdateProductCurrencyPrices(UpdateProductCurrencyPricesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_price_points/{product_price_point_id}/currency_prices.json"),
            [new TemplateParam("product_price_point_id", request.ProductPricePointId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<CurrencyPricesResponse>(),
            UpdateProductCurrencyPricesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Product Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates a product price point.
    /// <para>
    /// Note: Custom product price points cannot be updated.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>The product price point is a custom price point → the outcome is not stated. Do not try to update custom product price points.</description></item>
    /// </list>
    /// </remarks>
    public Task<ProductPricePointResponse> UpdateProductPricePoint(UpdateProductPricePointOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/price_points/{price_point_id}.json"),
            [
                new TemplateParam("product_id", request.ProductId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ProductPricePointResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
