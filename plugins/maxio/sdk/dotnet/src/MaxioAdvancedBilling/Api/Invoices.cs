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
using MaxioAdvancedBilling.Requests.Invoices;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Invoice object is the billing document generated for a subscription, itemizing charges, taxes, and payments, and tracking amounts owed.
/// <para>
/// The role indicates what triggered the invoice (signup, renewal, usage, proration, etc.), and the status and due_date reflect where an invoice stands in the payment lifecycle. For more information, see <see href="https://docs.maxio.com/billing/invoicing/overview/get-started-with-invoicing">Get Started with Invoicing</see>.
/// </para>
/// </summary>
public sealed class Invoices
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Invoices(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="InvoiceResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates an ad hoc invoice.
    /// <para>
    /// ### Basic Behavior
    /// </para>
    /// <para>
    /// You can create a basic invoice by sending an array of line items to this endpoint. Each line item, at a minimum, must include a title, a quantity and a unit price. Example:
    /// </para>
    /// <code>
    /// {
    ///   "invoice": {
    ///     "line_items": [
    ///       {
    ///         "title": "A Product",
    ///         "quantity": 12,
    ///         "unit_price": "150.00"
    ///       }
    ///     ]
    ///   }
    /// }
    /// </code>
    /// <para>
    /// ### Catalog items
    /// Instead of creating custom products like in above example, You can pass existing items like products, components.
    /// </para>
    /// <code>
    /// {
    ///   "invoice": {
    ///     "line_items": [
    ///       {
    ///         "product_id": "handle:gold-product",
    ///         "quantity": 2,
    ///       }
    ///     ]
    ///   }
    /// }
    /// </code>
    /// <para>
    ///
    /// The price for each line item will be calculated as well as a total due amount for the invoice. Multiple line items can be sent.
    /// </para>
    /// <para>
    /// ### Line item types
    /// When defining a line item, You can choose one of 3 types for a line item:
    /// #### Custom item
    /// As shown in the basic behavior example, You can pass <c>title</c> and <c>unit_price</c> for custom item.
    /// #### Product id
    /// Product handle (with handle: prefix) or id from the scope of current subscription's site can be provided with <c>product_id</c>. By default <c>unit_price</c> is taken from product's default price point, but can be overwritten by passing <c>unit_price</c> or <c>product_price_point_id</c>. If <c>product_id</c> is used, following fields cannot be used: <c>title</c>, <c>component_id</c>.
    /// #### Component id
    /// Component handle (with handle: prefix) or id from the scope of current subscription's site can be provided with <c>component_id</c>. If <c>component_id</c> is used, following fields cannot be used: <c>title</c>, <c>product_id</c>. By default <c>unit_price</c> is taken from product's default price point, but can be overwritten by passing <c>unit_price</c> or <c>price_point_id</c>. At this moment price points are supported only for quantity based, on/off and metered components. For prepaid and event based billing components <c>unit_price</c> is required.
    /// </para>
    /// <para>
    /// ### Coupons
    /// When creating ad hoc invoice, new discounts can be applied in following way:
    /// </para>
    /// <para>
    /// <code>
    /// {
    ///   "invoice": {
    ///     "line_items": [
    ///       {
    ///         "product_id": "handle:gold-product",
    ///         "quantity": 1
    ///       }
    ///     ],
    ///     "coupons": [
    ///       {
    ///         "code": "COUPONCODE",
    ///         "percentage": 50.0
    ///       }
    ///     ]
    ///   }
    /// }
    /// </code>
    /// If You want to use existing coupon for discount creation, only <c>code</c> and optional <c>product_family_id</c> is needed
    /// </para>
    /// <code>
    /// ...
    ///  "coupons": [
    ///       {
    ///         "code": "FREESETUP",
    ///         "product_family_id": 1
    ///       }
    ///   ]
    /// ...
    /// </code>
    /// <para>
    /// #### Using Coupon Subcodes
    /// You can also use coupon subcodes to apply existing coupons with specific subcodes:
    /// </para>
    /// <para>
    /// <code>
    /// ...
    ///  "coupons": [
    ///       {
    ///         "subcode": "SUB1",
    ///         "product_family_id": 1
    ///       }
    ///   ]
    /// ...
    /// </code>
    /// <b>Important:</b> You cannot specify both <c>code</c> and <c>subcode</c> for the same coupon. Use either:
    /// - <c>code</c> to apply a main coupon
    /// - <c>subcode</c> to apply a specific coupon subcode
    /// </para>
    /// <para>
    /// The API response will include both the main coupon code and the subcode used:
    /// </para>
    /// <code>
    /// ...
    ///  "coupons": [
    ///       {
    ///         "code": "MAIN123",
    ///         "subcode": "SUB1",
    ///         "product_family_id": 1,
    ///         "percentage": 10,
    ///         "description": "Special discount"
    ///       }
    ///   ]
    /// ...
    /// </code>
    /// <para>
    /// ### Coupon options
    /// #### Code
    /// Coupon <c>code</c> will be displayed on invoice discount section.
    /// Coupon code can only contain uppercase letters, numbers, and allowed special characters.
    /// Lowercase letters will be converted to uppercase. It can be used to select an existing coupon from the catalog, or as an ad hoc coupon when passed with <c>percentage</c> or <c>amount</c>.
    /// #### Subcode
    /// Coupon <c>subcode</c> allows you to apply existing coupons using their subcodes. When a subcode is used, the API response will include both the main coupon code and the specific subcode that was applied. Subcodes are case-insensitive and will be converted to uppercase automatically.
    /// #### Percentage
    /// Coupon <c>percentage</c> can take values from 0 to 100 and up to 4 decimal places. It cannot be used with <c>amount</c>. Only for ad hoc coupons, will be ignored if <c>code</c> is used to select an existing coupon from the catalog.
    /// #### Amount
    /// Coupon <c>amount</c> takes number value. It cannot be used with <c>percentage</c>. Used only when not matching existing coupon by <c>code</c>.
    /// #### Description
    /// Optional <c>description</c> will be displayed with coupon <c>code</c>. Used only when not matching existing coupon by <c>code</c>.
    /// #### Product Family id
    /// Optional <c>product_family_id</c> handle (with handle: prefix) or id is used to match existing coupon within site, when codes are not unique.
    /// #### Compounding Strategy
    /// Optional <c>compounding_strategy</c> for percentage coupons, can take values <c>compound</c> or <c>full-price</c>.
    /// </para>
    /// <para>
    /// For amount coupons, discounts will be always calculated against the original item price, before other discounts are applied.
    /// </para>
    /// <para>
    /// <c>compound</c> strategy:
    /// Percentage-based discounts will be calculated against the remaining price, after prior discounts have been calculated. It is set by default.
    /// </para>
    /// <para>
    /// <c>full-price</c> strategy:
    /// Percentage-based discounts will always be calculated against the original item price, before other discounts are applied.
    /// </para>
    /// <para>
    /// ### Line Item Options
    /// </para>
    /// <para>
    /// #### Period Date Range
    /// </para>
    /// <para>
    /// A custom period date range can be defined for each line item with the <c>period_range_start</c> and <c>period_range_end</c> parameters. Dates must be sent in the <c>YYYY-MM-DD</c> format.
    /// <c>period_range_end</c> must be greater or equal <c>period_range_start</c>.
    /// </para>
    /// <para>
    /// #### Taxes
    /// </para>
    /// <para>
    /// The <c>taxable</c> parameter can be sent as <c>true</c> if taxes should be calculated for a specific line item. For this to work, the site should be configured to use and calculate taxes. Further, if the site uses Avalara for tax calculations, a <c>tax_code</c> parameter should also be sent. For existing catalog items: products/components taxes cannot be overwritten.
    /// </para>
    /// <para>
    /// #### Price Point
    /// Price point handle (with handle: prefix) or id from the scope of current subscription's site can be provided with <c>price_point_id</c> for components with <c>component_id</c> or <c>product_price_point_id</c> for products with <c>product_id</c> parameter. If price point is passed <c>unit_price</c> cannot be used. It can be used only with catalog items products and components.
    /// </para>
    /// <para>
    /// #### Description
    /// Optional <c>description</c> parameter, it will overwrite default generated description for line item.
    /// </para>
    /// <para>
    /// ### Invoice Options
    /// </para>
    /// <para>
    /// #### Issue Date
    /// </para>
    /// <para>
    /// By default, invoices will be created with a issue date set to today in your site's time zone. The <c>issue_date</c> parameter can be sent to alter the default. Only today or dates in the past are accepted. This date is interpreted and validated in your site's time zone. The format for <c>issue_date</c> is <c>YYYY-MM-DD</c>.
    /// </para>
    /// <para>
    /// #### Net Terms
    /// </para>
    /// <para>
    /// By default, invoices will be created with a due date matching the date of invoice creation. If a different due date is desired, the <c>net_terms</c> parameter can be sent indicating the number of days in advance the due date should be.
    /// </para>
    /// <para>
    /// #### Addresses
    /// </para>
    /// <para>
    /// The seller, shipping and billing addresses can be sent to override the site's defaults. Each address requires to send a <c>first_name</c> at a minimum in order to work. See below for the details on which parameters can be sent for each address object.
    /// </para>
    /// <para>
    /// #### Memo and Payment Instructions
    /// </para>
    /// <para>
    /// A custom memo can be sent with the <c>memo</c> parameter to override the site's default. Likewise, custom payment instructions can be sent with the <c>payment_instructions</c> parameter.
    /// </para>
    /// <para>
    /// #### Status
    /// </para>
    /// <para>
    /// By default, invoices will be created with open status. Possible alternative is <c>draft</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A custom line item (no product_id or component_id) lacks a title, a quantity or a unit price → the outcome is not stated. Send title, quantity and unit_price on every custom line item.</description></item>
    ///   <item><description>A line item's period_range_end is before its period_range_start → the outcome is not stated. Make period_range_end the same as or later than period_range_start.</description></item>
    ///   <item><description>A taxable line item is sent without tax_code on a site that uses Avalara (AvaTax) → the outcome is not stated. Send a tax_code with every taxable line item when the site uses AvaTax.</description></item>
    ///   <item><description>A tax_code longer than 25 characters is sent → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>A coupon sends both code and subcode → the outcome is not stated. Send either code or subcode on each coupon, not both.</description></item>
    ///   <item><description>A coupon code contains characters other than uppercase letters, numbers and the allowed special characters → the outcome is not stated. Use only letters, numbers and allowed special characters in ad hoc coupon codes.</description></item>
    ///   <item><description>A line item sends product_id together with title or component_id → the outcome is not stated. Leave out title and component_id on product line items.</description></item>
    ///   <item><description>A coupon percentage has more than 4 decimal places or is outside 0 to 100 → the outcome is not stated. Send coupon percentages between 0 and 100 with at most 4 decimal places.</description></item>
    ///   <item><description>A coupon sends both percentage and amount → the outcome is not stated. Send either percentage or amount on an ad hoc coupon, not both.</description></item>
    ///   <item><description>issue_date is a future date in the site's time zone → the outcome is not stated. Send issue_date as today or an earlier date, judged in the site's time zone.</description></item>
    ///   <item><description>A line item sends component_id together with title or product_id → the outcome is not stated. Leave out title and product_id on component line items.</description></item>
    ///   <item><description>A seller, billing or shipping address is sent without first_name → Maxio answers 422 with "Billing address must include name.". Include first_name in every address object sent on an invoice.</description></item>
    ///   <item><description>A price point is sent for a component that is not quantity-based, on/off or metered → the outcome is not stated. Send price_point_id only for quantity-based, on/off and metered components.</description></item>
    ///   <item><description>A line item for a prepaid or event-based billing component is sent without unit_price → the outcome is not stated. Always send unit_price for prepaid and event-based components.</description></item>
    ///   <item><description>A line item sends a price point (price_point_id or product_price_point_id) together with unit_price → the outcome is not stated. Send either a price point or unit_price on a line item, never both.</description></item>
    ///   <item><description>A price point is sent on a custom (non-catalog) line item → the outcome is not stated. Use price points only on product or component line items.</description></item>
    ///   <item><description>A quantity or unit_price has more than 8 decimal places → The value is rounded down to the 8th decimal place. Round quantity and unit_price to 8 decimal places before sending.</description></item>
    ///   <item><description>A line item is sent with taxable: true on a site not configured to calculate taxes → the outcome is not stated. Send taxable: true only on sites that have tax calculation configured.</description></item>
    ///   <item><description>taxable is sent on a product or component (catalog) line item → The catalog item's own tax settings apply; they cannot be overwritten. Set taxability on the product or component in the catalog, not on the invoice line.</description></item>
    ///   <item><description>A line item is sent with a description → The given description replaces the line item's generated description. Leave description out to keep the generated description (which can carry period details).</description></item>
    ///   <item><description>A coupon code contains lowercase letters → Lowercase letters are converted to uppercase. Expect coupon codes to come back in uppercase.</description></item>
    ///   <item><description>A coupon is applied by subcode → The subcode is matched case-insensitively, stored in uppercase, and the response shows both the main code and the subcode. Match coupons in responses by both code and subcode.</description></item>
    ///   <item><description>A coupon's code matches an existing catalog coupon and percentage, amount or description is also sent → percentage, amount and description are ignored; the catalog coupon's settings apply. Send percentage, amount or description only for ad hoc coupons whose code is not in the catalog.</description></item>
    ///   <item><description>A coupon code is not unique across product families on the site → product_family_id is used to pick the matching existing coupon. Send product_family_id with the coupon code when codes repeat across product families.</description></item>
    ///   <item><description>A percentage coupon is sent without compounding_strategy → compound is used: the percentage applies to the price remaining after earlier discounts. Send compounding_strategy: full-price when the percentage should apply to the original price.</description></item>
    ///   <item><description>An amount (flat) coupon is applied → The discount is always calculated against the original item price, before other discounts. Do not expect compounding_strategy to change how amount coupons are calculated.</description></item>
    ///   <item><description>compounding_strategy is sent for a coupon that is not stackable → The setting has no effect. Rely on compounding_strategy only for stackable coupons.</description></item>
    ///   <item><description>An ad hoc invoice is created without issue_date → issue_date is today in the site's time zone. Send issue_date when the invoice should carry an earlier date.</description></item>
    ///   <item><description>An ad hoc invoice is created without net_terms → The due date is the invoice creation date; with net_terms it is that many days later. Send net_terms when the invoice should not be due immediately.</description></item>
    ///   <item><description>An ad hoc invoice is created without seller, billing or shipping addresses, memo or payment instructions → The site's defaults (seller address, memo) and the customer's defaults (billing and shipping addresses) are used. Send the address, memo or payment_instructions fields only to override the site and customer defaults.</description></item>
    ///   <item><description>A product line item is sent without unit_price or product_price_point_id → unit_price is taken from the product's default price point. Send unit_price or product_price_point_id when the product's default price should not apply.</description></item>
    ///   <item><description>A component line item is sent without unit_price or price_point_id → unit_price is taken from the default price point. Send unit_price or price_point_id when the default component price should not apply.</description></item>
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
    public Task<InvoiceResponse> CreateInvoice(CreateInvoiceOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/invoices.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<InvoiceResponse>(),
            CreateInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Draft Ad Hoc Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeleteInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes an ad hoc invoice while it is in the <c>draft</c> state.
    /// <para>
    /// <b>Important: only invoices with the <c>adhoc</c> role and <c>draft</c> status can be deleted.</b> Any other invoice — issued, or with a different role (e.g. <c>renewal</c>, <c>signup</c>) — cannot be deleted through this endpoint and the request returns a <c>422</c> error. Issued invoices should be voided instead. If the invoice does not belong to the provided subscription, a <c>404</c> error is returned.
    /// </para>
    /// <para>
    /// A successful deletion returns a <c>204 No Content</c> response and the invoice is permanently removed.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An invoice that is not in draft status is deleted → Maxio answers 422 with "Status must be draft.". Delete only draft invoices; void issued invoices instead.</description></item>
    ///   <item><description>An invoice that is not ad hoc (for example a renewal or signup invoice) is deleted → Maxio answers 422 with "Only adhoc invoices can be deleted.". Delete only ad hoc invoices.</description></item>
    ///   <item><description>The invoice does not belong to the subscription in the path → Maxio answers 404 with "Invoice not found". Send the subscription_id that owns the invoice.</description></item>
    ///   <item><description>A draft ad hoc invoice is deleted → Maxio answers 204. Confirm before deleting; a deleted invoice cannot be recovered.</description></item>
    /// </list>
    /// </remarks>
    public Task DeleteInvoice(DeleteInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/invoices/{uid}.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId), new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            DeleteInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Issue Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="IssueInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Issues an invoice that is in "pending" or "draft" status. For example, you can issue an invoice that was created when allocating new quantity on a component and using "accrue charges" option.
    /// <para>
    /// You cannot issue a pending child invoice that was created for a member subscription in a group.
    /// </para>
    /// <para>
    /// For Remittance subscriptions, the invoice will go into "open" status and payment won't be attempted. The value for <c>on_failed_payment</c> would be rejected if sent. Any prepayments or service credits that exist on the subscription will be automatically applied. Additionally, if the setting is enabled, an email will be sent for the issued invoice.
    /// </para>
    /// <para>
    /// For Automatic subscriptions, prepayments and service credits will apply to the invoice before payment is attempted. On successful payment, the invoice will go into "paid" status and email will be sent to the customer (if setting applies). When payment fails, the next event depends on the <c>on_failed_payment</c> value:
    /// - <c>leave_open_invoice</c> - prepayments and credits applied to invoice; invoice status set to "open"; email sent to the customer for the issued invoice (if setting applies); payment failure recorded in the invoice history. This is the default option.
    /// - <c>rollback_to_pending</c> - prepayments and credits not applied; invoice remains in "pending" status; no email sent to the customer; payment failure recorded in the invoice history.
    /// - <c>initiate_dunning</c> - prepayments and credits applied to the invoice; invoice status set to "open"; email sent to the customer for the issued invoice (if setting applies); payment failure recorded in the invoice history; subscription will  most likely go into "past_due" or "canceled" state (depending upon net terms and dunning settings).
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An invoice that is not in pending or draft status is issued → the outcome is not stated. Issue only pending or draft invoices.</description></item>
    ///   <item><description>A pending child invoice created for a member subscription of a group is issued → the outcome is not stated. Do not issue pending child invoices of grouped subscriptions.</description></item>
    ///   <item><description>on_failed_payment is sent when issuing an invoice of a remittance subscription → the outcome is not stated. Send on_failed_payment only for automatic subscriptions.</description></item>
    ///   <item><description>Payment fails while issuing with on_failed_payment initiate_dunning → The invoice becomes open with credits applied, an email is sent if the setting applies, and the subscription will most likely go past_due or canceled per net terms and dunning settings. Use initiate_dunning only when a failed charge should move the subscription into dunning.</description></item>
    ///   <item><description>An invoice is issued for a subscription on a Relationship Invoicing site → An invoice_issued webhook is sent. Listen for invoice_issued to learn about issued invoices.</description></item>
    ///   <item><description>An invoice is issued while the subscription has prepayments, with the site's prepayment setting (Config &gt; Settings &gt; Prepayments) in force → Prepayments are applied on issue, on the due date, or not at all per the setting; defaults are on the due date for automatic and on issue for remittance. Check the site's prepayment setting before assuming prepayments reduce an invoice when it is issued.</description></item>
    ///   <item><description>An invoice with net terms is issued while the subscription has prepayments → Prepayments are not applied automatically on issue; they are applied automatically only when the invoice has no net terms. Apply prepayments yourself (record a prepayment-type payment) to invoices that have net terms.</description></item>
    ///   <item><description>An invoice of a remittance subscription is issued → The invoice goes to open status and no payment is attempted. Collect remittance invoices by recording payments after issuing them.</description></item>
    ///   <item><description>An invoice of a remittance subscription is issued while the subscription has prepayments or service credits → The prepayments and service credits are applied to the invoice automatically. Expect a remittance invoice's due amount to be reduced by existing prepayments and service credits.</description></item>
    ///   <item><description>An invoice is issued (or paid on issue) and the site's invoice email setting is on → An email is sent to the customer for the issued invoice; with the setting off none is sent. Check the site's email settings before telling customers they will be emailed.</description></item>
    ///   <item><description>An invoice of an automatic subscription is issued → Prepayments and service credits are applied first, then payment is attempted; on success the invoice becomes paid. Expect a charge to the payment method when issuing an automatic subscription's invoice.</description></item>
    ///   <item><description>Payment fails while issuing an automatic subscription's invoice with on_failed_payment leave_open_invoice (the default) → Prepayments and credits stay applied, the invoice becomes open, an email is sent if the setting applies and the failure is recorded. Choose on_failed_payment deliberately; the default leaves an open invoice after a failed charge.</description></item>
    ///   <item><description>Payment fails while issuing with on_failed_payment rollback_to_pending → Prepayments and credits are not applied, the invoice stays pending, no email is sent and the failure is recorded. Use rollback_to_pending to retry issuing later without notifying the customer.</description></item>
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
    ///   <item><description>A payment is attempted or applied to an invoice → A payment_success or payment_failure webhook is sent. Listen for payment_success and payment_failure, and expect some payments (ACH, direct debit) to fail later.</description></item>
    /// </list>
    /// </remarks>
    public Task<Invoice> IssueInvoice(IssueInvoiceOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/{uid}/issue.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<Invoice>(),
            IssueInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Segments for Consolidated Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ConsolidatedInvoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists segments for a consolidated invoice. Invoice segments returned on the index will only include totals, not detailed breakdowns for <c>line_items</c>, <c>discounts</c>, <c>taxes</c>, <c>credits</c>, <c>payments</c>, or <c>custom_fields</c>.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Segments of a consolidated invoice are listed → Segments carry totals only, without line_items, discounts, taxes, credits, payments or custom_fields. Read a segment with Read Invoice when its detail is needed.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200, or no per_page → per_page defaults to 20 and any value over 200 is changed to 200 without an error. Page with per_page of 200 or less and keep requesting pages until one comes back short.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
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
    public Task<ConsolidatedInvoice> ListConsolidatedInvoiceSegments(ListConsolidatedInvoiceSegmentsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/{invoice_uid}/segments.json"),
            [new TemplateParam("invoice_uid", request.InvoiceUid)],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("direction", request.Direction),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ConsolidatedInvoice>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Credit Notes
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListCreditNotesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists credit notes for a site. Credit Notes are like inverse invoices. They reduce the amount a customer owes.
    /// <para>
    /// By default, the credit notes returned by this endpoint will exclude the arrays of <c>line_items</c>, <c>discounts</c>, <c>taxes</c>, <c>applications</c>, or <c>refunds</c>. To include these arrays, pass the specific field as a key in the query with a value set to <c>true</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Credit notes are listed without the array flags set to true → line_items, discounts, taxes, applications and refunds arrays are left out. Pass the array flags (e.g. line_items=true) when the app needs credit-note detail.</description></item>
    ///   <item><description>A date range (start_date/end_date) is sent without an explicit date_field → date_field defaults to issue_date. Send date_field explicitly with every date filter.</description></item>
    ///   <item><description>Only start_datetime/end_datetime are sent without an explicit date_field → date_field defaults to created_at instead of issue_date. Send date_field explicitly with datetime filters.</description></item>
    ///   <item><description>An unrecognized date_field value is sent → The date_field is ignored and no error is raised. Validate date_field against issue_date, applied_date, created_at and updated_at before sending.</description></item>
    ///   <item><description>Credit notes are filtered with start_date and/or end_date → start_date starts at midnight and end_date runs to 11:59:59 PM, both in the site's time zone. Convert date bounds to the site's time zone before filtering.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone offset → The value is interpreted as UTC. Send datetime filters with an explicit offset, or in UTC.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent together with start_date or end_date → The datetime parameter is used instead of the date parameter. Send either the date or the datetime form of each bound, not both.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent with a date_field other than created_at or updated_at → the outcome is not stated. Use datetime filters only with date_field=created_at or updated_at.</description></item>
    ///   <item><description>Credit notes are listed with a direction → The order is by sequence_number. Sort credit notes by sequence_number in the app, or rely on direction for sequence order only.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200, or no per_page → per_page defaults to 20 and any value over 200 is changed to 200 without an error. Page with per_page of 200 or less and keep requesting pages until one comes back short.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>A credit note is returned → total_amount is subtotal_amount - discount_amount + tax_amount and remaining_amount is total_amount - applied_amount. Use remaining_amount as the credit still available to apply.</description></item>
    ///   <item><description>A credit note is only partly applied → applied_date has no value until the credit note is fully applied. Do not treat an empty applied_date as unapplied; check applied_amount.</description></item>
    ///   <item><description>A credit note is returned → Subtotal, discount and tax amounts are positive numbers representing amounts credited back; discount and tax credits are proportional to the credited share of the original purchase. Treat credit-note amounts as positive credits, not negative invoice amounts.</description></item>
    ///   <item><description>An invoice or credit note returns issue_date, due_date or paid_date → The date is the site's local calendar date, not a UTC date. Interpret invoice and credit-note dates in the site's time zone.</description></item>
    /// </list>
    /// </remarks>
    public Task<ListCreditNotesResponse> ListCreditNotes(ListCreditNotesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/credit_notes.json"),
            [],
            [
                new Param("subscription_id", request.SubscriptionId),
                new Param("date_field", request.DateField),
                new Param("start_date", request.StartDate),
                new Param("end_date", request.EndDate),
                new Param("start_datetime", request.StartDatetime),
                new Param("end_datetime", request.EndDatetime),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("direction", request.Direction),
                new Param("line_items", request.LineItems),
                new Param("discounts", request.Discounts),
                new Param("taxes", request.Taxes),
                new Param("refunds", request.Refunds),
                new Param("applications", request.Applications),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListCreditNotesResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Invoice Events
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListInvoiceEventsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists invoice events for a site. Each event contains event "data" (such as an applied payment) as well as a snapshot of the <c>invoice</c> at the time of event completion.
    /// <para>
    /// Exposed event types are:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>issue_invoice</description></item>
    ///   <item><description>apply_credit_note</description></item>
    ///   <item><description>apply_payment</description></item>
    ///   <item><description>refund_invoice</description></item>
    ///   <item><description>void_invoice</description></item>
    ///   <item><description>void_remainder</description></item>
    ///   <item><description>backport_invoice</description></item>
    ///   <item><description>change_invoice_status</description></item>
    ///   <item><description>change_invoice_collection_method</description></item>
    ///   <item><description>remove_payment</description></item>
    ///   <item><description>failed_payment</description></item>
    ///   <item><description>apply_debit_note</description></item>
    ///   <item><description>create_debit_note</description></item>
    ///   <item><description>change_chargeback_status</description></item>
    /// </list>
    /// <para>
    /// Invoice events are returned in ascending order.
    /// </para>
    /// <para>
    /// If both a <c>since_date</c> and <c>since_id</c> are provided in request parameters, the <c>since_date</c> will be used.
    /// </para>
    /// <para>
    /// Note - invoice events that occurred prior to 09/05/2018 __will not__ contain an <c>invoice</c> snapshot.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Invoice events are listed → Events are returned in ascending order. Page forward through invoice events and keep the last id or time seen.</description></item>
    ///   <item><description>Both since_date and since_id are sent → since_date is used and since_id is ignored. Send only one of since_date or since_id.</description></item>
    ///   <item><description>since_id is sent → The event with that id is excluded; only events with a higher id are returned. Pass the id of the last event already processed as since_id.</description></item>
    ///   <item><description>since_date is sent as a date without a time → Events are returned from the beginning of that day; events before since_date are not returned. Send a full timestamp in since_date when resuming mid-day.</description></item>
    ///   <item><description>An invoice event occurred before 09/05/2018 → The event has no invoice snapshot. Handle a missing invoice snapshot on old invoice events.</description></item>
    ///   <item><description>Invoice events are listed with per_page above 200, or with no per_page → per_page defaults to 100 and any value over 200 is changed to 200. Page invoice events with per_page of 200 or less.</description></item>
    ///   <item><description>Invoice events are listed without with_change_invoice_status → change_invoice_status events are not included. Send with_change_invoice_status when the app needs invoice status-change events.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    /// </list>
    /// </remarks>
    public Task<ListInvoiceEventsResponse> ListInvoiceEvents(ListInvoiceEventsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/events.json"),
            [],
            [
                new Param("since_date", request.SinceDate),
                new Param("since_id", request.SinceId),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("invoice_uid", request.InvoiceUid),
                new Param("with_change_invoice_status", request.WithChangeInvoiceStatus),
                new Param("event_types", request.EventTypes),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListInvoiceEventsResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Invoices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListInvoicesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists invoices for a site. By default, invoices returned on the index will only include totals, not detailed breakdowns for <c>line_items</c>, <c>discounts</c>, <c>taxes</c>, <c>credits</c>, <c>payments</c>, <c>custom_fields</c>, or <c>refunds</c>. To include breakdowns, pass the specific field as a key in the query with a value set to <c>true</c>.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Invoices are listed without the breakdown flags set to true → Each invoice carries totals only; line_items, discounts, taxes, credits, payments, custom_fields and refunds are left out. Pass line_items=true (and the other breakdown flags) when the app needs the detail arrays.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>Invoices are filtered with start_date and/or end_date → start_date starts at midnight and end_date runs to 11:59:59 PM, both in the site's time zone. Convert date bounds to the site's time zone before filtering, not UTC.</description></item>
    ///   <item><description>Invoices are filtered with start_datetime or end_datetime as well as start_date or end_date → The datetime parameter is used and the matching date parameter is ignored. Send either the date or the datetime form of each bound, not both.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent with a date_field other than created_at or updated_at → the outcome is not stated. Use start_datetime/end_datetime only with date_field=created_at or updated_at, and start_date/end_date with due_date, issue_date or paid_date.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The value is read in the site's time zone. Include an explicit offset in datetime filters, or convert to the site's time zone first.</description></item>
    ///   <item><description>Invoices are listed with subscription_group_uid → The list holds the consolidated invoices of that group. Use subscription_group_uid to fetch a group's consolidated invoices, not its members' individual invoices.</description></item>
    ///   <item><description>Invoices are filtered by consolidation_level → The filter accepts one level or a comma-separated list of levels (none, parent, child). Send several consolidation levels as one comma-separated value, e.g. none,parent.</description></item>
    ///   <item><description>Invoices are filtered by product_ids → Invoices match when any of their line items has one of the given product ids. Expect product_ids to match on line-item products, not only the subscription's base product.</description></item>
    ///   <item><description>A list request sends per_page above 200, or no per_page → per_page defaults to 20 and any value over 200 is changed to 200 without an error. Page with per_page of 200 or less and keep requesting pages until one comes back short.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
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
    public Task<ListInvoicesResponse> ListInvoices(ListInvoicesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices.json"),
            [],
            [
                new Param("start_date", request.StartDate),
                new Param("end_date", request.EndDate),
                new Param("status", request.Status),
                new Param("subscription_id", request.SubscriptionId),
                new Param("subscription_group_uid", request.SubscriptionGroupUid),
                new Param("consolidation_level", request.ConsolidationLevel),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("direction", request.Direction),
                new Param("line_items", request.LineItems),
                new Param("discounts", request.Discounts),
                new Param("taxes", request.Taxes),
                new Param("credits", request.Credits),
                new Param("payments", request.Payments),
                new Param("custom_fields", request.CustomFields),
                new Param("refunds", request.Refunds),
                new Param("date_field", request.DateField),
                new Param("start_datetime", request.StartDatetime),
                new Param("end_datetime", request.EndDatetime),
                new Param("customer_ids", request.CustomerIds),
                new Param("number", request.Number),
                new Param("product_ids", request.ProductIds),
                new Param("sort", request.Sort),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListInvoicesResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Preview Customer Information Changes
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CustomerChangesPreviewResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="PreviewCustomerInformationChangesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Previews the effect of customer information changes on an open invoice. Customer information may change after an invoice is issued, which may lead to a mismatch between customer information that is present on an open invoice and actual customer information. This endpoint allows you to preview these differences, if any.
    /// <para>
    /// The endpoint doesn't accept a request body. Customer information differences are calculated on the application side.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The invoice is not in open status → Maxio answers 422 with "Invoice must have an open status". Preview or update customer information only on open invoices.</description></item>
    ///   <item><description>Customer information is previewed or updated on an invoice → No request body is taken; the differences between the invoice and the current customer record are calculated by Maxio. Update the customer record first, then call these endpoints without a body.</description></item>
    /// </list>
    /// </remarks>
    public Task<CustomerChangesPreviewResponse> PreviewCustomerInformationChanges(PreviewCustomerInformationChangesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/{uid}/customer_information/preview.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<CustomerChangesPreviewResponse>(),
            PreviewCustomerInformationChangesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Credit Note
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CreditNote"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns the details for a credit note.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A credit note is returned → total_amount is subtotal_amount - discount_amount + tax_amount and remaining_amount is total_amount - applied_amount. Use remaining_amount as the credit still available to apply.</description></item>
    ///   <item><description>A credit note is only partly applied → applied_date has no value until the credit note is fully applied. Do not treat an empty applied_date as unapplied; check applied_amount.</description></item>
    ///   <item><description>A credit note is returned → Subtotal, discount and tax amounts are positive numbers representing amounts credited back; discount and tax credits are proportional to the credited share of the original purchase. Treat credit-note amounts as positive credits, not negative invoice amounts.</description></item>
    ///   <item><description>An invoice or credit note returns issue_date, due_date or paid_date → The date is the site's local calendar date, not a UTC date. Interpret invoice and credit-note dates in the site's time zone.</description></item>
    /// </list>
    /// </remarks>
    public Task<CreditNote> ReadCreditNote(ReadCreditNoteRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/credit_notes/{uid}.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<CreditNote>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns the details for an invoice.
    /// <para>
    /// ## PDF Invoice retrieval
    /// </para>
    /// <para>
    /// Individual PDF Invoices can be retrieved by using the "Accept" header application/pdf or appending .pdf as the format portion of the URL:
    /// <code>
    /// Accept:application/pdf -H
    /// https://acme.chargify.com/invoices/inv_8gd8tdhtd3hgr.pdf &gt; output_file.pdf
    /// URL: `https://&lt;subdomain&gt;.chargify.com/invoices/&lt;uid&gt;.&lt;format&gt;`
    /// Method: GET
    /// Required parameters: `uid`
    /// Response: A single Invoice.
    /// </code>
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An invoice is requested with Accept: application/pdf or a .pdf extension → The invoice is returned as a PDF document instead of JSON. Request /invoices/{uid}.pdf or send Accept: application/pdf to get the PDF.</description></item>
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
    public Task<Invoice> ReadInvoice(ReadInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/{uid}.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<Invoice>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Record Payment for Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RecordPaymentForInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Applies a payment of a given type against a specific invoice. If you would like to apply a payment across multiple invoices, you can use the <see href="$e/Invoices/recordPaymentForMultipleInvoices">Record Payment for Multiple Invoices</see> endpoint.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The payment amount is more than the invoice's due amount → Maxio answers 422 with "Payment amount 10.50 exceeds due amount 4.00". Read the invoice's due_amount and do not pay more than it on a single invoice.</description></item>
    ///   <item><description>A payment is recorded against an invoice that is not open or in collections → Maxio answers 422 with "Invoice must be open or in collections to apply payment". Record payments only on open or in-collections invoices.</description></item>
    ///   <item><description>The payment amount is not a number → Maxio answers 422 with "Amount: is not a number.". Send the amount as a numeric value or numeric string.</description></item>
    ///   <item><description>received_on is today or a future date → Maxio answers 422 with "Effective date must occur in the past". Send received_on as a date in the past.</description></item>
    ///   <item><description>A subscription's prepayment account balance changes (prepayment added, or used to pay an invoice) → A subscription_prepayment_account_balance_changed webhook is sent. Listen for subscription_prepayment_account_balance_changed to keep prepayment balances in sync.</description></item>
    ///   <item><description>A subscription's service credit account balance changes (credit given, deducted or used for payment) → A subscription_service_credit_account_balance_changed webhook is sent. Listen for subscription_service_credit_account_balance_changed to keep service credit balances in sync.</description></item>
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
    ///   <item><description>A payment is recorded without a type → The type defaults to external. Send type explicitly (external, prepayment, service_credit or payment).</description></item>
    ///   <item><description>A payment is recorded without a method → The method defaults to other. Send the payment method explicitly so reports show how it was paid.</description></item>
    ///   <item><description>memo, details or received_on are sent on a payment whose type is not external → These fields apply only to external payments. Send memo, details and received_on only with external payments.</description></item>
    ///   <item><description>A payment is recorded with type prepayment or service_credit → The payment is taken from the subscription's prepayment or service credit account. Check the subscription's prepayment or service credit balance before paying from it.</description></item>
    ///   <item><description>A payment is attempted or applied to an invoice → A payment_success or payment_failure webhook is sent. Listen for payment_success and payment_failure, and expect some payments (ACH, direct debit) to fail later.</description></item>
    /// </list>
    /// </remarks>
    public Task<Invoice> RecordPaymentForInvoice(RecordPaymentForInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/{uid}/payments.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<Invoice>(),
            RecordPaymentForInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Record Payment for Multiple Invoices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="MultiInvoicePaymentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RecordPaymentForMultipleInvoicesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Records an external payment against multiple invoices.
    /// <para>
    /// To apply a payment to multiple invoices, at minimum, specify the <c>amount</c> and <c>applications</c> (i.e., <c>invoice_uid</c> and <c>amount</c>) details.
    /// </para>
    /// <para>
    /// Note that the invoice payment amounts must be greater than 0. Total amount must be greater or equal to invoices payment amount sum.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>received_on is today or a future date → Maxio answers 422 with "Effective date must occur in the past". Send received_on as a date in the past.</description></item>
    ///   <item><description>The total payment amount is zero or less → Maxio answers 422 with "Total amount must be greater than 0". Send a total amount greater than 0.</description></item>
    ///   <item><description>An application names an invoice with no open balance → Maxio answers 422 with "Invoice must have an open balance". Include only invoices that still have an amount due.</description></item>
    ///   <item><description>The payment's effective date is missing → Maxio answers 422 with "Effective date is missing". Send the date the payment was received (received_on).</description></item>
    ///   <item><description>The payment's effective date is invalid or malformed → Maxio answers 422 with "Effective date is invalid or malformed". Send received_on as a valid YYYY-MM-DD date.</description></item>
    ///   <item><description>More than one application names the same invoice → Maxio answers 422 with "Multiple applications associated to a single invoice, please aggregate and send as a single application per invoice.". Aggregate amounts per invoice and send one application per invoice_uid.</description></item>
    ///   <item><description>The total amount is less than the sum of the application amounts → Maxio answers 422 with "Total amount must be higher or equal to invoices payment amount sum". Make the total amount at least the sum of the per-invoice amounts.</description></item>
    ///   <item><description>An application names an invoice that does not exist → Maxio answers 422 with "Invoice does not exist". Check every invoice_uid exists before sending; expect 422, not 404, for unknown invoices.</description></item>
    ///   <item><description>The invoices in one multi-invoice payment have different currencies → Maxio answers 422 with "Invoices must be in the same currency". Group invoices by currency and send one payment per currency.</description></item>
    ///   <item><description>The total amount exceeds the invoices' due amounts and the invoices belong to different subscriptions → Maxio answers 422 with "Invoices belong to different subscriptions. Only invoices from same subscriptions are allowed when total amount exceeds invoices due amount.". Only overpay (create a prepayment) when all invoices belong to one subscription.</description></item>
    ///   <item><description>An application amount is more than that invoice's due amount → Maxio answers 422 with "Individual invoices may not be overpaid. To create a prepayment, add overage to the total amount.". Cap each application at the invoice's due amount and put any overage in the total amount instead.</description></item>
    ///   <item><description>An application names a consolidated invoice → Maxio answers 422 with "Paying consolidated invoices is forbidden". Do not include consolidated invoices in a multi-invoice payment.</description></item>
    ///   <item><description>An application amount is zero or less → Maxio answers 422 with "Invoice payment amounts must be greater than 0". Send every application amount greater than 0.</description></item>
    ///   <item><description>A subscription's prepayment account balance changes (prepayment added, or used to pay an invoice) → A subscription_prepayment_account_balance_changed webhook is sent. Listen for subscription_prepayment_account_balance_changed to keep prepayment balances in sync.</description></item>
    ///   <item><description>A payment is recorded without a method → The method defaults to other. Send the payment method explicitly so reports show how it was paid.</description></item>
    ///   <item><description>The total amount is greater than the sum of the application amounts → The overage becomes a prepayment. Expect any overage in the total amount to be kept as a prepayment on the subscription.</description></item>
    /// </list>
    /// </remarks>
    public Task<MultiInvoicePaymentResponse> RecordPaymentForMultipleInvoices(RecordPaymentForMultipleInvoicesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/payments.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<MultiInvoicePaymentResponse>(),
            RecordPaymentForMultipleInvoicesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Record Payment For Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="RecordPaymentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RecordPaymentForSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Records an external payment made against a subscription that will pay partially or in full one or more invoices.
    /// <para>
    /// Payment will be applied starting with the oldest open invoice and then next oldest, and so on until the amount of the payment is fully consumed.
    /// </para>
    /// <para>
    /// Excess payment will result in the creation of a prepayment on the Invoice Account.
    /// </para>
    /// <para>
    /// Only ungrouped or primary subscriptions may be paid using the "bulk" payment request.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A payment is recorded for a subscription in a group that is not the group's primary subscription → Maxio answers 422 with "If in a group, the Subscription must be the primary". Record group payments against the group's primary subscription.</description></item>
    ///   <item><description>The payment amount is zero or less → Maxio answers 422 with "Payment amount must be greater than zero". Send a payment amount greater than zero.</description></item>
    ///   <item><description>A subscription's prepayment account balance changes (prepayment added, or used to pay an invoice) → A subscription_prepayment_account_balance_changed webhook is sent. Listen for subscription_prepayment_account_balance_changed to keep prepayment balances in sync.</description></item>
    ///   <item><description>A payment is recorded against a subscription → It is applied to the oldest open invoice first, then the next oldest, until the amount is used up. Do not expect to choose which invoices a subscription payment pays; use the invoice payment endpoints for that.</description></item>
    ///   <item><description>A subscription payment is larger than the open invoices it pays → The excess becomes a prepayment on the invoice account. Expect any excess to show up as a prepayment in the response and on the subscription.</description></item>
    /// </list>
    /// </remarks>
    public Task<RecordPaymentResponse> RecordPaymentForSubscription(RecordPaymentForSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/payments.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<RecordPaymentResponse>(),
            RecordPaymentForSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Refund Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RefundInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Refunds an invoice, segment, or consolidated invoice.
    /// <para>
    /// ## Partial Refund for Consolidated Invoice
    /// </para>
    /// <para>
    /// A refund less than the total of a consolidated invoice will be split across its segments.
    /// </para>
    /// <para>
    /// For a $50.00 refund on a $100.00 consolidated invoice with one $60.00 segment and one $40.00 segment, the refunded amount will be applied as 50% of each ($30.00 and $20.00, respectively).
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The refund amount is zero or negative → Maxio answers 422 with "Amount: must be greater than 0.". Send a refund amount greater than 0.</description></item>
    ///   <item><description>The refund amount is more than the remaining refundable balance of the payment → the outcome is not stated. Keep each refund at or below what is left refundable on that payment.</description></item>
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
    ///   <item><description>A refund smaller than the total of a consolidated invoice is requested → The refund is split across the segments in proportion to each segment's share of the total. Expect partial consolidated refunds to be spread proportionally over all segments, not taken from one segment.</description></item>
    ///   <item><description>A refund is sent with external: true (it defaults to false) → The refund is recorded but no money is returned to the customer. Send external: true only for refunds already paid outside Maxio; leave it false to return money through the gateway.</description></item>
    ///   <item><description>A refund is sent with apply_credit: true (it defaults to false) → A credit is created and applied to the invoice. Set apply_credit only when the refunded amount should come back to the invoice as credit.</description></item>
    ///   <item><description>A full-amount refund is sent with void_invoice: true and apply_credit false → The invoice is voided after the refund; in other cases void_invoice has no effect. Expect the invoice to become voided only for a full refund without apply_credit when void_invoice is true.</description></item>
    ///   <item><description>A consolidated-invoice refund is sent without an amount → The refund defaults to the full amount of the payment. Always send an amount on consolidated refunds unless the whole payment should be refunded.</description></item>
    ///   <item><description>A consolidated-invoice refund is sent with segment_uids set to the string 'all' → All segments of the consolidated invoice are refunded. Send 'all' only when every segment should be refunded; otherwise list the segment uids.</description></item>
    /// </list>
    /// </remarks>
    public Task<Invoice> RefundInvoice(RefundInvoiceOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/{uid}/refunds.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<Invoice>(),
            RefundInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Reopen Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReopenInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Reopens any invoice with the "canceled" status. Invoices enter "canceled" status if they were open at the time the subscription was canceled (whether through dunning or an intentional cancellation).
    /// <para>
    /// Invoices with "canceled" status are no longer considered to be due. Once reopened, they are considered due for payment. Payment may then be captured in one of the following ways:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Reactivating the subscription, which will capture all open invoices (See note below about automatic reopening of invoices.)</description></item>
    ///   <item><description>Recording a payment directly against the invoice</description></item>
    /// </list>
    /// <para>
    /// A note about reactivations: any canceled invoices from the most recent active period are automatically opened as a part of the reactivation process. Reactivating via this endpoint prior to reactivation is only necessary when you wish to capture older invoices from previous periods during the reactivation.
    /// </para>
    /// <para>
    /// ### Reopening Consolidated Invoices
    /// </para>
    /// <para>
    /// When reopening a consolidated invoice, all of its canceled segments will also be reopened.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An invoice that is not in canceled status is reopened → Maxio answers 422 with "The invoice must be in canceled status to be reopened.". Reopen only invoices whose status is canceled.</description></item>
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
    ///   <item><description>A canceled invoice is reopened → The invoice becomes due for payment again. Collect payment on a reopened invoice by reactivating the subscription or recording a payment.</description></item>
    ///   <item><description>A consolidated invoice is reopened → All its canceled segments are reopened too. Expect the segments of a reopened consolidated invoice to become due as well.</description></item>
    /// </list>
    /// </remarks>
    public Task<Invoice> ReopenInvoice(ReopenInvoiceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/{uid}/reopen.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<Invoice>(),
            ReopenInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Send Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="SendInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Sends an invoice to the customer via email. This endpoint supports the delivery of both ad-hoc and automatically generated invoices. Additionally, this endpoint supports email delivery to direct recipients, carbon-copy (cc) recipients, and blind carbon-copy (bcc) recipients.
    /// <para>
    /// <b>File Attachments</b>: You can attach files to invoice emails using <c>attachment_urls[]</c> parameter by providing URLs to the files you want to attach. When using attachments, the request must use <c>multipart/form-data</c> content type. Max 10 files, 10MB per file.
    /// </para>
    /// <para>
    /// If no recipient email addresses are specified in the request, then the subscription's default email configuration will be used. For example, if <c>recipient_emails</c> is left blank, then the invoice will be delivered to the subscription's customer email address.
    /// </para>
    /// <para>
    /// On success, a 204 no-content response will be returned. The response does not indicate that email(s) have been delivered, but instead indicates that emails have been successfully queued for delivery. If _any_ invalid or malformed email address is found in the request body, the entire request will be rejected and a 422 response will be returned.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An invoice email is sent with attachment_urls → the outcome is not stated. Send requests with attachment_urls as multipart/form-data.</description></item>
    ///   <item><description>An attachment file is larger than 10MB → the outcome is not stated. Attach at most 10 files of 10MB or less each.</description></item>
    ///   <item><description>Any recipient, cc or bcc email address in the request is invalid or malformed → Maxio answers 422 with "cc_recipient_emails: must be a valid email address". Validate every email address before sending; one bad address rejects the whole request.</description></item>
    ///   <item><description>No recipient email addresses are sent → The subscription's default email configuration is used, e.g. the customer's email address. Send recipient_emails when the invoice must go somewhere other than the subscription's defaults.</description></item>
    ///   <item><description>An invoice email request succeeds → Maxio answers 204. Do not report the email as delivered on a 204; it was only queued.</description></item>
    /// </list>
    /// </remarks>
    public Task SendInvoice(SendInvoiceOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/{uid}/deliveries.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            VoidResponse.Instance,
            SendInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Customer Information
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateCustomerInformationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates customer information on an open invoice and returns the updated invoice. If you would like to preview changes that will be applied, use the <c>/invoices/{uid}/customer_information/preview.json</c> endpoint first.
    /// <para>
    /// The endpoint doesn't accept a request body. Customer information differences are calculated on the application side.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The invoice is not in open status → Maxio answers 422 with "Invoice must have an open status". Preview or update customer information only on open invoices.</description></item>
    ///   <item><description>Customer information is previewed or updated on an invoice → No request body is taken; the differences between the invoice and the current customer record are calculated by Maxio. Update the customer record first, then call these endpoints without a body.</description></item>
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
    public Task<Invoice> UpdateCustomerInformation(UpdateCustomerInformationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/{uid}/customer_information.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            JsonResponse.Create<Invoice>(),
            UpdateCustomerInformationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Draft Ad Hoc Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="InvoiceResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates an ad hoc invoice while it is in the <c>draft</c> state.
    /// <para>
    /// <b>Important: only invoices with the <c>adhoc</c> role and <c>draft</c> status can be updated.</b> Any other invoice — issued, or with a different role (e.g. <c>renewal</c>, <c>signup</c>) — cannot be updated through this endpoint and the request returns a <c>422</c> error. If the invoice does not belong to the provided subscription, a <c>404</c> error is returned.
    /// </para>
    /// <para>
    /// Only the attributes submitted in the request are changed — omitted attributes keep their current values.
    /// </para>
    /// <para>
    /// ### Line Items
    /// </para>
    /// <para>
    /// The <c>line_items</c> array describes changes to the invoice's line items. Line items not referenced in the array remain unchanged.
    /// </para>
    /// <para>
    /// #### Adding a line item
    /// </para>
    /// <para>
    /// A line item without a <c>uid</c> is added to the invoice. The same line item types and options as on invoice creation are supported (custom items, <c>product_id</c>, <c>component_id</c>, price points, period date ranges, taxes).
    /// </para>
    /// <para>
    /// #### Updating a line item
    /// </para>
    /// <para>
    /// A line item with the <c>uid</c> of an existing line item updates that line item with the submitted attributes. Amounts and taxes are recalculated.
    /// </para>
    /// <para>
    /// #### Removing a line item
    /// </para>
    /// <para>
    /// A line item with a <c>uid</c> and <c>"_destroy": true</c> is removed from the invoice. Other line items remain unchanged.
    /// </para>
    /// <para>
    /// Referencing a <c>uid</c> which does not exist on the invoice returns a <c>422</c> error.
    /// </para>
    /// <para>
    /// ### Coupons
    /// </para>
    /// <para>
    /// When the <c>coupons</c> key is present, the submitted coupons replace all discounts currently applied to the invoice. Send an empty array to remove all discounts. Coupon options are the same as on invoice creation.
    /// </para>
    /// <para>
    /// ### Invoice Options
    /// </para>
    /// <para>
    /// #### Issue Date and Net Terms
    /// </para>
    /// <para>
    /// The <c>issue_date</c> parameter can be sent to change the invoice's issue date. Only today or dates in the past are accepted. The date is interpreted and validated in your site's time zone, using the <c>YYYY-MM-DD</c> format. The <c>net_terms</c> parameter indicates the number of days after the issue date on which the invoice is due. The due date is recalculated whenever the issue date or net terms change.
    /// </para>
    /// <para>
    /// #### Addresses
    /// </para>
    /// <para>
    /// The seller, shipping and billing addresses can be sent to replace the addresses on the invoice. Each address requires to send a <c>first_name</c> at a minimum in order to work. Taxes are recalculated after an address change.
    /// </para>
    /// <para>
    /// #### Memo and Payment Instructions
    /// </para>
    /// <para>
    /// A custom memo can be sent with the <c>memo</c> parameter. Likewise, custom payment instructions can be sent with the <c>payment_instructions</c> parameter.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A custom line item (no product_id or component_id) lacks a title, a quantity or a unit price → the outcome is not stated. Send title, quantity and unit_price on every custom line item.</description></item>
    ///   <item><description>A line item's period_range_end is before its period_range_start → the outcome is not stated. Make period_range_end the same as or later than period_range_start.</description></item>
    ///   <item><description>A taxable line item is sent without tax_code on a site that uses Avalara (AvaTax) → the outcome is not stated. Send a tax_code with every taxable line item when the site uses AvaTax.</description></item>
    ///   <item><description>A tax_code longer than 25 characters is sent → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>A coupon sends both code and subcode → the outcome is not stated. Send either code or subcode on each coupon, not both.</description></item>
    ///   <item><description>A coupon code contains characters other than uppercase letters, numbers and the allowed special characters → the outcome is not stated. Use only letters, numbers and allowed special characters in ad hoc coupon codes.</description></item>
    ///   <item><description>A line item sends product_id together with title or component_id → the outcome is not stated. Leave out title and component_id on product line items.</description></item>
    ///   <item><description>A coupon percentage has more than 4 decimal places or is outside 0 to 100 → the outcome is not stated. Send coupon percentages between 0 and 100 with at most 4 decimal places.</description></item>
    ///   <item><description>A coupon sends both percentage and amount → the outcome is not stated. Send either percentage or amount on an ad hoc coupon, not both.</description></item>
    ///   <item><description>issue_date is a future date in the site's time zone → the outcome is not stated. Send issue_date as today or an earlier date, judged in the site's time zone.</description></item>
    ///   <item><description>A line item sends component_id together with title or product_id → the outcome is not stated. Leave out title and product_id on component line items.</description></item>
    ///   <item><description>A seller, billing or shipping address is sent without first_name → Maxio answers 422 with "Billing address must include name.". Include first_name in every address object sent on an invoice.</description></item>
    ///   <item><description>A price point is sent for a component that is not quantity-based, on/off or metered → the outcome is not stated. Send price_point_id only for quantity-based, on/off and metered components.</description></item>
    ///   <item><description>A line item for a prepaid or event-based billing component is sent without unit_price → the outcome is not stated. Always send unit_price for prepaid and event-based components.</description></item>
    ///   <item><description>A line item sends a price point (price_point_id or product_price_point_id) together with unit_price → the outcome is not stated. Send either a price point or unit_price on a line item, never both.</description></item>
    ///   <item><description>A price point is sent on a custom (non-catalog) line item → the outcome is not stated. Use price points only on product or component line items.</description></item>
    ///   <item><description>An invoice that is not an ad hoc invoice in draft status is updated → Maxio answers 422. Update only draft ad hoc invoices; change issued invoices by voiding and recreating them.</description></item>
    ///   <item><description>The invoice does not belong to the subscription in the path → Maxio answers 404 with "Invoice not found". Send the subscription_id that owns the invoice.</description></item>
    ///   <item><description>A line item uid that does not exist on the invoice is sent → Maxio answers 422 with "Line item not found.". Use line item uids read from the same invoice.</description></item>
    ///   <item><description>A quantity or unit_price has more than 8 decimal places → The value is rounded down to the 8th decimal place. Round quantity and unit_price to 8 decimal places before sending.</description></item>
    ///   <item><description>A line item is sent with taxable: true on a site not configured to calculate taxes → the outcome is not stated. Send taxable: true only on sites that have tax calculation configured.</description></item>
    ///   <item><description>taxable is sent on a product or component (catalog) line item → The catalog item's own tax settings apply; they cannot be overwritten. Set taxability on the product or component in the catalog, not on the invoice line.</description></item>
    ///   <item><description>A line item is sent with a description → The given description replaces the line item's generated description. Leave description out to keep the generated description (which can carry period details).</description></item>
    ///   <item><description>A coupon code contains lowercase letters → Lowercase letters are converted to uppercase. Expect coupon codes to come back in uppercase.</description></item>
    ///   <item><description>A coupon is applied by subcode → The subcode is matched case-insensitively, stored in uppercase, and the response shows both the main code and the subcode. Match coupons in responses by both code and subcode.</description></item>
    ///   <item><description>A coupon's code matches an existing catalog coupon and percentage, amount or description is also sent → percentage, amount and description are ignored; the catalog coupon's settings apply. Send percentage, amount or description only for ad hoc coupons whose code is not in the catalog.</description></item>
    ///   <item><description>A coupon code is not unique across product families on the site → product_family_id is used to pick the matching existing coupon. Send product_family_id with the coupon code when codes repeat across product families.</description></item>
    ///   <item><description>A percentage coupon is sent without compounding_strategy → compound is used: the percentage applies to the price remaining after earlier discounts. Send compounding_strategy: full-price when the percentage should apply to the original price.</description></item>
    ///   <item><description>An amount (flat) coupon is applied → The discount is always calculated against the original item price, before other discounts. Do not expect compounding_strategy to change how amount coupons are calculated.</description></item>
    ///   <item><description>compounding_strategy is sent for a coupon that is not stackable → The setting has no effect. Rely on compounding_strategy only for stackable coupons.</description></item>
    ///   <item><description>A product line item is sent without unit_price or product_price_point_id → unit_price is taken from the product's default price point. Send unit_price or product_price_point_id when the product's default price should not apply.</description></item>
    ///   <item><description>A component line item is sent without unit_price or price_point_id → unit_price is taken from the default price point. Send unit_price or price_point_id when the default component price should not apply.</description></item>
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
    ///   <item><description>issue_date or net_terms changes on a draft invoice → The due date is recalculated as issue date plus net terms days. Read the new due_date from the response after changing issue_date or net_terms.</description></item>
    ///   <item><description>An address is changed on a draft invoice → Taxes are recalculated. Re-read tax and total amounts after changing an invoice address.</description></item>
    ///   <item><description>A draft invoice is updated with only some attributes → Only the submitted attributes change; omitted attributes keep their values. Send only the fields to change; there is no need to resend the whole invoice.</description></item>
    ///   <item><description>line_items is sent on an update → Existing line items not referenced in the array stay unchanged. Remove a line item explicitly with uid and _destroy; leaving it out of the array keeps it.</description></item>
    ///   <item><description>A line item without a uid is sent on an update → A new line item is added to the invoice. Send the uid of an existing line item to change it; without uid a new line is added.</description></item>
    ///   <item><description>A line item with the uid of an existing line item is sent on an update → The line item is updated with the submitted attributes and the invoice's amounts and taxes are recalculated. Re-read the returned totals after changing a line item.</description></item>
    ///   <item><description>A line item with a uid and _destroy: true is sent on an update → That line item is removed; other line items stay unchanged. Send _destroy: true together with the line item's uid to remove it.</description></item>
    ///   <item><description>The coupons key is present on an update → The submitted coupons replace all discounts on the invoice; an empty array removes all discounts. Send the full set of coupons wanted on the invoice, or omit the coupons key to keep the current discounts.</description></item>
    /// </list>
    /// </remarks>
    public Task<InvoiceResponse> UpdateInvoice(UpdateInvoiceOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/invoices/{uid}.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId), new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<InvoiceResponse>(),
            UpdateInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Void Invoice
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="VoidInvoiceError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Voids any invoice with the "open" or "canceled" status.  It will also allow voiding of an invoice with the "pending" status if it is not a consolidated invoice.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An invoice whose status is not open, canceled or pending is voided → Maxio answers 422 with "Invoice status must be 'open', 'canceled', or 'pending' and non-consolidated to be voided.". Void only open, canceled or pending invoices.</description></item>
    ///   <item><description>A pending invoice that is a consolidated invoice is voided → Maxio answers 422 with "Invoice status must be 'open', 'canceled', or 'pending' and non-consolidated to be voided.". Do not void pending consolidated invoices.</description></item>
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
    public Task<Invoice> VoidInvoice(VoidInvoiceOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/invoices/{uid}/void.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<Invoice>(),
            VoidInvoiceError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
