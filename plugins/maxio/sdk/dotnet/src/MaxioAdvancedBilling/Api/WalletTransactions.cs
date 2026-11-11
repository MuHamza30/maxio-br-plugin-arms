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
