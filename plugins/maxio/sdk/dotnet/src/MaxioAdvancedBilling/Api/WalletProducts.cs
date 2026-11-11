using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.WalletProducts;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Wallet Product object is a billing product created specifically to denominate wallets and to price wallet purchases. A Wallet Product does not belong to a product family, and the Wallet Product behind a wallet cannot be changed after the wallet is created. For more information, see <see href="https://docs.maxio.com/billing/catalog/wallets/create-and-manage-wallet-products">Create and Manage Wallet Products</see>.
/// </summary>
public sealed class WalletProducts
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal WalletProducts(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Archive Wallet Product
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ArchiveWalletProductError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Archives the Wallet Product. Refuses when an active Feature's <c>wallet_behavior</c> still points at it, or when it's used in a scheduled renewal. This protects Feature-provisioned wallets, but not a wallet created manually against this product: a manual wallet can be left denominated by an archived Wallet Product, and <see href="$e/Wallet%20Purchases/createWalletPurchase">Create Wallet Purchase</see> doesn't check whether the billing product is archived before pricing and charging a purchase against it.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>archiveWalletProduct is called while an active Feature's wallet_behavior still points at the product → Maxio answers 422 with "Wallet product is in use by one or more features". Remove the product from every active Feature's wallet_behavior before archiving it.</description></item>
    ///   <item><description>archiveWalletProduct is called while the product is used in a scheduled renewal → Maxio answers 422. Remove the product from scheduled renewals before archiving it.</description></item>
    ///   <item><description>archiveWalletProduct is called on a product that is already archived → Maxio answers 422. Treat this 422 as already archived.</description></item>
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>A Wallet Product is referenced by its handle without the handle: prefix (wallet_product_id on createWallet, or the id path parameter) → Maxio answers 404. Reference Wallet Products by numeric id or as handle:&lt;handle&gt;.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>archiveWalletProduct is called while manually created wallets are denominated by the product → The archive succeeds and those manual wallets stay on the archived product. Check for manual wallets on the product before archiving it.</description></item>
    ///   <item><description>A Wallet Product is returned → The full Component field set comes back under a wallet_product root, with kind wallet_product and the product-family fields null. Parse Wallet Products as Components under the wallet_product key.</description></item>
    /// </list>
    /// </remarks>
    public Task<WalletProductResponse> ArchiveWalletProduct(ArchiveWalletProductRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallet_products/{id}/archive.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<WalletProductResponse>(),
            ArchiveWalletProductError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Wallet Product
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateWalletProductError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a Wallet Product: a billing product with no product family, meant to denominate wallets and to be purchased through <see href="$e/Wallet%20Purchases/createWalletPurchase">Create Wallet Purchase</see>. It's a Component under the hood, so it returns the full Component field set, and can optionally be priced with one or more price points in the same call.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>createWalletProduct is sent with product_family_id → Maxio answers 422 with "Product family cannot be set on a wallet product". Never send product_family_id for a Wallet Product.</description></item>
    ///   <item><description>Any price point in price_point or price_points fails validation → Maxio answers 422. Fix every price point and resend the whole request; nothing was created.</description></item>
    ///   <item><description>A Wallet Product price point is sent without a name → Maxio answers 422 with "Name: cannot be blank.". Give every price point a name, and map 'Name: cannot be blank.' to the price point, not the product.</description></item>
    ///   <item><description>A Wallet Product price point has pricing_scheme per_unit and no unit_price → Maxio answers 422. Send unit_price for per_unit price points.</description></item>
    ///   <item><description>A Wallet Product price point has a pricing_scheme other than per_unit and no prices → Maxio answers 422. Send prices brackets for stairstep, volume and tiered price points.</description></item>
    ///   <item><description>A Wallet Product price point sends interval on a site without Multifrequency enabled → Maxio answers 422. Enable Multifrequency before setting a price point interval.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>createWalletProduct is sent with both price_point and price_points → price_point is ignored and price_points is used. Send either price_point or price_points, not both.</description></item>
    ///   <item><description>createWalletProduct creates several price points through price_points → The first price point created becomes the Wallet Product's default. Put the intended default price point first in price_points.</description></item>
    ///   <item><description>A Wallet Product is returned → The full Component field set comes back under a wallet_product root, with kind wallet_product and the product-family fields null. Parse Wallet Products as Components under the wallet_product key.</description></item>
    /// </list>
    /// </remarks>
    public Task<WalletProductResponse> CreateWalletProduct(CreateWalletProductOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallet_products.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<WalletProductResponse>(),
            CreateWalletProductError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Wallet Products
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletProductsListResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListWalletProductsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the Wallet Products on your site.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>listWalletProducts is called without page and without per_page → The full matching set is returned in one response; sending either turns on pagination at 20 per page, up to 200. Send page or per_page to paginate Wallet Products.</description></item>
    ///   <item><description>listWalletProducts is called with include_archived and also status → include_archived is ignored and status decides; without either, archived products are left out. Use status=all or status=archived rather than include_archived when combining filters.</description></item>
    ///   <item><description>listWalletProducts is called with an unrecognized status value → status falls back to active instead of returning 422. Validate status yourself; a typo silently returns only active products.</description></item>
    ///   <item><description>listWalletProducts is called with q → Products whose name, handle or unit name contains q, case-insensitively, are returned. Expect q to match handles and unit names as well as names.</description></item>
    ///   <item><description>listWalletProducts is called with an unrecognized sort_by value → Results are sorted by name. Send only name, unit_name or updated_at as sort_by.</description></item>
    ///   <item><description>listWalletProducts is called with a sort_direction other than desc → It is treated as asc. Send exactly desc for descending order.</description></item>
    ///   <item><description>listWalletProducts returns archived_count with a status other than archived → archived_count is 0 whenever total_count is non-zero; only when total_count is 0 is it the number of archived products matching q (ignoring status). Use archived_count only as an empty-state hint, not as a running total.</description></item>
    /// </list>
    /// </remarks>
    public Task<WalletProductsListResponse> ListWalletProducts(ListWalletProductsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallet_products.json"),
            [],
            [
                new Param("include_archived", request.IncludeArchived),
                new Param("status", request.Status),
                new Param("q", request.Q),
                new Param("sort_by", request.SortBy),
                new Param("sort_direction", request.SortDirection),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<WalletProductsListResponse>(),
            ListWalletProductsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Wallet Product
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadWalletProductError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns the Wallet Product.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>A Wallet Product is referenced by its handle without the handle: prefix (wallet_product_id on createWallet, or the id path parameter) → Maxio answers 404. Reference Wallet Products by numeric id or as handle:&lt;handle&gt;.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>A Wallet Product is returned → The full Component field set comes back under a wallet_product root, with kind wallet_product and the product-family fields null. Parse Wallet Products as Components under the wallet_product key.</description></item>
    /// </list>
    /// </remarks>
    public Task<WalletProductResponse> ReadWalletProduct(ReadWalletProductRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallet_products/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<WalletProductResponse>(),
            ReadWalletProductError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Wallet Product Usage
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletProductUsageResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadWalletProductUsageError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns everything drawing on this Wallet Product: the wallet templates billing through it, split into ones using its default price point and ones overriding it, plus a sample of up to 50 of the active wallets actually created from them. A wallet template can appear more than once in <c>wallet_templates</c>/<c>price_point_overrides</c>, once per Feature and owning plan/component sharing it, so these arrays aren't one row per template.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>A Wallet Product is referenced by its handle without the handle: prefix (wallet_product_id on createWallet, or the id path parameter) → Maxio answers 404. Reference Wallet Products by numeric id or as handle:&lt;handle&gt;.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>readWalletProductUsage is called for a Wallet Product with many active wallets → active_wallets.sample holds at most 50 wallets, the most recently created first; total gives the full count. Use active_wallets.total for counts; the sample is capped at 50.</description></item>
    ///   <item><description>A wallet template is shared by several Features or owning plans/components → It appears once per Feature and owner in wallet_templates or price_point_overrides, so the arrays are not one row per template. Deduplicate wallet templates yourself before counting them.</description></item>
    /// </list>
    /// </remarks>
    public Task<WalletProductUsageResponse> ReadWalletProductUsage(ReadWalletProductUsageRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallet_products/{id}/usage.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<WalletProductUsageResponse>(),
            ReadWalletProductUsageError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Unarchive Wallet Product
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UnarchiveWalletProductError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Restores an archived Wallet Product.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>A Wallet Product is referenced by its handle without the handle: prefix (wallet_product_id on createWallet, or the id path parameter) → Maxio answers 404. Reference Wallet Products by numeric id or as handle:&lt;handle&gt;.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>unarchiveWalletProduct is called on a product that is not archived → Maxio answers 422. Treat this 422 as already active.</description></item>
    ///   <item><description>A Wallet Product is returned → The full Component field set comes back under a wallet_product root, with kind wallet_product and the product-family fields null. Parse Wallet Products as Components under the wallet_product key.</description></item>
    /// </list>
    /// </remarks>
    public Task<WalletProductResponse> UnarchiveWalletProduct(UnarchiveWalletProductRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallet_products/{id}/unarchive.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<WalletProductResponse>(),
            UnarchiveWalletProductError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Wallet Product
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletProductResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateWalletProductError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the Wallet Product's catalog-facing fields. Omitted fields are left unchanged. Unlike <see href="$e/Wallet%20Products/createWalletProduct">Create Wallet Product</see>, this does not accept <c>unit_name</c>, <c>handle</c>, <c>price_point</c>, or <c>price_points</c>; see the request schema for which fields are actually mutable.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>A Wallet Product is referenced by its handle without the handle: prefix (wallet_product_id on createWallet, or the id path parameter) → Maxio answers 404. Reference Wallet Products by numeric id or as handle:&lt;handle&gt;.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>A Wallet Product is returned → The full Component field set comes back under a wallet_product root, with kind wallet_product and the product-family fields null. Parse Wallet Products as Components under the wallet_product key.</description></item>
    ///   <item><description>updateWalletProduct is sent with unit_name, handle, price_point or price_points → Maxio answers 200. Change only name, description, taxable, tax_code, accounting_code and item_category on update.</description></item>
    ///   <item><description>updateWalletProduct leaves some mutable fields out of the body → Omitted fields keep their current values. Send only the fields you want to change.</description></item>
    /// </list>
    /// </remarks>
    public Task<WalletProductResponse> UpdateWalletProduct(UpdateWalletProductOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallet_products/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<WalletProductResponse>(),
            UpdateWalletProductError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
