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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The server refuses to archive the product (the spec gives no reason) → Maxio answers 422 with "Product cannot be archived.". Handle a 422 from Archive Product and check the product's current state.</description></item>
    ///   <item><description>A product is archived → Current subscribers are unaffected and keep being charged. Migrate or cancel existing subscriptions yourself if archiving should stop their charges.</description></item>
    ///   <item><description>A product is archived → The product can no longer be chosen for purchase in the Billing Portal. Expect archived products to disappear from Billing Portal purchase options.</description></item>
    ///   <item><description>A product is archived → Public Signup Pages for the product are disabled. Replace links to the product's Public Signup Pages before archiving it.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has the new Catalog experience enabled and the request includes auto_create_signup_page with any value → Maxio answers 422 with "Auto Create Signup Page: is not allowed". Leave auto_create_signup_page out of Create Product on Catalog-experience sites.</description></item>
    ///   <item><description>A product is created → the outcome is not stated. Create the product family first and create products inside it.</description></item>
    ///   <item><description>The product is created under a product family that does not exist → the outcome is not stated. Create the product family first and create the product under its id or handle.</description></item>
    ///   <item><description>A subscription is to be sold for a product that has not been created → the outcome is not stated. Create and configure the product before creating subscriptions to it.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A tax_code longer than 25 characters is sent for a component or product → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>A component or product is saved with unspsc_code set (or without it) → The unspsc_code is sent as the commodity code on invoice line items instead of the default derived from item_category. Set unspsc_code when the commodity code derived from item_category is not the one you want on invoice line items.</description></item>
    ///   <item><description>A new product is created → A Public Signup Page is created automatically for the product. Expect a public signup page to exist for every new product.</description></item>
    ///   <item><description>require_credit_card is sent for a product on a site without legacy hosted pages → The deprecated value can be ignored; Public Signup Pages carry their own setting. Set the credit-card requirement on the Public Signup Page, not through require_credit_card.</description></item>
    ///   <item><description>trial_type is no_obligation and a subscription's trial ends with no credit card on file → The subscription moves to Trial Ended and Maxio sends no emails or statements. Choose no_obligation only when trials should end quietly without dunning.</description></item>
    ///   <item><description>trial_type is payment_expected and a subscription's trial ends with no credit card on file → The subscription moves to Past Due and Maxio sends normal dunning emails and statements. Choose payment_expected only when trials without a card on file should go into dunning.</description></item>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A list is filtered with start_date or end_date → start_date matches from midnight and end_date up to and including 11:59:59 PM, both in the site's time zone. Convert date filters to the site's time zone, or use start_datetime/end_datetime with an explicit offset.</description></item>
    ///   <item><description>Both start_datetime and start_date (or end_datetime and end_date) are sent → The datetime parameter is used and the date parameter is ignored. Send either the date or the datetime form of each bound, not both.</description></item>
    ///   <item><description>date_field is given a value other than created_at or updated_at → The value is ignored and the date filters are not applied. Send date_field as created_at or updated_at only, and check that date-filtered results really are filtered.</description></item>
    ///   <item><description>include_archived is not set to true → Archived components or products are left out of the list. Pass include_archived=true when archived items must be listed.</description></item>
    ///   <item><description>filter[use_site_exchange_rate] is used → The filter matches the setting of each item's default price point, not of its other price points. Remember filter[use_site_exchange_rate] tests only the default price point.</description></item>
    ///   <item><description>per_page is omitted or set above 200 → 20 records are returned by default; a per_page above 200 is changed to 200. Page through results with per_page of at most 200 and do not assume a larger page was honoured.</description></item>
    ///   <item><description>A page number past the last page of results is requested → An empty result set is returned, not an error. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>filter[prepaid_product_price_point] is sent without include=prepaid_product_price_point → the outcome is not stated. Send include=prepaid_product_price_point whenever you use the prepaid product price point filter.</description></item>
    ///   <item><description>include_features=true is passed to read features embedded in product or component responses → List Products, Read Product and Read Component embed features; List Components does not support include_features. Read component features one by one or with the features endpoints instead of List Components.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include an explicit time zone in start_datetime and end_datetime.</description></item>
    ///   <item><description>A product or component is read without include_features=true → The response has no features array; with include_features=true it embeds only the active feature catalog items. Pass include_features=true when you need the features attached to a product or component.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>include_features=true is passed to read features embedded in product or component responses → List Products, Read Product and Read Component embed features; List Components does not support include_features. Read component features one by one or with the features endpoints instead of List Components.</description></item>
    ///   <item><description>A product or component is read without include_features=true → The response has no features array; with include_features=true it embeds only the active feature catalog items. Pass include_features=true when you need the features attached to a product or component.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A tax_code longer than 25 characters is sent for a component or product → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>A component or product is saved with unspsc_code set (or without it) → The unspsc_code is sent as the commodity code on invoice line items instead of the default derived from item_category. Set unspsc_code when the commodity code derived from item_category is not the one you want on invoice line items.</description></item>
    ///   <item><description>require_credit_card is sent for a product on a site without legacy hosted pages → The deprecated value can be ignored; Public Signup Pages carry their own setting. Set the credit-card requirement on the Public Signup Page, not through require_credit_card.</description></item>
    ///   <item><description>trial_type is no_obligation and a subscription's trial ends with no credit card on file → The subscription moves to Trial Ended and Maxio sends no emails or statements. Choose no_obligation only when trials should end quietly without dunning.</description></item>
    ///   <item><description>trial_type is payment_expected and a subscription's trial ends with no credit card on file → The subscription moves to Past Due and Maxio sends normal dunning emails and statements. Choose payment_expected only when trials without a card on file should go into dunning.</description></item>
    ///   <item><description>A product is updated through Update Product → A new price point is created and set as the product's default price point. Use the product price point endpoints to change an existing price point; expect Update Product to add a new default one.</description></item>
    /// </list>
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
