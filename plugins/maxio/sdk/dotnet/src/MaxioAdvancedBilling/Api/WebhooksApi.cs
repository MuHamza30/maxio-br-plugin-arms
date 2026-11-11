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
