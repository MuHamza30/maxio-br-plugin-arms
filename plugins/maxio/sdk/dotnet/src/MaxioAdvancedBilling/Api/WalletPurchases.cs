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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A grant, debit, purchase or adjustment is attempted on an archived wallet → Archiving blocks further grants, debits, purchases and adjustments, but reads and list results are unaffected. Do not write to archived wallets; they remain readable.</description></item>
    ///   <item><description>A wallet amount (initial_balance, conversion_rate, transaction or adjustment amount, purchase quantity) is sent as a JSON number with a decimal point → Maxio answers 422. Send fractional wallet amounts as decimal strings (a whole JSON integer is also accepted).</description></item>
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>A wallet amount is sent with more than 8 fractional digits → Maxio answers 422. Round wallet amounts to at most 8 decimal places before sending.</description></item>
    ///   <item><description>The immediate payment for an automatic-collection wallet purchase is declined → Maxio answers 422 with "Payment for this purchase was declined: insufficient funds". Show the gateway's decline reason and let the customer retry; no units were added.</description></item>
    ///   <item><description>The invoice for a wallet purchase cannot be created at all → Maxio answers 422 with "The invoice for this purchase could not be created". Treat purchase_invoice_failed as nothing charged and nothing credited, and retry later.</description></item>
    ///   <item><description>A wallet purchase's payment settled but the units could not be credited to the wallet → Maxio answers 422 with "Purchase was paid but the units could not be credited: abc123". On purchase_paid_but_uncredited, take the invoice uid from the end of the message and reconcile; do not charge again.</description></item>
    ///   <item><description>A wallet purchase or preview names no price point and the wallet has none set → Maxio answers 422. Send price_point_id when the wallet has no default price point.</description></item>
    ///   <item><description>A wallet purchase or preview names a price_point_id that does not belong to the wallet's billing product → Maxio answers 422. Use a price point of the wallet's own Wallet Product.</description></item>
    ///   <item><description>A wallet purchase or preview uses an archived, custom or otherwise unusable price point → Maxio answers 422. Purchase only at an active, non-custom price point.</description></item>
    ///   <item><description>A wallet purchase or preview uses a price point that has no price in the subscription's currency → Maxio answers 422. Give the price point a price in every currency your subscriptions use.</description></item>
    ///   <item><description>A wallet purchase or preview sends a quantity that is malformed, 0 or less, or out of range → Maxio answers 422. Send quantity as a positive decimal string.</description></item>
    ///   <item><description>A wallet purchase or preview is made on a wallet whose billing product is not a Wallet Product (a wallet created before Wallet Products became required) → Maxio answers 422. Move legacy wallets onto a new wallet backed by a Wallet Product before selling units.</description></item>
    ///   <item><description>A grant, debit, adjustment, purchase (or its preview) or update is sent for an archived wallet → Maxio answers 422 with "Wallet is archived". Check the wallet's status before writing to it; archived wallets accept no further writes.</description></item>
    ///   <item><description>A wallet amount is sent as a JSON number with a decimal point → Maxio answers 422. Send wallet amounts as decimal strings such as "150.0".</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>A wallet purchase or preview is sent without price_point_id → The wallet's own price_point_id is used. Pass price_point_id only to buy at a different price point of the wallet's product.</description></item>
    ///   <item><description>A wallet purchase is made for an invoice-billed (remittance) subscription → An invoice is drafted, the wallet is credited right away, and the invoice is issued afterwards; no immediate charge happens. Collect remittance purchase invoices through your normal invoicing follow-up.</description></item>
    ///   <item><description>Issuing the invoice for a remittance wallet purchase fails after the wallet was credited → Maxio answers 201. Watch for draft purchase invoices after a 201 on remittance subscriptions.</description></item>
    ///   <item><description>A wallet purchase is made into an overdrawn wallet → The units repay the overdraft first; the grant is still created, with original equal to the quantity and remaining 0 if the debt absorbed it all. Expect a purchase into an overdrawn wallet to raise the balance by less than the quantity bought.</description></item>
    ///   <item><description>A wallet purchase is made on a manual wallet whose Wallet Product has been archived → The purchase is not blocked; it is priced and charged against the archived product. Check the Wallet Product's archived state yourself before selling units.</description></item>
    ///   <item><description>A wallet purchase succeeds → A grant of origin purchase is written whose expiry follows the funding entitlement's grant expiration policy, or is null when the wallet has no funding entitlement or its entitlements disagree. Read expires_at on the purchase result rather than assuming purchased units never expire.</description></item>
    ///   <item><description>A wallet purchase is made for a subscription on card/automatic collection → An invoice is created and the subscription's payment method is charged immediately; the wallet is credited once payment succeeds. Expect an immediate charge and an invoice_uid on automatic-collection purchases.</description></item>
    ///   <item><description>A wallet is funded (manual grant, purchase, recurring refill or allowance increase) while its balance is negative → The overdraft is repaid first (a grant row with grant_id null) and the new grant gets only what is left, possibly 0; the grant is still created with its full original. Expect a top-up into an overdrawn wallet to show less remaining than the amount sent.</description></item>
    ///   <item><description>A wallet's balance changes → Append-only ledger rows are written; rows are never modified and corrections are always new rows. Reconstruct wallet history from ledger rows and never expect an earlier row to change.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet amount (initial_balance, conversion_rate, transaction or adjustment amount, purchase quantity) is sent as a JSON number with a decimal point → Maxio answers 422. Send fractional wallet amounts as decimal strings (a whole JSON integer is also accepted).</description></item>
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>A wallet amount is sent with more than 8 fractional digits → Maxio answers 422. Round wallet amounts to at most 8 decimal places before sending.</description></item>
    ///   <item><description>A wallet purchase or preview names no price point and the wallet has none set → Maxio answers 422. Send price_point_id when the wallet has no default price point.</description></item>
    ///   <item><description>A wallet purchase or preview names a price_point_id that does not belong to the wallet's billing product → Maxio answers 422. Use a price point of the wallet's own Wallet Product.</description></item>
    ///   <item><description>A wallet purchase or preview uses an archived, custom or otherwise unusable price point → Maxio answers 422. Purchase only at an active, non-custom price point.</description></item>
    ///   <item><description>A wallet purchase or preview uses a price point that has no price in the subscription's currency → Maxio answers 422. Give the price point a price in every currency your subscriptions use.</description></item>
    ///   <item><description>A wallet purchase or preview sends a quantity that is malformed, 0 or less, or out of range → Maxio answers 422. Send quantity as a positive decimal string.</description></item>
    ///   <item><description>A wallet purchase or preview is made on a wallet whose billing product is not a Wallet Product (a wallet created before Wallet Products became required) → Maxio answers 422. Move legacy wallets onto a new wallet backed by a Wallet Product before selling units.</description></item>
    ///   <item><description>A grant, debit, adjustment, purchase (or its preview) or update is sent for an archived wallet → Maxio answers 422 with "Wallet is archived". Check the wallet's status before writing to it; archived wallets accept no further writes.</description></item>
    ///   <item><description>A wallet amount is sent as a JSON number with a decimal point → Maxio answers 422. Send wallet amounts as decimal strings such as "150.0".</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>A wallet purchase or preview is sent without price_point_id → The wallet's own price_point_id is used. Pass price_point_id only to buy at a different price point of the wallet's product.</description></item>
    ///   <item><description>A wallet purchase is previewed → The quote's currency is the subscription's currency; nothing is charged and nothing is written to the wallet. Use previewWalletPurchase to show a price before calling createWalletPurchase.</description></item>
    /// </list>
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
