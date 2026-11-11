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
using MaxioAdvancedBilling.Requests.WebhooksApi;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Webhook object represents a single notification sent to your configured endpoint when an event occurs.
/// <para>
/// A webhook tracks delivery status, including when it was last sent, whether the merchant endpoint accepted it, and any error from a failed attempt. For more information, see <see href="https://docs.maxio.com/configuration/advanced-billing/webhooks/understand-webhooks-in-advanced-billing">Understand Webhooks in Advanced Billing</see>.
/// </para>
/// </summary>
public sealed class WebhooksApi
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal WebhooksApi(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Endpoint
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="EndpointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateEndpointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates an endpoint and assigns a list of webhook subscriptions (events) to it.
    /// See the <see href="page:introduction/webhooks/webhooks-reference#events">Webhooks Reference</see> page for available events.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An endpoint is created with a URL whose scheme is not http or https → Maxio answers 422 with "URL: invalid, only http and https supported". Give endpoint URLs an http or https scheme.</description></item>
    ///   <item><description>A webhook endpoint URL uses plain HTTP while the site is in live mode → the outcome is not stated. Use an HTTPS endpoint URL before taking the site live.</description></item>
    ///   <item><description>A site already has the maximum number of webhook endpoints allowed by its merchant limits → the outcome is not stated. Check the site's endpoint limit (Merchant Limits) before adding endpoints and reuse existing ones.</description></item>
    ///   <item><description>A webhook endpoint is hosted on a port other than 80 or 443 → Ports other than 80 and 443 are not supported. Host webhook endpoints on port 80 or 443.</description></item>
    /// </list>
    /// </remarks>
    public Task<EndpointResponse> CreateEndpoint(CreateEndpointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/endpoints.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<EndpointResponse>(),
            CreateEndpointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Enable Webhooks
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="EnableWebhooksResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Enables webhooks for your site.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Enable Webhooks is called with webhooks_enabled → Webhook sending is switched on (or off) for the whole site, not for one endpoint. Remember that this switch is site-wide and affects every configured endpoint.</description></item>
    /// </list>
    /// </remarks>
    public Task<EnableWebhooksResponse> EnableWebhooks(EnableWebhooksOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/webhooks/settings.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<EnableWebhooksResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Endpoints
    /// </summary>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="Endpoint"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists endpoints configured for a site.
    /// </remarks>
    public Task<IReadOnlyList<Endpoint>> ListEndpoints(RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/endpoints.json"),
            [],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<Endpoint>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Webhooks
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="WebhookResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves a list of webhooks.  You can pass query parameters if you want to filter webhooks. See the <see href="page:introduction/webhooks/webhooks">Webhooks</see> documentation for more information.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A webhook is retried or replayed → The webhook keeps the same id across retries and replays, so the same event can arrive more than once with one id. Record processed webhook ids and skip ones already handled.</description></item>
    ///   <item><description>A webhook that failed earlier is retried or resent and then accepted → last_error and last_error_at are cleared. Do not rely on last_error to keep the history of failed attempts.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>List Webhooks is called with since_date and/or until_date → The dates (YYYY-MM-DD) filter on the webhook created_at date and both bounds are inclusive. Send since_date and until_date as YYYY-MM-DD and treat both bounds as inclusive.</description></item>
    ///   <item><description>Webhook data is older than the site's webhook data retention limit → Advanced Billing stores webhook data only for a limited time (see Merchant Limits). Do not rely on old webhooks being available to list or replay; keep your own record of what you need.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<WebhookResponse>> ListWebhooks(ListWebhooksRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/webhooks.json"),
            [],
            [
                new Param("status", request.Status),
                new Param("since_date", request.SinceDate),
                new Param("until_date", request.UntilDate),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("order", request.Order),
                new Param("subscription", request.Subscription),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<WebhookResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Replay Webhooks
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ReplayWebhooksResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Replays webhooks. Posting to this endpoint does not immediately resend the webhooks. They are added to a queue and sent as soon as possible, depending on available system resources. You can submit an array of up to 1000 webhook IDs in the replay request.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A replay request lists more than 1000 webhook IDs → the outcome is not stated. Send at most 1000 webhook IDs per replay request and split larger sets.</description></item>
    ///   <item><description>A webhook is retried or replayed → The webhook keeps the same id across retries and replays, so the same event can arrive more than once with one id. Record processed webhook ids and skip ones already handled.</description></item>
    ///   <item><description>Webhook data is older than the site's webhook data retention limit → Advanced Billing stores webhook data only for a limited time (see Merchant Limits). Do not rely on old webhooks being available to list or replay; keep your own record of what you need.</description></item>
    ///   <item><description>Replay Webhooks is accepted → The webhooks are not resent immediately; they are queued and sent as soon as system resources allow. Do not expect replayed webhooks to arrive at once; watch last_sent_at or your endpoint instead of the replay response.</description></item>
    ///   <item><description>A webhook is explicitly replayed by the merchant → The webhook's accepted_at is cleared until the endpoint accepts it again. Do not treat a null accepted_at on a replayed webhook as a past delivery failure.</description></item>
    ///   <item><description>Paused webhooks reference an endpoint that has been deleted from Advanced Billing → Those paused webhooks cannot be resent and are effectively discarded. Replay or resend paused webhooks before deleting their endpoint.</description></item>
    ///   <item><description>Webhooks are replayed while the automatic retries for them may still be running → The same webhook can be delivered more than once (the replay and a retry). Track processed webhook IDs and replay only once last_sent_at is well outside the automatic retry intervals.</description></item>
    /// </list>
    /// </remarks>
    public Task<ReplayWebhooksResponse> ReplayWebhooks(ReplayWebhooksOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/webhooks/replay.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ReplayWebhooksResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Endpoint
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="EndpointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateEndpointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates an Endpoint. You can change the <c>url</c> of your endpoint or the list of <c>webhook_subscriptions</c> to which you are subscribed. See the <see href="page:introduction/webhooks/webhooks-reference#events">Webhooks Reference</see> page for available events.
    /// <para>
    /// Always send a complete list of events to which you want to subscribe. Sending a PUT request for an existing endpoint with an empty list of <c>webhook_subscriptions</c> will unsubscribe all events.
    /// </para>
    /// <para>
    /// If you want to unsubscribe from a specific event, send a list of <c>webhook_subscriptions</c> without the specific event key.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A webhook endpoint URL uses plain HTTP while the site is in live mode → the outcome is not stated. Use an HTTPS endpoint URL before taking the site live.</description></item>
    ///   <item><description>Update Endpoint names an event key that is not a valid webhook event → Maxio answers 422 with "Event key is not valid - foobar". Use only event keys from the Webhooks Reference event list.</description></item>
    ///   <item><description>A webhook endpoint is hosted on a port other than 80 or 443 → Ports other than 80 and 443 are not supported. Host webhook endpoints on port 80 or 443.</description></item>
    ///   <item><description>Update Endpoint is called with a webhook_subscriptions list → The list replaces the endpoint's subscribed events; any event left out is unsubscribed. Always send the complete list of events the endpoint should stay subscribed to.</description></item>
    ///   <item><description>Update Endpoint is called with an empty webhook_subscriptions list → The endpoint is unsubscribed from all events. Never send an empty webhook_subscriptions list unless you mean to unsubscribe every event.</description></item>
    /// </list>
    /// </remarks>
    public Task<EndpointResponse> UpdateEndpoint(UpdateEndpointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/endpoints/{endpoint_id}.json"),
            [new TemplateParam("endpoint_id", request.EndpointId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<EndpointResponse>(),
            UpdateEndpointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
