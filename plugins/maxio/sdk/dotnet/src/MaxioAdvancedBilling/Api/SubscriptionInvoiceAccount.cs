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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The prepayment charge requires 3D Secure authentication (SCA) → Maxio answers 422. On a 422 with an action_link, send the customer through the link to finish the 3DS flow.</description></item>
    ///   <item><description>A prepayment is created with method credit_card_on_file → The amount is charged to the default credit card payment profile and added to the prepayment balance. Use credit_card_on_file only when the customer's card should be charged now; other methods only record money received.</description></item>
    ///   <item><description>A prepayment is created with amount_in_cents → amount_in_cents is accepted. Send either amount or amount_in_cents for a prepayment.</description></item>
    ///   <item><description>A subscription's prepayment account balance changes (prepayment added, or used to pay an invoice) → A subscription_prepayment_account_balance_changed webhook is sent. Listen for subscription_prepayment_account_balance_changed to keep prepayment balances in sync.</description></item>
    ///   <item><description>A prepayment changes a prepaid subscription's prepayment balance → A prepaid_subscription_balance_change webhook is sent. Listen for prepaid_subscription_balance_change for prepaid subscriptions.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The deduction amount is more than the subscription's current service credit balance → Maxio answers 422 with "Amount cannot exceed current service credit account balance.". Read the service credit balance and deduct no more than it.</description></item>
    ///   <item><description>A deduction amount of 0 or less is sent → Maxio answers 422 with "Amount must be greater than 0.". Send a deduction amount greater than 0.</description></item>
    ///   <item><description>A subscription's service credit account balance changes (credit given, deducted or used for payment) → A subscription_service_credit_account_balance_changed webhook is sent. Listen for subscription_service_credit_account_balance_changed to keep service credit balances in sync.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A service credit amount of 0 or less is sent → Maxio answers 422 with "Amount must be greater than 0". Send a service credit amount greater than 0.</description></item>
    ///   <item><description>A service credit is issued → The credit is applied to the next generated invoice. Expect the next invoice's due amount to be reduced by the service credit.</description></item>
    ///   <item><description>A subscription's service credit account balance changes (credit given, deducted or used for payment) → A subscription_service_credit_account_balance_changed webhook is sent. Listen for subscription_service_credit_account_balance_changed to keep service credit balances in sync.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200, or no per_page → per_page defaults to 20 and any value over 200 is changed to 200 without an error. Page with per_page of 200 or less and keep requesting pages until one comes back short.</description></item>
    ///   <item><description>Prepayments are filtered with filter[start_date] and/or filter[end_date] → start_date starts at midnight and end_date runs to 11:59:59 PM, both in the site's time zone. Convert date bounds to the site's time zone before filtering prepayments.</description></item>
    ///   <item><description>filter[start_date] is sent → Prepayments whose date_field timestamp is at or after midnight of that date in the site's time zone are returned. Give start_date as a calendar date in the site's time zone.</description></item>
    ///   <item><description>filter[end_date] is sent → Prepayments whose date_field timestamp is up to and including 11:59:59 PM of that date in the site's time zone are returned. Give end_date as a calendar date in the site's time zone; the whole day is included.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Service credits are listed for a subscription that has no credit invoice account → Maxio answers 422 with "Credit invoice account not found for subscription". Treat this 422 as no service credit history for the subscription.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200, or no per_page → per_page defaults to 20 and any value over 200 is changed to 200 without an error. Page with per_page of 200 or less and keep requesting pages until one comes back short.</description></item>
    ///   <item><description>A service credit entry is returned → invoice_uid is present only on debit entries. Expect invoice_uid only on debit entries.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A subscription's account balances are read → Balances are in cents; open_invoices and pending_invoices are sums of the subscription's open and pending payable invoices. Divide the balances by 100 for currency amounts and treat the invoice balances as computed sums.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A prepayment with no amount remaining is refunded → the outcome is not stated. Refund only prepayments with a remaining amount.</description></item>
    ///   <item><description>A refund of a prepayment made through a payment profile is sent without the external flag → the outcome is not stated. Always send external on prepayment refunds; it may be omitted only for external prepayments.</description></item>
    ///   <item><description>The refund amount is more than the prepayment amount → Maxio answers 422 with "Refund amount exceeds prepayment amount". Refund no more than the prepayment's remaining amount.</description></item>
    ///   <item><description>A prepayment refund amount of 0 or less is sent → Maxio answers 422 with "Amount: must be greater than 0.". Send a refund amount greater than 0.</description></item>
    ///   <item><description>A subscription's prepayment account balance changes (prepayment added, or used to pay an invoice) → A subscription_prepayment_account_balance_changed webhook is sent. Listen for subscription_prepayment_account_balance_changed to keep prepayment balances in sync.</description></item>
    ///   <item><description>A prepayment refund is sent with only one of amount or amount_in_cents → The refund is accepted; the other amount field is not required. Send the refund amount once, either as amount (decimal) or amount_in_cents (integer).</description></item>
    /// </list>
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
