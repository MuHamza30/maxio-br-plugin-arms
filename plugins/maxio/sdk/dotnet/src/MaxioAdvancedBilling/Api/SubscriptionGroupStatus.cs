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
using MaxioAdvancedBilling.Requests.SubscriptionGroupStatus;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Use the Subscription Group Status object to manage the lifecycle of a Subscription Group as a whole, such as canceling every subscription within the group together.
/// </summary>
public sealed class SubscriptionGroupStatus
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal SubscriptionGroupStatus(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Cancel Delayed Group Cancellation
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CancelDelayedCancellationForGroupError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Removes the delayed cancellation on a subscription group.
    /// <para>
    /// Removing the delayed cancellation on a subscription group will ensure that the subscriptions do not get canceled at the end of the period. The request will reset the <c>cancel_at_end_of_period</c> flag to false on each member in the group.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>cancelDelayedCancellationForGroup is called on a group with no pending delayed cancellation → Maxio answers 422 with "Subscriptions group does not have a pending delayed cancellation". Only remove a delayed cancellation that was scheduled; treat this 422 as already cleared.</description></item>
    ///   <item><description>cancelDelayedCancellationForGroup succeeds → cancel_at_end_of_period is reset to false on every member of the group. Expect every member's cancel_at_end_of_period to be cleared, not just the primary's.</description></item>
    /// </list>
    /// </remarks>
    public Task CancelDelayedCancellationForGroup(CancelDelayedCancellationForGroupRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/delayed_cancel.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            CancelDelayedCancellationForGroupError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Cancel Grouped Subscriptions
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CancelSubscriptionsInGroupError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Cancels all subscriptions within the specified group immediately. The group is identified by the <c>uid</c> that is passed in the URL. To successfully cancel the group, the primary subscription must be on automatic billing. The group members must be on automatic billing or prepaid.
    /// <para>
    /// To cancel a subscription group while also charging for any unbilled usage on metered or prepaid components, the <c>charge_unbilled_usage=true</c> parameter must be included in the request.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>cancelSubscriptionsInGroup is called while the group's primary subscription is not on automatic billing → Maxio answers 422 with "One or more subscriptions are not on automatic billing". Move the primary subscription to automatic collection before cancelling the group.</description></item>
    ///   <item><description>cancelSubscriptionsInGroup is called while a group member is on neither automatic billing nor prepaid → Maxio answers 422 with "One or more subscriptions are not on automatic billing". Make sure every member is on automatic or prepaid collection before cancelling the group.</description></item>
    ///   <item><description>cancelSubscriptionsInGroup succeeds → Every subscription in the group is canceled immediately. Use initiateDelayedCancellationForGroup instead when the group should run to the end of its period.</description></item>
    ///   <item><description>cancelSubscriptionsInGroup is called without charge_unbilled_usage=true while metered or prepaid components have unbilled usage → The unbilled usage is not charged at cancellation. Send charge_unbilled_usage=true to bill outstanding metered or prepaid usage when cancelling.</description></item>
    /// </list>
    /// </remarks>
    public Task CancelSubscriptionsInGroup(CancelSubscriptionsInGroupRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/cancel.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            VoidResponse.Instance,
            CancelSubscriptionsInGroupError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Initiate Delayed Group Cancellation
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="InitiateDelayedCancellationForGroupError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Schedules all subscriptions within the specified group to be canceled at the end of their billing period. The group is identified by its uid passed in the URL.
    /// <para>
    /// All subscriptions in the group must be on automatic billing in order to successfully cancel them, and the group must not be in a "past_due" state.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>initiateDelayedCancellationForGroup is called while a subscription in the group is not on automatic billing → Maxio answers 422. Put every group member on automatic billing before scheduling a delayed cancellation.</description></item>
    ///   <item><description>initiateDelayedCancellationForGroup is called while the group is past_due → Maxio answers 422 with "Subscriptions group is in a past due state". Bring the group out of past_due before scheduling a delayed cancellation.</description></item>
    ///   <item><description>initiateDelayedCancellationForGroup succeeds → Every subscription in the group is scheduled to cancel at the end of its billing period. Expect members to stay active until each one's period ends.</description></item>
    /// </list>
    /// </remarks>
    public Task InitiateDelayedCancellationForGroup(InitiateDelayedCancellationForGroupRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/delayed_cancel.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            VoidResponse.Instance,
            InitiateDelayedCancellationForGroupError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Reactivate / Resume Subscription Group
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ReactivateSubscriptionGroupResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReactivateSubscriptionGroupError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Reactivates or resumes a cancelled subscription group. Upon reactivation, any canceled invoices created after the beginning of the primary subscription's billing period will be reopened and payment will be attempted on them. If the subscription group is being reactivated (as opposed to resumed), new charges will also be assessed for the new billing period.
    /// <para>
    /// Whether a subscription group is reactivated (a new billing period is created) or resumed (the current billing period is respected) will depend on the parameters that are sent with the request as well as the date of the request relative to the primary subscription's period.
    /// </para>
    /// <para>
    /// ## Reactivating within the current period
    /// </para>
    /// <para>
    /// If a subscription group is cancelled and reactivated within the primary subscription's current period, we can choose to either start a new billing period or maintain the existing one. If we want to maintain the existing billing period, the <c>resume=true</c> option must be passed in request parameters.
    /// </para>
    /// <para>
    /// An exception to the above are subscriptions that are on calendar billing. These subscriptions cannot be reactivated within the current period. If the <c>resume=true</c> option is not passed, the request will return an error.
    /// </para>
    /// <para>
    /// The <c>resume_members</c> option is ignored in this case. All eligible group members will be automatically resumed.
    /// </para>
    /// <para>
    ///
    /// ## Reactivating beyond the current period
    /// </para>
    /// <para>
    /// In this case, a subscription group can only be reactivated with a new billing period. If the <c>resume=true</c> option is passed it will be ignored.
    /// </para>
    /// <para>
    /// Member subscriptions can have billing periods that are longer than the primary (e.g. a monthly primary with annual group members). If the primary subscription in a group cannot be reactivated within the current period, but other group members can be, passing <c>resume_members=true</c> will resume the existing billing period for eligible group members. The primary subscription will begin a new billing period.
    /// </para>
    /// <para>
    /// For calendar billing subscriptions, the new billing period created will be a partial one, spanning from the date of reactivation to the next corresponding calendar renewal date.
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
    ///   <item><description>A group on calendar billing is reactivated within the current period without resume=true → Maxio answers 422. Send resume=true when reactivating a calendar-billing group inside its current period.</description></item>
    ///   <item><description>The reactivation payment requires 3D Secure authentication → Maxio answers 422. On a 422 with an action_link, send the customer to it to finish 3DS, then check the result.</description></item>
    ///   <item><description>A canceled subscription group is reactivated or resumed → Canceled invoices created after the start of the primary subscription's billing period are reopened and payment is attempted on them. Expect reopened invoices to be charged on reactivation.</description></item>
    ///   <item><description>A subscription group is reactivated (a new billing period), not resumed → New charges are assessed for the new billing period. Expect a new period charge when the group is reactivated rather than resumed.</description></item>
    ///   <item><description>A canceled group is reactivated within the primary subscription's current period → With resume=true the existing billing period is kept; without it a new billing period is started. Send resume=true to keep the current period when reactivating inside it.</description></item>
    ///   <item><description>resume_members is sent when reactivating within the primary subscription's current period → resume_members is ignored and all eligible members are resumed automatically. Do not rely on resume_members inside the current period; members are resumed anyway.</description></item>
    ///   <item><description>resume=true is sent when reactivating after the primary subscription's current period has ended → resume is ignored and the group is reactivated with a new billing period. Expect a new billing period, not a resume, once the primary's period has passed.</description></item>
    ///   <item><description>resume_members=true is sent when the primary subscription cannot be resumed within its current period but other members can → Eligible members keep their existing billing period while the primary starts a new one. Send resume_members=true to keep longer member periods (e.g. annual members under a monthly primary).</description></item>
    ///   <item><description>A calendar-billing group is reactivated with a new billing period → The new period is partial, from the reactivation date to the next calendar renewal date. Expect a partial first period for calendar-billing groups.</description></item>
    /// </list>
    /// </remarks>
    public Task<ReactivateSubscriptionGroupResponse> ReactivateSubscriptionGroup(ReactivateSubscriptionGroupOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}/reactivate.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ReactivateSubscriptionGroupResponse>(),
            ReactivateSubscriptionGroupError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
