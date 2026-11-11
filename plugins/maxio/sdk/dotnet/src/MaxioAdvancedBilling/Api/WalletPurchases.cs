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
using MaxioAdvancedBilling.Requests.WalletPurchases;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Wallet Purchases let a subscription buy additional prepaid units for a wallet, charged according to the subscription's collection method. A purchase can be previewed to quote the price before anything is charged. For more information, see <see href="https://docs.maxio.com/billing/catalog/wallets/understand-wallets">Understand Wallets</see>.
/// </summary>
public sealed class WalletPurchases
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal WalletPurchases(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Wallet Purchase
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletPurchaseResultResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateWalletPurchaseError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Purchases prepaid units for the wallet and charges the subscription's payment method for them, using the wallet's <c>price_point_id</c> unless a different price point is named. A successful purchase writes a grant of origin <c>purchase</c>, with its expiration following the funding entitlement's grant expiration policy.
    /// <para>
    /// How "charged" works depends on the subscription's collection method. For card/automatic collection, the payment is attempted immediately; a decline writes nothing to the wallet, and the call fails with a <c>422</c> naming the gateway's decline reason. For invoice-billed (remittance) subscriptions, there's no immediate charge to decline: an invoice is drafted, the wallet is credited right away, and the invoice is issued afterward. If issuing the invoice fails at that point, the invoice is left in draft state and reported for follow-up, but the call still returns <c>201</c> and the units already credited are <b>not</b> rolled back. See <see href="$e/Wallet%20Purchases/previewWalletPurchase">Preview Wallet Purchase</see> to quote a price before charging anything.
    /// </para>
    /// </remarks>
    public Task<WalletPurchaseResultResponse> CreateWalletPurchase(CreateWalletPurchaseOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallets/{wallet_id}/purchases.json"),
            [new TemplateParam("wallet_id", request.WalletId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<WalletPurchaseResultResponse>(),
            CreateWalletPurchaseError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Preview Wallet Purchase
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletPurchasePreviewResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="PreviewWalletPurchaseError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Quotes the price for a purchase at the given quantity and price point, without charging the payment method or writing to the wallet. Takes the same parameters as <see href="$e/Wallet%20Purchases/createWalletPurchase">Create Wallet Purchase</see>.
    /// </remarks>
    public Task<WalletPurchasePreviewResponse> PreviewWalletPurchase(PreviewWalletPurchaseRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallets/{wallet_id}/purchases/preview.json"),
            [new TemplateParam("wallet_id", request.WalletId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<WalletPurchasePreviewResponse>(),
            PreviewWalletPurchaseError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
