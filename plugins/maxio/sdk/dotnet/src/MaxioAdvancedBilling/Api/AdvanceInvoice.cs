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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An advance invoice is requested for a subscription that has nothing due → Maxio answers 422 with "Cannot generate an invoice in advance when nothing is due". Check that the subscription has an amount due at its next renewal before issuing an advance invoice.</description></item>
    ///   <item><description>An advance invoice is requested for a subscription that uses prepaid components → Maxio answers 422 with "Cannot generate an invoice in advance when a subscription uses prepaid components". Do not offer advance invoices for subscriptions with prepaid components.</description></item>
    ///   <item><description>An advance invoice is requested while the subscription already has one for the billing period (and force is not true) → Maxio answers 422 with "Advance invoice already exists". Read the existing advance invoice first, or send force: true to replace it on purpose.</description></item>
    ///   <item><description>An advance invoice is requested for a subscription that belongs to a subscription group → Maxio answers 422 with "Cannot generate an invoice in advance for subscription in the group". Do not offer advance invoices for grouped subscriptions.</description></item>
    ///   <item><description>An advance invoice is requested for an inactive (end-of-life) subscription → Maxio answers 422 with "Cannot generate an invoice in advance for inactive subscription". Issue advance invoices only for live subscriptions.</description></item>
    ///   <item><description>An advance invoice is requested for a subscription on calendar billing → Maxio answers 422 with "Cannot generate an invoice in advance for a calendar billing subscription". Do not offer advance invoices for calendar-billing subscriptions.</description></item>
    ///   <item><description>An advance invoice is requested for a prepaid subscription → Maxio answers 422 with "Cannot generate an invoice in advance for a prepaid subscription". Do not offer advance invoices for prepaid subscriptions.</description></item>
    ///   <item><description>An advance invoice is requested within an hour of the subscription's renewal → Maxio answers 422 with "Cannot generate an invoice in advance within an hour of renewal". Compare the time to the next renewal and do not request an advance invoice in the last hour before it.</description></item>
    ///   <item><description>An advance invoice is requested for a subscription that uses the site-level exchange rate → Maxio answers 422 with "Cannot generate an invoice in advance for a subscription using the site level exchange rate". Do not offer advance invoices for subscriptions priced with the site-level exchange rate.</description></item>
    ///   <item><description>An advance invoice is issued with force: true → Any existing advance invoice is voided and a new one is generated; if none exists a new one is generated. Send force: true only when voiding the current advance invoice is intended, and treat the old invoice as voided.</description></item>
    ///   <item><description>An invoice is issued for a subscription on a Relationship Invoicing site → An invoice_issued webhook is sent. Listen for invoice_issued to learn about issued invoices.</description></item>
    ///   <item><description>An invoice is issued while the subscription has prepayments, with the site's prepayment setting (Config &gt; Settings &gt; Prepayments) in force → Prepayments are applied on issue, on the due date, or not at all per the setting; defaults are on the due date for automatic and on issue for remittance. Check the site's prepayment setting before assuming prepayments reduce an invoice when it is issued.</description></item>
    ///   <item><description>An invoice with net terms is issued while the subscription has prepayments → Prepayments are not applied automatically on issue; they are applied automatically only when the invoice has no net terms. Apply prepayments yourself (record a prepayment-type payment) to invoices that have net terms.</description></item>
    ///   <item><description>A line item's period_range_start and period_range_end are set → Charges in advance run from the billing date to the next billing date; charges in arrears (metered) run from the previous billing to the current one; one-off charges have equal dates. Read the period range according to whether the charge is billed in advance, in arrears or once.</description></item>
    ///   <item><description>A line item bills a component → product_id may be set even for component charges; only a nil component_id marks a product-only charge. Use component_id, not product_id, to tell component charges from product charges.</description></item>
    ///   <item><description>A consolidated (parent) invoice is returned → The parent invoice has no line items of its own; its totals aggregate the segments. Read a consolidated invoice's lines from its segments, not from the parent.</description></item>
    ///   <item><description>Branding Themes are not enabled for the site → branding_theme_id is not available on invoices. Treat branding_theme_id as optional and only rely on it when Branding Themes are enabled.</description></item>
    ///   <item><description>An invoice is returned → total_amount is computed as subtotal_amount - discount_amount + tax_amount. Use the returned total_amount rather than recomputing it from line items.</description></item>
    ///   <item><description>An invoice is returned → due_amount is computed as total_amount - credit_amount - paid_amount. Use due_amount as the amount still owed; it already nets out credits and payments.</description></item>
    ///   <item><description>An invoice has received only partial payment → paid_date stays empty until the invoice is paid in full. Do not treat an empty paid_date as no payment; check paid_amount for partial payments.</description></item>
    ///   <item><description>An invoice or credit note returns issue_date, due_date or paid_date → The date is the site's local calendar date, not a UTC date. Interpret invoice and credit-note dates in the site's time zone.</description></item>
    ///   <item><description>Line-item amounts are summed or recomputed by the client → subtotal_amount is the canonical line amount; the sum of line total_amounts may not equal the invoice total_amount because of rounding. Reconcile against the invoice totals, not the sum of line total_amount values.</description></item>
    ///   <item><description>A line item shows discount_amount or tax_amount → The per-line discount and tax are approximations of the invoice-level amounts apportioned across lines. Use the invoice-level discount_amount and tax_amount for accounting, not the sum of per-line values.</description></item>
    ///   <item><description>A line item is returned with tax_included → tax_included is inherited from the source price point; custom or ad hoc lines always return false. Do not expect tax-inclusive pricing on custom ad hoc lines.</description></item>
    ///   <item><description>A line item was priced with a tiered scheme → unit_price is the blended average per unit and tiered_unit_price is true. Do not multiply unit_price by quantity to re-derive tiered charges; use subtotal_amount.</description></item>
    ///   <item><description>The subscription's advance invoice for the period has been voided → A new advance invoice can be issued for the same period. Allow issuing a new advance invoice after voiding the old one.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A line item's period_range_start and period_range_end are set → Charges in advance run from the billing date to the next billing date; charges in arrears (metered) run from the previous billing to the current one; one-off charges have equal dates. Read the period range according to whether the charge is billed in advance, in arrears or once.</description></item>
    ///   <item><description>A line item bills a component → product_id may be set even for component charges; only a nil component_id marks a product-only charge. Use component_id, not product_id, to tell component charges from product charges.</description></item>
    ///   <item><description>A consolidated (parent) invoice is returned → The parent invoice has no line items of its own; its totals aggregate the segments. Read a consolidated invoice's lines from its segments, not from the parent.</description></item>
    ///   <item><description>Branding Themes are not enabled for the site → branding_theme_id is not available on invoices. Treat branding_theme_id as optional and only rely on it when Branding Themes are enabled.</description></item>
    ///   <item><description>An invoice is returned → total_amount is computed as subtotal_amount - discount_amount + tax_amount. Use the returned total_amount rather than recomputing it from line items.</description></item>
    ///   <item><description>An invoice is returned → due_amount is computed as total_amount - credit_amount - paid_amount. Use due_amount as the amount still owed; it already nets out credits and payments.</description></item>
    ///   <item><description>An invoice has received only partial payment → paid_date stays empty until the invoice is paid in full. Do not treat an empty paid_date as no payment; check paid_amount for partial payments.</description></item>
    ///   <item><description>An invoice or credit note returns issue_date, due_date or paid_date → The date is the site's local calendar date, not a UTC date. Interpret invoice and credit-note dates in the site's time zone.</description></item>
    ///   <item><description>Line-item amounts are summed or recomputed by the client → subtotal_amount is the canonical line amount; the sum of line total_amounts may not equal the invoice total_amount because of rounding. Reconcile against the invoice totals, not the sum of line total_amount values.</description></item>
    ///   <item><description>A line item shows discount_amount or tax_amount → The per-line discount and tax are approximations of the invoice-level amounts apportioned across lines. Use the invoice-level discount_amount and tax_amount for accounting, not the sum of per-line values.</description></item>
    ///   <item><description>A line item is returned with tax_included → tax_included is inherited from the source price point; custom or ad hoc lines always return false. Do not expect tax-inclusive pricing on custom ad hoc lines.</description></item>
    ///   <item><description>A line item was priced with a tiered scheme → unit_price is the blended average per unit and tiered_unit_price is true. Do not multiply unit_price by quantity to re-derive tiered charges; use subtotal_amount.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription's advance invoice is not in open status when a void is requested → the outcome is not stated. Void an advance invoice only while its status is open.</description></item>
    ///   <item><description>A line item's period_range_start and period_range_end are set → Charges in advance run from the billing date to the next billing date; charges in arrears (metered) run from the previous billing to the current one; one-off charges have equal dates. Read the period range according to whether the charge is billed in advance, in arrears or once.</description></item>
    ///   <item><description>A line item bills a component → product_id may be set even for component charges; only a nil component_id marks a product-only charge. Use component_id, not product_id, to tell component charges from product charges.</description></item>
    ///   <item><description>A consolidated (parent) invoice is returned → The parent invoice has no line items of its own; its totals aggregate the segments. Read a consolidated invoice's lines from its segments, not from the parent.</description></item>
    ///   <item><description>Branding Themes are not enabled for the site → branding_theme_id is not available on invoices. Treat branding_theme_id as optional and only rely on it when Branding Themes are enabled.</description></item>
    ///   <item><description>An invoice is returned → total_amount is computed as subtotal_amount - discount_amount + tax_amount. Use the returned total_amount rather than recomputing it from line items.</description></item>
    ///   <item><description>An invoice is returned → due_amount is computed as total_amount - credit_amount - paid_amount. Use due_amount as the amount still owed; it already nets out credits and payments.</description></item>
    ///   <item><description>An invoice has received only partial payment → paid_date stays empty until the invoice is paid in full. Do not treat an empty paid_date as no payment; check paid_amount for partial payments.</description></item>
    ///   <item><description>An invoice or credit note returns issue_date, due_date or paid_date → The date is the site's local calendar date, not a UTC date. Interpret invoice and credit-note dates in the site's time zone.</description></item>
    ///   <item><description>Line-item amounts are summed or recomputed by the client → subtotal_amount is the canonical line amount; the sum of line total_amounts may not equal the invoice total_amount because of rounding. Reconcile against the invoice totals, not the sum of line total_amount values.</description></item>
    ///   <item><description>A line item shows discount_amount or tax_amount → The per-line discount and tax are approximations of the invoice-level amounts apportioned across lines. Use the invoice-level discount_amount and tax_amount for accounting, not the sum of per-line values.</description></item>
    ///   <item><description>A line item is returned with tax_included → tax_included is inherited from the source price point; custom or ad hoc lines always return false. Do not expect tax-inclusive pricing on custom ad hoc lines.</description></item>
    ///   <item><description>A line item was priced with a tiered scheme → unit_price is the blended average per unit and tiered_unit_price is true. Do not multiply unit_price by quantity to re-derive tiered charges; use subtotal_amount.</description></item>
    ///   <item><description>An advance invoice is voided → Prepayments and credits that were applied to the invoice are returned to the subscription. Expect the subscription's prepayment and service credit balances to rise again after voiding an advance invoice.</description></item>
    ///   <item><description>The subscription's advance invoice for the period has been voided → A new advance invoice can be issued for the same period. Allow issuing a new advance invoice after voiding the old one.</description></item>
    /// </list>
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
