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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription is neither awaiting signup nor trialing → the outcome is not stated. Activate only awaiting-signup or trialing subscriptions.</description></item>
    ///   <item><description>The site is not on the Relationship Invoicing architecture → the outcome is not stated. Use activation only on Relationship Invoicing sites.</description></item>
    ///   <item><description>The subscription belongs to a subscription group → It cannot be activated immediately. Do not immediately activate grouped subscriptions.</description></item>
    ///   <item><description>Activation fails (for example the payment fails) with revert_on_failure true → Maxio answers 400 with "Purchase Declined. The subscription is now in the 'awaiting_signup' state.". On a 400 from activation, keep the subscription's previous state and retry after fixing payment.</description></item>
    ///   <item><description>Activation fails (for example the payment fails) with revert_on_failure false → Activation continues into an end-of-life state: trialing subscriptions go to trial ended (no-obligation trial), past due (trial with obligation) or canceled (no dunning strategy or cancel-immediately strategy); awaiting signup subscriptions are always canceled. Send revert_on_failure true unless a failed activation should end the subscription.</description></item>
    ///   <item><description>revert_on_failure is omitted or null → The site's Subscription Activation Settings value is used (default true). Send revert_on_failure explicitly rather than relying on the site setting.</description></item>
    ///   <item><description>Activation of a trialing subscription fails with revert_on_failure true → The subscription stays Trialing, the activation invoice is voided, and prepayments and credits applied to it are returned to the subscription. Expect a voided activation invoice after a reverted trial activation.</description></item>
    ///   <item><description>An awaiting-signup subscription is activated successfully → It moves to Trialing when the product has a trial, otherwise to Active. Expect trialing rather than active after activation when the product has a trial.</description></item>
    ///   <item><description>The call changes the subscription's state → A subscription_state_change webhook is sent with previous_state and state. Update cached subscription state from subscription_state_change events.</description></item>
    ///   <item><description>The call attempts a payment (signup, retry after failure, reactivation, resumption or migration charge) → A payment_success or payment_failure webhook is sent for every payment attempted. Handle payment_success and payment_failure events caused by these calls.</description></item>
    ///   <item><description>The payment is made by ACH/eCheck or Direct Debit → A payment reported as successful may fail later. Do not treat payment_success for ACH or Direct Debit as final; watch for later failure events.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A coupon is added that is not stackable, or the subscription already has a non-stackable coupon → Maxio answers 422 with "That coupon is not stackable". Combine several coupons on a subscription only when every coupon is stackable.</description></item>
    ///   <item><description>A coupon code that does not exist is applied → Maxio answers 422 with "Coupon code could not be found.". Apply only existing coupon codes.</description></item>
    ///   <item><description>The coupon code is passed in the deprecated code query parameter → All existing coupon codes on the subscription are replaced by this code. Pass codes in the request body; avoid the code query parameter.</description></item>
    ///   <item><description>Coupon codes are passed in the request body codes array → The codes are added to the existing list of coupons on the subscription. Remove unwanted coupons separately; body codes do not replace existing ones.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription body names neither product_handle nor product_id, uses no offer_id, and is not a components-only subscription under the new Catalog experience → the outcome is not stated. Identify the product with product_handle or product_id (or pass offer_id) unless building a components-only subscription on a Catalog-experience site.</description></item>
    ///   <item><description>A subscription is to be sold for a product that has not been created → the outcome is not stated. Create and configure the product before creating subscriptions to it.</description></item>
    ///   <item><description>A subscription is created in a customer hierarchy with group.target self → the outcome is not stated. Include a valid payment method in the subscription parameters.</description></item>
    ///   <item><description>The card cannot be successfully charged for the charges due at creation → The subscription is not created. Handle a failed signup charge as a failed creation and collect a working payment method before retrying.</description></item>
    ///   <item><description>payment_profile_id does not name an existing card or bank account belonging to an existing customer → the outcome is not stated. Use payment_profile_id only for a stored profile of an existing customer; otherwise send payment_profile_attributes.</description></item>
    ///   <item><description>A Maxio.js chargify_token is sent together with other payment profile attributes → the outcome is not stated. Send chargify_token as the only attribute of payment_profile_attributes.</description></item>
    ///   <item><description>Card attributes are sent without expiration_month or expiration_year and the signup is not a Subscription Import via vault_token → the outcome is not stated. Send expiration_month and expiration_year with raw card details; they may be omitted only when importing via vault_token.</description></item>
    ///   <item><description>On a site with the new Catalog experience, a subscription is sent without product_id or product_handle and has no active component with a positive allocated_quantity, a positive unit_balance, enabled: true, or a configured metered component → the outcome is not stated. Only omit the product on a Catalog-experience site, and then include at least one active qualifying component.</description></item>
    ///   <item><description>The product configuration or gateway settings require billing address fields (street, city, state, country, zip) and they are missing → the outcome is not stated. Check product and gateway settings and send the billing address fields they require.</description></item>
    ///   <item><description>The gateway settings require a CVV and none is sent → the outcome is not stated. Send cvv when the gateway requires it.</description></item>
    ///   <item><description>An ACH subscription is created with a billing_country other than US → the outcome is not stated. Use US as the billing country for ACH subscriptions.</description></item>
    ///   <item><description>The site's gateway is Square, no payment_method_nonce is sent, and the signup is not an import with vault_token and customer_vault_token → the outcome is not stated. Send the Square payment_method_nonce unless importing with vault_token and customer_vault_token.</description></item>
    ///   <item><description>gateway_handle is sent on a site without the MultiGateway feature → the outcome is not stated. Send gateway_handle only on MultiGateway-enabled sites.</description></item>
    ///   <item><description>A subscription is created with ACH and bank_name, bank_routing_number or bank_account_number is missing → Maxio answers 422 with "Bank routing number: cannot be blank.". Send bank_name, bank_routing_number and bank_account_number for ACH signups.</description></item>
    ///   <item><description>A subscription is created with GoCardless without bank_name, or with neither bank_account_number nor bank_iban → the outcome is not stated. For GoCardless send bank_name and either local bank details (bank_account_number) or an IBAN.</description></item>
    ///   <item><description>A subscription is created with Maxio Payments without agreement_acceptance (including terms_url) → the outcome is not stated. Send agreement_acceptance with terms_url when the site uses Maxio Payments.</description></item>
    ///   <item><description>agreement_acceptance is sent without ip_address → the outcome is not stated. Include the customer's ip_address whenever agreement_acceptance is sent.</description></item>
    ///   <item><description>agreement_acceptance is sent with neither terms_url nor privacy_policy_url → the outcome is not stated. Include terms_url or privacy_policy_url in agreement_acceptance.</description></item>
    ///   <item><description>ach_agreement is sent without all of agreement_terms, authorizer_first_name, authorizer_last_name and ip_address → the outcome is not stated. Send all four ACH agreement fields together whenever ach_agreement is present.</description></item>
    ///   <item><description>A referral code (ref) is supplied that is not valid → Subscription creation fails. Validate the referral code first (validateReferralCode) or omit ref.</description></item>
    ///   <item><description>A non-default currency is passed while Multi-Currency is disabled or the currency is not configured on the site → the outcome is not stated. Pass currency only when Multi-Currency is enabled and that currency is configured.</description></item>
    ///   <item><description>The app tries to change the currency of an existing subscription → the outcome is not stated. Choose the subscription currency at signup; it cannot be changed afterwards.</description></item>
    ///   <item><description>calendar_billing is sent together with next_billing_at → the outcome is not stated. Send either calendar_billing or next_billing_at, not both.</description></item>
    ///   <item><description>The request gives none of customer_id, customer_reference or customer_attributes → the outcome is not stated. Identify an existing customer with customer_id or customer_reference, or pass customer_attributes to create one.</description></item>
    ///   <item><description>snap_day is not a day from 1 to 28 or 'end' → the outcome is not stated. Send snap_day as 1 to 28 or 'end'.</description></item>
    ///   <item><description>payment_collection_method is not valid for the site's architecture (statements: automatic or invoice; Relationship Invoicing: automatic, remittance or prepaid) → the outcome is not stated. Pick the collection method from the set allowed by the site's invoicing architecture.</description></item>
    ///   <item><description>previous_billing_at is sent without next_billing_at → the outcome is not stated. Send next_billing_at whenever previous_billing_at is used.</description></item>
    ///   <item><description>net_terms is outside 0 to 180 days → the outcome is not stated. Send net_terms between 0 (due immediately) and 180.</description></item>
    ///   <item><description>branding_theme_id is sent on a site without Branding Themes enabled → the outcome is not stated. Send branding_theme_id only when Branding Themes are enabled for the site.</description></item>
    ///   <item><description>custom_price.list_price_point_id is sent on a site with list pricing enabled and the value is invalid → the outcome is not stated. Send a valid list price point id when list pricing is enabled.</description></item>
    ///   <item><description>A custom_price would give the subscription more than 30 custom price points → the outcome is not stated. Keep each subscription at 30 custom price points or fewer.</description></item>
    ///   <item><description>A component custom_price sets interval or interval_unit on a site without Multifrequency → the outcome is not stated. Set component custom price intervals only on Multifrequency-enabled sites.</description></item>
    ///   <item><description>A custom_price for an on/off component includes a pricing_scheme or more than one price bracket starting at 1 → the outcome is not stated. For on/off components omit pricing_scheme and send one price bracket starting at 1.</description></item>
    ///   <item><description>customer_attributes are passed to create a customer without first_name, last_name or email → the outcome is not stated. Include first_name, last_name and email whenever creating a customer through customer_attributes.</description></item>
    ///   <item><description>group parameters are sent on a site without Relationship Billing and Customer Hierarchy / subscription groups → the outcome is not stated. Send group parameters only on Relationship Billing sites with Customer Hierarchy enabled.</description></item>
    ///   <item><description>group.target is self and no valid payment method is given in the subscription parameters → the outcome is not stated. Send a valid payment method with a self-paid hierarchy subscription.</description></item>
    ///   <item><description>The product's options require payment information (a paid product or one with a billable component) and the request carries no payment method → the outcome is not stated. Send a payment method (payment_profile_id, payment profile/credit card or bank account attributes) for paid products or products with billable components.</description></item>
    ///   <item><description>cancel_at_end_of_period is set to true at subscription creation (the announcement names the case where next_billing_at is also present) → Maxio answers 422. Create the subscription first, then schedule a delayed cancellation separately.</description></item>
    ///   <item><description>A payment taken by the call requires 3D Secure authentication under Strong Customer Authentication → Maxio answers 422 with "action_link". Treat a 422 carrying an action_link as a 3DS challenge: send the customer to the link and complete the post-authentication flow.</description></item>
    ///   <item><description>payment_collection_method is set to a value the site's invoicing architecture does not support (invoice/automatic on legacy Statements; remittance/automatic/prepaid on Relationship Invoicing) → the outcome is not stated. Choose payment_collection_method from the values valid for the site's architecture.</description></item>
    ///   <item><description>A subscriber is to be taxed but neither the customer nor the payment profile has a full valid address with an ISO 2-3 character state and ISO 2 character country → The subscriber is not eligible to be taxed. Send a complete billing or shipping address with ISO state and country codes for every customer who must be taxed.</description></item>
    ///   <item><description>current_vault is sent with a value that does not match a gateway configured for the site, such as stripe instead of stripe_connect → The value is not currently validated against the site's gateways; that validation was briefly applied, reverted, and is to be re-introduced. Send the current_vault value that matches the site's configured gateway so the request keeps working when validation returns.</description></item>
    ///   <item><description>next_billing_at is not provided → Any trial and/or initial charges are assessed and charged at the time of subscription creation. Expect the payment method to be charged immediately at creation when next_billing_at is omitted.</description></item>
    ///   <item><description>A subscription becomes active → A Self-Service Page is created automatically for the subscription so the customer can manage payment methods. Use the subscription's self-service page for customer payment-method updates where suitable.</description></item>
    ///   <item><description>group parameters are sent to create a subscription inside a customer hierarchy → Creating subscriptions within a customer hierarchy is available only on sites using the Relationship Billing and Customer Hierarchy features. Send customer-hierarchy group parameters only on sites with Relationship Billing and Customer Hierarchy.</description></item>
    ///   <item><description>group.target names a customer that is already part of a customer hierarchy → The new subscription becomes a member of that customer hierarchy. Target the right customer, since the subscription joins that customer's hierarchy.</description></item>
    ///   <item><description>group.target names a customer that is not part of a customer hierarchy → A new customer hierarchy is created containing the target customer and the new subscription, with the target as responsible payer for all subscriptions in it. Expect a new hierarchy, paid by the target customer, when targeting a customer outside any hierarchy.</description></item>
    ///   <item><description>group.target is self → The subscription is paid for by the subscribing customer, whether that customer is new, in a hierarchy or outside one. Use target self when the subscribing customer pays for the subscription.</description></item>
    ///   <item><description>group parameters are sent to create a subscription inside a subscription group → Grouping at signup is available for sites using relationship billing. Send subscription-group parameters only on relationship billing sites.</description></item>
    ///   <item><description>group.target names a subscription that is already part of a subscription group → The new subscription becomes a member of that group. Target the right subscription, since the new one joins its group.</description></item>
    ///   <item><description>group.target names a subscription that is not part of a subscription group → A new group is created containing both subscriptions, with the target as the group's primary subscription. Expect a new subscription group with the target as primary when targeting an ungrouped subscription.</description></item>
    ///   <item><description>group.billing.accrue is set for a subscription created in a hierarchy or group → Payment capture is deferred and charges accrue until the next assessment date. Set accrue only when the first charge should wait for the next assessment.</description></item>
    ///   <item><description>group.billing.align_date is set together with prorate → The new subscription's billing date is aligned with the target and its charges are prorated to the target subscription's billing period. Send prorate only together with align_date.</description></item>
    ///   <item><description>initial_billing_at is set to a future date/time → The subscription is created in the Awaiting Signup state instead of Active or Trialing. Expect state awaiting_signup when a future initial_billing_at is given; omit it to activate immediately.</description></item>
    ///   <item><description>defer_signup is true → The subscription is created in the Awaiting Signup Date state with no known first billing date. Later update the subscription with initial_billing_at once the first billing date is known.</description></item>
    ///   <item><description>A delayed subscription is created successfully in an awaiting signup state → A delayed_subscription_creation_success webhook is sent. Subscribe to delayed_subscription_creation_success to learn about delayed signups.</description></item>
    ///   <item><description>Creating a delayed subscription fails → A delayed_subscription_creation_failure webhook is sent. Subscribe to delayed_subscription_creation_failure and act on failed delayed signups.</description></item>
    ///   <item><description>first_name or last_name is omitted from the payment profile attributes → The name on the card or bank account is taken from the customer attributes. Send the cardholder's names explicitly when they differ from the customer's names.</description></item>
    ///   <item><description>A card stored in Authorize.Net is imported without last_four → No masked card number can be built, and refunds in Authorize.Net need the last 4 digits. Send last_four when importing Authorize.Net card profiles so refunds stay possible.</description></item>
    ///   <item><description>A subscription has no associated product (built from components on a site with the new Catalog experience) → product, product_price_point_id and product_price_point_type are returned as null; component details are returned instead. Handle null product and product price point fields in subscription responses.</description></item>
    ///   <item><description>ach_agreement is passed → Proof of the authorized ACH agreement terms is persisted. Send the exact agreement terms the customer authorized, since they are stored as proof.</description></item>
    ///   <item><description>agreement_terms are passed and the feature is enabled → An email with a copy of the ACH terms is sent to the customer. Expect the customer to receive an email copy of the ACH terms.</description></item>
    ///   <item><description>expiration_tracks_next_billing_change is true, next_billing_at is present and the subscription expires → expires_at shifts by the same amount as the change between the old and new next billing dates. Expect expires_at to move with next billing date changes when this flag is set.</description></item>
    ///   <item><description>A calendar-billing subscription is created without calendar_billing_first_charge → The default 'prorated' applies: the prorated product price is charged immediately ('immediate' charges full price now, 'delayed' charges full price at the first renewal). Set calendar_billing_first_charge explicitly if a prorated immediate charge is not wanted.</description></item>
    ///   <item><description>previous_billing_at in the past is passed on an import → current_period_starts_at is set from it, and activated_at too unless activated_at is passed explicitly. Pass activated_at explicitly if it should differ from previous_billing_at.</description></item>
    ///   <item><description>import_mrr is true but next_billing_at is missing or not in the future → import_mrr is not honored; the subscription's MRR is not added to MRR analytics immediately. Pair import_mrr with a future next_billing_at.</description></item>
    ///   <item><description>dunning_communication_delay_enabled is true → No dunning email or SMS is sent to the customer between 9PM and 8AM in dunning_communication_delay_time_zone. Set dunning_communication_delay_time_zone to the customer's zone when enabling the delay.</description></item>
    ///   <item><description>skip_billing_manifest_taxes is sent to Create Subscription → The attribute is valid only for the Subscription Preview endpoint. Send skip_billing_manifest_taxes only to previewSubscription.</description></item>
    ///   <item><description>receives_invoice_emails is omitted → Defaults to true: the subscription receives its related emails. Send receives_invoice_emails false to suppress those emails.</description></item>
    ///   <item><description>The request passes customer_attributes → A new customer is created along with the subscription. Pass customer_id or customer_reference instead of customer_attributes when the customer already exists, to avoid creating a duplicate customer.</description></item>
    ///   <item><description>branding_theme_id is set on the subscription → The subscription-level Branding Theme replaces the customer's default theme for subscription-related documents and communications that use subscription theming. Expect subscription documents and emails to use the subscription's theme rather than the customer's.</description></item>
    ///   <item><description>branding_theme_id is set on create or update → branding_theme_id is not returned in the subscription response. Store the assigned branding theme on your side; do not expect to read it back from the response.</description></item>
    ///   <item><description>A subscription uses custom pricing as the sales price and no list price is given for an item → The list price defaults to the sales price, so no implicit discount is shown on invoices or reports. Provide a list price when an implicit discount should appear.</description></item>
    ///   <item><description>custom_price.list_price_point_id is sent on a site with list pricing disabled → The input is ignored and list price fields are omitted from responses. Do not rely on list_price_point_id unless list pricing is enabled for the site.</description></item>
    ///   <item><description>renew_prepaid_allocation or rollover_prepaid_remainder is set in a component custom_price for a component that is not a prepaid usage component → The settings apply only to prepaid usage components. Set renewal and rollover options only for prepaid usage components.</description></item>
    ///   <item><description>expiration_interval or expiration_interval_unit is set in a component custom_price without rollover enabled → Rollover expiration settings apply only when rollover is enabled. Enable rollover_prepaid_remainder before setting rollover expiration.</description></item>
    ///   <item><description>A signup component sets a quantity field that does not match its kind (enabled for on/off, unit_balance for metered and events-based, allocated_quantity for quantity-based) → Each quantity field is used only for its own component kind. Use enabled, unit_balance or allocated_quantity according to the component's kind.</description></item>
    ///   <item><description>group.target names a customer that is not part of a hierarchy → A new customer hierarchy is created with the target customer as responsible payer for all its subscriptions; if the target is already in a hierarchy, the subscription joins it. Expect the target customer to become the payer of the new hierarchy.</description></item>
    ///   <item><description>A subscription is created in a customer hierarchy that has no payment method yet, and a payment method is passed → The passed payment method becomes the default for the whole customer hierarchy, regardless of the responsible payer. Pass the hierarchy's intended default payment method on its first subscription.</description></item>
    ///   <item><description>group.target names a subscription that is not part of a group → A new subscription group is created with the target as primary subscription; if the target is already grouped, the new subscription joins that group. Expect the target subscription to become the primary of a new group.</description></item>
    ///   <item><description>group.target.id is sent while target type is self, parent or eldest → The id is ignored. Omit target.id when the target type is self, parent or eldest.</description></item>
    ///   <item><description>group.billing.prorate is true while align_date is not true → The prorate value is ignored. Set align_date true whenever prorate is true.</description></item>
    ///   <item><description>group.billing.accrue is true for a new grouped or hierarchy subscription → Payment capture is deferred and charges accrue until the next assessment date. Expect no immediate charge when accrue is set.</description></item>
    ///   <item><description>group.billing.align_date is true → The new subscription's billing date is aligned with the primary/target subscription's billing date. Expect the new subscription's billing date to follow the group's primary subscription.</description></item>
    ///   <item><description>group.billing.align_date and prorate are both true → Charges for the new subscription are prorated according to the target subscription's billing period. Expect a prorated first charge when aligning and prorating.</description></item>
    ///   <item><description>metafields (subscription or customer level) use a key with no existing metafield on the site → The metafield is created on the fly for that key. Use exact, consistent metafield keys; a typo creates a new custom field.</description></item>
    ///   <item><description>Custom field values are added at signup → A custom_field_value_change webhook is sent. Expect custom_field_value_change events for metafields sent at signup.</description></item>
    ///   <item><description>A subscription is created successfully → A signup_success webhook is sent, typically with a payment_success webhook when a payment was needed to start the subscription. Handle both signup_success and payment_success for one signup without double-provisioning.</description></item>
    ///   <item><description>A signup fails to begin, usually because of a payment gateway failure → A signup_failure webhook is sent; it is not sent for input validation errors. Do not expect signup_failure for validation errors; handle those from the API response.</description></item>
    ///   <item><description>A signup_success webhook is delivered → The payload does not include component allocations or reason_code. Read the subscription or its components through the API when those details are needed after signup.</description></item>
    ///   <item><description>Component allocations are set during subscription creation → No component_allocation_change webhook is sent for them. Take initial allocations from the signup, not from component_allocation_change events.</description></item>
    ///   <item><description>A billing address is sent without payment profile (credit card) information → The billing address is not stored, because it is kept on the payment profile. Send billing address fields inside the payment profile / credit card attributes.</description></item>
    ///   <item><description>A subscription to a taxable product lacks a full valid billing or shipping address with ISO-format state and country → The subscriber is not eligible to be taxed (the address is not valid for tax locale). Send a full address with ISO 3166 state codes and 2-letter country codes for taxable subscriptions.</description></item>
    ///   <item><description>The customer's vat_number is sent with the country code prefix → Taxes are assessed on the purchase despite the VAT number. Strip the country code from vat_number before sending it.</description></item>
    ///   <item><description>surcharging is omitted from customer_attributes on a site where surcharging control is enabled → Surcharging defaults to true for the customer; on other sites the value is not applied. Send surcharging false to exempt a customer where surcharging control is enabled.</description></item>
    ///   <item><description>next_billing_at is set to a future date/time → No trial or initial charges are applied and no payment is captured at creation; the first payment is captured near next_billing_at at the product's prices. Expect no signup charge when importing with a future next_billing_at; the first charge happens at that date.</description></item>
    ///   <item><description>The call attempts a payment (signup, retry after failure, reactivation, resumption or migration charge) → A payment_success or payment_failure webhook is sent for every payment attempted. Handle payment_success and payment_failure events caused by these calls.</description></item>
    ///   <item><description>The payment is made by ACH/eCheck or Direct Debit → A payment reported as successful may fail later. Do not treat payment_success for ACH or Direct Debit as final; watch for later failure events.</description></item>
    ///   <item><description>A metafields key is sent that does not yet exist as a custom field on the site → The metafield is created on the fly for that key. Check metafield keys before sending; a misspelt key silently creates a new custom field.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>product_price_point_id is supplied without the product filter → the outcome is not stated. Send the product filter whenever filtering by product_price_point_id.</description></item>
    ///   <item><description>A subscription has no associated product (built from components on a site with the new Catalog experience) → product, product_price_point_id and product_price_point_type are returned as null; component details are returned instead. Handle null product and product price point fields in subscription responses.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>per_page is omitted or above 200 → 20 records per page by default; values over 200 are changed to 200. Page through results and do not expect more than 200 records per page.</description></item>
    ///   <item><description>include[]=self_service_page_token is not sent → self_service_page_token is not returned. Send include[]=self_service_page_token when the Self-Service Page token is needed.</description></item>
    ///   <item><description>Subscriptions are read in XML and a date or integer field has no value → The nil element carries no type attribute (for example &lt;trial-started-at nil="true"/&gt;). Parse XML nil elements without relying on a type attribute.</description></item>
    ///   <item><description>The state filter is used → It accepts a comma-separated list of states. Pass several states as one comma-separated state value.</description></item>
    ///   <item><description>The requested page is past the last page of results → An empty result set is returned. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>The product filter is given a product handle → Handles are not supported; only a product ID or exact product name is matched. Filter by product ID or exact product name, never by handle.</description></item>
    ///   <item><description>The coupon filter is given a coupon code instead of the coupon's numeric id → The coupon code cannot be used in this filter. Use the numeric coupon id with coupon, or use the coupon_code filter for codes.</description></item>
    ///   <item><description>start_date filters the date_field → Matches timestamps at or after midnight (12:00:00 AM) in the site's time zone on that date. Convert date boundaries to the site's time zone, not UTC.</description></item>
    ///   <item><description>end_date filters the date_field → Matches timestamps up to and including 11:59:59 PM in the site's time zone on that date. Treat end_date as inclusive of the whole day in the site's time zone.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include an explicit time zone in start_datetime and end_datetime.</description></item>
    ///   <item><description>start_datetime is sent with start_date, or end_datetime with end_date → The datetime parameter is used instead of the date parameter. Send only one of each date/datetime pair.</description></item>
    ///   <item><description>current_billing_amount_in_cents is wanted from an operation other than readSubscription → The field (balance plus estimated renewal amount) is returned only by readSubscription, as it is compute intensive. Read the single subscription to get current_billing_amount_in_cents.</description></item>
    ///   <item><description>Coupon details are wanted on a subscription → The coupons array is returned only by Read Subscription and only with include[]=coupons. Call readSubscription with include[]=coupons to get coupon data.</description></item>
    ///   <item><description>Referrals are enabled on the site → Every subscription response includes a referral_code the customer can share. Read referral_code from the subscription when referrals are enabled.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>current_period_starts_at is passed for a subscription that is already billed (has statements or invoices) → the outcome is not stated. Override current_period_starts_at only on unbilled subscriptions (created with a future initial billing date).</description></item>
    ///   <item><description>current_period_starts_at is not before the current date/time → the outcome is not stated. Send a current_period_starts_at in the past.</description></item>
    ///   <item><description>Parameters other than activated_at, canceled_at, cancellation_message, expires_at and current_period_starts_at are sent → Maxio answers 400. Send only the overridable fields.</description></item>
    ///   <item><description>expires_at is overridden on a site with Multi-frequency enabled → the outcome is not stated. Use Update Subscription to change the term end on Multi-frequency sites.</description></item>
    ///   <item><description>Fields are overridden → No other attribute changes; for example adding an expiration date does not change the next assessment date. Update dependent fields (such as billing dates) yourself through other endpoints.</description></item>
    ///   <item><description>Historical dates such as activation or cancellation are overridden → Historical events are not backfilled (for example in the Events API); only static data changes. Do not expect events to be created for overridden historical dates.</description></item>
    ///   <item><description>An existing subscription's expiration date is changed → An expiration_date_change webhook is sent. Handle expiration_date_change after changing expires_at.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription body names neither product_handle nor product_id, uses no offer_id, and is not a components-only subscription under the new Catalog experience → the outcome is not stated. Identify the product with product_handle or product_id (or pass offer_id) unless building a components-only subscription on a Catalog-experience site.</description></item>
    ///   <item><description>On a site with the new Catalog experience, a subscription is sent without product_id or product_handle and has no active component with a positive allocated_quantity, a positive unit_balance, enabled: true, or a configured metered component → the outcome is not stated. Only omit the product on a Catalog-experience site, and then include at least one active qualifying component.</description></item>
    ///   <item><description>A custom_price would give the subscription more than 30 custom price points → the outcome is not stated. Keep each subscription at 30 custom price points or fewer.</description></item>
    ///   <item><description>payment_collection_method is set to a value the site's invoicing architecture does not support (invoice/automatic on legacy Statements; remittance/automatic/prepaid on Relationship Invoicing) → the outcome is not stated. Choose payment_collection_method from the values valid for the site's architecture.</description></item>
    ///   <item><description>A subscription has no associated product (built from components on a site with the new Catalog experience) → product, product_price_point_id and product_price_point_type are returned as null; component details are returned instead. Handle null product and product price point fields in subscription responses.</description></item>
    ///   <item><description>group.target.id is sent while target type is self, parent or eldest → The id is ignored. Omit target.id when the target type is self, parent or eldest.</description></item>
    ///   <item><description>A subscription body is POSTed to the preview endpoint → No subscription is created; the response is a prediction only. Call createSubscription separately to actually create the subscription.</description></item>
    ///   <item><description>Taxes are not configured, the purchase has no taxable product or component, or the payload lacks a full billing or shipping address → Taxes are not previewed. Send a full billing or shipping address for a taxable product on a tax-configured site to preview taxes.</description></item>
    ///   <item><description>skip_billing_manifest_taxes is true in a preview → Tax calculation is skipped for the current and next billing manifests even if addresses are sent. Leave skip_billing_manifest_taxes unset (default false) when the preview must include taxes.</description></item>
    ///   <item><description>A metafields key is sent that does not yet exist as a custom field on the site → The metafield is created on the fly for that key. Check metafield keys before sending; a misspelt key silently creates a new custom field.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A purge is requested on a site that is not in test mode → the outcome is not stated. Purge only on test-mode sites; contact support to remove subscriptions from a live site.</description></item>
    ///   <item><description>The ack query parameter is not the subscription's customer ID → the outcome is not stated. Confirm the purge by passing the subscription's customer ID as ack.</description></item>
    ///   <item><description>cascade[]=customer and/or cascade[]=payment_profile is passed → The customer record and/or payment profiles are deleted with the subscription. Pass cascade values only when the customer or payment profiles should also be deleted.</description></item>
    ///   <item><description>A subscription and its customer are deleted together → No customer_delete webhook is sent; it fires only when the customer is deleted on its own. Clean up the customer locally after a cascading purge instead of waiting for customer_delete.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A subscription has no associated product (built from components on a site with the new Catalog experience) → product, product_price_point_id and product_price_point_type are returned as null; component details are returned instead. Handle null product and product price point fields in subscription responses.</description></item>
    ///   <item><description>include[]=self_service_page_token is not sent → self_service_page_token is not returned. Send include[]=self_service_page_token when the Self-Service Page token is needed.</description></item>
    ///   <item><description>Subscriptions are read in XML and a date or integer field has no value → The nil element carries no type attribute (for example &lt;trial-started-at nil="true"/&gt;). Parse XML nil elements without relying on a type attribute.</description></item>
    ///   <item><description>current_billing_amount_in_cents is wanted from an operation other than readSubscription → The field (balance plus estimated renewal amount) is returned only by readSubscription, as it is compute intensive. Read the single subscription to get current_billing_amount_in_cents.</description></item>
    ///   <item><description>Referrals are enabled for the site → Every subscription response includes a referral_code. Read referral_code only on sites with referrals enabled and treat it as absent otherwise.</description></item>
    ///   <item><description>Coupon details are wanted on a subscription → The coupons array is returned only by Read Subscription and only with include[]=coupons. Call readSubscription with include[]=coupons to get coupon data.</description></item>
    ///   <item><description>Referrals are enabled on the site → Every subscription response includes a referral_code the customer can share. Read referral_code from the subscription when referrals are enabled.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>No coupon is applied to the subscription → Maxio answers 422 with "There's no coupon applied to this subscription". Remove a coupon only from a subscription that has one applied.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription's payment collection method is not prepaid → Maxio answers 422 with "Subscription payment method must be Prepaid". Update prepaid configuration only for subscriptions on prepaid collection.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The app tries to change the currency of an existing subscription → the outcome is not stated. Choose the subscription currency at signup; it cannot be changed afterwards.</description></item>
    ///   <item><description>snap_day is not a day from 1 to 28 or 'end' → the outcome is not stated. Send snap_day as 1 to 28 or 'end'.</description></item>
    ///   <item><description>payment_collection_method is not valid for the site's architecture (statements: automatic or invoice; Relationship Invoicing: automatic, remittance or prepaid) → the outcome is not stated. Pick the collection method from the set allowed by the site's invoicing architecture.</description></item>
    ///   <item><description>branding_theme_id is sent on a site without Branding Themes enabled → the outcome is not stated. Send branding_theme_id only when Branding Themes are enabled for the site.</description></item>
    ///   <item><description>A custom_price would give the subscription more than 30 custom price points → the outcome is not stated. Keep each subscription at 30 custom price points or fewer.</description></item>
    ///   <item><description>A component custom_price sets interval or interval_unit on a site without Multifrequency → the outcome is not stated. Set component custom price intervals only on Multifrequency-enabled sites.</description></item>
    ///   <item><description>A custom_price for an on/off component includes a pricing_scheme or more than one price bracket starting at 1 → the outcome is not stated. For on/off components omit pricing_scheme and send one price bracket starting at 1.</description></item>
    ///   <item><description>The payment profile of a customer who pays taxes is updated → the outcome is not stated. Send complete address information when updating the payment profile of a taxed customer.</description></item>
    ///   <item><description>A partial card update is sent for a card on Authorize.Net → the outcome is not stated. Update Authorize.Net cards through the Payment Profile endpoint instead.</description></item>
    ///   <item><description>snap_day is sent for a subscription whose product is not eligible for calendar billing → the outcome is not stated. Send snap_day only for subscriptions on calendar-billing-eligible products.</description></item>
    ///   <item><description>payment_collection_method is set to 'invoice' while the subscription is past due → Maxio answers 422 with "Payment collection method cannot be set to 'invoice' when the subscription is past due". Resolve the past-due balance before switching to invoice collection.</description></item>
    ///   <item><description>expires_at is set on a subscription on calendar billing → Maxio answers 422 with "'expires_at' cannot be set for subscriptions on calendar billing". Do not set expires_at on calendar-billing subscriptions.</description></item>
    ///   <item><description>Only the card's expiration date is changed on a gateway that does not allow it → the outcome is not stated. Check that the gateway supports expiration-date updates before relying on them.</description></item>
    ///   <item><description>payment_collection_method is switched to prepaid on a subscription holding any active wallet (manual or provisioned) → the outcome is not stated. Close the subscription's active wallets before switching it to prepaid collection.</description></item>
    ///   <item><description>The payment profile of a customer who pays taxes is updated without complete address information → the outcome is not stated. Send the complete billing address when updating a taxed customer's payment profile.</description></item>
    ///   <item><description>The product change names a product that is not from the same site as the subscription → the outcome is not stated. Change only to a different product on the subscription's own site.</description></item>
    ///   <item><description>A delayed product change (product_change_delayed=true) is set on a subscription → The product change happens automatically at the next renewal date, with no proration; setting next_product_id to an empty string cancels it. Expect the new product only from the next renewal and cancel with an empty next_product_id if needed.</description></item>
    ///   <item><description>A subscriber is to be taxed but neither the customer nor the payment profile has a full valid address with an ISO 2-3 character state and ISO 2 character country → The subscriber is not eligible to be taxed. Send a complete billing or shipping address with ISO state and country codes for every customer who must be taxed.</description></item>
    ///   <item><description>A subscription has no associated product (built from components on a site with the new Catalog experience) → product, product_price_point_id and product_price_point_type are returned as null; component details are returned instead. Handle null product and product price point fields in subscription responses.</description></item>
    ///   <item><description>dunning_communication_delay_enabled is true → No dunning email or SMS is sent to the customer between 9PM and 8AM in dunning_communication_delay_time_zone. Set dunning_communication_delay_time_zone to the customer's zone when enabling the delay.</description></item>
    ///   <item><description>branding_theme_id is set on the subscription → The subscription-level Branding Theme replaces the customer's default theme for subscription-related documents and communications that use subscription theming. Expect subscription documents and emails to use the subscription's theme rather than the customer's.</description></item>
    ///   <item><description>branding_theme_id is set on create or update → branding_theme_id is not returned in the subscription response. Store the assigned branding theme on your side; do not expect to read it back from the response.</description></item>
    ///   <item><description>renew_prepaid_allocation or rollover_prepaid_remainder is set in a component custom_price for a component that is not a prepaid usage component → The settings apply only to prepaid usage components. Set renewal and rollover options only for prepaid usage components.</description></item>
    ///   <item><description>expiration_interval or expiration_interval_unit is set in a component custom_price without rollover enabled → Rollover expiration settings apply only when rollover is enabled. Enable rollover_prepaid_remainder before setting rollover expiration.</description></item>
    ///   <item><description>next_billing_at is an invalid calendar date (for example February 30) → The date rolls over automatically (February 30 becomes March 2 in a non-leap year). Validate dates before sending; an impossible date is not rejected.</description></item>
    ///   <item><description>next_billing_at is changed → After that date is processed, later billing dates follow the product's period from it. Expect the new date to become the anchor of future billing dates.</description></item>
    ///   <item><description>next_billing_at is changed → The response has no next_billing_at; the new date shows as current_period_ends_at. Verify the change through current_period_ends_at.</description></item>
    ///   <item><description>The billing date is changed explicitly through the API → A billing_date_change webhook is sent (not for normal renewals or migrations). Use billing_date_change only for explicit billing date edits.</description></item>
    ///   <item><description>The product of a subscription with a snap_day is changed → The subscription keeps the original snap_day until the next billing cycle, when snap_day is reset to null. Do not read the old snap_day right after a product change as the lasting value.</description></item>
    ///   <item><description>An existing subscription's expiration date is changed → An expiration_date_change webhook is sent. Handle expiration_date_change after changing expires_at.</description></item>
    ///   <item><description>A future initial_billing_at is sent for a subscription in the Awaiting Signup Date state → The subscription moves to Awaiting Signup with that initial billing date. Set initial_billing_at once the first billing date of a deferred signup is known.</description></item>
    ///   <item><description>defer_signup is true for a subscription in the Awaiting Signup state → The subscription moves back to Awaiting Signup Date with no scheduled first billing date. Use defer_signup when the first billing date becomes unknown again.</description></item>
    ///   <item><description>branding_theme_id is sent as null or an empty value → The subscription-level Branding Theme is cleared and the customer's default applies. Send null or empty branding_theme_id only when the subscription theme should be removed.</description></item>
    ///   <item><description>The active credit-card payment profile is changed, including partial card or billing address updates → A subscription_card_update webhook is sent. Handle subscription_card_update after card updates.</description></item>
    ///   <item><description>The product is changed (product_handle or product_id set to another product) without product_change_delayed → The product changes now, but the new payment amount is calculated and charged at the normal start of the next period (no prorated upgrade or downgrade). Use migrateSubscriptionProduct when a prorated or immediate charge is needed.</description></item>
    ///   <item><description>A product change is sent without product_price_point_id or product_price_point_handle → The new product's default price point is used. Pass the price point explicitly when the default price point is not wanted.</description></item>
    ///   <item><description>A product change is sent with product_change_delayed true → The change is scheduled for the next renewal date and no proration applies. Expect the product to change only at the next renewal.</description></item>
    ///   <item><description>next_product_id is set to an empty string on a subscription with a delayed product change → The scheduled delayed product change is canceled. Send next_product_id as an empty string to cancel a delayed product change.</description></item>
    ///   <item><description>A delayed product change is scheduled → A subscription_product_change_scheduled webhook fires as soon as the change is scheduled. Use subscription_product_change_scheduled to react before the renewal.</description></item>
    ///   <item><description>A subscription's product (or product version) changes successfully → A subscription_product_change webhook is sent. Handle subscription_product_change for product changes, migrations and version changes.</description></item>
    /// </list>
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
