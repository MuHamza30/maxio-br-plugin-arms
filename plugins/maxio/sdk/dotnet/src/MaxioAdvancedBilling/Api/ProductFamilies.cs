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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A product is created → the outcome is not stated. Create the product family first and create products inside it.</description></item>
    ///   <item><description>The product family handle is already taken → Maxio answers 422 with "API Handle: must be unique - that value has been taken.". Use a product family handle not already taken on the site.</description></item>
    ///   <item><description>A product family is created without a handle → The handle is generated automatically. Read the generated handle from the response if you did not send one.</description></item>
    ///   <item><description>surcharging is omitted when creating a product family → surcharging defaults to true. Send surcharging: false explicitly if the family must not be surcharged.</description></item>
    ///   <item><description>surcharging is sent on a site where surcharging is not enabled → The value is not applied. Rely on the surcharging setting only on sites with surcharging enabled.</description></item>
    ///   <item><description>A product family is returned on a site where surcharging is not enabled → The surcharging field is left out of the product family. Treat a missing surcharging field as normal on sites without surcharging.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A list is filtered with start_date or end_date → start_date matches from midnight and end_date up to and including 11:59:59 PM, both in the site's time zone. Convert date filters to the site's time zone, or use start_datetime/end_datetime with an explicit offset.</description></item>
    ///   <item><description>Both start_datetime and start_date (or end_datetime and end_date) are sent → The datetime parameter is used and the date parameter is ignored. Send either the date or the datetime form of each bound, not both.</description></item>
    ///   <item><description>date_field is given a value other than created_at or updated_at → The value is ignored and the date filters are not applied. Send date_field as created_at or updated_at only, and check that date-filtered results really are filtered.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include an explicit time zone in start_datetime and end_datetime.</description></item>
    ///   <item><description>A product family is returned on a site where surcharging is not enabled → The surcharging field is left out of the product family. Treat a missing surcharging field as normal on sites without surcharging.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The product family id in the path is not a valid product family → Maxio answers 404 with "A valid product_family_id is required". List products only for a product family that exists.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A list is filtered with start_date or end_date → start_date matches from midnight and end_date up to and including 11:59:59 PM, both in the site's time zone. Convert date filters to the site's time zone, or use start_datetime/end_datetime with an explicit offset.</description></item>
    ///   <item><description>Both start_datetime and start_date (or end_datetime and end_date) are sent → The datetime parameter is used and the date parameter is ignored. Send either the date or the datetime form of each bound, not both.</description></item>
    ///   <item><description>date_field is given a value other than created_at or updated_at → The value is ignored and the date filters are not applied. Send date_field as created_at or updated_at only, and check that date-filtered results really are filtered.</description></item>
    ///   <item><description>include_archived is not set to true → Archived components or products are left out of the list. Pass include_archived=true when archived items must be listed.</description></item>
    ///   <item><description>filter[use_site_exchange_rate] is used → The filter matches the setting of each item's default price point, not of its other price points. Remember filter[use_site_exchange_rate] tests only the default price point.</description></item>
    ///   <item><description>per_page is omitted or set above 200 → 20 records are returned by default; a per_page above 200 is changed to 200. Page through results with per_page of at most 200 and do not assume a larger page was honoured.</description></item>
    ///   <item><description>A page number past the last page of results is requested → An empty result set is returned, not an error. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>filter[prepaid_product_price_point] is sent without include=prepaid_product_price_point → the outcome is not stated. Send include=prepaid_product_price_point whenever you use the prepaid product price point filter.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include an explicit time zone in start_datetime and end_datetime.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A product family is returned on a site where surcharging is not enabled → The surcharging field is left out of the product family. Treat a missing surcharging field as normal on sites without surcharging.</description></item>
    ///   <item><description>The product family is given in the path as handle:&lt;handle&gt; instead of a numeric id → The product family is looked up by handle. Use either the numeric id or handle:&lt;handle&gt; in the path.</description></item>
    /// </list>
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
