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
                new Param("event_types", request.EventTypes, SerializationFormat.Csv),
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
                new Param("customer_ids", request.CustomerIds, SerializationFormat.Csv),
                new Param("number", request.Number, SerializationFormat.Csv),
                new Param("product_ids", request.ProductIds, SerializationFormat.Csv),
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
