using System;
using System.Collections.Generic;
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
using MaxioAdvancedBilling.Requests.PaymentProfiles;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Payment Profile object represents a stored payment method on a customer's account. For more information, see <see href="https://docs.maxio.com/getting-started/maxio-platform-basics/advanced-billing-overview-and-resources/subscription-summary-tabs/managing-payment-methods">Managing Payment Methods</see>.
/// </summary>
public sealed class PaymentProfiles
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal PaymentProfiles(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Change Subscription Default Payment Profile
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PaymentProfileResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ChangeSubscriptionDefaultPaymentProfileError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Changes the default payment profile on the subscription to the existing payment profile with the specified ID.
    /// <para>
    /// You must elect to change the existing payment profile to a new payment profile ID in order to receive a satisfactory response from this endpoint.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The payment profile given is already the current default for the subscription or subscription group → Maxio answers 422 with "This is already the current payment profile". Compare with the current default before calling, and treat this 422 as no change needed.</description></item>
    ///   <item><description>A subscription's active credit-card-type payment profile changes, including a partial card or billing address update or the deletion of a PayPal profile → A subscription_card_update webhook is sent with the previous and updated payment profiles. Listen for subscription_card_update to keep stored card details in sync.</description></item>
    ///   <item><description>A subscription's bank account (ACH) payment profile is added, updated or deleted → No subscription_card_update is sent; a subscription_bank_account_update is sent when a subscription adds or updates a bank account. Listen for subscription_bank_account_update for bank account changes and read the profile back after bank account deletions.</description></item>
    /// </list>
    /// </remarks>
    public Task<PaymentProfileResponse> ChangeSubscriptionDefaultPaymentProfile(ChangeSubscriptionDefaultPaymentProfileRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production(
                "/subscriptions/{subscription_id}/payment_profiles/{payment_profile_id}/change_payment_profile.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("payment_profile_id", request.PaymentProfileId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<PaymentProfileResponse>(),
            ChangeSubscriptionDefaultPaymentProfileError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Change Subscription Group Default Payment Profile
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PaymentProfileResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ChangeSubscriptionGroupDefaultPaymentProfileError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Changes the default payment profile on the subscription group to the existing payment profile with the specified ID.
    /// <para>
    /// You must elect to change the existing payment profile to a new payment profile ID in order to receive a satisfactory response from this endpoint.
    /// </para>
    /// <para>
    /// The new payment profile must belong to the subscription group's customer, otherwise you will receive an error.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The payment profile given is already the current default for the subscription or subscription group → Maxio answers 422 with "This is already the current payment profile". Compare with the current default before calling, and treat this 422 as no change needed.</description></item>
    ///   <item><description>The new default payment profile does not belong to the subscription group's customer → the outcome is not stated. Use only payment profiles of the group's own customer as the group default.</description></item>
    ///   <item><description>The active credit-card-type payment profile on a subscription group changes → A subscription_group_card_update webhook is sent with the previous and updated payment profiles. Listen for subscription_group_card_update to keep group payment details in sync.</description></item>
    /// </list>
    /// </remarks>
    public Task<PaymentProfileResponse> ChangeSubscriptionGroupDefaultPaymentProfile(ChangeSubscriptionGroupDefaultPaymentProfileRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production(
                "/subscription_groups/{uid}/payment_profiles/{payment_profile_id}/change_payment_profile.json"),
            [new TemplateParam("uid", request.Uid), new TemplateParam("payment_profile_id", request.PaymentProfileId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<PaymentProfileResponse>(),
            ChangeSubscriptionGroupDefaultPaymentProfileError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Payment Profile
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PaymentProfileResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreatePaymentProfileError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a payment profile for a customer.
    /// <para>
    /// When you create a new payment profile for a customer via the API, it does not automatically make the profile current for any of the customer’s subscriptions. To use the payment profile as the default, you must set it explicitly for the subscription or subscription group.
    /// </para>
    /// <para>
    /// Select an option from the <b>Request Examples</b> drop-down on the right side of the portal to see examples of common scenarios for creating payment profiles.
    /// </para>
    /// <para>
    /// Do not use real card information for testing. See the Sites articles that cover <see href="https://docs.maxio.com/hc/en-us/articles/24250712113165-Testing-Overview#testing-overview-0-0">testing your site setup</see> for more details on testing in your sandbox.
    /// </para>
    /// <para>
    /// Note that collecting and sending raw card details in production requires <see href="https://docs.maxio.com/hc/en-us/articles/24183956938381-PCI-Compliance#pci-compliance-0-0">PCI compliance</see> on your end. If your business is not PCI compliant, use <see href="https://docs.maxio.com/hc/en-us/articles/38163190843789-Chargify-js-Overview#chargify-js-overview-0-0">Maxio.js (formerly Chargify.js)</see> to collect credit card or bank account information.
    /// </para>
    /// <para>
    /// See the following articles to learn more about subscriptions and payments:
    /// </para>
    /// <list type="bullet">
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24251599929613-Subscription-Summary-Payment-Details-Tab">Subscriber Payment Details</see></description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24261425318541-Self-Service-Pages">Self Service Pages</see> (Allows credit card updates by Subscriber)</description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24261368332557-Individual-Page-Settings">Public Signup Pages payment settings</see></description></item>
    ///   <item><description><see href="https://developers.chargify.com/docs/developer-docs/d2e9e34db740e-signups#taxes">Taxes</see></description></item>
    ///   <item><description><see href="https://docs.maxio.com/hc/en-us/articles/38163190843789-Chargify-js-Overview">Maxio.js (formerly Chargify.js)</see>
    ///     <list type="bullet">
    ///       <item><description><see href="https://docs.maxio.com/hc/en-us/articles/38206331271693-Examples#h_01K0PJ15QQZKCER8CFK40MR6XJ">Maxio.js with GoCardless - minimal example</see></description></item>
    ///       <item><description><see href="https://docs.maxio.com/hc/en-us/articles/38206331271693-Examples#h_01K0PJ15QR09JVHWW0MCA7HVJV">Maxio.js with GoCardless - full example</see></description></item>
    ///       <item><description><see href="https://docs.maxio.com/hc/en-us/articles/38206331271693-Examples#h_01K0PJ15QQFKKN8Z7B7DZ9AJS5">Maxio.js with Stripe Direct Debit - minimal example</see></description></item>
    ///       <item><description><see href="https://docs.maxio.com/hc/en-us/articles/38206331271693-Examples#h_01K0PJ15QRECQQ4ECS3ZA55GY7">Maxio.js with Stripe Direct Debit - full example</see></description></item>
    ///       <item><description><see href="https://developers.chargify.com/docs/developer-docs/ZG9jOjE0NjAzNDIy-examples#minimal-example-with-sepa-or-becs-direct-debit-stripe-gateway">Maxio.js with Stripe BECS Direct Debit - minimal example</see></description></item>
    ///       <item><description><see href="https://developers.chargify.com/docs/developer-docs/ZG9jOjE0NjAzNDIy-examples#full-example-with-sepa-direct-debit-stripe-gateway">Maxio.js with Stripe BECS Direct Debit - full example</see></description></item>
    ///     </list>
    ///   </description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24176159136909-GoCardless">Full documentation on GoCardless</see></description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24176170430093-Stripe-SEPA-and-BECS-Direct-Debit">Full documentation on Stripe SEPA Direct Debit</see></description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24176170430093-Stripe-SEPA-and-BECS-Direct-Debit">Full documentation on Stripe BECS Direct Debit</see></description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24176170430093-Stripe-SEPA-and-BECS-Direct-Debit">Full documentation on Stripe BACS Direct Debit</see></description></item>
    /// </list>
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
    ///   <item><description>An ACH bank-account profile is created without bank_routing_number → the outcome is not stated. Send bank_routing_number for ACH; with GoCardless it is optional and is passed on as bank_code.</description></item>
    ///   <item><description>A bank-account profile is created with ACH, GoCardless, Stripe BECS or BACS Direct Debit with neither bank_account_number nor bank_iban → the outcome is not stated. Send bank_account_number whenever bank_iban is blank on these gateways.</description></item>
    ///   <item><description>A Stripe BECS or BACS Direct Debit bank-account profile is created without bank_branch_code → the outcome is not stated. Send bank_branch_code for Stripe BECS and BACS; with GoCardless an IBAN may be sent instead.</description></item>
    ///   <item><description>An ACH payment profile has a billing_country other than US → the outcome is not stated. Use billing_country US for ACH bank accounts.</description></item>
    ///   <item><description>The payment needs 3D Secure authentication to meet Strong Customer Authentication → Maxio answers 422. On this 422, send the customer to the returned action_link to complete 3DS and handle the post-authentication flow.</description></item>
    ///   <item><description>The chargify_token sent does not match an existing Advanced Billing token → Maxio answers 422 with "Advanced Billing token not found". Obtain a fresh token from Maxio.js and send it unchanged.</description></item>
    ///   <item><description>A credit card profile is created without expiration_month or expiration_year and is not an import via vault_token → Maxio answers 422 with "Credit card expiration month: cannot be blank.". Send expiration_month and expiration_year for every card that is not imported via vault_token.</description></item>
    ///   <item><description>A new payment profile is created without customer_id → the outcome is not stated. Always send customer_id when creating a payment profile.</description></item>
    ///   <item><description>A bank-account profile is created with ACH or GoCardless without bank_name → the outcome is not stated. Send bank_name for ACH and GoCardless bank accounts; it is optional only with Stripe Direct Debit.</description></item>
    ///   <item><description>A Stripe Direct Debit bank-account profile is created without bank_iban → the outcome is not stated. Send bank_iban for Stripe Direct Debit; with GoCardless send either the IBAN or local bank details.</description></item>
    ///   <item><description>A subscriber is to be taxed but neither the customer nor the payment profile has a full valid address with an ISO 2-3 character state and ISO 2 character country → The subscriber is not eligible to be taxed. Send a complete billing or shipping address with ISO state and country codes for every customer who must be taxed.</description></item>
    ///   <item><description>A new payment profile is created for a customer → The profile is not made current for any of the customer's subscriptions. After creating a profile, set it as the default on each subscription or subscription group that should use it.</description></item>
    ///   <item><description>bank_account_type is omitted for a bank-account profile → bank_account_type defaults to checking. Send bank_account_type savings explicitly for savings accounts.</description></item>
    ///   <item><description>bank_account_holder_type is omitted for a bank-account profile → bank_account_holder_type defaults to personal. Send bank_account_holder_type business explicitly for business accounts.</description></item>
    ///   <item><description>Billing address fields or a CVV are sent on a payment profile → The values are merely passed through to the payment gateway, which may require its own formats such as specific country codes. Format billing address and country values to your gateway's requirements; Advanced Billing does not normalise them.</description></item>
    ///   <item><description>customer_vault_token is sent for a vault other than Authorize.Net CIM or Square → customer_vault_token applies only to Authorize.Net CIM storage or Square. Send customer_vault_token only when importing from Authorize.Net CIM or Square.</description></item>
    ///   <item><description>gateway_handle is sent on a site without the MultiGateway feature enabled → gateway_handle is only available with MultiGateway; with it, the handle selects the gateway that stores the profile. Send gateway_handle only on MultiGateway sites, using the unique handle of the target gateway.</description></item>
    ///   <item><description>current_vault is sent with a value that does not match a gateway configured for the site, such as stripe instead of stripe_connect → The value is not currently validated against the site's gateways; that validation was briefly applied, reverted, and is to be re-introduced. Send the current_vault value that matches the site's configured gateway so the request keeps working when validation returns.</description></item>
    ///   <item><description>A new PayPal account payment profile is added → No webhook is generated. Do not wait for a webhook after adding a PayPal account; read the profile back instead.</description></item>
    ///   <item><description>A bank-account profile is created from an IBAN or local bank details → A customer, bank account and mandate are created in the site's current vault. If the customer, bank account and mandate already exist in the vault, import them with vault_token, customer_vault_token and current_vault instead of sending bank details.</description></item>
    ///   <item><description>first_name or last_name is omitted when creating a payment profile → The customer's first_name or last_name is used. Send the card or account holder's name when it differs from the customer's name.</description></item>
    ///   <item><description>A one-time token is used for ACH (bank account) data on a gateway other than Authorize.Net or Stripe → One-time tokens hold ACH data only for Authorize.Net or Stripe. Use one-time tokens for ACH only on Authorize.Net or Stripe sites.</description></item>
    /// </list>
    /// </remarks>
    public Task<PaymentProfileResponse> CreatePaymentProfile(CreatePaymentProfileOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/payment_profiles.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<PaymentProfileResponse>(),
            CreatePaymentProfileError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Subscription Group Payment Profile
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes a Payment Profile belonging to a Subscription Group.
    /// <para>
    /// <b>Note</b>: If the Payment Profile belongs to multiple Subscription Groups and/or Subscriptions, it will be removed from all of them.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The deleted payment profile belongs to several subscription groups and/or subscriptions → The payment profile is removed from all of them. Check every group and subscription that used the profile for a missing payment method after this delete.</description></item>
    /// </list>
    /// </remarks>
    public Task DeleteSubscriptionGroupPaymentProfile(DeleteSubscriptionGroupPaymentProfileRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/payment_profiles/{payment_profile_id}.json"),
            [new TemplateParam("uid", request.Uid), new TemplateParam("payment_profile_id", request.PaymentProfileId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Subscription Payment Profile
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes a payment profile belonging to the customer on the subscription.
    /// <para>
    /// If the customer has multiple subscriptions, the payment profile is removed from all of them.
    /// </para>
    /// <para>
    /// If you delete the default payment profile for a subscription, you need to specify another payment profile to be the default through the API, or either prompt the user to enter a card in the billing portal or on the self-service page, or visit the Payment Details tab on the subscription in the Admin UI and use the “Add New Credit Card” or “Make Active Payment Method” link, (depending on whether there are other cards present).
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A payment profile is deleted through one subscription while the customer has several subscriptions → The payment profile is removed from all of the customer's subscriptions. Check every subscription of the customer for a missing payment method after this delete.</description></item>
    ///   <item><description>The deleted payment profile is the subscription's default payment profile → The subscription is left without a default payment profile until another is set. Set another default payment profile through the API, or prompt the customer to add one, after deleting the default.</description></item>
    ///   <item><description>A subscription's active credit-card-type payment profile changes, including a partial card or billing address update or the deletion of a PayPal profile → A subscription_card_update webhook is sent with the previous and updated payment profiles. Listen for subscription_card_update to keep stored card details in sync.</description></item>
    ///   <item><description>A subscription's bank account (ACH) payment profile is added, updated or deleted → No subscription_card_update is sent; a subscription_bank_account_update is sent when a subscription adds or updates a bank account. Listen for subscription_bank_account_update for bank account changes and read the profile back after bank account deletions.</description></item>
    /// </list>
    /// </remarks>
    public Task DeleteSubscriptionsPaymentProfile(DeleteSubscriptionsPaymentProfileRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/payment_profiles/{payment_profile_id}.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("payment_profile_id", request.PaymentProfileId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Unused Payment Profile
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeleteUnusedPaymentProfileError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes an unused payment profile.
    /// <para>
    /// If the payment profile is in use by one or more subscriptions or groups, an error message is returned.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The payment profile is in use by one or more subscriptions or subscription groups → Maxio answers 422 with "The payment profile is in use by one or more subscriptions and cannot be deleted". Move subscriptions and groups to another payment profile before deleting this one, or use the subscription-scoped delete.</description></item>
    /// </list>
    /// </remarks>
    public Task DeleteUnusedPaymentProfile(DeleteUnusedPaymentProfileRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/payment_profiles/{payment_profile_id}.json"),
            [new TemplateParam("payment_profile_id", request.PaymentProfileId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            DeleteUnusedPaymentProfileError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Payment Profiles
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="PaymentProfileResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists all active payment profiles for a site, or for one customer within a site. If no payment profiles are found, this endpoint returns an empty array.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>per_page is sent above 200 → per_page is changed to 200. Page with per_page of at most 200 and expect no more than 200 records per page.</description></item>
    ///   <item><description>page asks for a page past the last page of results → An empty result set is returned. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>Payment profiles are listed, with or without customer_id → Only active payment profiles are returned, for the site or for the one customer given; an empty array is returned when none are found. Do not expect inactive profiles in this list, and treat an empty array as no profiles rather than an error.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<PaymentProfileResponse>> ListPaymentProfiles(ListPaymentProfilesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/payment_profiles.json"),
            [],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("customer_id", request.CustomerId),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<PaymentProfileResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read one time token details
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="GetOneTimeTokenRequest"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadOneTimeTokenError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns the one-time token data, including credit card or ACH details, associated with the provided token ID. One Time Tokens aka Advanced Billing Tokens house the credit card or ACH (Authorize.Net or Stripe only) data for a customer.
    /// <para>
    /// You can use One Time Tokens while creating a subscription or payment profile instead of passing all bank account or credit card data directly to a given API endpoint.
    /// </para>
    /// <para>
    /// To obtain a One Time Token you have to use <see href="https://docs.maxio.com/hc/en-us/articles/38163190843789-Chargify-js-Overview#chargify-js-overview-0-0">Chargify.js</see>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The chargify_token in the path does not match an Advanced Billing token, which can only be obtained through Chargify.js → Maxio answers 404 with "Advanced Billing token not found". Obtain the token through Chargify.js before reading it, and treat a 404 as an unknown token.</description></item>
    ///   <item><description>A one-time token is used for ACH (bank account) data on a gateway other than Authorize.Net or Stripe → One-time tokens hold ACH data only for Authorize.Net or Stripe. Use one-time tokens for ACH only on Authorize.Net or Stripe sites.</description></item>
    /// </list>
    /// </remarks>
    public Task<GetOneTimeTokenRequest> ReadOneTimeToken(ReadOneTimeTokenRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/one_time_tokens/{chargify_token}.json"),
            [new TemplateParam("chargify_token", request.ChargifyToken)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<GetOneTimeTokenRequest>(),
            ReadOneTimeTokenError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Payment Profile
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PaymentProfileResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadPaymentProfileError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a payment profile identified by its unique ID.
    /// <para>
    /// Note that a different JSON object will be returned if the card method on file is a bank account.
    /// </para>
    /// <para>
    /// ### Response for Bank Account
    /// </para>
    /// <para>
    /// Example response for Bank Account:
    /// </para>
    /// <code>
    /// {
    ///   "payment_profile": {
    ///     "id": 10089892,
    ///     "first_name": "Chester",
    ///     "last_name": "Tester",
    ///     "created_at": "2025-01-01T00:00:00-05:00",
    ///     "updated_at": "2025-01-01T00:00:00-05:00",
    ///     "customer_id": 14543792,
    ///     "current_vault": "bogus",
    ///     "vault_token": "0011223344",
    ///     "billing_address": "456 Juniper Court",
    ///     "billing_city": "Boulder",
    ///     "billing_state": "CO",
    ///     "billing_zip": "80302",
    ///     "billing_country": "US",
    ///     "customer_vault_token": null,
    ///     "billing_address_2": "",
    ///     "bank_name": "Bank of Kansas City",
    ///     "masked_bank_routing_number": "XXXX6789",
    ///     "masked_bank_account_number": "XXXX3344",
    ///     "bank_account_type": "checking",
    ///     "bank_account_holder_type": "personal",
    ///     "payment_type": "bank_account",
    ///     "site_gateway_setting_id": 1,
    ///     "gateway_handle": null
    ///   }
    /// }
    /// </code>
    /// </remarks>
    public Task<PaymentProfileResponse> ReadPaymentProfile(ReadPaymentProfileRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/payment_profiles/{payment_profile_id}.json"),
            [new TemplateParam("payment_profile_id", request.PaymentProfileId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<PaymentProfileResponse>(),
            ReadPaymentProfileError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Send request payment update email
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="SendRequestUpdatePaymentEmailError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Sends a "request payment update" email to the customer associated with the subscription.
    /// <para>
    /// If you attempt to send a "request payment update" email more than five times within a 30-minute period, you will receive a <c>422</c> response with an error message in the body. This error message will indicate that the request has been rejected due to excessive attempts, and will provide instructions on how to resubmit the request.
    /// </para>
    /// <para>
    /// Additionally, if you attempt to send a "request payment update" email for a subscription that does not exist, you will receive a <c>404</c> error response. This error message will indicate that the subscription could not be found, and will provide instructions on how to correct the error and resubmit the request.
    /// </para>
    /// <para>
    /// These error responses are designed to prevent excessive or invalid requests, and to provide clear and helpful information to users who encounter errors during the request process.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A request payment update email is sent more than five times within 30 minutes → Maxio answers 422 with "Too many requests. You can perform 5 requests within 00:30:00". Send at most 5 request-payment-update emails per subscription in any 30 minutes.</description></item>
    ///   <item><description>The request payment update email is requested for a subscription that does not exist → Maxio answers 404. Treat a 404 as a missing subscription, not a transient failure.</description></item>
    /// </list>
    /// </remarks>
    public Task SendRequestUpdatePaymentEmail(SendRequestUpdatePaymentEmailRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/request_payment_profiles_update.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            VoidResponse.Instance,
            SendRequestUpdatePaymentEmailError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Payment Profile
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PaymentProfileResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdatePaymentProfileError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates a payment profile.
    /// <para>
    /// ## Partial Card Updates
    /// </para>
    /// <para>
    /// In the event that you are using the Authorize.net, Stripe, Cybersource, Forte or Braintree Blue payment gateways, you can update just the billing and contact information for a payment method. Note the lack of credit-card related data contained in the JSON payload.
    /// </para>
    /// <para>
    /// In this case, the following JSON is acceptable:
    /// </para>
    /// <code>
    /// {
    ///   "payment_profile": {
    ///     "first_name": "Kelly",
    ///     "last_name": "Test",
    ///     "billing_address": "789 Juniper Court",
    ///     "billing_city": "Boulder",
    ///     "billing_state": "CO",
    ///     "billing_zip": "80302",
    ///     "billing_country": "US",
    ///     "billing_address_2": null
    ///   }
    /// }
    /// </code>
    /// <para>
    /// The result will be that you have updated the billing information for the card, yet retained the original card number data.
    /// </para>
    /// <para>
    /// ## Specific notes on updating payment profiles
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Merchants with <b>Authorize.net</b>, <b>Cybersource</b>, <b>Forte</b>, <b>Braintree Blue</b> or <b>Stripe</b> as their payment gateway can update their Customer’s credit cards without passing in the full credit card number and CVV.</description></item>
    /// </list>
    /// <list type="bullet">
    ///   <item><description>If you are using <b>Authorize.net</b>, <b>Cybersource</b>, <b>Forte</b>, <b>Braintree Blue</b> or <b>Stripe</b>, Advanced Billing will ignore the credit card number and CVV when processing an update via the API, and attempt a partial update instead. If you wish to change the card number on a payment profile, you will need to create a new payment profile for the given customer.</description></item>
    /// </list>
    /// <list type="bullet">
    ///   <item><description>A Payment Profile cannot be updated with the attributes of another type of Payment Profile. For example, if the payment profile you are attempting to update is a credit card, you cannot pass in bank account attributes (like <c>bank_account_number</c>), and vice versa.</description></item>
    /// </list>
    /// <list type="bullet">
    ///   <item><description>Updating a payment profile directly will not trigger an attempt to capture a past-due balance. If this is the intent, update the card details via the Subscription instead.</description></item>
    /// </list>
    /// <list type="bullet">
    ///   <item><description>If you are using Authorize.net or Stripe, you may elect to manually trigger a retry for a past due subscription after a partial update.</description></item>
    /// </list>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An ACH payment profile has a billing_country other than US → the outcome is not stated. Use billing_country US for ACH bank accounts.</description></item>
    ///   <item><description>The payment profile of a customer who pays taxes is updated → the outcome is not stated. Send complete address information when updating the payment profile of a taxed customer.</description></item>
    ///   <item><description>A payment profile is updated with attributes of another profile type, such as bank account attributes on a credit card profile → the outcome is not stated. Send only attributes of the profile's own type; create a new profile to switch between card and bank account.</description></item>
    ///   <item><description>A card profile is updated without expiration_month or expiration_year, is not an import via vault_token, and the site's gateway is not one that allows billing-only partial updates → the outcome is not stated. Send expiration_month and expiration_year on card updates unless the gateway supports billing-only partial updates.</description></item>
    ///   <item><description>The payment profile of a customer who pays taxes is updated without complete address information → the outcome is not stated. Send the complete billing address when updating a taxed customer's payment profile.</description></item>
    ///   <item><description>A subscriber is to be taxed but neither the customer nor the payment profile has a full valid address with an ISO 2-3 character state and ISO 2 character country → The subscriber is not eligible to be taxed. Send a complete billing or shipping address with ISO state and country codes for every customer who must be taxed.</description></item>
    ///   <item><description>Billing address fields or a CVV are sent on a payment profile → The values are merely passed through to the payment gateway, which may require its own formats such as specific country codes. Format billing address and country values to your gateway's requirements; Advanced Billing does not normalise them.</description></item>
    ///   <item><description>current_vault is sent with a value that does not match a gateway configured for the site, such as stripe instead of stripe_connect → The value is not currently validated against the site's gateways; that validation was briefly applied, reverted, and is to be re-introduced. Send the current_vault value that matches the site's configured gateway so the request keeps working when validation returns.</description></item>
    ///   <item><description>A card profile is updated with only billing and contact information on the Authorize.net, Stripe, Cybersource, Forte or Braintree Blue gateway → The billing information is updated and the original card number data is retained. On these gateways, send only billing and contact fields to change them without re-entering the card.</description></item>
    ///   <item><description>A card number or CVV is sent in an update on the Authorize.net, Cybersource, Forte, Braintree Blue or Stripe gateway → The card number and CVV are ignored and a partial update is attempted. To change the card number on these gateways, create a new payment profile for the customer instead of updating.</description></item>
    ///   <item><description>A payment profile used by a past-due subscription is updated directly → No attempt is made to capture the past-due balance. Update the card through the subscription, or trigger a retry yourself after a partial update, when the past-due balance should be collected.</description></item>
    ///   <item><description>A subscription's active credit-card-type payment profile changes, including a partial card or billing address update or the deletion of a PayPal profile → A subscription_card_update webhook is sent with the previous and updated payment profiles. Listen for subscription_card_update to keep stored card details in sync.</description></item>
    ///   <item><description>A subscription's bank account (ACH) payment profile is added, updated or deleted → No subscription_card_update is sent; a subscription_bank_account_update is sent when a subscription adds or updates a bank account. Listen for subscription_bank_account_update for bank account changes and read the profile back after bank account deletions.</description></item>
    /// </list>
    /// </remarks>
    public Task<PaymentProfileResponse> UpdatePaymentProfile(UpdatePaymentProfileOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/payment_profiles/{payment_profile_id}.json"),
            [new TemplateParam("payment_profile_id", request.PaymentProfileId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<PaymentProfileResponse>(),
            UpdatePaymentProfileError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Verify Bank Account
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="BankAccountResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="VerifyBankAccountError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Verifies a bank account. Submit the two small deposit amounts the customer received in their bank account to verify the bank account. (Stripe only)
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A bank account is verified on a site whose gateway is not Stripe → the outcome is not stated. Call bank account verification only for bank accounts stored with Stripe.</description></item>
    ///   <item><description>Verification has already been attempted 10 times for the bank account → Maxio answers 422 with "You have tried to verify this bank account 10 times. To continue trying to verify the account, please reach out to us directly.". Limit deposit-amount attempts and route the customer to support after the tenth failure.</description></item>
    ///   <item><description>Verification is submitted before the customer has received the two small deposits, or with amounts that are not the deposits received → the outcome is not stated. Wait until the customer sees the two deposits in the bank account and submit exactly those amounts in cents.</description></item>
    ///   <item><description>The two deposit amounts are provided successfully for a bank account → The bank account's verified flag records that it has been verified. Read verified on the bank account profile to know whether verification has completed.</description></item>
    /// </list>
    /// </remarks>
    public Task<BankAccountResponse> VerifyBankAccount(VerifyBankAccountRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/bank_accounts/{bank_account_id}/verification.json"),
            [new TemplateParam("bank_account_id", request.BankAccountId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<BankAccountResponse>(),
            VerifyBankAccountError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
