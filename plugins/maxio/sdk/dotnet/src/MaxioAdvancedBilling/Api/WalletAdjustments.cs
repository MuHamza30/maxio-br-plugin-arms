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
using MaxioAdvancedBilling.Requests.WalletAdjustments;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Wallet Adjustments post a signed correction against a specific grant, or against an overdraft, on a wallet's ledger. Adjustments are intended for support and operations corrections, and each adjustment requires a reason that is recorded on the ledger. For more information, see <see href="https://docs.maxio.com/billing/catalog/wallets/understand-wallets">Understand Wallets</see>.
/// </summary>
public sealed class WalletAdjustments
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal WalletAdjustments(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Wallet Adjustment
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="WalletGrantResultResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateWalletAdjustmentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Posts a signed correction against a specific grant (or, for an overdraft entry, against the overdraft): positive adds units to the grant, negative removes them. <c>reason</c> is required and is written to the ledger row's <c>memo</c>. Writes exactly one ledger row, of kind <c>adjustment</c>.
    /// <para>
    /// Use <c>source_transaction_id</c> to name a specific <c>debit</c> row when you're correcting one recorded consumption rather than the grant's balance as a whole. When it names a grant-attributed debit, it's recorded for audit only: naming it doesn't change how <c>amount</c> is applied, and more than one adjustment may name the same row. When it names an overdraft entry, it switches the correction to the overdraft path described below.
    /// </para>
    /// <para>
    /// An overdraft entry (a <c>debit</c> row with no <c>grant_id</c>, written when an <c>allow_negative</c> wallet consumes past its grants) can be corrected without a <c>grant_id</c>. That correction is credit-only, credits no grant, and is bounded by the usage the entry still records and by the overdraft the wallet still owes. Once a later grant has repaid the overdraft, correct that grant's balance instead.
    /// </para>
    /// <para>
    /// An adjustment corrects the ledger, so it works on the grant's recorded remainder, <c>posted_remaining</c>, not on <c>remaining</c>, which on a metered wallet read through <see href="$e/Wallets/readWallet">Read Wallet</see> also subtracts usage not yet posted. Send <c>posted_remaining</c> back as <c>expected_remaining</c>; sending <c>remaining</c> on a metered wallet with pending usage returns <c>409</c>.
    /// </para>
    /// <para>
    /// <c>expected_remaining</c> and <c>expected_source_corrections</c> are optional optimistic-concurrency checks; see their field descriptions. Use them when the correction's <c>amount</c> was derived from a value read earlier that might have moved since.
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
    ///   <item><description>createWalletAdjustment is sent with an amount of zero → Maxio answers 422. Send a non-zero signed amount.</description></item>
    ///   <item><description>An overdraft correction exceeds the overdraft the wallet still owes (for example after a later grant repaid it) → Maxio answers 422 with "Adjustment exceeds the wallet's outstanding overdraft". Once a later grant has repaid the overdraft, correct that grant's balance instead.</description></item>
    ///   <item><description>expected_remaining is sent and no longer matches the grant's current posted_remaining → Maxio answers 409 with "The grant balance changed since this correction was prepared. Reload and try again.". On 409, reload the wallet, re-derive the amount and retry.</description></item>
    ///   <item><description>expected_remaining is filled from a grant's remaining (not posted_remaining) on a metered wallet with pending usage → Maxio answers 409. Send the grant's posted_remaining as expected_remaining, never remaining.</description></item>
    ///   <item><description>expected_remaining or expected_source_corrections is sent as an empty string → Maxio answers 422. Leave these fields out entirely to skip the check; never send them empty.</description></item>
    ///   <item><description>expected_source_corrections is sent with source_transaction_id and no longer matches the sum of corrections posted against that entry → Maxio answers 409. Send the entry's source_corrections_total as last read, and on 409 reload and retry.</description></item>
    ///   <item><description>A negative adjustment would take the grant's posted_remaining below zero → Maxio answers 422 with "Adjustment exceeds the grant's remaining balance". Bound negative adjustments by the grant's posted_remaining; the server rejects rather than clamps.</description></item>
    ///   <item><description>A positive adjustment would credit the grant past its original amount → Maxio answers 422. Bound positive adjustments by the grant's original minus its posted_remaining.</description></item>
    ///   <item><description>createWalletAdjustment omits grant_id, or names a grant not on this wallet, while source_transaction_id does not name an overdraft entry → Maxio answers 422. Send the grant_id of a grant on this wallet unless correcting an overdraft entry.</description></item>
    ///   <item><description>createWalletAdjustment sends a grant_id while source_transaction_id names an overdraft entry → Maxio answers 422. Omit grant_id when correcting an overdraft entry.</description></item>
    ///   <item><description>source_transaction_id names a row that is not on this wallet or is not of kind debit → Maxio answers 422. Point source_transaction_id only at debit rows of the same wallet; correct grants and adjustments with a further adjustment.</description></item>
    ///   <item><description>An overdraft correction (no grant_id) is sent with a negative amount → Maxio answers 422 with "Overdraft usage can only be corrected downward". Send overdraft corrections as positive credits only.</description></item>
    ///   <item><description>An overdraft correction exceeds the usage the overdraft entry still records → Maxio answers 422 with "Adjustment exceeds the usage this entry still records". Bound an overdraft correction by the entry's amount net of its source_corrections_total.</description></item>
    ///   <item><description>A grant, debit, adjustment, purchase (or its preview) or update is sent for an archived wallet → Maxio answers 422 with "Wallet is archived". Check the wallet's status before writing to it; archived wallets accept no further writes.</description></item>
    ///   <item><description>A wallet amount is sent as a JSON number with a decimal point → Maxio answers 422. Send wallet amounts as decimal strings such as "150.0".</description></item>
    ///   <item><description>Any wallet, wallet transaction, adjustment, purchase or Wallet Product endpoint is called on a site without the Wallets feature enabled → Maxio answers 403 with "403 Forbidden". Enable Wallets for the site, and do not parse a 403 body from these endpoints as JSON.</description></item>
    ///   <item><description>A wallet or Wallet Product endpoint returns 404, 409, 422, 500 or 503 → The body is JSON with an errors array of messages that may be reworded and a stable machine-readable code. Branch on the code field, never on the message text.</description></item>
    ///   <item><description>A wallet or Wallet Product request fails a record validation rule → code is validation_failed rather than a field-specific code. Treat validation_failed as a generic validation failure and read errors for detail.</description></item>
    ///   <item><description>expected_remaining is sent with a different number of decimal places than posted_remaining → It is compared numerically, so "100" and "100.00000000" both match 100. Send expected_remaining in any decimal formatting; only its value matters.</description></item>
    ///   <item><description>expected_source_corrections is sent without source_transaction_id → It is ignored. Send expected_source_corrections only together with source_transaction_id.</description></item>
    ///   <item><description>An adjustment corrects an earlier ledger row → The earlier row is never edited; a new adjustment row is written (reason in its memo) and the debit's source_corrections_total sums the corrections. Net source_corrections_total out of a debit's amount to get its corrected quantity.</description></item>
    ///   <item><description>source_transaction_id names a debit row that is attributed to a grant → It is recorded for audit only: it does not change how amount is applied, and several adjustments may name the same row. Do not rely on source_transaction_id to limit or route a grant correction.</description></item>
    ///   <item><description>A wallet's balance changes → Append-only ledger rows are written; rows are never modified and corrections are always new rows. Reconstruct wallet history from ledger rows and never expect an earlier row to change.</description></item>
    ///   <item><description>A ledger row carries a kind the app does not know → More kinds beyond grant, debit, adjustment, initial_balance and expiration are expected over time. Treat a ledger row's kind as an open string and do not fail on unknown kinds.</description></item>
    /// </list>
    /// </remarks>
    public Task<WalletGrantResultResponse> CreateWalletAdjustment(CreateWalletAdjustmentOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/wallets/{wallet_id}/adjustments.json"),
            [new TemplateParam("wallet_id", request.WalletId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<WalletGrantResultResponse>(),
            CreateWalletAdjustmentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
