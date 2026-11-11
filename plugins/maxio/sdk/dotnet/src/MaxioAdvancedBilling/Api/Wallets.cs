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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A grant, debit, purchase or adjustment is attempted on an archived wallet → Archiving blocks further grants, debits, purchases and adjustments, but reads and list results are unaffected. Do not write to archived wallets; they remain readable.</description></item>
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>A wallet is archived → Metered drawdown stops and any recurring funding schedule is deactivated with reason wallet_archived (not removed). Expect recurring_funding to show status stopped with reason wallet_archived after archiving.</description></item>
    ///   <item><description>A wallet has been archived → Nothing on it is destroyed or clawed back; it stays readable, listed and its ledger available, but nothing on it can be spent. Keep reading archived wallets for history; do not expect their balance to be usable.</description></item>
    ///   <item><description>archiveWallet is called on a wallet that is already archived → Maxio answers 200. Retry archiveWallet freely; it is idempotent.</description></item>
    ///   <item><description>A wallet is archived → There is no restore endpoint, so the archive cannot be undone through the API. Archive a wallet only when it will never be used again.</description></item>
    ///   <item><description>A wallet with recurring funding is archived → Recurring funding stops with reason wallet_archived. Do not archive a wallet that should keep receiving refills.</description></item>
    ///   <item><description>A wallet is returned by any operation other than readWallet, including a metered wallet → balance equals posted_balance (the stored figure as of the last ledger write), grants' remaining equal posted_remaining, and the metered_* fields are null. Call readWallet when a live metered balance is needed.</description></item>
    ///   <item><description>A wallet's grants are returned → Every grant, including spent and expired ones, is listed in drain order: soonest-expiring first, evergreen grants last. Filter spent or expired grants yourself; do not assume the array holds only open grants.</description></item>
    ///   <item><description>A wallet paired with a meter is returned → Only Read Wallet computes a live balance, and only for an active wallet on a site with metered consumption enabled; every other response reports the stored balance (balance equal to posted_balance) with the metered_* fields null. Call Read Wallet when you need the live metered balance and reconcile against posted_balance.</description></item>
    ///   <item><description>A wallet response carries a depletion_policy the app does not know → New depletion_policy values are expected over time; hard_stop is the only one today. Treat depletion_policy as an open string and do not fail on unknown values.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet amount (initial_balance, conversion_rate, transaction or adjustment amount, purchase quantity) is sent as a JSON number with a decimal point → Maxio answers 422. Send fractional wallet amounts as decimal strings (a whole JSON integer is also accepted).</description></item>
    ///   <item><description>createWallet sends a price_point_id that is archived, is a subscription-specific custom price point, or does not belong to the Wallet Product → Maxio answers 422. Pick an active, non-custom price point of the same Wallet Product.</description></item>
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>meter_id or conversion_rate is sent on a site without metered wallet consumption enabled → Maxio answers 403 with "403 Forbidden". Send meter_id and conversion_rate only when metered wallet consumption is enabled for the site.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>A wallet is created for a prepaid subscription → Maxio answers 422. Do not create wallets for prepaid subscriptions.</description></item>
    ///   <item><description>createWallet sends a depletion_policy other than hard_stop → Maxio answers 422. Send hard_stop or leave depletion_policy out.</description></item>
    ///   <item><description>createWallet pairs a meter that is not a sum or count meter → Maxio answers 422. Pair only sum or count meters.</description></item>
    ///   <item><description>createWallet sends meter_id or conversion_rate on a site without metered wallet consumption enabled → Maxio answers 403 with "403 Forbidden". Enable metered wallet consumption before pairing wallets with meters.</description></item>
    ///   <item><description>createWallet sends a conversion_rate that is not greater than 0, is 10^16 or more, or has more than 8 fractional digits → Maxio answers 422. Send conversion_rate as a decimal string above 0 and below 10^16 with at most 8 decimals.</description></item>
    ///   <item><description>createWallet pairs a meter whose stream bills by subscription reference with a subscription that has no reference → Maxio answers 422. Set a reference on the subscription before pairing a meter that bills by subscription reference.</description></item>
    ///   <item><description>A wallet amount is sent with more than 8 fractional digits → Maxio answers 422. Round wallet amounts to at most 8 decimal places before sending.</description></item>
    ///   <item><description>createWallet sends a negative initial_balance or one of 10^16 or more → Maxio answers 422. Send initial_balance as a non-negative decimal string below 10^16.</description></item>
    ///   <item><description>createWallet sends initial_balance_expires_at without a non-zero initial_balance → Maxio answers 422. Send initial_balance_expires_at only together with a non-zero initial_balance.</description></item>
    ///   <item><description>An expiry (initial_balance_expires_at on createWallet, expires_at on a grant) is malformed or not in the future → Maxio answers 422. Send expiry dates in the future as YYYY-MM-DD or a full timestamp.</description></item>
    ///   <item><description>An expiry date names a calendar day that does not exist, such as 2027-02-30 → Maxio answers 422. Validate expiry dates as real calendar days before sending.</description></item>
    ///   <item><description>createWallet names a wallet_product_id that does not resolve to a Wallet Product on this site (including an ordinary component) → Maxio answers 404 with "Wallet product not found". Create a Wallet Product first and pass its id; ordinary components cannot denominate a wallet.</description></item>
    ///   <item><description>createWallet names an archived Wallet Product → Maxio answers 422 with "Wallet product is archived". Unarchive the Wallet Product or pick an active one.</description></item>
    ///   <item><description>A Wallet Product is referenced by its handle without the handle: prefix (wallet_product_id on createWallet, or the id path parameter) → Maxio answers 404. Reference Wallet Products by numeric id or as handle:&lt;handle&gt;.</description></item>
    ///   <item><description>createWallet is sent with entitlement_id → Maxio answers 422 with "Recurring funding comes from a wallet-backed feature, not from this endpoint". Never send entitlement_id; configure wallet_behavior on the feature template for funded wallets.</description></item>
    ///   <item><description>createWallet names a subscription on prepaid collection → Maxio answers 422 with "Wallets are not available on prepaid subscriptions". Do not create wallets for prepaid subscriptions.</description></item>
    ///   <item><description>createWallet names a subscription that does not exist or belongs to another site → Maxio answers 404. Pass a subscription id from the same site as the API key.</description></item>
    ///   <item><description>createWallet sends a meter_id that is not a merchant-owned meter on this site (a system-maintained shadow meter included) → Maxio answers 404. Pair wallets only with your own meters on this site.</description></item>
    ///   <item><description>A wallet amount is sent as a JSON number with a decimal point → Maxio answers 422. Send wallet amounts as decimal strings such as "150.0".</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>Anything other than a wallet's name would be changed after creation, including its Wallet Product → the outcome is not stated. Set the Wallet Product and settings correctly at creation; change only name with Update Wallet.</description></item>
    ///   <item><description>createWallet is sent without price_point_id → The wallet takes the Wallet Product's default price point, or null if the product has none; it cannot be changed later. Pass price_point_id at creation if purchases should use a non-default price point.</description></item>
    ///   <item><description>createWallet is sent without depletion_policy → depletion_policy is set to hard_stop. Leave depletion_policy out unless you need to state it.</description></item>
    ///   <item><description>createWallet is sent without hard_stop_overflow → hard_stop_overflow is set to discard, so debits past zero are capped and the excess rejected. Send hard_stop_overflow=allow_negative at creation if the wallet may go into overdraft.</description></item>
    ///   <item><description>createWallet pairs a meter_id without a conversion_rate → conversion_rate is set to 1.0 and cannot be changed later; without meter_id the rate has no meaning. Send conversion_rate at creation if one meter unit is not one wallet unit.</description></item>
    ///   <item><description>createWallet is sent with a non-zero initial_balance → An initial-balance grant and a matching initial_balance ledger row are written; the balance is free and appears on no invoice. Use initial_balance only for free opening credit; use createWalletPurchase for paid units.</description></item>
    ///   <item><description>createWallet is sent with initial_balance omitted or exactly 0 → A zero-balance wallet with no grants is created; this is not an error. Omit initial_balance for an empty wallet.</description></item>
    ///   <item><description>An expiry (initial_balance_expires_at or a grant's expires_at) is sent as a bare calendar date → It is resolved to the end of that day in the site's time zone. Send a full timestamp if the grant must expire at a time other than end of day in the site's time zone.</description></item>
    ///   <item><description>A wallet name is sent with surrounding spaces, blank, or (on update) null → Surrounding spaces are stripped; a blank value is stored as null, and on update a blank or null clears the name. Send null or blank only when you mean to clear the wallet name.</description></item>
    ///   <item><description>A wallet is returned by any operation other than readWallet, including a metered wallet → balance equals posted_balance (the stored figure as of the last ledger write), grants' remaining equal posted_remaining, and the metered_* fields are null. Call readWallet when a live metered balance is needed.</description></item>
    ///   <item><description>A wallet's grants are returned → Every grant, including spent and expired ones, is listed in drain order: soonest-expiring first, evergreen grants last. Filter spent or expired grants yourself; do not assume the array holds only open grants.</description></item>
    ///   <item><description>A wallet paired with a meter is returned → Only Read Wallet computes a live balance, and only for an active wallet on a site with metered consumption enabled; every other response reports the stored balance (balance equal to posted_balance) with the metered_* fields null. Call Read Wallet when you need the live metered balance and reconcile against posted_balance.</description></item>
    ///   <item><description>A wallet response carries a depletion_policy the app does not know → New depletion_policy values are expected over time; hard_stop is the only one today. Treat depletion_policy as an open string and do not fail on unknown values.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>listWallets is called with a status value other than active or archived → Maxio answers 422 with "Status must be one of: active, archived". Send only active or archived as status.</description></item>
    ///   <item><description>A wallet has been archived → Nothing on it is destroyed or clawed back; it stays readable, listed and its ledger available, but nothing on it can be spent. Keep reading archived wallets for history; do not expect their balance to be usable.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>listWallets is called → Wallets are returned in creation order. Do not expect any other ordering from listWallets.</description></item>
    ///   <item><description>listWallets is called without subscription_id → Every wallet on the site is returned; the API key is the tenancy boundary and subscription_id only narrows it. Pass subscription_id to list one subscription's wallets.</description></item>
    ///   <item><description>listWallets is called without status=active → Archived wallets are included in the results. Pass status=active to leave archived wallets out.</description></item>
    ///   <item><description>A wallet is returned by any operation other than readWallet, including a metered wallet → balance equals posted_balance (the stored figure as of the last ledger write), grants' remaining equal posted_remaining, and the metered_* fields are null. Call readWallet when a live metered balance is needed.</description></item>
    ///   <item><description>A wallet's grants are returned → Every grant, including spent and expired ones, is listed in drain order: soonest-expiring first, evergreen grants last. Filter spent or expired grants yourself; do not assume the array holds only open grants.</description></item>
    ///   <item><description>A wallet paired with a meter is returned → Only Read Wallet computes a live balance, and only for an active wallet on a site with metered consumption enabled; every other response reports the stored balance (balance equal to posted_balance) with the metered_* fields null. Call Read Wallet when you need the live metered balance and reconcile against posted_balance.</description></item>
    ///   <item><description>A wallet's grants are returned → Grants are listed in drain order (soonest-expiring first, evergreen grants last) and spent or expired grants stay in the array. Filter spent and expired grants yourself when you only need open ones.</description></item>
    ///   <item><description>A wallet response carries a depletion_policy the app does not know → New depletion_policy values are expected over time; hard_stop is the only one today. Treat depletion_policy as an open string and do not fail on unknown values.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>A wallet has been archived → Nothing on it is destroyed or clawed back; it stays readable, listed and its ledger available, but nothing on it can be spent. Keep reading archived wallets for history; do not expect their balance to be usable.</description></item>
    ///   <item><description>A wallet's grants are returned → Every grant, including spent and expired ones, is listed in drain order: soonest-expiring first, evergreen grants last. Filter spent or expired grants yourself; do not assume the array holds only open grants.</description></item>
    ///   <item><description>A wallet paired with a meter is returned → Only Read Wallet computes a live balance, and only for an active wallet on a site with metered consumption enabled; every other response reports the stored balance (balance equal to posted_balance) with the metered_* fields null. Call Read Wallet when you need the live metered balance and reconcile against posted_balance.</description></item>
    ///   <item><description>Live metered usage cannot be read fresh when a metered wallet is read → metered_stale is true and balance falls back to the last known snapshot, while posted_balance stays exact. Check metered_stale and wait metered_recheck_after_seconds before reading again.</description></item>
    ///   <item><description>A wallet's grants are returned → Grants are listed in drain order (soonest-expiring first, evergreen grants last) and spent or expired grants stay in the array. Filter spent and expired grants yourself when you only need open ones.</description></item>
    ///   <item><description>A wallet response carries a depletion_policy the app does not know → New depletion_policy values are expected over time; hard_stop is the only one today. Treat depletion_policy as an open string and do not fail on unknown values.</description></item>
    ///   <item><description>readWallet is called for an active, metered wallet on a site with metered consumption enabled → balance and grants' remaining include usage not yet posted to the ledger (through metered_through_at), so balance can be below posted_balance; this is the only operation that computes it. Enforce against balance from readWallet, and reconcile the ledger against posted_balance.</description></item>
    ///   <item><description>readWallet cannot read a metered wallet's live usage fresh → metered_stale is true and balance falls back to the last known snapshot, while posted_balance stays exact. Check metered_stale before trusting balance as live.</description></item>
    ///   <item><description>readWallet returns a live metered balance → metered_recheck_after_seconds says how long to wait before reading again to see usage past metered_through_at. Wait metered_recheck_after_seconds before re-reading a metered wallet's balance.</description></item>
    ///   <item><description>readWallet is called for an archived metered wallet → The stored balance is reported; no live metered balance is computed. Do not expect live metered figures from an archived wallet.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A wallet endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Have the Wallets feature enabled for the site and handle a plain-text 403 body.</description></item>
    ///   <item><description>A wallet endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with errors (human-readable messages) and code (a stable machine-readable reason); record validation failures use validation_failed. Branch on the error code, not on the message text.</description></item>
    ///   <item><description>A grant, debit, adjustment, purchase (or its preview) or update is sent for an archived wallet → Maxio answers 422 with "Wallet is archived". Check the wallet's status before writing to it; archived wallets accept no further writes.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>Anything other than a wallet's name would be changed after creation, including its Wallet Product → the outcome is not stated. Set the Wallet Product and settings correctly at creation; change only name with Update Wallet.</description></item>
    ///   <item><description>updateWallet is sent with no editable attribute in the body → Maxio answers 422. Include name in every updateWallet body.</description></item>
    ///   <item><description>A wallet name is sent with surrounding spaces, blank, or (on update) null → Surrounding spaces are stripped; a blank value is stored as null, and on update a blank or null clears the name. Send null or blank only when you mean to clear the wallet name.</description></item>
    ///   <item><description>A wallet is returned by any operation other than readWallet, including a metered wallet → balance equals posted_balance (the stored figure as of the last ledger write), grants' remaining equal posted_remaining, and the metered_* fields are null. Call readWallet when a live metered balance is needed.</description></item>
    ///   <item><description>A wallet's grants are returned → Every grant, including spent and expired ones, is listed in drain order: soonest-expiring first, evergreen grants last. Filter spent or expired grants yourself; do not assume the array holds only open grants.</description></item>
    ///   <item><description>A wallet paired with a meter is returned → Only Read Wallet computes a live balance, and only for an active wallet on a site with metered consumption enabled; every other response reports the stored balance (balance equal to posted_balance) with the metered_* fields null. Call Read Wallet when you need the live metered balance and reconcile against posted_balance.</description></item>
    ///   <item><description>A wallet response carries a depletion_policy the app does not know → New depletion_policy values are expected over time; hard_stop is the only one today. Treat depletion_policy as an open string and do not fail on unknown values.</description></item>
    ///   <item><description>updateWallet is sent with attributes other than name (wallet product, meter, conversion rate, price point and so on) → Every attribute other than name is ignored; those are fixed for the life of the wallet. Create a new wallet when anything other than the name must change.</description></item>
    /// </list>
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
