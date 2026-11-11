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
using MaxioAdvancedBilling.Requests.Wallets;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Wallet object is a balance, owned by a subscription and denominated in the unit of a Wallet Product, that you can create, fund, drain, and read through the API.
/// <para>
/// Wallets suit prepaid allowances, credits, and consumption pools that don't map cleanly onto the regular usage-billing cycle. Every balance change is recorded on the wallet's append-only ledger. For more information, see <see href="https://docs.maxio.com/billing/catalog/wallets/understand-wallets">Understand Wallets</see>.
/// </para>
/// </summary>
public sealed class Wallets
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Wallets(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Archive Wallet
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ArchiveWalletError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Archives the wallet: blocks further grants, debits, adjustments, and purchases (each returns <c>422</c>), stops any metered drawdown, and stops recurring funding (its schedule is deactivated with reason <c>wallet_archived</c>, not removed), while reads and the ledger stay available. Nothing already on the wallet is destroyed or clawed back, but nothing on it can be spent either, by any path, once archived. Idempotent: archiving an already-archived wallet returns the same <c>200</c>. There is no restore endpoint.
    /// </remarks>
    public Task<WalletResponse> ArchiveWallet(ArchiveWalletRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallets/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<WalletResponse>(),
            ArchiveWalletError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Wallet
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateWalletError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a wallet owned by a subscription and denominated by a <see href="$e/Wallet%20Products/createWalletProduct">Wallet Product</see>: only a Wallet Product can denominate a wallet, not an ordinary component. The Wallet Product must be active; it never belongs to a product family, so there's no family check against the subscription. It's <b>immutable for the life of the wallet</b>; there's no way to repoint an existing wallet at a different one.
    /// <para>
    /// This endpoint always creates a <c>manual</c> wallet. To provision a wallet automatically from a usage-limit entitlement instead, configure <c>wallet_behavior</c> on the Feature behind that entitlement (see <see href="page:introduction/basic-concepts/wallets#provisioning-a-wallet-from-a-feature">the Wallets concept guide</see>) rather than sending <c>entitlement_id</c> here.
    /// </para>
    /// </remarks>
    public Task<WalletResponse> CreateWallet(CreateWalletOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallets.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<WalletResponse>(),
            CreateWalletError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Wallets
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletsListResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListWalletsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the wallets on your site, in creation order, paginated. Without <c>subscription_id</c>, returns every wallet on the site. The API key is the tenancy boundary; <c>subscription_id</c> only narrows the result set further. Archived wallets are included unless you filter them out with <c>status=active</c>.
    /// </remarks>
    public Task<WalletsListResponse> ListWallets(ListWalletsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallets.json"),
            [],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("subscription_id", request.SubscriptionId),
                new Param("status", request.Status),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<WalletsListResponse>(),
            ListWalletsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Wallet
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadWalletError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns the wallet's balance and grant breakdown. For an active, metered wallet on a site with metered consumption enabled, this is the only operation that computes a live balance from meter usage; see the wallet resource's <c>metered_*</c> fields. It is also the only operation where <c>balance</c> can differ from <c>posted_balance</c>, and a grant's <c>remaining</c> from its <c>posted_remaining</c>: the pending usage is drawn from the grants in drain order, so the grants' <c>remaining</c> add up to <c>balance</c>. Readable regardless of status. Archiving a wallet doesn't affect reads.
    /// </remarks>
    public Task<WalletResponse> ReadWallet(ReadWalletRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallets/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<WalletResponse>(),
            ReadWalletError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Wallet
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateWalletError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the wallet's editable attributes. Only <c>name</c> is editable; any other attribute in the body is ignored. Send only the attributes to change.
    /// </remarks>
    public Task<WalletResponse> UpdateWallet(UpdateWalletOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallets/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            new HttpMethod("PATCH"),
            JsonRequest.Create(request.Body),
            JsonResponse.Create<WalletResponse>(),
            UpdateWalletError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
