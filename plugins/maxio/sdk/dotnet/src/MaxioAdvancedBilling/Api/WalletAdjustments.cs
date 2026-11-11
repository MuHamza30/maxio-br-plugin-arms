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
