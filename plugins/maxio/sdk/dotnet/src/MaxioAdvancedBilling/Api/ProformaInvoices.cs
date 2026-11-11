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
using MaxioAdvancedBilling.Requests.ProformaInvoices;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Proforma Invoice object is a preview of an upcoming invoice, showing the charges a customer will be billed before the actual invoice is issued. A proforma invoice is not a finalized billing document and is commonly used to give customers advance visibility into what they owe. For more information, see <see href="https://docs.maxio.com/billing/invoicing/proforma-invoices/understand-proforma-invoices">Understand Proforma Invoices</see>.
/// </summary>
public sealed class ProformaInvoices
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal ProformaInvoices(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Consolidated Proforma Invoices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateConsolidatedProformaInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a consolidated proforma invoice asynchronously. To find and view the new consolidated proforma invoice, you can poll the subscription group listing for proforma invoices; only one consolidated proforma invoice can be created per group at a time.
    /// <para>
    /// If the information becomes outdated, simply void the old consolidated proforma invoice and generate a new one.
    /// </para>
    /// <para>
    /// ## Restrictions
    /// </para>
    /// <para>
    /// Proforma invoices are only available on Relationship Invoicing sites. To create a proforma invoice, the subscription must not be prepaid, and must be in a live state.
    /// </para>
    /// </remarks>
    public Task CreateConsolidatedProformaInvoice(CreateConsolidatedProformaInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/proforma_invoices.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            VoidResponse.Instance,
            CreateConsolidatedProformaInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Proforma Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProformaInvoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateProformaInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a proforma invoice and returns it as a response. If the information becomes outdated, simply void the old proforma invoice and generate a new one.
    /// <para>
    /// If you would like to preview the next billing amounts without generating a full proforma invoice, use the renewal preview endpoint.
    /// </para>
    /// <para>
    /// ## Restrictions
    /// </para>
    /// <para>
    /// Proforma invoices are only available on Relationship Invoicing sites. To create a proforma invoice, the subscription must not be in a group, must not be prepaid, and must be in a live state.
    /// </para>
    /// </remarks>
    public Task<ProformaInvoice> CreateProformaInvoice(CreateProformaInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/proforma_invoices.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<ProformaInvoice>(),
            CreateProformaInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create signup proforma invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProformaInvoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateSignupProformaInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a proforma invoice to preview costs before a subscription's signup. This endpoint is only available for Relationship Invoicing sites and cannot be used to create consolidated proforma invoices or preview prepaid subscriptions. Like other proforma invoices, it can be emailed to the customer, voided, and publicly viewed on the chargifypay domain.
    /// <para>
    /// Pass a payload that resembles a subscription create or signup preview request. For example, you can specify components, coupons/a referral, offers, custom pricing, and an existing customer or payment profile to populate a shipping or billing address.
    /// </para>
    /// <para>
    /// A product and customer first name, last name, and email are the minimum requirements. We recommend associating the proforma invoice with a customer_id to easily find their proforma invoices, since the subscription_id will always be blank.
    /// </para>
    /// </remarks>
    public Task<ProformaInvoice> CreateSignupProformaInvoice(CreateSignupProformaInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/proforma_invoices.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ProformaInvoice>(),
            CreateSignupProformaInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Deliver Proforma Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProformaInvoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeliverProformaInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Delivers a proforma invoice programmatically via email. Supports email
    /// delivery to direct recipients, carbon-copy (cc) recipients, and blind carbon-copy (bcc) recipients.
    /// <para>
    /// If <c>recipient_emails</c> is omitted, the system will fall back to the primary recipient derived from the invoice or
    /// subscription. At least one recipient must be present, either via the request body or via this default behavior, so an
    /// empty body may still succeed when defaults are available.
    /// </para>
    /// </remarks>
    public Task<ProformaInvoice> DeliverProformaInvoice(DeliverProformaInvoiceOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/proforma_invoices/{proforma_invoice_uid}/deliveries.json"),
            [new TemplateParam("proforma_invoice_uid", request.ProformaInvoiceUid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ProformaInvoice>(),
            DeliverProformaInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Subscription Proforma Invoices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListProformaInvoicesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists proforma invoices for a subscription. By default, results only include totals, not detailed breakdowns for <c>line_items</c>, <c>discounts</c>, <c>taxes</c>, <c>credits</c>, <c>payments</c>, or <c>custom_fields</c>. To include breakdowns, pass the specific field as a key in the query with a value set to <c>true</c>.
    /// </remarks>
    public Task<ListProformaInvoicesResponse> ListProformaInvoices(ListProformaInvoicesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/proforma_invoices.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [
                new Param("start_date", request.StartDate),
                new Param("end_date", request.EndDate),
                new Param("status", request.Status),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("direction", request.Direction),
                new Param("line_items", request.LineItems),
                new Param("discounts", request.Discounts),
                new Param("taxes", request.Taxes),
                new Param("credits", request.Credits),
                new Param("payments", request.Payments),
                new Param("custom_fields", request.CustomFields),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListProformaInvoicesResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Subscription Group Proforma Invoices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListProformaInvoicesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListSubscriptionGroupProformaInvoicesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists proforma invoices with a <c>consolidation_level</c> of parent for the subscription group.
    /// <para>
    /// By default, proforma invoices returned on the index will only include totals, not detailed breakdowns for <c>line_items</c>, <c>discounts</c>, <c>taxes</c>, <c>credits</c>, <c>payments</c>, <c>custom_fields</c>. To include breakdowns, pass the specific field as a key in the query with a value set to true.
    /// </para>
    /// </remarks>
    public Task<ListProformaInvoicesResponse> ListSubscriptionGroupProformaInvoices(ListSubscriptionGroupProformaInvoicesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/proforma_invoices.json"),
            [new TemplateParam("uid", request.Uid)],
            [
                new Param("line_items", request.LineItems),
                new Param("discounts", request.Discounts),
                new Param("taxes", request.Taxes),
                new Param("credits", request.Credits),
                new Param("payments", request.Payments),
                new Param("custom_fields", request.CustomFields),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListProformaInvoicesResponse>(),
            ListSubscriptionGroupProformaInvoicesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Preview Proforma Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProformaInvoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="PreviewProformaInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Previews the data that will be included on a given subscription's proforma invoice if one were to be generated. It will have similar line items and totals as a renewal preview, but the response will be presented in the format of a proforma invoice. Consequently it will include additional information such as the name and addresses that will appear on the proforma invoice.
    /// <para>
    /// The preview endpoint is subject to all the same conditions as the proforma invoice endpoint. For example, previews are only available on the Relationship Invoicing architecture, and previews cannot be made for end-of-life subscriptions.
    /// </para>
    /// <para>
    /// If all the data returned in the preview is as expected, you may then create a static proforma invoice and send it to your customer. The data within a preview will not be saved and will not be accessible after the call is made.
    /// </para>
    /// <para>
    /// Alternatively, if you have some proforma invoices already, you may make a preview call to determine whether any billing information for the subscription's upcoming renewal has changed.
    /// </para>
    /// </remarks>
    public Task<ProformaInvoice> PreviewProformaInvoice(PreviewProformaInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/proforma_invoices/preview.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<ProformaInvoice>(),
            PreviewProformaInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create signup proforma preview
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SignupProformaPreviewResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="PreviewSignupProformaInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a signup preview in the format of a proforma invoice to preview costs before a subscription's signup. This endpoint is only available for Relationship Invoicing sites and cannot be used to create consolidated proforma invoice previews or preview prepaid subscriptions. You have the option of previewing the first renewal's costs as well. The proforma invoice preview will not be persisted.
    /// <para>
    /// Pass a payload that resembles a subscription create or signup preview request. For example, you can specify components, coupons/a referral, offers, custom pricing, and an existing customer or payment profile to populate a shipping or billing address.
    /// </para>
    /// <para>
    /// A product and customer first name, last name, and email are the minimum requirements.
    /// </para>
    /// </remarks>
    public Task<SignupProformaPreviewResponse> PreviewSignupProformaInvoice(PreviewSignupProformaInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/proforma_invoices/preview.json"),
            [],
            [new Param("include", request.Include)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SignupProformaPreviewResponse>(),
            PreviewSignupProformaInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Proforma Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProformaInvoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadProformaInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns the details of an existing proforma invoice.
    /// <para>
    /// ## Restrictions
    /// </para>
    /// <para>
    /// Proforma invoices are only available on Relationship Invoicing sites.
    /// </para>
    /// </remarks>
    public Task<ProformaInvoice> ReadProformaInvoice(ReadProformaInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/proforma_invoices/{proforma_invoice_uid}.json"),
            [new TemplateParam("proforma_invoice_uid", request.ProformaInvoiceUid)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ProformaInvoice>(),
            ReadProformaInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Void Proforma Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ProformaInvoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="VoidProformaInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Voids a proforma invoice that has the status "draft".
    /// <para>
    /// ## Restrictions
    /// </para>
    /// <para>
    /// Proforma invoices are only available on Relationship Invoicing sites.
    /// </para>
    /// <para>
    /// Only proforma invoices that have the appropriate status may be reopened. If the invoice identified by {uid} does not have the appropriate status, the response will have HTTP status code 422 and an error message.
    /// </para>
    /// <para>
    /// A reason for the void operation is required to be included in the request body. If one is not provided, the response will have HTTP status code 422 and an error message.
    /// </para>
    /// </remarks>
    public Task<ProformaInvoice> VoidProformaInvoice(VoidProformaInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/proforma_invoices/{proforma_invoice_uid}/void.json"),
            [new TemplateParam("proforma_invoice_uid", request.ProformaInvoiceUid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ProformaInvoice>(),
            VoidProformaInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
