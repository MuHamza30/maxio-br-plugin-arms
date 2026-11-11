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
using MaxioAdvancedBilling.Requests.AdvanceInvoice;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Use the Advance Invoice feature to issue, retrieve, or void an invoice for a subscription ahead of its normal billing schedule, collecting an outstanding balance before the next scheduled renewal. For more information, see <see href="https://docs.maxio.com/billing/invoicing/advance-invoices/issue-advance-invoices-for-subscription-renewals">Issue Advance Invoices for Subscription Renewals</see>.
/// </summary>
public sealed class AdvanceInvoice
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal AdvanceInvoice(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Issue advance invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="IssueAdvanceInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Issues an invoice in advance for a subscription's next renewal date. For the most part, advance invoices function like any other invoice, except they are issued early and have special behavior upon being voided. For more information on advance invoices, including eligibility for generating one, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24252026404749-Issue-Invoice-In-Advance">Issue Invoice In Advance</see>.
    /// <para>
    /// A subscription can only have one advance invoice per billing period. Attempting to issue an advance invoice when one already exists returns an error.
    /// </para>
    /// <para>
    /// Regeneration of the invoice can be forced with the params <c>force: true</c>, which voids an advance invoice if one exists and generates a new one. If no advance invoice exists, a new one is generated.
    /// </para>
    /// <para>
    /// Consider using either the create or preview endpoints for proforma invoices to preview this advance invoice before using this endpoint to generate it.
    /// </para>
    /// </remarks>
    public Task<Invoice> IssueAdvanceInvoice(IssueAdvanceInvoiceOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/advance_invoice/issue.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<Invoice>(),
            IssueAdvanceInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read advance invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadAdvanceInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns the advance invoice generated for a subscription's upcoming renewal. There can only be one advance invoice per subscription per billing cycle.
    /// </remarks>
    public Task<Invoice> ReadAdvanceInvoice(ReadAdvanceInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/advance_invoice.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<Invoice>(),
            ReadAdvanceInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Void advance invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="VoidAdvanceInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Voids a subscription's existing advance invoice. Once voided, it can later be regenerated if desired.
    /// <para>
    /// A <c>reason</c> is required to void, and the invoice must have an open status. Voiding causes any prepayments and credits that were applied to the invoice to be returned to the subscription.
    /// </para>
    /// <para>
    /// For a full overview of the impact of voiding, see <see href="$m/Invoice">Invoice</see>.
    /// </para>
    /// </remarks>
    public Task<Invoice> VoidAdvanceInvoice(VoidAdvanceInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/advance_invoice/void.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<Invoice>(),
            VoidAdvanceInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
