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
using MaxioAdvancedBilling.Requests.SubscriptionStatus;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Subscription Status allows you to manage a subscription's lifecycle, including retrying a failed payment, canceling, pausing, resuming, and reactivating a subscription. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/management/subscription-states/understand-subscription-states">Understand Subscription States</see>.
/// </summary>
public sealed class SubscriptionStatus
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal SubscriptionStatus(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Cancel Delayed Cancellation
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="DelayedCancellationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CancelDelayedCancellationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Removes the delayed cancellation from a subscription, ensuring it is not canceled at the end of the current period. The request will reset the <c>cancel_at_end_of_period</c> flag to <c>false</c>.
    /// <para>
    /// This endpoint is idempotent. If the subscription was not set to cancel in the future, removing the delayed cancellation has no effect and the call will be successful.
    /// </para>
    /// </remarks>
    public Task<DelayedCancellationResponse> CancelDelayedCancellation(CancelDelayedCancellationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/delayed_cancel.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<DelayedCancellationResponse>(),
            CancelDelayedCancellationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Cancel Dunning
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CancelDunningError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Cancels the active dunning process for a subscription and sets it to active.
    /// </remarks>
    public Task<SubscriptionResponse> CancelDunning(CancelDunningRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/cancel_dunning.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<SubscriptionResponse>(),
            CancelDunningError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Cancel Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CancelSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Cancels the Subscription. The Delete method sets the Subscription state to <c>canceled</c>.
    /// To cancel the subscription immediately, omit any schedule parameters from the request. To use the schedule options, the Schedule Subscription Cancellation feature must be enabled on your site.
    /// </remarks>
    public Task<SubscriptionResponse> CancelSubscription(CancelSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionResponse>(),
            CancelSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Initiate Delayed Cancellation
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="DelayedCancellationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="InitiateDelayedCancellationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Cancels a subscription at the end of the current billing period based on the subscription's current product. You cannot set <c>cancel_at_end_of_period</c> at subscription creation, or if the subscription is past due.
    /// </remarks>
    public Task<DelayedCancellationResponse> InitiateDelayedCancellation(InitiateDelayedCancellationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/delayed_cancel.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<DelayedCancellationResponse>(),
            InitiateDelayedCancellationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Hold / Pause Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="PauseSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Places the subscription on hold, preventing it from renewing.
    /// <para>
    /// ## Limitations
    /// </para>
    /// <para>
    /// You may not place a subscription on hold if the <c>next_billing_at</c> date is within 24 hours.
    /// </para>
    /// </remarks>
    public Task<SubscriptionResponse> PauseSubscription(PauseSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/hold.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionResponse>(),
            PauseSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Preview Renewal
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="RenewalPreviewResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="PreviewRenewalError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Previews a subscription’s next renewal assessment. Renewal Preview is an object representing a subscription’s next assessment. You can retrieve it to see a snapshot of how much your customer will be charged on their next renewal.
    /// <para>
    /// The "Next Billing" amount and "Next Billing" date are already represented in the UI on each Subscriber's Summary. For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24252493695757-Subscriber-Interface-Overview">Subscriber Interface Overview</see>.
    /// </para>
    /// <para>
    /// ## Optional Component Fields
    /// </para>
    /// <para>
    /// This endpoint is particularly useful because it returns the computed billing amount for the base product and the components which are in use by a subscriber.
    /// </para>
    /// <para>
    /// By default, the preview includes billing details for all components _at their <b>current</b> quantities_. This means:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Current <c>allocated_quantity</c> for quantity-based components</description></item>
    ///   <item><description>Current enabled/disabled status for on/off components</description></item>
    ///   <item><description>Current metered usage <c>unit_balance</c> for metered components</description></item>
    ///   <item><description>Current metric quantity value for events recorded thus far for events-based components</description></item>
    /// </list>
    /// <para>
    /// In the above statements, "current" means the quantity or value as of the call to the renewal preview endpoint. End-of-period values for components are not predicted, so metered or events-based usage may be less than it will eventually be at the end of the period.
    /// </para>
    /// <para>
    /// Optionally, <b>you can provide your own custom quantities</b> for any component to see a billing preview for non-current quantities. This is accomplished by sending a request body with data under the <c>components</c> key. See the request body documentation below.
    /// </para>
    /// <para>
    /// ## Preview Behavior
    /// </para>
    /// <para>
    /// Sending a <c>POST</c> request to this endpoint returns preview data without modifying the subscription. This method previews data, but does not log any changes against a subscription.
    /// </para>
    /// </remarks>
    public Task<RenewalPreviewResponse> PreviewRenewal(PreviewRenewalRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/renewals/preview.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<RenewalPreviewResponse>(),
            PreviewRenewalError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Reactivate Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReactivateSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Reactivates a previously canceled subscription. For details on how the reactivation works, and how to reactivate subscriptions through the application, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24252109503629-Reactivating-and-Resuming">reactivation</see>.
    /// <para>
    /// <b>Note: The term "resume" is used also during another process in Advanced Billing. This occurs when an on-hold subscription is "resumed". This returns the subscription to an active state.</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The response returns the subscription object in the <c>active</c> or <c>trialing</c> state.</description></item>
    ///   <item><description>The <c>canceled_at</c> and <c>cancellation_message</c> fields do not have values.</description></item>
    ///   <item><description>The method works for "Canceled" or "Trial Ended" subscriptions.</description></item>
    ///   <item><description>It will not work for items not marked as "Canceled", "Unpaid", or "Trial Ended".</description></item>
    /// </list>
    /// <para>
    /// ## Resume the current billing period for a subscription
    /// </para>
    /// <para>
    /// A subscription is considered "resumable" if you are attempting to reactivate within the billing period the subscription was canceled in.
    /// </para>
    /// <para>
    /// A resumed subscription's billing date remains the same as before it was canceled. In other words, it does not start a new billing period. Payment may or may not be collected for a resumed subscription, depending on whether or not the subscription had a balance when it was canceled (for example, if it was canceled because of dunning).
    /// </para>
    /// <para>
    /// Consider a subscription which was created on June 1st, and would renew on July 1st. The subscription is then canceled on June 15.
    /// </para>
    /// <para>
    /// If a reactivation with <c>resume: true</c> were attempted _before_ what would have been the next billing date of July 1st, then Advanced Billing would resume the subscription.
    /// </para>
    /// <para>
    /// If a reactivation with <c>resume: true</c> were attempted _after_ what would have been the next billing date of July 1st, then Advanced Billing would not resume the subscription, and instead it would be reactivated with a new billing period.
    /// </para>
    /// <para>
    /// If a reactivation with <c>resume: false</c>, or where 'resume' is omitted were attempted, then Advanced Billing would reactivate the subscription with a new billing period regardless of whether or not resuming the previous billing period was possible.
    /// </para>
    /// <para>
    /// | Canceled | Reactivation | Resumable? |
    /// |---|---|---|
    /// | Jun 15 | June 28 | Yes |
    /// | Jun 15 | July 2 | No |
    /// </para>
    /// <para>
    /// ## Reactivation Scenarios
    /// </para>
    /// <para>
    /// ### Reactivating Canceled Subscription While Preserving Balance
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Given you have a product that costs $20</description></item>
    ///   <item><description>Given you have a canceled subscription to the $20 product
    ///     <list type="bullet">
    ///       <item><description>1 charge should exist for $20</description></item>
    ///       <item><description>1 payment should exist for $20</description></item>
    ///     </list>
    ///   </description></item>
    ///   <item><description>When the subscription has canceled due to dunning, it retained a negative balance of $20</description></item>
    /// </list>
    /// <para>
    /// #### Results
    /// </para>
    /// <para>
    /// The resulting charges upon reactivation will be:
    /// + 1 charge for $20 for the new product
    /// + 1 charge for $20 for the balance due
    /// + Total charges = $40
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription will transition to active</description></item>
    ///   <item><description>The subscription balance will be zero</description></item>
    /// </list>
    /// <para>
    /// ### Reactivating a Canceled Subscription With Coupon
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Given you have a canceled subscription</description></item>
    ///   <item><description>It has no current period defined</description></item>
    ///   <item><description>You have a coupon code "EARLYBIRD"</description></item>
    ///   <item><description>The coupon is set to recur for 6 periods</description></item>
    /// </list>
    /// <para>
    /// PUT request sent to:
    /// <c>https://acme.chargify.com/subscriptions/{subscription_id}/reactivate.json?coupon_code=EARLYBIRD</c>
    /// </para>
    /// <para>
    /// #### Results
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription will transition to active</description></item>
    ///   <item><description>The subscription should have applied a coupon with code "EARLYBIRD"</description></item>
    /// </list>
    /// <para>
    /// ### Reactivating Canceled Subscription With a Trial, Without the include_trial Flag
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Given you have a canceled subscription</description></item>
    ///   <item><description>The product associated with the subscription has a trial</description></item>
    /// </list>
    /// <para>
    /// + PUT request to
    /// <c>https://acme.chargify.com/subscriptions/{subscription_id}/reactivate.json</c>
    /// </para>
    /// <para>
    ///
    /// #### Results
    /// + The subscription will transition to active
    /// </para>
    /// <para>
    /// ### Reactivating Canceled Subscription With Trial, With the include_trial Flag
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Given you have a canceled subscription</description></item>
    ///   <item><description>The product associated with the subscription has a trial</description></item>
    /// </list>
    /// <list type="bullet">
    ///   <item><description>Send a PUT request to <c>https://acme.chargify.com/subscriptions/{subscription_id}/reactivate.json?include_trial=1</c></description></item>
    /// </list>
    /// <para>
    ///
    /// #### Results
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription will transition to trialing</description></item>
    /// </list>
    /// <para>
    /// ### Reactivating Trial Ended Subscription
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Given you have a trial_ended subscription</description></item>
    ///   <item><description>Send a PUT request to <c>https://acme.chargify.com/subscriptions/{subscription_id}/reactivate.json</c></description></item>
    /// </list>
    /// <para>
    /// #### Results
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription will transition to active</description></item>
    /// </list>
    /// <para>
    /// ### Resuming a Canceled Subscription
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Given you have a <c>canceled</c> subscription and it is resumable</description></item>
    ///   <item><description>Send a PUT request to <c>https://acme.chargify.com/subscriptions/{subscription_id}/reactivate.json?resume=true</c></description></item>
    /// </list>
    /// <para>
    /// #### Results
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription will transition to active</description></item>
    ///   <item><description>The next billing date should not have changed</description></item>
    /// </list>
    /// <para>
    /// ### Attempting to resume a subscription which is not resumable
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Given you have a <c>canceled</c> subscription, and it is not resumable</description></item>
    ///   <item><description>Send a PUT request to <c>https://acme.chargify.com/subscriptions/{subscription_id}/reactivate.json?resume=true</c></description></item>
    /// </list>
    /// <para>
    /// #### Results
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription will transition to active, with a new billing period.</description></item>
    /// </list>
    /// <para>
    /// ### Attempting to resume but not reactivate a subscription which is not resumable
    /// </para>
    /// <para>
    /// + Given you have a <c>canceled</c> subscription, and it is not resumable
    /// + Send a PUT request to <c>https://acme.chargify.com/subscriptions/{subscription_id}/reactivate.json?resume[require_resume]=true</c>
    /// + The response status should be "422 UNPROCESSABLE ENTITY"
    /// + The subscription should be canceled with the following response
    /// <code>
    ///   {
    ///     "errors": ["Request was 'resume only', but this subscription cannot be resumed."]
    ///   }
    /// </code>
    /// </para>
    /// <para>
    /// #### Results
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription should remain <c>canceled</c></description></item>
    ///   <item><description>The next billing date should not have changed</description></item>
    /// </list>
    /// <para>
    /// ### Resuming Subscription Which Was Trialing
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Given you have a <c>trial_ended</c> subscription, and it is resumable</description></item>
    ///   <item><description>And the subscription was canceled in the middle of a trial</description></item>
    ///   <item><description>And there is still time left on the trial</description></item>
    ///   <item><description>Send a PUT request to <c>https://acme.chargify.com/subscriptions/{subscription_id}/reactivate.json?resume=true</c></description></item>
    /// </list>
    /// <para>
    /// #### Results
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription will transition to trialing</description></item>
    ///   <item><description>The next billing date should not have changed</description></item>
    /// </list>
    /// <para>
    /// ### Resuming Subscription Which Was trial_ended
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Given you have a <c>trial_ended</c> subscription, and it is resumable</description></item>
    ///   <item><description>Send a PUT request to <c>https://acme.chargify.com/subscriptions/{subscription_id}/reactivate.json?resume=true</c></description></item>
    /// </list>
    /// <para>
    /// #### Results
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription will transition to active</description></item>
    ///   <item><description>The next billing date should not have changed</description></item>
    ///   <item><description>Any product-related charges should have been collected</description></item>
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
    public Task<SubscriptionResponse> ReactivateSubscription(ReactivateSubscriptionOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/reactivate.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionResponse>(),
            ReactivateSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Resume Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ResumeSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Resumes a paused (on-hold) subscription. If the normal next renewal date has not passed, the subscription will return to active and will renew on that date.  Otherwise, it will behave like a reactivation, setting the billing date to 'now' and charging the subscriber.
    /// </remarks>
    public Task<SubscriptionResponse> ResumeSubscription(ResumeSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/resume.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [new Param("calendar_billing['resumption_charge']", request.CalendarBillingResumptionCharge)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<SubscriptionResponse>(),
            ResumeSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Retry Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RetrySubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retries collecting the balance due on a past-due subscription without waiting for the next scheduled attempt.
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
    public Task<SubscriptionResponse> RetrySubscription(RetrySubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/retry.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            JsonResponse.Create<SubscriptionResponse>(),
            RetrySubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Automatic Subscription Resumption
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateAutomaticSubscriptionResumptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the date on which a paused subscription will automatically resume.
    /// <para>
    /// To update a subscription's resume date, use this method to change or update the <c>automatically_resume_at</c> date.
    /// </para>
    /// <para>
    /// ### Remove the resume date
    /// </para>
    /// <para>
    /// Alternatively, you can change the <c>automatically_resume_at</c> to <c>null</c> if you would like the subscription to not have a resume date.
    /// </para>
    /// </remarks>
    public Task<SubscriptionResponse> UpdateAutomaticSubscriptionResumption(UpdateAutomaticSubscriptionResumptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/hold.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionResponse>(),
            UpdateAutomaticSubscriptionResumptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
