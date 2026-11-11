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
