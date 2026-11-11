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
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.Events;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Event object records a discrete occurrence within your account, such as a payment, renewal, or subscription state change.
/// <para>
/// The key identifies which type of event occurred, and event_specific_data carries a payload whose shape depends on that key.
/// </para>
/// </summary>
public sealed class Events
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Events(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// List Events
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="EventResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists events for a site.
    /// <para>
    /// Events include various activity that happens around a Site. This information is <b>especially</b> useful to track down issues that arise when subscriptions are not created due to errors.
    /// </para>
    /// <para>
    /// Within the UI, Events are referred to as Site Activity. For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24250671733517-Site-Activity">Site Activity</see>.
    /// </para>
    /// <para>
    /// Use query string filters to narrow down results. You can use the <c>filter</c> parameter to filter by event key.
    /// </para>
    /// <para>
    /// ### Legacy Filters
    /// </para>
    /// <para>
    /// The following keys are no longer supported.
    /// </para>
    /// <list type="bullet">
    ///   <item><description><c>payment_failure_recreated</c></description></item>
    ///   <item><description><c>payment_success_recreated</c></description></item>
    ///   <item><description><c>renewal_failure_recreated</c></description></item>
    ///   <item><description><c>renewal_success_recreated</c></description></item>
    ///   <item><description><c>zferral_revenue_post_failure</c> - (Specific to the deprecated Zferral integration)</description></item>
    ///   <item><description><c>zferral_revenue_post_success</c> - (Specific to the deprecated Zferral integration)</description></item>
    /// </list>
    /// <para>
    /// ## Event Key
    /// The event type is identified by the key property. See <see href="$m/Event%20Key">Event Key</see> for a complete list of supported keys.
    /// </para>
    /// <para>
    /// ## Event Specific Data
    /// </para>
    /// <para>
    /// Different event types may include additional data in <c>event_specific_data</c> property.
    /// While some events share the same schema for <c>event_specific_data</c>, others may not include it at all.
    /// For precise mappings from key to event_specific_data, refer to <see href="$m/Event">Event</see>.
    /// </para>
    /// <example>
    /// Here’s an example event for the <c>subscription_product_change</c> event:
    /// <code>
    /// {
    ///     "event": {
    ///         "id": 351,
    ///         "key": "subscription_product_change",
    ///         "message": "Product changed on Mark Alan's subscription from 'Basic' to 'Pro'",
    ///         "subscription_id": 205,
    ///         "event_specific_data": {
    ///             "new_product_id": 3,
    ///             "previous_product_id": 2
    ///         },
    ///         "created_at": "2012-01-30T10:43:31-05:00"
    ///     }
    /// }
    /// </code>
    /// <para>
    /// Here’s an example event for the <c>subscription_state_change</c> event:
    /// </para>
    /// <code>
    ///  {
    ///      "event": {
    ///          "id": 353,
    ///          "key": "subscription_state_change",
    ///          "message": "State changed on Mark Alan's subscription to Pro from trialing to active",
    ///          "subscription_id": 205,
    ///          "event_specific_data": {
    ///              "new_subscription_state": "active",
    ///              "previous_subscription_state": "trialing"
    ///          },
    ///          "created_at": "2012-01-30T10:43:33-05:00"
    ///      }
    ///  }
    /// </code>
    /// <para>
    /// ## Enhanced Catalog Experience
    /// </para>
    /// <para>
    /// If you’re using the <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">enhanced Catalog experience</see>, you’ll see updated naming in webhook events and messages.
    /// </para>
    /// <para>
    /// Event name changes:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>subscription_product_change → subscription_plan_change</description></item>
    ///   <item><description>component_allocation_change → allocation_change</description></item>
    ///   <item><description>component_billing_date_change → product_billing_date_change</description></item>
    /// </list>
    /// <para>
    /// Message updates:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>“Plan changed on Subscription from previous plan to new plan”</description></item>
    ///   <item><description>“Successful payment for allocation changes to Product on Subscription”</description></item>
    ///   <item><description>“Failed payment for allocation changes to Product on Subscription”</description></item>
    /// </list>
    /// </example>
    /// </remarks>
    public Task<IReadOnlyList<EventResponse>> ListEvents(ListEventsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/events.json"),
            [],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("since_id", request.SinceId),
                new Param("max_id", request.MaxId),
                new Param("direction", request.Direction),
                new Param("filter", request.Filter),
                new Param("date_field", request.DateField),
                new Param("start_date", request.StartDate),
                new Param("end_date", request.EndDate),
                new Param("start_datetime", request.StartDatetime),
                new Param("end_datetime", request.EndDatetime),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<EventResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Events for Subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="EventResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists events for a subscription.
    /// <para>
    /// ## Event Key
    /// The event type is identified by the key property. See <see href="$m/Event%20Key">Event Key</see> for a complete list of supported keys.
    /// </para>
    /// <para>
    /// ## Event Specific Data
    /// </para>
    /// <para>
    /// Different event types may include additional data in <c>event_specific_data</c> property.
    /// While some events share the same schema for <c>event_specific_data</c>, others may not include it at all.
    /// For precise mappings from key to event_specific_data, refer to <see href="$m/Event">Event</see>.
    /// </para>
    /// <para>
    /// ## Enhanced Catalog Experience
    /// </para>
    /// <para>
    /// If you’re using the <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">enhanced Catalog experience</see>, you’ll see updated naming in webhook events and messages.
    /// </para>
    /// <para>
    /// Event name changes:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>subscription_product_change → subscription_plan_change</description></item>
    ///   <item><description>component_allocation_change → allocation_change</description></item>
    ///   <item><description>component_billing_date_change → product_billing_date_change</description></item>
    /// </list>
    /// <para>
    /// Message updates:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>“Successful payment for allocation changes to Product on Subscription”</description></item>
    ///   <item><description>“Failed payment for allocation changes to Product on Subscription”</description></item>
    ///   <item><description>“Plan changed on Subscription from previous plan to new plan”</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<EventResponse>> ListSubscriptionEvents(ListSubscriptionEventsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/events.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("since_id", request.SinceId),
                new Param("max_id", request.MaxId),
                new Param("direction", request.Direction),
                new Param("filter", request.Filter),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<EventResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Total Event Count
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CountResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns the total count of events for a given site.
    /// <para>
    /// If you’re using the <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">enhanced Catalog experience</see>, you’ll see updated naming in webhook events and messages.
    /// </para>
    /// <para>
    /// Event name changes:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>subscription_product_change → subscription_plan_change</description></item>
    ///   <item><description>component_allocation_change → allocation_change</description></item>
    ///   <item><description>component_billing_date_change → product_billing_date_change</description></item>
    /// </list>
    /// <para>
    /// Message updates:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>“Successful payment for allocation changes to Product on Subscription”</description></item>
    ///   <item><description>“Failed payment for allocation changes to Product on Subscription”</description></item>
    ///   <item><description>“Plan changed on Subscription from previous plan to new plan”</description></item>
    /// </list>
    /// </remarks>
    public Task<CountResponse> ReadEventsCount(ReadEventsCountRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/events/count.json"),
            [],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("since_id", request.SinceId),
                new Param("max_id", request.MaxId),
                new Param("direction", request.Direction),
                new Param("filter", request.Filter),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<CountResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
