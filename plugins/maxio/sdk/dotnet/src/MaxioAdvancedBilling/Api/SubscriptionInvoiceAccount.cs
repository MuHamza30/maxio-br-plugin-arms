using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.SubscriptionInvoiceAccount;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Subscription Invoice Account object lets you manage a subscription's account balance, including prepayments, service credits, and refunds. For more information, see <see href="https://docs.maxio.com/getting-started/maxio-platform-basics/advanced-billing-overview-and-resources/subscription-summary-tabs/recording-prepayments">Recording Prepayments</see> and <see href="https://docs.maxio.com/billing/subscriptions/management/subscription-actions/give-deduct-and-apply-service-credits">Give, Deduct, and Apply Service Credits</see>.
/// </summary>
public sealed class SubscriptionInvoiceAccount
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal SubscriptionInvoiceAccount(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Prepayment
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CreatePrepaymentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreatePrepaymentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a prepayment for a subscription.
    /// <para>
    /// In order to specify a prepayment made against a subscription, specify the <c>amount, memo, details, method</c>.
    /// </para>
    /// <para>
    /// When the <c>method</c> specified is <c>"credit_card_on_file"</c>, the prepayment amount will be collected using the default credit card payment profile and applied to the prepayment account balance.  This is especially useful for manual replenishment of prepaid subscriptions.
    /// </para>
    /// <para>
    /// Note that passing <c>amount_in_cents</c> is now allowed.
    /// </para>
    /// <para>
    /// ## 3D Secure (3DS) Authentication post-authentication flow
    /// </para>
    /// <para>
    /// When a payment requires 3DS Authentication to adhere to Strong Customer Authentication (SCA), the request enters a post-authentication flow where a 422 Unprocessable Entity status is returned with an action_link that will direct the customer through 3DS Authentication.
    /// </para>
    /// <para>
    /// See the <see href="https://docs.maxio.com/hc/en-us/articles/44277749524365-3D-Secure-Post-Authentication-Flow">3D Secure Post-Authentication Flow</see> article in the product documentation to learn how to manage the redirect flow.
    /// </para>
    /// </remarks>
    public Task<CreatePrepaymentResponse> CreatePrepayment(CreatePrepaymentOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/prepayments.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<CreatePrepaymentResponse>(),
            CreatePrepaymentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Deduct Service Credit
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeductServiceCreditError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deducts a service credit from the subscription in the specified amount. The credit amount being deducted must be equal to or less than the current credit balance.
    /// </remarks>
    public Task DeductServiceCredit(DeductServiceCreditOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/service_credit_deductions.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            VoidResponse.Instance,
            DeductServiceCreditError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Issue Service Credit
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ServiceCredit"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="IssueServiceCreditError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Adds a service credit to the subscription in the specified amount. The credit is subsequently applied to the next generated invoice.
    /// </remarks>
    public Task<ServiceCredit> IssueServiceCredit(IssueServiceCreditOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/service_credits.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ServiceCredit>(),
            IssueServiceCreditError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Prepayments
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PrepaymentsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListPrepaymentsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists a subscription's prepayments.
    /// </remarks>
    public Task<PrepaymentsResponse> ListPrepayments(ListPrepaymentsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/prepayments.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("filter", request.Filter),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<PrepaymentsResponse>(),
            ListPrepaymentsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Service Credits
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListServiceCreditsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListServiceCreditsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists a subscription's service credits.
    /// </remarks>
    public Task<ListServiceCreditsResponse> ListServiceCredits(ListServiceCreditsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/service_credits/list.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("direction", request.Direction),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListServiceCreditsResponse>(),
            ListServiceCreditsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Account Balances
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="AccountBalances"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns the <c>balance_in_cents</c> of the Subscription's Pending Discount, Service Credit, and Prepayment accounts, as well as the sum of the Subscription's open, payable invoices.
    /// </remarks>
    public Task<AccountBalances> ReadAccountBalances(ReadAccountBalancesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/account_balances.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<AccountBalances>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Refund Prepayment
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PrepaymentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RefundPrepaymentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Refunds a prepayment applied to a subscription, either fully or partially. The <c>prepayment_id</c> will be the account transaction ID of the original payment. The prepayment must have some amount remaining in order to be refunded.
    /// <para>
    /// The amount may be passed either as a decimal, with <c>amount</c>, or an integer in cents, with <c>amount_in_cents</c>.
    /// </para>
    /// </remarks>
    public Task<PrepaymentResponse> RefundPrepayment(RefundPrepaymentOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/prepayments/{prepayment_id}/refunds.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("prepayment_id", request.PrepaymentId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<PrepaymentResponse>(),
            RefundPrepaymentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
