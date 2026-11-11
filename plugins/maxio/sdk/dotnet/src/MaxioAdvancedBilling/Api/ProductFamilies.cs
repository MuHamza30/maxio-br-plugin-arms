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
using MaxioAdvancedBilling.Requests.ProductFamilies;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Product Family object groups related Products together for organizational purposes, such as products belonging to a single line of business, each sharing a family-level handle and accounting code. For more information, see <see href="https://docs.maxio.com/billing/catalog/plans/organize-product-families">Organize Product Families</see>.
/// </summary>
public sealed class ProductFamilies
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal ProductFamilies(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Product Family
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductFamilyResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateProductFamilyError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a Product Family within your site. Create a Product Family to act as a container for your products, components, and coupons.
    /// <para>
    /// Full documentation on how Product Families operate within the Advanced Billing UI can be located <see href="https://maxio.zendesk.com/hc/en-us/articles/24261098936205-Product-Families">here</see>.
    /// </para>
    /// </remarks>
    public Task<ProductFamilyResponse> CreateProductFamily(CreateProductFamilyOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ProductFamilyResponse>(),
            CreateProductFamilyError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Product Families
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="ProductFamilyResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists Product Families for a site.
    /// </remarks>
    public Task<IReadOnlyList<ProductFamilyResponse>> ListProductFamilies(ListProductFamiliesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families.json"),
            [],
            [
                new Param("date_field", request.DateField),
                new Param("start_date", request.StartDate?.ToDate()),
                new Param("end_date", request.EndDate?.ToDate()),
                new Param("start_datetime", request.StartDatetime?.ToIso8601()),
                new Param("end_datetime", request.EndDatetime?.ToIso8601()),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<ProductFamilyResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Products for Product Family
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="ProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListProductsForProductFamilyError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves a list of Products belonging to a Product Family.
    /// </remarks>
    public Task<IReadOnlyList<ProductResponse>> ListProductsForProductFamily(ListProductsForProductFamilyRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/products.json"),
            [new TemplateParam("product_family_id", request.ProductFamilyId)],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("date_field", request.DateField),
                new Param("filter", request.Filter),
                new Param("start_date", request.StartDate?.ToDate()),
                new Param("end_date", request.EndDate?.ToDate()),
                new Param("start_datetime", request.StartDatetime?.ToIso8601()),
                new Param("end_datetime", request.EndDatetime?.ToIso8601()),
                new Param("include_archived", request.IncludeArchived),
                new Param("include", request.Include),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<ProductResponse>>(),
            ListProductsForProductFamilyError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Product Family
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProductFamilyResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves a Product Family via the <c>product_family_id</c>. The response will contain a Product Family object.
    /// <para>
    /// The product family can be specified either with the id number, or with the <c>handle:my-family</c> format.
    /// </para>
    /// </remarks>
    public Task<ProductFamilyResponse> ReadProductFamily(ReadProductFamilyRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ProductFamilyResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
