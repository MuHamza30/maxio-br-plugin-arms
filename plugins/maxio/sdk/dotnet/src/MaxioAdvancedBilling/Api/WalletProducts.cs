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
