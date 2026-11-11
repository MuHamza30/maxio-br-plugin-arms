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
using MaxioAdvancedBilling.Requests.SubscriptionGroupInvoiceAccount;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Subscription Group Invoice Account object lets you manage prepayments and service credits for the shared invoice account of a Subscription Group, applying at the group level rather than to an individual subscription.
/// </summary>
public sealed class SubscriptionGroupInvoiceAccount
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal SubscriptionGroupInvoiceAccount(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Subscription Group Prepayment
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionGroupPrepaymentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateSubscriptionGroupPrepaymentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Adds a prepayment for a subscription group. This endpoint requires an <c>amount</c>, <c>details</c>, <c>method</c>, and <c>memo</c>. On success, the prepayment will be added to the group's prepayment balance.
    /// </remarks>
    public Task<SubscriptionGroupPrepaymentResponse> CreateSubscriptionGroupPrepayment(CreateSubscriptionGroupPrepaymentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/prepayments.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionGroupPrepaymentResponse>(),
            CreateSubscriptionGroupPrepaymentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Deduct Subscription Group Service Credit
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ServiceCredit"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeductSubscriptionGroupServiceCreditError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deducts service credit for a subscription group. Credit will be deducted from the group in the amount specified in the request body.
    /// </remarks>
    public Task<ServiceCredit> DeductSubscriptionGroupServiceCredit(DeductSubscriptionGroupServiceCreditRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/service_credit_deductions.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ServiceCredit>(),
            DeductSubscriptionGroupServiceCreditError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Issue Subscription Group Service Credit
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ServiceCreditResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="IssueSubscriptionGroupServiceCreditError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Issues service credit for a subscription group. Credit will be added to the group in the amount specified in the request body. The credit will be applied to group member invoices as they are generated.
    /// </remarks>
    public Task<ServiceCreditResponse> IssueSubscriptionGroupServiceCredit(IssueSubscriptionGroupServiceCreditRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/service_credits.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ServiceCreditResponse>(),
            IssueSubscriptionGroupServiceCreditError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Prepayments For Subscription Group
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListSubscriptionGroupPrepaymentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListPrepaymentsForSubscriptionGroupError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists a subscription group's prepayments.
    /// </remarks>
    public Task<ListSubscriptionGroupPrepaymentResponse> ListPrepaymentsForSubscriptionGroup(ListPrepaymentsForSubscriptionGroupRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/prepayments.json"),
            [new TemplateParam("uid", request.Uid)],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("filter", request.Filter),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListSubscriptionGroupPrepaymentResponse>(),
            ListPrepaymentsForSubscriptionGroupError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
