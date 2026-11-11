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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A delayed cancellation is set on a subscription → The subscription is canceled at the end of the current billing period, unless the delayed cancellation is removed first. Remove the delayed cancellation before the period ends if the customer stays.</description></item>
    ///   <item><description>A delayed cancellation is removed → cancel_at_end_of_period is reset to false and the subscription is not canceled at period end. Expect cancel_at_end_of_period false after removing the delayed cancellation.</description></item>
    ///   <item><description>The subscription was not set to cancel in the future → The call has no effect and still succeeds. Safely retry this call; success does not prove a cancellation was pending.</description></item>
    ///   <item><description>A subscription is set to cancel at the end of the period, or a pending cancellation is cleared → A pending_cancellation_change webhook is sent. Track pending_cancellation_change to know when a delayed cancellation is set or removed.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription is not eligible to have its dunning canceled → Maxio answers 422 with "This subscription is not eligible to have its dunning canceled.". Cancel dunning only for subscriptions currently in dunning.</description></item>
    ///   <item><description>Dunning is canceled for a subscription in an active dunning process → The dunning process stops and the subscription is set to active. Expect the subscription to become active after canceling dunning, even with an unpaid balance.</description></item>
    ///   <item><description>The call changes the subscription's state → A subscription_state_change webhook is sent with previous_state and state. Update cached subscription state from subscription_state_change events.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>cancel_at_end_of_period, scheduled_cancellation_at or refund_prepayment_account_balance is sent while the Schedule Subscription Cancellation feature is not enabled on the site → the outcome is not stated. Use the schedule options only on sites with Schedule Subscription Cancellation enabled.</description></item>
    ///   <item><description>The subscription is already canceled → Maxio answers 422 with "The subscription is already canceled". Check the state before canceling and treat this 422 as already done.</description></item>
    ///   <item><description>reason_code is longer than 255 characters → Maxio answers 422 with "reason_code size cannot be greater than 255". Keep reason_code at 255 characters or fewer.</description></item>
    ///   <item><description>cancellation_message is longer than 65535 characters → Maxio answers 422 with "cancellation_message size cannot be greater than 65535". Keep cancellation_message at 65535 characters or fewer.</description></item>
    ///   <item><description>A delayed cancellation is set on a subscription → The subscription is canceled at the end of the current billing period, unless the delayed cancellation is removed first. Remove the delayed cancellation before the period ends if the customer stays.</description></item>
    ///   <item><description>A cancel request carries no schedule parameters → The subscription is canceled immediately and its state set to canceled. Omit cancel_at_end_of_period and scheduled_cancellation_at only when an immediate cancel is wanted.</description></item>
    ///   <item><description>The call changes the subscription's state → A subscription_state_change webhook is sent with previous_state and state. Update cached subscription state from subscription_state_change events.</description></item>
    ///   <item><description>cancel_at_end_of_period is true → The subscription is canceled at the end of the current period instead of immediately (a delayed cancellation). Keep serving the customer until the period ends after a delayed cancellation.</description></item>
    ///   <item><description>scheduled_cancellation_at is sent for a prepaid subscription → The scheduled-date option does not apply to prepaid subscriptions. Do not schedule a cancellation date for prepaid subscriptions.</description></item>
    ///   <item><description>A prepaid subscription is canceled without refund_prepayment_account_balance set to false → The remaining prepaid balance is refunded as part of cancellation (default true). Send refund_prepayment_account_balance false to keep the prepaid balance unrefunded.</description></item>
    ///   <item><description>A subscription is set to cancel at the end of the period, or a pending cancellation is cleared → A pending_cancellation_change webhook is sent. Track pending_cancellation_change to know when a delayed cancellation is set or removed.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription is already canceled → Maxio answers 422 with "The subscription is already canceled". Check the state before canceling and treat this 422 as already done.</description></item>
    ///   <item><description>reason_code is longer than 255 characters → Maxio answers 422 with "reason_code size cannot be greater than 255". Keep reason_code at 255 characters or fewer.</description></item>
    ///   <item><description>cancellation_message is longer than 65535 characters → Maxio answers 422 with "cancellation_message size cannot be greater than 65535". Keep cancellation_message at 65535 characters or fewer.</description></item>
    ///   <item><description>The subscription is past due → the outcome is not stated. Settle the past-due balance (or cancel immediately) instead of scheduling a delayed cancellation.</description></item>
    ///   <item><description>A delayed cancellation is set on a subscription → The subscription is canceled at the end of the current billing period, unless the delayed cancellation is removed first. Remove the delayed cancellation before the period ends if the customer stays.</description></item>
    ///   <item><description>A subscription is set to cancel at the end of the period, or a pending cancellation is cleared → A pending_cancellation_change webhook is sent. Track pending_cancellation_change to know when a delayed cancellation is set or removed.</description></item>
    ///   <item><description>A delayed cancellation is initiated → The subscription cancels at the end of the current billing period, based on its current product. Expect service to continue until the current period ends.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription's next_billing_at is within 24 hours → the outcome is not stated. Place subscriptions on hold more than 24 hours before their next billing date.</description></item>
    ///   <item><description>The subscription is not eligible to be put on hold → Maxio answers 422 with "This subscription is not eligible to be put on hold.". Handle this 422 by leaving the subscription active and telling the user it cannot be paused now.</description></item>
    ///   <item><description>A subscription is on hold → It does not renew, and it resumes automatically on automatically_resume_at when that date is set. Set or clear automatically_resume_at to control when a held subscription resumes.</description></item>
    ///   <item><description>The call changes the subscription's state → A subscription_state_change webhook is sent with previous_state and state. Update cached subscription state from subscription_state_change events.</description></item>
    ///   <item><description>A subscription is placed on hold → It goes on hold and does not renew while on hold. Expect no renewals until the subscription is resumed.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A component in the body is referenced by a handle that does not exist → Maxio answers 422 with "Couldn't find Component by handle:unknown". Reference existing components by id or handle: prefix.</description></item>
    ///   <item><description>A renewal preview is requested without a components body → Components are billed at their quantities as of the call; end-of-period usage is not predicted, so metered or events-based usage may be lower than at renewal. Treat the preview as a lower bound for usage-based components, or pass predicted quantities.</description></item>
    ///   <item><description>Component definitions are sent in the request body → They replace the subscription's actual components and quantities; the preview contains only those components plus product base fees. List every component that should appear in the preview when sending components.</description></item>
    ///   <item><description>A renewal preview is POSTed → The subscription is not modified and no changes are logged against it. Call the preview freely; it changes nothing.</description></item>
    ///   <item><description>The subscription has free components with a recorded quantity above 0 → They are returned as line items (since 2021-11-18; before, only components costing more than 0 were returned). Expect zero-amount component line items in renewal previews.</description></item>
    ///   <item><description>The site uses Avalara and the subscription's address is in one of its taxable regions → uncalculated_taxes is true: additional taxes will be calculated at renewal and are not in the preview totals. Check uncalculated_taxes before showing preview totals as final.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A payment taken by the call requires 3D Secure authentication under Strong Customer Authentication → Maxio answers 422 with "action_link". Treat a 422 carrying an action_link as a 3DS challenge: send the customer to the link and complete the post-authentication flow.</description></item>
    ///   <item><description>The subscription is not Canceled, Unpaid or Trial Ended → Maxio answers 422 with "Cannot reactivate a subscription that is not marked "Canceled", "Unpaid", or "Trial Ended".". Reactivate only canceled, unpaid or trial-ended subscriptions; resume on-hold ones instead.</description></item>
    ///   <item><description>The reactivation payment is declined by the gateway → Maxio answers 422. Show the returned decline message and collect a new payment method.</description></item>
    ///   <item><description>Reactivation needs a payment and no payment profile is on file → Maxio answers 422 with "No payment method was on file for the X balance". Attach a payment method before reactivating a subscription that will owe a balance.</description></item>
    ///   <item><description>resume[require_resume] is true and the subscription is not resumable → Maxio answers 422 with "Request was 'resume only', but this subscription cannot be resumed.". Use require_resume only when a new billing period must not be started; handle the 422.</description></item>
    ///   <item><description>The call changes the subscription's state → A subscription_state_change webhook is sent with previous_state and state. Update cached subscription state from subscription_state_change events.</description></item>
    ///   <item><description>A calendar-billing subscription is reactivated → calendar_billing.reactivation_charge decides the charge: prorated (default) completes the period, immediate charges full price now, delayed charges full price at the next renewal; it has no effect on non-calendar subscriptions. Choose reactivation_charge for calendar-billing subscriptions.</description></item>
    ///   <item><description>resume is true for a resumable trial_ended subscription that was canceled mid-trial with trial time left → The subscription returns to trialing and its next billing date does not change. Expect trialing, not active, when resuming a subscription canceled mid-trial.</description></item>
    ///   <item><description>resume is true for a resumable trial_ended subscription → It transitions to active, the next billing date is unchanged, and product-related charges are collected. Expect product charges to be collected when resuming a trial-ended subscription.</description></item>
    ///   <item><description>A subscription is reactivated successfully → It is returned in the active or trialing state with canceled_at and cancellation_message cleared. Expect canceled_at and cancellation_message to be empty after reactivation.</description></item>
    ///   <item><description>resume is true → If reactivated within the billing period it was canceled in, the period is resumed and the billing date unchanged; otherwise it is reactivated with a new billing period. Check the returned billing dates; resume: true may still start a new period.</description></item>
    ///   <item><description>resume is false or omitted → The subscription is reactivated with a new billing period even if it could have been resumed. Send resume: true to keep the original billing period when possible.</description></item>
    ///   <item><description>resume.forgive_balance is true → The existing balance is cleared before the resume attempt; if the subscription cannot be resumed the balance stays as it was. Do not assume the balance was forgiven unless the resume succeeded.</description></item>
    ///   <item><description>preserve_balance is true → The existing balance is not cleared; reactivation charges are added on top (for example a $20 product plus a $20 dunning balance gives $40 of charges). Without it the balance is cleared/reset. Set preserve_balance only when the customer should pay the old balance on reactivation.</description></item>
    ///   <item><description>The product has a trial and include_trial is not true → The trial is ignored and the subscription goes active; with include_trial true it goes trialing. Send include_trial true to restart the product's trial on reactivation.</description></item>
    ///   <item><description>use_credits_and_prepayments is not false → Service credits and prepayments are applied by default to the reactivation invoice. Send use_credits_and_prepayments false to keep credits and prepayments untouched.</description></item>
    ///   <item><description>The call attempts a payment (signup, retry after failure, reactivation, resumption or migration charge) → A payment_success or payment_failure webhook is sent for every payment attempted. Handle payment_success and payment_failure events caused by these calls.</description></item>
    ///   <item><description>The payment is made by ACH/eCheck or Direct Debit → A payment reported as successful may fail later. Do not treat payment_success for ACH or Direct Debit as final; watch for later failure events.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription is not on hold → Maxio answers 422 with "Only subscriptions that are on hold can be resumed.". Resume only on-hold subscriptions; use reactivate for canceled ones.</description></item>
    ///   <item><description>The call changes the subscription's state → A subscription_state_change webhook is sent with previous_state and state. Update cached subscription state from subscription_state_change events.</description></item>
    ///   <item><description>An on-hold subscription is resumed before its normal next renewal date has passed → It returns to active and renews on that original date. Expect no immediate charge when resuming before the renewal date.</description></item>
    ///   <item><description>An on-hold subscription is resumed after its normal next renewal date has passed → It behaves like a reactivation: the billing date is set to now and the subscriber is charged. Expect an immediate charge and a new billing date when resuming after the renewal date.</description></item>
    ///   <item><description>calendar_billing['resumption_charge'] is sent for a subscription that is not on calendar billing → The setting applies only to calendar billing subscriptions (default prorated). Send resumption_charge only for calendar billing subscriptions.</description></item>
    ///   <item><description>The call attempts a payment (signup, retry after failure, reactivation, resumption or migration charge) → A payment_success or payment_failure webhook is sent for every payment attempted. Handle payment_success and payment_failure events caused by these calls.</description></item>
    ///   <item><description>The payment is made by ACH/eCheck or Direct Debit → A payment reported as successful may fail later. Do not treat payment_success for ACH or Direct Debit as final; watch for later failure events.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A payment taken by the call requires 3D Secure authentication under Strong Customer Authentication → Maxio answers 422 with "action_link". Treat a 422 carrying an action_link as a 3DS challenge: send the customer to the link and complete the post-authentication flow.</description></item>
    ///   <item><description>The subscription is not past due → the outcome is not stated. Call retry only for past-due subscriptions with a balance due.</description></item>
    ///   <item><description>The immediate retry to collect the past-due balance does not succeed → Maxio answers 422 with "base: subscription retry was unsuccessful". On this 422 keep the subscription in dunning and ask the customer for a working payment method.</description></item>
    ///   <item><description>The call changes the subscription's state → A subscription_state_change webhook is sent with previous_state and state. Update cached subscription state from subscription_state_change events.</description></item>
    ///   <item><description>The call attempts a payment (signup, retry after failure, reactivation, resumption or migration charge) → A payment_success or payment_failure webhook is sent for every payment attempted. Handle payment_success and payment_failure events caused by these calls.</description></item>
    ///   <item><description>The payment is made by ACH/eCheck or Direct Debit → A payment reported as successful may fail later. Do not treat payment_success for ACH or Direct Debit as final; watch for later failure events.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subscription is not currently on hold → Maxio answers 422 with "Subscription is not currently on hold.". Change the automatic resume date only for on-hold subscriptions.</description></item>
    ///   <item><description>automatically_resume_at is less than 24 hours in the future → Maxio answers 422 with "Automatic resume date: must be at least 24 hours in the future.". Set automatically_resume_at at least 24 hours ahead.</description></item>
    ///   <item><description>A subscription is on hold → It does not renew, and it resumes automatically on automatically_resume_at when that date is set. Set or clear automatically_resume_at to control when a held subscription resumes.</description></item>
    ///   <item><description>automatically_resume_at is sent as null → The subscription no longer has a resume date and stays on hold until resumed manually. Send null only when the subscription should stay on hold indefinitely.</description></item>
    /// </list>
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
