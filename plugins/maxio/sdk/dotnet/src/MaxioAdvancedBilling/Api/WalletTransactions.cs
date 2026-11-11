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
using MaxioAdvancedBilling.Requests.WalletTransactions;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Use Wallet Transactions to fund a wallet with a one-time grant, consume units with a direct debit, and read the wallet's append-only ledger. For more information, see <see href="https://docs.maxio.com/billing/catalog/wallets/understand-wallets">Understand Wallets</see>.
/// </summary>
public sealed class WalletTransactions
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal WalletTransactions(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Wallet Transaction
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletTransactionResultResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateWalletTransactionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Grants units to the wallet, or debits units from it, depending on <c>kind</c>. <c>amount</c> is always sent positive, and <c>kind</c> determines the direction written to the ledger.
    /// <para>
    /// <b><c>kind: grant</c></b>: a one-time manual top-up. On a non-negative balance, this creates a grant and writes one ledger row. On a negative balance, it repays the overdraft first (a <c>kind: grant</c> row with <c>grant_id: null</c>, for the repaid amount), then creates the grant: its <c>original</c> is always the full <c>amount</c> sent, and its <c>remaining</c> is what's left after the overdraft was repaid, which is <c>0</c> if the overdraft absorbed the whole top-up. The grant row is still written, and its <c>amount</c> is that <c>remaining</c>, not <c>original</c>, so the ledger rows sum to the balance change. A grant may carry <c>expires_at</c>; a debit may not.
    /// </para>
    /// <para>
    /// <b><c>kind: debit</c></b>: direct consumption. Drains grants earliest-expiration-first, one <c>kind: debit</c> row per grant touched, plus any overdue expiration rows this same call triggers first (see below):
    /// - Under the wallet's default <c>hard_stop_overflow: discard</c>, the drain is capped at the available balance. For a metered wallet, that cap is the live metered balance, not the stored one, so a debit can drain less than its stored balance would suggest, or even drain nothing at all (<c>201</c> with <c>drained_amount: "0.0"</c>) if live usage has already consumed what the ledger hadn't caught up to yet. A debit against a wallet whose *stored* balance is already zero or negative (<c>discard</c> only ever reaches zero) returns <c>422 "Wallet is depleted"</c>. Before that failure is returned, this call still expires any grants that were already overdue, so a <c>422</c> response can still leave new <c>kind: expiration</c> rows written.
    /// - Under <c>hard_stop_overflow: allow_negative</c>, the full amount is deducted. The excess past the wallet's grants is booked as one overdraft row with <c>grant_id: null</c>, <c>rejected_amount</c> is always <c>"0.0"</c>, and the balance goes negative.
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
    ///   <item><description>An expiry (initial_balance_expires_at on createWallet, expires_at on a grant) is malformed or not in the future → Maxio answers 422. Send expiry dates in the future as YYYY-MM-DD or a full timestamp.</description></item>
    ///   <item><description>An expiry date names a calendar day that does not exist, such as 2027-02-30 → Maxio answers 422. Validate expiry dates as real calendar days before sending.</description></item>
    ///   <item><description>A grant, debit, adjustment, purchase (or its preview) or update is sent for an archived wallet → Maxio answers 422 with "Wallet is archived". Check the wallet's status before writing to it; archived wallets accept no further writes.</description></item>
    ///   <item><description>A wallet amount is sent as a JSON number with a decimal point → Maxio answers 422. Send wallet amounts as decimal strings such as "150.0".</description></item>
    ///   <item><description>A kind: debit is posted to a metered wallet and live usage cannot be read in time → Maxio answers 503. Retry the debit later on 503; it is not returned for grants.</description></item>
    ///   <item><description>For a metered wallet, usage is read but settling it against the ledger fails → Maxio answers 500. Re-read the wallet and ledger before retrying after a 500 on a metered wallet.</description></item>
    ///   <item><description>createWalletTransaction is sent with an amount of 0 or less → Maxio answers 422. Always send amount positive; kind decides whether it is added or removed.</description></item>
    ///   <item><description>createWalletTransaction sends expires_at with kind: debit → Maxio answers 422. Send expires_at only on grants.</description></item>
    ///   <item><description>A kind: debit is posted to a discard wallet whose stored balance is already zero or below → Maxio answers 422 with "Wallet is depleted". Handle wallet_depleted as 'no units left' rather than as a malformed request.</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>An expiry (initial_balance_expires_at or a grant's expires_at) is sent as a bare calendar date → It is resolved to the end of that day in the site's time zone. Send a full timestamp if the grant must expire at a time other than end of day in the site's time zone.</description></item>
    ///   <item><description>A kind: debit larger than the available balance is posted to a wallet with hard_stop_overflow: allow_negative → The full amount is deducted; the excess is booked as one overdraft row with grant_id null, rejected_amount is 0.0 and the balance goes negative. Watch for negative balances on allow_negative wallets.</description></item>
    ///   <item><description>A debit drains several grants or goes negative → One ledger row is written per grant drained, plus an overdraft row if the balance goes negative. Expect several ledger rows from a single debit call.</description></item>
    ///   <item><description>A wallet is funded (manual grant, purchase, recurring refill or allowance increase) while its balance is negative → The overdraft is repaid first (a grant row with grant_id null) and the new grant gets only what is left, possibly 0; the grant is still created with its full original. Expect a top-up into an overdrawn wallet to show less remaining than the amount sent.</description></item>
    ///   <item><description>A debit is made against a wallet with several open grants → Grants are drained earliest-expiration-first. Expect debits to consume the soonest-expiring grants first.</description></item>
    ///   <item><description>A kind: grant is created without a memo → The grant's name is set to Grant #&lt;id&gt;; otherwise it is the memo. Send a memo to give the grant a readable name.</description></item>
    ///   <item><description>A kind: grant is posted while the wallet's balance is negative → The overdraft is repaid first (a grant row with grant_id null), then the grant is created with original equal to the full amount and remaining, and its row amount, equal to what is left (possibly 0). Expect two rows and a grant whose remaining is below original when topping up an overdrawn wallet.</description></item>
    ///   <item><description>A kind: debit is posted → Grants are drained earliest-expiration-first (evergreen grants last), writing one debit row per grant touched. Expect several ledger rows from one debit and read them all.</description></item>
    ///   <item><description>A kind: debit is posted while some grants are past their expires_at but not yet written off → Those grants are expired first (kind: expiration rows), even if the debit then fails with 422 wallet_depleted. Re-read the ledger after a failed debit; it may contain new expiration rows.</description></item>
    ///   <item><description>A kind: debit larger than the available balance is posted to a wallet with hard_stop_overflow: discard → Maxio answers 201. Read drained_amount and rejected_amount on every debit; a 201 does not mean the full amount was taken.</description></item>
    ///   <item><description>A kind: debit is posted to a metered discard wallet whose live usage has already consumed part or all of the stored balance → Maxio answers 201. Do not assume a stored-balance check predicts how much a debit on a metered wallet will drain.</description></item>
    ///   <item><description>A wallet's balance changes → Append-only ledger rows are written; rows are never modified and corrections are always new rows. Reconstruct wallet history from ledger rows and never expect an earlier row to change.</description></item>
    ///   <item><description>A ledger row carries a kind the app does not know → More kinds beyond grant, debit, adjustment, initial_balance and expiration are expected over time. Treat a ledger row's kind as an open string and do not fail on unknown kinds.</description></item>
    /// </list>
    /// </remarks>
    public Task<WalletTransactionResultResponse> CreateWalletTransaction(CreateWalletTransactionOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallets/{wallet_id}/transactions.json"),
            [new TemplateParam("wallet_id", request.WalletId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<WalletTransactionResultResponse>(),
            CreateWalletTransactionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Wallet Transactions
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletTransactionsListResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListWalletTransactionsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the wallet's append-only ledger, newest first, paginated.
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
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>listWalletTransactions is called → Ledger rows are returned newest first. Reverse the order yourself if you need the ledger oldest first.</description></item>
    ///   <item><description>A wallet's balance changes → Append-only ledger rows are written; rows are never modified and corrections are always new rows. Reconstruct wallet history from ledger rows and never expect an earlier row to change.</description></item>
    ///   <item><description>A wallet's ledger is listed → Rows are returned newest first. Reverse the order when you rebuild balance history from the oldest row.</description></item>
    ///   <item><description>A ledger row's balance_after is compared with the wallet's balance → balance_after is the posted balance after that row, so it can differ from balance on a metered wallet with pending usage. Reconcile balance_after against posted_balance, not balance.</description></item>
    ///   <item><description>A ledger row carries a kind the app does not know → More kinds beyond grant, debit, adjustment, initial_balance and expiration are expected over time. Treat a ledger row's kind as an open string and do not fail on unknown kinds.</description></item>
    /// </list>
    /// </remarks>
    public Task<WalletTransactionsListResponse> ListWalletTransactions(ListWalletTransactionsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallets/{wallet_id}/transactions.json"),
            [new TemplateParam("wallet_id", request.WalletId)],
            [new Param("page", request.Page), new Param("per_page", request.PerPage)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<WalletTransactionsListResponse>(),
            ListWalletTransactionsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
