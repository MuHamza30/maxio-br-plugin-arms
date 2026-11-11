using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.Subscriptions;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Subscriptions connect a customer to a product and drive all recurring billing for that relationship, including invoice generation, billing start dates, and pricing structure across plan-based and planless configurations. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/overview/understand-subscriptions">Understand Subscriptions</see>.
/// </summary>
public sealed class Subscriptions
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Subscriptions(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Activate Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ActivateSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Activates awaiting signup and trialing subscriptions. This feature is only available on the Relationship Invoicing architecture. Subscriptions in a group cannot be activated immediately.
    /// <para>
    /// The <c>revert_on_failure</c> parameter controls the behavior upon activation failure.
    /// - If set to <c>true</c> and something goes wrong i.e. payment fails, the subscription's state does not change. The subscription’s billing period also remains the same.
    /// - If set to <c>false</c> and something goes wrong i.e. payment fails, the activation continues and enters an end of life state. For trialing subscriptions, that is either trial ended (if the trial is no obligation), past due (if the trial has an obligation), or canceled (if the site has no dunning strategy, or has a strategy that says to cancel immediately). For awaiting signup subscriptions, that is always canceled.
    /// </para>
    /// <para>
    /// The default activation failure behavior can be configured per activation attempt, or you can set a default value under Config &gt; Settings &gt; Subscription Activation Settings.
    /// </para>
    /// <para>
    /// ## Activation Scenarios
    /// </para>
    /// <para>
    /// ### Activate Awaiting Signup subscription
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Given you have a product without trial</description></item>
    ///   <item><description>Given you have a site without dunning strategy</description></item>
    /// </list>
    /// <code>
    ///   flowchart LR
    ///     AS[Awaiting Signup] --&gt; A{Activate}
    ///     A --&gt;|Success| Active
    ///     A --&gt;|Failure| ROF{revert_on_failure}
    ///     ROF --&gt;|true| AS
    ///     ROF --&gt;|false| Canceled
    /// </code>
    /// <list type="bullet">
    ///   <item><description>Given you have a product with trial</description></item>
    ///   <item><description>Given you have a site with dunning strategy</description></item>
    /// </list>
    /// <code>
    ///   flowchart LR
    ///     AS[Awaiting Signup] --&gt; A{Activate}
    ///     A --&gt;|Success| Trialing
    ///     A --&gt;|Failure| ROF{revert_on_failure}
    ///     ROF --&gt;|true| AS
    ///     ROF --&gt;|false| PD[Past Due]
    /// </code>
    /// <para>
    /// ### Activate Trialing subscription
    /// </para>
    /// <para>
    /// For more information about the behavior of trialing subscriptions, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24252155721869-Trialing-Subscriptions">Trialing Subscriptions</see>.
    /// When the <c>revert_on_failure</c> parameter is set to <c>true</c>, the subscription's state remains Trialing; the invoice from activation is voided, and any prepayments and credits applied to the invoice are returned to the subscription.
    /// </para>
    /// </remarks>
    public Task<SubscriptionResponse> ActivateSubscription(ActivateSubscriptionOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/activate.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionResponse>(),
            ActivateSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Apply Coupons to Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ApplyCouponsToSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Applies one or more coupon codes to an existing subscription.
    /// <para>
    /// An existing subscription can accommodate multiple discounts/coupon codes. This is only applicable if each coupon is stackable. For more information on stackable coupons, we recommend reviewing our <see href="https://maxio.zendesk.com/hc/en-us/articles/24261259337101-Coupons-and-Subscriptions#stackability-rules">coupon documentation.</see>
    /// </para>
    /// <para>
    /// ## Query Parameters vs Request Body Parameters
    /// </para>
    /// <para>
    /// Passing in a coupon code as a query parameter will add the code to the subscription, completely replacing all existing coupon codes on the subscription.
    /// </para>
    /// <para>
    /// For this reason, using this query parameter on this endpoint has been deprecated in favor of using the request body parameters as described below. When passing in request body parameters, the list of coupon codes will simply be added to any existing list of codes on the subscription.
    /// </para>
    /// </remarks>
    public Task<SubscriptionResponse> ApplyCouponsToSubscription(ApplyCouponsToSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/add_coupon.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [new Param("code", request.Code)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionResponse>(),
            ApplyCouponsToSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a Subscription for a customer and product.
    /// <para>
    /// Specify the product with <c>product_id</c> or <c>product_handle</c>. To set a specific product price point, use <c>product_price_point_handle</c> or <c>product_price_point_id</c>.
    /// </para>
    /// <para>
    /// Identify an existing customer with <c>customer_id</c> or <c>customer_reference</c>. Optionally, include an existing payment profile using <c>payment_profile_id</c>. To create a new customer, pass customer_attributes.
    /// </para>
    /// <para>
    /// Select an option from the <b>Request Examples</b> drop-down on the right side of the portal to see examples of common scenarios for creating subscriptions.
    /// </para>
    /// <para>
    /// ## List vs Sales Pricing
    /// </para>
    /// <para>
    /// When a subscription uses custom pricing as the sales price, you can optionally provide a list price for any item. If omitted, the list price defaults to the sales price. The difference between the list price and sales price is used to calculate implicit discounts, which appear on Invoices and in reporting. List price can also support revenue allocations in <see href="https://docs.maxio.com/hc/en-us/articles/24177001342861-Create-and-Configure-RevenueBooks">Advanced Revenue</see>.
    /// </para>
    /// <para>
    /// If your site has list pricing enabled, the API accepts <c>custom_price.list_price_point_id</c> for custom pricing, validates and persists it, and returns list price metadata in subscription responses. If list pricing is disabled, this input is ignored and related response fields are omitted.
    /// </para>
    /// <para>
    /// When list pricing is enabled:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Subscription → Product <c>product_price_point_list_price_point_id</c> (integer)</description></item>
    ///   <item><description><c>product_price_point_list_price_point_handle</c> (string)</description></item>
    ///   <item><description>Subscription Components (when components are included in the response, such as with subscriptions built from components or component serialization paths) <c>component_id</c> (integer)</description></item>
    ///   <item><description><c>price_point_id</c> (integer)</description></item>
    ///   <item><description><c>list_price_point_id</c> (integer)</description></item>
    /// </list>
    /// <para>
    /// When list pricing is disabled:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Subscription → Product <c>product_price_point_list_price_point_id</c>: omitted</description></item>
    ///   <item><description><c>product_price_point_list_price_point_handle</c>: omitted</description></item>
    ///   <item><description>Subscription Components <c>list_price_point_id</c>: omitted</description></item>
    /// </list>
    /// <para>
    /// This functionality is supported in the API, but is not currently supported in SDKs.
    /// </para>
    /// <para>
    /// ## Subscriptions can now work independently from the catalog
    /// </para>
    /// <para>
    ///  If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, you can create subscriptions without a <c>product_id</c> or <c>product_handle</c> using POST /subscriptions, building them entirely from components.
    /// </para>
    /// <para>
    /// A valid subscription must include at least one active component with:
    /// - a positive <c>allocated_quantity</c>,
    /// - a positive <c>unit_balance</c>, or
    /// - 'enabled: true' (for on/off components)
    /// - a configured metered component
    /// </para>
    /// <para>
    /// <c>component_id</c> can be provided as a numeric ID or in handle: format. If <c>trial_interval</c> and <c>trial_interval_unit</c> are included, they are applied at creation.
    /// </para>
    /// <para>
    /// In the response, product and product price point fields are null, and component details are returned instead.
    /// </para>
    /// <para>
    /// This functionality is supported in the API, but is not currently supported in SDKs.
    /// </para>
    /// <para>
    /// ## Payment information
    /// </para>
    /// <para>
    /// Payment information may be required to create a subscription, depending on the options for the Product being subscribed. See <see href="https://docs.maxio.com/hc/en-us/articles/24261076617869-Edit-Products">product options</see> for more information. See the <see href="$e/Payment%20Profiles/createPaymentProfile">Payments Profile</see> endpoint for details on payment parameters.
    /// See the <see href="page:introduction/basic-concepts/subscription-signup">Subscription Signups</see> article for more information on working with subscriptions in Advanced Billing.
    /// </para>
    /// <para>
    /// ## Payment information
    /// </para>
    /// <para>
    /// Payment information may be required to create a subscription, depending on the options for the Product being subscribed. See <see href="https://docs.maxio.com/hc/en-us/articles/24261076617869-Edit-Products">product options</see> for more information. See the <see href="$e/Payment%20Profiles/createPaymentProfile">Payments Profile</see> endpoint for details on payment parameters.
    /// </para>
    /// <para>
    /// Do not use real card information for testing. See the Sites articles that cover <see href="https://docs.maxio.com/hc/en-us/articles/24250712113165-Testing-Overview#testing-overview-0-0">testing your site setup</see> for more details on testing in your sandbox.
    /// </para>
    /// <para>
    /// Note that collecting and sending raw card details in production requires <see href="https://docs.maxio.com/hc/en-us/articles/24183956938381-PCI-Compliance#pci-compliance-0-0">PCI compliance</see> on your end. If your business is not PCI compliant, use <see href="https://docs.maxio.com/hc/en-us/articles/38163190843789-Chargify-js-Overview#chargify-js-overview-0-0">Maxio.js (formerly Chargify.js)</see> to collect credit card or bank account information.
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
    public Task<SubscriptionResponse> CreateSubscription(CreateSubscriptionOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionResponse>(),
            CreateSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Find Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="FindSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Finds a subscription by its reference.
    /// </remarks>
    public Task<SubscriptionResponse> FindSubscription(FindSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/lookup.json"),
            [],
            [new Param("reference", request.Reference)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<SubscriptionResponse>(),
            FindSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Subscriptions
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists subscriptions for a site. Use the query string filters and pagination to control responses from the server.
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, some subscriptions may not have an associated product. For subscriptions without an associated product, 'product', 'product_price_point_id', and 'product_price_point_type' are returned as 'null'.
    /// </para>
    /// <para>
    /// ## Search for a subscription
    /// </para>
    /// <para>
    /// Use the query strings below to search for a subscription using the criteria available. The return value will be an array.
    /// </para>
    /// <para>
    /// ## Self-Service Page token
    /// </para>
    /// <para>
    /// Self-Service Page token for the subscriptions is not returned by default. If this information is desired, the include[]=self_service_page_token parameter must be provided with the request.
    /// </para>
    /// </remarks>
    public Task<IReadOnlyList<SubscriptionResponse>> ListSubscriptions(ListSubscriptionsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions.json"),
            [],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("sort", request.Sort),
                new Param("direction", request.Direction),
                new Param("state", request.State),
                new Param("product", request.Product),
                new Param("q", request.Q),
                new Param("q_scope", request.QScope),
                new Param("customer_id", request.CustomerId),
                new Param("product_price_point_id", request.ProductPricePointId),
                new Param("coupon", request.Coupon),
                new Param("coupon_code", request.CouponCode),
                new Param("collection_method", request.CollectionMethod),
                new Param("branding_theme_id", request.BrandingThemeId),
                new Param("date_field", request.DateField),
                new Param("start_date", request.StartDate?.ToDate()),
                new Param("end_date", request.EndDate?.ToDate()),
                new Param("start_datetime", request.StartDatetime?.ToIso8601()),
                new Param("end_datetime", request.EndDatetime?.ToIso8601()),
                new Param("metadata", request.Metadata),
                new Param("group_status", request.GroupStatus),
                new Param("dunning_exemption", request.DunningExemption),
                new Param("payment_gateways", request.PaymentGateways),
                new Param("currencies", request.Currencies),
                new Param("include", request.Include),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<SubscriptionResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Override Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="OverrideSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Sets certain subscription fields that are usually managed automatically. Some of the fields can be set via the normal Subscriptions Update API, but others can only be set using this endpoint.
    /// <para>
    /// This endpoint is provided for cases where you need to “align” Advanced Billing data with data that happened in your system, perhaps before you started using Advanced Billing. For example, you may choose to import your historical subscription data, and would like the activation and cancellation dates in Advanced Billing to match your existing historical dates. Advanced Billing does not backfill historical events (i.e. from the Events API), but some static data can be changed via this API.
    /// </para>
    /// <para>
    /// Why are some fields only settable from this endpoint, and not the normal subscription create and update endpoints? Because we want users of this endpoint to be aware that these fields are usually managed by Advanced Billing, and using this API means <b>you are stepping out on your own.</b>
    /// </para>
    /// <para>
    /// Changing these fields will not affect any other attributes. For example, adding an expiration date will not affect the next assessment date on the subscription.
    /// </para>
    /// <para>
    /// If you regularly need to override the current_period_starts_at for new subscriptions, this can also be accomplished by setting both <c>previous_billing_at</c> and <c>next_billing_at</c> at subscription creation. See the documentation on <see href="./b3A6MTQxMDgzODg-create-subscription#subscriptions-import">Importing Subscriptions</see> for more information.
    /// </para>
    /// <para>
    /// ## Limitations
    /// </para>
    /// <para>
    /// When passing <c>current_period_starts_at</c> some validations are made:
    /// </para>
    /// <list type="number">
    ///   <item><description>The subscription needs to be unbilled (no statements or invoices).</description></item>
    ///   <item><description>The value passed must be a valid date/time. We recommend using the iso 8601 format.</description></item>
    ///   <item><description>The value passed must be before the current date/time.</description></item>
    /// </list>
    /// <para>
    /// If unpermitted parameters are sent, a 400 HTTP response is sent along with a string giving the reason for the problem.
    /// </para>
    /// </remarks>
    public Task OverrideSubscription(OverrideSubscriptionOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/override.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            VoidResponse.Instance,
            OverrideSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Preview Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionPreviewResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Previews a subscription by POSTing the same JSON or XML as for a subscription creation.
    /// <para>
    /// The "Next Billing" amount and "Next Billing" date are represented in each Subscriber's Summary.
    /// </para>
    /// <para>
    /// This endpoint does not create a subscription; it is meant to serve as a prediction.
    /// </para>
    /// <para>
    /// For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24252493695757-Subscriber-Interface-Overview">Subscriber Interface Overview</see>.
    /// </para>
    /// <para>
    /// ## Subscriptions can now work independently from the catalog
    /// </para>
    /// <para>
    ///  If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, you can create subscriptions without a <c>product_id</c> or <c>product_handle</c> using POST /subscriptions, building them entirely from components.
    /// </para>
    /// <para>
    /// A valid subscription must include at least one active component with:
    /// - a positive <c>allocated_quantity</c>,
    /// - a positive <c>unit_balance</c>, or
    /// - 'enabled: true' (for on/off components)
    /// </para>
    /// <para>
    /// <c>component_id</c> can be provided as a numeric ID or in handle: format. If <c>trial_interval</c> and <c>trial_interval_unit</c> are included, they are applied at creation.
    /// </para>
    /// <para>
    /// In the response, product and product price point fields are null, and component details are returned instead.
    /// </para>
    /// <para>
    /// This functionality is supported in the API, but is not currently supported in SDKs.
    /// </para>
    /// <para>
    /// ## Taxable Subscriptions
    /// </para>
    /// <para>
    /// This endpoint previews taxes applicable to a purchase. For taxes to be previewed, the following conditions must be met:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Taxes must be configured on the subscription</description></item>
    ///   <item><description>The preview must be for the purchase of a taxable product or component, or combination of the two.</description></item>
    ///   <item><description>The subscription payload must contain a full billing or shipping address to calculate tax</description></item>
    /// </list>
    /// <para>
    /// For more information about creating taxable previews, see <see href="https://maxio.zendesk.com/hc/en-us/sections/24287012349325-Taxes">Taxes</see>.
    /// </para>
    /// <para>
    /// You do <b>not</b> need to include a card number to generate tax information when you are previewing a subscription. However, when you actually want to create the subscription, you must include the credit card information if you want the billing address to be stored. The billing address and the credit card information are stored together within the payment profile object. Also, you cannot send a billing address without payment profile information, as the address is stored on the card.
    /// </para>
    /// <para>
    /// You can pass shipping and billing addresses and still decide not to calculate taxes. To do that, pass <c>skip_billing_manifest_taxes: true</c> attribute.
    /// </para>
    /// <para>
    /// ## Non-taxable Subscriptions
    /// </para>
    /// <para>
    /// If you'd like to calculate subscriptions that do not include tax, you can leave off the billing information.
    /// </para>
    /// </remarks>
    public Task<SubscriptionPreviewResponse> PreviewSubscription(PreviewSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/preview.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionPreviewResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Purge Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="PurgeSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Purges an individual subscription for sites in test mode.
    /// <para>
    /// Provide the subscription ID in the URL.  To confirm, supply the customer ID in the query string <c>ack</c> parameter. You may also delete the customer record and/or payment profiles by passing <c>cascade</c> parameters. For example, to delete just the customer record, the query params would be: <c>?ack={customer_id}&amp;cascade[]=customer</c>
    /// </para>
    /// <para>
    /// If you need to remove subscriptions from a live site, contact support to discuss your use case.
    /// </para>
    /// <para>
    /// ### Delete customer and payment profile
    /// </para>
    /// <para>
    /// The query params will be: <c>?ack={customer_id}&amp;cascade[]=customer&amp;cascade[]=payment_profile</c>
    /// </para>
    /// </remarks>
    public Task<SubscriptionResponse> PurgeSubscription(PurgeSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/purge.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [new Param("ack", request.Ack), new Param("cascade", request.Cascade)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<SubscriptionResponse>(),
            PurgeSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves subscription details.
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, some subscriptions may not have an associated product. For subscriptions without an associated product, 'product', 'product_price_point_id', and 'product_price_point_type' are returned as 'null'.
    /// </para>
    /// <para>
    /// ## Self-Service Page token
    /// </para>
    /// <para>
    /// Self-Service Page token for the subscription is not returned by default. If this information is desired, the include[]=self_service_page_token parameter must be provided with the request.
    /// </para>
    /// </remarks>
    public Task<SubscriptionResponse> ReadSubscription(ReadSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [new Param("include", request.Include)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<SubscriptionResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Remove Coupon from Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="string"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RemoveCouponFromSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Removes a coupon from an existing subscription.
    /// <para>
    /// For more information on the expected behavior of removing a coupon from a subscription, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24261259337101-Coupons-and-Subscriptions#removing-a-coupon">Coupons and Subscriptions</see>.
    /// </para>
    /// </remarks>
    public Task<string> RemoveCouponFromSubscription(RemoveCouponFromSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/remove_coupon.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [new Param("coupon_code", request.CouponCode)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<string>(),
            RemoveCouponFromSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Prepaid Subscription Configuration
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PrepaidConfigurationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdatePrepaidSubscriptionConfigurationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates a subscription's prepaid configuration.
    /// </remarks>
    public Task<PrepaidConfigurationResponse> UpdatePrepaidSubscriptionConfiguration(UpdatePrepaidSubscriptionConfigurationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/prepaid_configurations.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<PrepaidConfigurationResponse>(),
            UpdatePrepaidSubscriptionConfigurationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates one or more attributes of a subscription.
    /// <para>
    /// ## Update Subscription Payment Method
    /// </para>
    /// <para>
    /// Change the card that your subscriber uses for their subscription. You can also use this method to change the expiration date of the card <b>if your gateway allows</b>.
    /// </para>
    /// <para>
    /// Do not use real card information for testing. See the Sites articles that cover <see href="https://docs.maxio.com/hc/en-us/articles/24250712113165-Testing-Overview#testing-overview-0-0">testing your site setup</see> for more details on testing in your sandbox.
    /// </para>
    /// <para>
    /// Note that collecting and sending raw card details in production requires <see href="https://docs.maxio.com/hc/en-us/articles/24183956938381-PCI-Compliance#pci-compliance-0-0">PCI compliance</see> on your end. If your business is not PCI compliant, use <see href="https://docs.maxio.com/hc/en-us/articles/38163190843789-Chargify-js-Overview#chargify-js-overview-0-0">Chargify.js</see> to collect credit card or bank account information.
    /// </para>
    /// <para>
    /// &gt; Note: Partial card updates for <b>Authorize.Net</b> are not allowed via this endpoint. The existing Payment Profile must be directly updated instead.
    /// </para>
    /// <para>
    /// ## Update Product
    /// </para>
    /// <para>
    /// You also use this method to change the subscription to a different product by setting a new value for product_handle. A product change can be done in two different ways, <b>product change</b> or <b>delayed product change</b>.
    /// </para>
    /// <para>
    /// ### Product Change
    /// </para>
    /// <para>
    /// You can change a subscription's product. The new payment amount is calculated and charged at the normal start of the next period. If you require complex product changes or prorated upgrades and downgrades instead, please see the documentation on <see href="https://docs.maxio.com/hc/en-us/articles/24252069837581-Product-Changes-and-Migrations#product-changes-and-migrations-0-0">Migrating Subscription Products</see>.
    /// </para>
    /// <para>
    /// To perform a product change, set either the <c>product_handle</c> or <c>product_id</c> attribute to that of a different product from the same site as the subscription. You can also change the price point by passing in either <c>product_price_point_id</c> or <c>product_price_point_handle</c> - otherwise the new product's default price point is used.
    /// </para>
    /// <para>
    /// ### Delayed Product Change
    /// </para>
    /// <para>
    /// This method also changes the product and/or price point, and the new payment amount is calculated and charged at the normal start of the next period.
    /// </para>
    /// <para>
    /// This method schedules the product change to happen automatically at the subscription’s next renewal date. To perform a delayed product change, set the <c>product_handle</c> attribute as you would in a regular product change, but also set the <c>product_change_delayed</c> attribute to <c>true</c>. No proration applies in this case.
    /// </para>
    /// <para>
    /// You can also perform a delayed change to the price point by passing in either <c>product_price_point_id</c> or <c>product_price_point_handle</c>
    /// </para>
    /// <para>
    /// &gt; <b>Note:</b> To cancel a delayed product change, set <c>next_product_id</c> to an empty string.
    /// </para>
    /// <para>
    /// ## Billing Date Changes
    /// </para>
    /// <para>
    /// You can update dates for a subscription.
    /// </para>
    /// <para>
    /// ### Regular Billing Date Changes
    /// </para>
    /// <para>
    /// Send the <c>next_billing_at</c> to set the next billing date for the subscription. After that date passes and the subscription is processed, the following billing date will be set according to the subscription's product period.
    /// </para>
    /// <para>
    /// &gt; Note: If you pass an invalid date, the correct date is automatically set to the correct date. For example, if February 30 is passed, the next billing would be set to March 2nd in a non-leap year.
    /// </para>
    /// <para>
    /// The server response will not return data under the key/value pair of <c>next_billing_at</c>. View the key/value pair of <c>current_period_ends_at</c> to verify that the <c>next_billing_at</c> date has been changed successfully.
    /// </para>
    /// <para>
    /// ### Calendar Billing and Snap Day Changes
    /// </para>
    /// <para>
    /// For a subscription using Calendar Billing, setting the next billing date is a bit different. Send the <c>snap_day</c> attribute to change the calendar billing date for <b>a subscription using a product eligible for calendar billing</b>.
    /// </para>
    /// <para>
    /// &gt; Note: If you change the product associated with a subscription that contains a <c>snap_day</c> and immediately READ/GET the subscription data, it will still contain the original <c>snap_day</c>. The <c>snap_day</c> will be reset to <c>null</c> on the next billing cycle. This is because a product change is instantaneous and only affects the product associated with a subscription.
    /// </para>
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, some subscriptions may not have an associated product. For subscriptions without an associated product, <c>product</c>, <c>product_price_point_id</c>, and <c>product_price_point_type</c> are returned as <c>null</c>.
    /// </para>
    /// </remarks>
    public Task<SubscriptionResponse> UpdateSubscription(UpdateSubscriptionOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionResponse>(),
            UpdateSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
