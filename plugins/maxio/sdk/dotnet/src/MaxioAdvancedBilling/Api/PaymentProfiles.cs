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
