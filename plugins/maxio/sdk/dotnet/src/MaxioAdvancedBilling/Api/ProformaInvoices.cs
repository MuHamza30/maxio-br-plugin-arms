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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A proforma invoice endpoint is called on a site that does not use Relationship Invoicing → the outcome is not stated. Use proforma invoices only on Relationship Invoicing sites.</description></item>
    ///   <item><description>A consolidated proforma invoice is requested while one is already being generated for the group → Maxio answers 422 with "Consolidated proforma invoice generation already in progress". Wait for the running generation to finish before requesting another for the group.</description></item>
    ///   <item><description>A proforma invoice is requested for a prepaid subscription → Maxio answers 422 with "this subscription is not eligible to create proforma invoices". Do not request proforma invoices for prepaid subscriptions.</description></item>
    ///   <item><description>A proforma invoice is requested for a subscription that is not in a live state (e.g. end-of-life) → Maxio answers 422 with "this subscription is not eligible to create proforma invoices". Request proforma invoices only for live subscriptions.</description></item>
    ///   <item><description>A consolidated proforma invoice is requested for a subscription group → Maxio answers 201. Poll List Subscription Group Proforma Invoices to find the new consolidated proforma invoice.</description></item>
    ///   <item><description>The subscription's billing information changes after a proforma invoice was created → The existing proforma invoice is not updated. Void an outdated proforma invoice and create a new one; use the preview endpoint to check for changes.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A proforma invoice endpoint is called on a site that does not use Relationship Invoicing → the outcome is not stated. Use proforma invoices only on Relationship Invoicing sites.</description></item>
    ///   <item><description>A proforma invoice is requested for a subscription that is in a group → Maxio answers 422 with "this subscription is not eligible to create proforma invoices". Use the consolidated proforma endpoint for grouped subscriptions.</description></item>
    ///   <item><description>A proforma invoice is requested for a prepaid subscription → Maxio answers 422 with "this subscription is not eligible to create proforma invoices". Do not request proforma invoices for prepaid subscriptions.</description></item>
    ///   <item><description>A proforma invoice is requested for a subscription that is not in a live state (e.g. end-of-life) → Maxio answers 422 with "this subscription is not eligible to create proforma invoices". Request proforma invoices only for live subscriptions.</description></item>
    ///   <item><description>The subscription's billing information changes after a proforma invoice was created → The existing proforma invoice is not updated. Void an outdated proforma invoice and create a new one; use the preview endpoint to check for changes.</description></item>
    ///   <item><description>A line item's period_range_start and period_range_end are set → Charges in advance run from the billing date to the next billing date; charges in arrears (metered) run from the previous billing to the current one; one-off charges have equal dates. Read the period range according to whether the charge is billed in advance, in arrears or once.</description></item>
    ///   <item><description>A line item bills a component → product_id may be set even for component charges; only a nil component_id marks a product-only charge. Use component_id, not product_id, to tell component charges from product charges.</description></item>
    ///   <item><description>Line-item amounts are summed or recomputed by the client → subtotal_amount is the canonical line amount; the sum of line total_amounts may not equal the invoice total_amount because of rounding. Reconcile against the invoice totals, not the sum of line total_amount values.</description></item>
    ///   <item><description>A line item shows discount_amount or tax_amount → The per-line discount and tax are approximations of the invoice-level amounts apportioned across lines. Use the invoice-level discount_amount and tax_amount for accounting, not the sum of per-line values.</description></item>
    ///   <item><description>A line item is returned with tax_included → tax_included is inherited from the source price point; custom or ad hoc lines always return false. Do not expect tax-inclusive pricing on custom ad hoc lines.</description></item>
    ///   <item><description>A line item was priced with a tiered scheme → unit_price is the blended average per unit and tiered_unit_price is true. Do not multiply unit_price by quantity to re-derive tiered charges; use subtotal_amount.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A proforma invoice endpoint is called on a site that does not use Relationship Invoicing → the outcome is not stated. Use proforma invoices only on Relationship Invoicing sites.</description></item>
    ///   <item><description>A signup proforma invoice or preview is requested for a consolidated (group) signup → the outcome is not stated. Do not use the signup proforma endpoints for subscription group signups.</description></item>
    ///   <item><description>A signup proforma invoice or preview is requested for a prepaid subscription → the outcome is not stated. Do not use the signup proforma endpoints for prepaid subscriptions.</description></item>
    ///   <item><description>The signup payload has no customer_id and lacks the customer's first name, last name or email → Maxio answers 422 with "Missing required customer attributes". Send a product plus either customer_id or customer first_name, last_name and email.</description></item>
    ///   <item><description>The signup payload names a product that does not exist on the site → Maxio answers 422 with "Couldn't find Product by 1". Check the product id or handle exists before requesting a signup proforma.</description></item>
    ///   <item><description>The subscription body names neither product_handle nor product_id, uses no offer_id, and is not a components-only subscription under the new Catalog experience → the outcome is not stated. Identify the product with product_handle or product_id (or pass offer_id) unless building a components-only subscription on a Catalog-experience site.</description></item>
    ///   <item><description>A custom_price would give the subscription more than 30 custom price points → the outcome is not stated. Keep each subscription at 30 custom price points or fewer.</description></item>
    ///   <item><description>payment_collection_method is set to a value the site's invoicing architecture does not support (invoice/automatic on legacy Statements; remittance/automatic/prepaid on Relationship Invoicing) → the outcome is not stated. Choose payment_collection_method from the values valid for the site's architecture.</description></item>
    ///   <item><description>A signup proforma invoice is created → Its subscription_id is always blank. Send customer_id so the signup proforma invoice can be found by customer later.</description></item>
    ///   <item><description>group.target.id is sent while target type is self, parent or eldest → The id is ignored. Omit target.id when the target type is self, parent or eldest.</description></item>
    ///   <item><description>A line item's period_range_start and period_range_end are set → Charges in advance run from the billing date to the next billing date; charges in arrears (metered) run from the previous billing to the current one; one-off charges have equal dates. Read the period range according to whether the charge is billed in advance, in arrears or once.</description></item>
    ///   <item><description>A line item bills a component → product_id may be set even for component charges; only a nil component_id marks a product-only charge. Use component_id, not product_id, to tell component charges from product charges.</description></item>
    ///   <item><description>Line-item amounts are summed or recomputed by the client → subtotal_amount is the canonical line amount; the sum of line total_amounts may not equal the invoice total_amount because of rounding. Reconcile against the invoice totals, not the sum of line total_amount values.</description></item>
    ///   <item><description>A line item shows discount_amount or tax_amount → The per-line discount and tax are approximations of the invoice-level amounts apportioned across lines. Use the invoice-level discount_amount and tax_amount for accounting, not the sum of per-line values.</description></item>
    ///   <item><description>A line item is returned with tax_included → tax_included is inherited from the source price point; custom or ad hoc lines always return false. Do not expect tax-inclusive pricing on custom ad hoc lines.</description></item>
    ///   <item><description>A line item was priced with a tiered scheme → unit_price is the blended average per unit and tiered_unit_price is true. Do not multiply unit_price by quantity to re-derive tiered charges; use subtotal_amount.</description></item>
    ///   <item><description>A metafields key is sent that does not yet exist as a custom field on the site → The metafield is created on the fly for that key. Check metafield keys before sending; a misspelt key silently creates a new custom field.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>No recipient is given and none can be derived from the invoice or subscription → Maxio answers 422 with "recipient_emails: must include at least one email". Send at least one recipient email when the subscription has no default recipient.</description></item>
    ///   <item><description>A recipient email address is not valid → Maxio answers 422 with "recipient_emails: must be a valid email address". Validate email addresses before delivering a proforma invoice.</description></item>
    ///   <item><description>The proforma invoice cannot be queued for email delivery → Maxio answers 422 with "Unable to enqueue proforma invoice for email delivery". Treat this 422 as not sent and show the error rather than assuming delivery.</description></item>
    ///   <item><description>A proforma invoice is delivered without recipient_emails → It is sent to the primary recipient derived from the invoice or subscription. Send recipient_emails when the proforma must go to someone other than the default recipient.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200, or no per_page → per_page defaults to 20 and any value over 200 is changed to 200 without an error. Page with per_page of 200 or less and keep requesting pages until one comes back short.</description></item>
    ///   <item><description>A subscription's proforma invoices are filtered with start_date and end_date → The range applies to the proforma invoice's due date. Filter proforma invoices by their due date range.</description></item>
    ///   <item><description>Proforma invoices are listed without the breakdown flags set to true → Each proforma invoice carries totals only; line_items, discounts, taxes, credits, payments and custom_fields are left out. Pass the breakdown flags (e.g. line_items=true) when the app needs the detail arrays.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>A line item's period_range_start and period_range_end are set → Charges in advance run from the billing date to the next billing date; charges in arrears (metered) run from the previous billing to the current one; one-off charges have equal dates. Read the period range according to whether the charge is billed in advance, in arrears or once.</description></item>
    ///   <item><description>A line item bills a component → product_id may be set even for component charges; only a nil component_id marks a product-only charge. Use component_id, not product_id, to tell component charges from product charges.</description></item>
    ///   <item><description>Line-item amounts are summed or recomputed by the client → subtotal_amount is the canonical line amount; the sum of line total_amounts may not equal the invoice total_amount because of rounding. Reconcile against the invoice totals, not the sum of line total_amount values.</description></item>
    ///   <item><description>A line item shows discount_amount or tax_amount → The per-line discount and tax are approximations of the invoice-level amounts apportioned across lines. Use the invoice-level discount_amount and tax_amount for accounting, not the sum of per-line values.</description></item>
    ///   <item><description>A line item is returned with tax_included → tax_included is inherited from the source price point; custom or ad hoc lines always return false. Do not expect tax-inclusive pricing on custom ad hoc lines.</description></item>
    ///   <item><description>A line item was priced with a tiered scheme → unit_price is the blended average per unit and tiered_unit_price is true. Do not multiply unit_price by quantity to re-derive tiered charges; use subtotal_amount.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A subscription group's proforma invoices are listed → Only proforma invoices with consolidation_level parent are returned. Use List Subscription Proforma Invoices to see a member subscription's own proforma invoices.</description></item>
    ///   <item><description>Proforma invoices are listed without the breakdown flags set to true → Each proforma invoice carries totals only; line_items, discounts, taxes, credits, payments and custom_fields are left out. Pass the breakdown flags (e.g. line_items=true) when the app needs the detail arrays.</description></item>
    ///   <item><description>A line item's period_range_start and period_range_end are set → Charges in advance run from the billing date to the next billing date; charges in arrears (metered) run from the previous billing to the current one; one-off charges have equal dates. Read the period range according to whether the charge is billed in advance, in arrears or once.</description></item>
    ///   <item><description>A line item bills a component → product_id may be set even for component charges; only a nil component_id marks a product-only charge. Use component_id, not product_id, to tell component charges from product charges.</description></item>
    ///   <item><description>Line-item amounts are summed or recomputed by the client → subtotal_amount is the canonical line amount; the sum of line total_amounts may not equal the invoice total_amount because of rounding. Reconcile against the invoice totals, not the sum of line total_amount values.</description></item>
    ///   <item><description>A line item shows discount_amount or tax_amount → The per-line discount and tax are approximations of the invoice-level amounts apportioned across lines. Use the invoice-level discount_amount and tax_amount for accounting, not the sum of per-line values.</description></item>
    ///   <item><description>A line item is returned with tax_included → tax_included is inherited from the source price point; custom or ad hoc lines always return false. Do not expect tax-inclusive pricing on custom ad hoc lines.</description></item>
    ///   <item><description>A line item was priced with a tiered scheme → unit_price is the blended average per unit and tiered_unit_price is true. Do not multiply unit_price by quantity to re-derive tiered charges; use subtotal_amount.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A proforma invoice endpoint is called on a site that does not use Relationship Invoicing → the outcome is not stated. Use proforma invoices only on Relationship Invoicing sites.</description></item>
    ///   <item><description>A proforma invoice is requested for a subscription that is in a group → Maxio answers 422 with "this subscription is not eligible to create proforma invoices". Use the consolidated proforma endpoint for grouped subscriptions.</description></item>
    ///   <item><description>A proforma invoice is requested for a prepaid subscription → Maxio answers 422 with "this subscription is not eligible to create proforma invoices". Do not request proforma invoices for prepaid subscriptions.</description></item>
    ///   <item><description>A proforma invoice is requested for a subscription that is not in a live state (e.g. end-of-life) → Maxio answers 422 with "this subscription is not eligible to create proforma invoices". Request proforma invoices only for live subscriptions.</description></item>
    ///   <item><description>A proforma invoice preview is requested → Nothing is saved; the preview cannot be retrieved after the call. Store the preview response yourself if it is needed later, or create a proforma invoice.</description></item>
    ///   <item><description>A line item's period_range_start and period_range_end are set → Charges in advance run from the billing date to the next billing date; charges in arrears (metered) run from the previous billing to the current one; one-off charges have equal dates. Read the period range according to whether the charge is billed in advance, in arrears or once.</description></item>
    ///   <item><description>A line item bills a component → product_id may be set even for component charges; only a nil component_id marks a product-only charge. Use component_id, not product_id, to tell component charges from product charges.</description></item>
    ///   <item><description>Line-item amounts are summed or recomputed by the client → subtotal_amount is the canonical line amount; the sum of line total_amounts may not equal the invoice total_amount because of rounding. Reconcile against the invoice totals, not the sum of line total_amount values.</description></item>
    ///   <item><description>A line item shows discount_amount or tax_amount → The per-line discount and tax are approximations of the invoice-level amounts apportioned across lines. Use the invoice-level discount_amount and tax_amount for accounting, not the sum of per-line values.</description></item>
    ///   <item><description>A line item is returned with tax_included → tax_included is inherited from the source price point; custom or ad hoc lines always return false. Do not expect tax-inclusive pricing on custom ad hoc lines.</description></item>
    ///   <item><description>A line item was priced with a tiered scheme → unit_price is the blended average per unit and tiered_unit_price is true. Do not multiply unit_price by quantity to re-derive tiered charges; use subtotal_amount.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A proforma invoice endpoint is called on a site that does not use Relationship Invoicing → the outcome is not stated. Use proforma invoices only on Relationship Invoicing sites.</description></item>
    ///   <item><description>A signup proforma invoice or preview is requested for a consolidated (group) signup → the outcome is not stated. Do not use the signup proforma endpoints for subscription group signups.</description></item>
    ///   <item><description>A signup proforma invoice or preview is requested for a prepaid subscription → the outcome is not stated. Do not use the signup proforma endpoints for prepaid subscriptions.</description></item>
    ///   <item><description>The signup payload has no customer_id and lacks the customer's first name, last name or email → Maxio answers 422 with "Missing required customer attributes". Send a product plus either customer_id or customer first_name, last_name and email.</description></item>
    ///   <item><description>The signup payload names a product that does not exist on the site → Maxio answers 422 with "Couldn't find Product by 1". Check the product id or handle exists before requesting a signup proforma.</description></item>
    ///   <item><description>The subscription body names neither product_handle nor product_id, uses no offer_id, and is not a components-only subscription under the new Catalog experience → the outcome is not stated. Identify the product with product_handle or product_id (or pass offer_id) unless building a components-only subscription on a Catalog-experience site.</description></item>
    ///   <item><description>A custom_price would give the subscription more than 30 custom price points → the outcome is not stated. Keep each subscription at 30 custom price points or fewer.</description></item>
    ///   <item><description>payment_collection_method is set to a value the site's invoicing architecture does not support (invoice/automatic on legacy Statements; remittance/automatic/prepaid on Relationship Invoicing) → the outcome is not stated. Choose payment_collection_method from the values valid for the site's architecture.</description></item>
    ///   <item><description>group.target.id is sent while target type is self, parent or eldest → The id is ignored. Omit target.id when the target type is self, parent or eldest.</description></item>
    ///   <item><description>A proforma invoice preview is requested → Nothing is saved; the preview cannot be retrieved after the call. Store the preview response yourself if it is needed later, or create a proforma invoice.</description></item>
    ///   <item><description>A signup proforma preview is requested without include=next_proforma_invoice → Only the current proforma invoice is returned; the first renewal's preview is left out. Send include=next_proforma_invoice to preview the first renewal's costs too.</description></item>
    ///   <item><description>A line item's period_range_start and period_range_end are set → Charges in advance run from the billing date to the next billing date; charges in arrears (metered) run from the previous billing to the current one; one-off charges have equal dates. Read the period range according to whether the charge is billed in advance, in arrears or once.</description></item>
    ///   <item><description>A line item bills a component → product_id may be set even for component charges; only a nil component_id marks a product-only charge. Use component_id, not product_id, to tell component charges from product charges.</description></item>
    ///   <item><description>Line-item amounts are summed or recomputed by the client → subtotal_amount is the canonical line amount; the sum of line total_amounts may not equal the invoice total_amount because of rounding. Reconcile against the invoice totals, not the sum of line total_amount values.</description></item>
    ///   <item><description>A line item shows discount_amount or tax_amount → The per-line discount and tax are approximations of the invoice-level amounts apportioned across lines. Use the invoice-level discount_amount and tax_amount for accounting, not the sum of per-line values.</description></item>
    ///   <item><description>A line item is returned with tax_included → tax_included is inherited from the source price point; custom or ad hoc lines always return false. Do not expect tax-inclusive pricing on custom ad hoc lines.</description></item>
    ///   <item><description>A line item was priced with a tiered scheme → unit_price is the blended average per unit and tiered_unit_price is true. Do not multiply unit_price by quantity to re-derive tiered charges; use subtotal_amount.</description></item>
    ///   <item><description>A metafields key is sent that does not yet exist as a custom field on the site → The metafield is created on the fly for that key. Check metafield keys before sending; a misspelt key silently creates a new custom field.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A proforma invoice endpoint is called on a site that does not use Relationship Invoicing → the outcome is not stated. Use proforma invoices only on Relationship Invoicing sites.</description></item>
    ///   <item><description>A line item's period_range_start and period_range_end are set → Charges in advance run from the billing date to the next billing date; charges in arrears (metered) run from the previous billing to the current one; one-off charges have equal dates. Read the period range according to whether the charge is billed in advance, in arrears or once.</description></item>
    ///   <item><description>A line item bills a component → product_id may be set even for component charges; only a nil component_id marks a product-only charge. Use component_id, not product_id, to tell component charges from product charges.</description></item>
    ///   <item><description>Line-item amounts are summed or recomputed by the client → subtotal_amount is the canonical line amount; the sum of line total_amounts may not equal the invoice total_amount because of rounding. Reconcile against the invoice totals, not the sum of line total_amount values.</description></item>
    ///   <item><description>A line item shows discount_amount or tax_amount → The per-line discount and tax are approximations of the invoice-level amounts apportioned across lines. Use the invoice-level discount_amount and tax_amount for accounting, not the sum of per-line values.</description></item>
    ///   <item><description>A line item is returned with tax_included → tax_included is inherited from the source price point; custom or ad hoc lines always return false. Do not expect tax-inclusive pricing on custom ad hoc lines.</description></item>
    ///   <item><description>A line item was priced with a tiered scheme → unit_price is the blended average per unit and tiered_unit_price is true. Do not multiply unit_price by quantity to re-derive tiered charges; use subtotal_amount.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A proforma invoice endpoint is called on a site that does not use Relationship Invoicing → the outcome is not stated. Use proforma invoices only on Relationship Invoicing sites.</description></item>
    ///   <item><description>A proforma invoice that is not in draft status is voided → Maxio answers 422 with "Only draft proforma invoices may be voided.". Void only draft proforma invoices.</description></item>
    /// </list>
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
