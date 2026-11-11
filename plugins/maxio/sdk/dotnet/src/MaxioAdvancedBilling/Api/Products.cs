using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.Products;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Product object defines what you sell, including default pricing, billing interval, and trial configuration. With the new catalog experience, the UI labels products as Plans, while the API keeps the product name. See <see href="https://docs.maxio.com/billing/catalog/overview/using-the-new-product-catalog#catalog-terminology">Catalog terminology</see>.
/// <para>
/// Products are grouped under a Product Family and are the base type a customer subscribes to. Products can also grant features to subscribers. See the <b>Create Product Feature Catalog Item</b> endpoint for details. For more information, see <see href="https://docs.maxio.com/billing/catalog/plans/understand-plans">Understand Plans</see>.
/// </para>
/// </summary>
public sealed class Products
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Products(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Archive Product
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ArchiveProductError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Archives the product. All current subscribers will be unaffected; their subscription/purchase will continue to be charged monthly.
    /// <para>
    /// This will restrict the option to chose the product for purchase via the Billing Portal, as well as disable Public Signup Pages for the product.
    /// </para>
    /// </remarks>
    public Task<ProductResponse> ArchiveProduct(ArchiveProductRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}.json"),
            [new TemplateParam("product_id", request.ProductId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<ProductResponse>(),
            ArchiveProductError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Product
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateProductError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a product in your site.
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, the <c>auto_create_signup_page</c> parameter is not supported. If <c>auto_create_signup_page</c> is included (with any value) an error is returned.
    /// </para>
    /// <para>
    /// For more information, see:
    /// </para>
    /// <list type="bullet">
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24261090117645-Products-Overview">Products Overview</see></description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24252069837581-Product-Changes-and-Migrations">Changing a Subscription's Product</see></description></item>
    /// </list>
    /// </remarks>
    public Task<ProductResponse> CreateProduct(CreateProductRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/products.json"),
            [new TemplateParam("product_family_id", request.ProductFamilyId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ProductResponse>(),
            CreateProductError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Products
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="ProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists products belonging to a site.
    /// </remarks>
    public Task<IReadOnlyList<ProductResponse>> ListProducts(ListProductsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products.json"),
            [],
            [
                new Param("date_field", request.DateField),
                new Param("filter", request.Filter),
                new Param("end_date", request.EndDate?.ToDate()),
                new Param("end_datetime", request.EndDatetime?.ToIso8601()),
                new Param("start_date", request.StartDate?.ToDate()),
                new Param("start_datetime", request.StartDatetime?.ToIso8601()),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("include_archived", request.IncludeArchived),
                new Param("include", request.Include),
                new Param("include_features", request.IncludeFeatures),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<ProductResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Product
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Reads the current details of a product.
    /// </remarks>
    public Task<ProductResponse> ReadProduct(ReadProductRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}.json"),
            [new TemplateParam("product_id", request.ProductId)],
            [new Param("include_features", request.IncludeFeatures)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ProductResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Product by Handle
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves a Product object by its <c>api_handle</c>.
    /// </remarks>
    public Task<ProductResponse> ReadProductByHandle(ReadProductByHandleRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/handle/{api_handle}.json"),
            [new TemplateParam("api_handle", request.ApiHandle)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ProductResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Product
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateProductError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates aspects of an existing product.
    /// <para>
    /// ### Input Attributes Update Notes
    /// </para>
    /// <list type="bullet">
    ///   <item><description><c>update_return_params</c> The parameters we will append to your <c>update_return_url</c>. See Return URLs and Parameters</description></item>
    /// </list>
    /// <para>
    /// ### Product Price Point
    /// </para>
    /// <para>
    /// Updating a product using this endpoint will create a new price point and set it as the default price point for this product. If you should like to update an existing product price point, that must be done separately.
    /// </para>
    /// </remarks>
    public Task<ProductResponse> UpdateProduct(UpdateProductRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}.json"),
            [new TemplateParam("product_id", request.ProductId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ProductResponse>(),
            UpdateProductError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
