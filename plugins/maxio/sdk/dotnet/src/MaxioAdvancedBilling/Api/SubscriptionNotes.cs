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
using MaxioAdvancedBilling.Requests.SubscriptionNotes;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Subscription Note object stores an internal note attached to a subscription, used to record context, decisions, or history for your team.
/// <para>
/// Notes are never exposed to the customer; they exist purely for internal reference.
/// </para>
/// </summary>
public sealed class SubscriptionNotes
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal SubscriptionNotes(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Subscription Note
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionNoteResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateSubscriptionNoteError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a note for a subscription.
    /// <para>
    /// Notes allow you to record information about a particular Subscription in a free text format.
    /// </para>
    /// <para>
    /// If you have structured data such as birth date, color, etc., consider using <see href="$e/Custom%20Fields/createMetadata">Metadata</see> instead.
    /// </para>
    /// <para>
    /// For more information, see <see href="https://docs.maxio.com/hc/en-us/articles/24251654953997-Understanding-the-Subscription-Summary-Page#billing-portal-status:~:text=documentation%20for%20more.-,Adding%20Notes,-Notes%20are%20optional">Adding Notes</see> in the product documentation.
    /// </para>
    /// </remarks>
    public Task<SubscriptionNoteResponse> CreateSubscriptionNote(CreateSubscriptionNoteRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/notes.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionNoteResponse>(),
            CreateSubscriptionNoteError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Subscription Note
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes a note for a Subscription.
    /// </remarks>
    public Task DeleteSubscriptionNote(DeleteSubscriptionNoteRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/notes/{note_id}.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("note_id", request.NoteId),
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
    /// List Subscription Notes
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="SubscriptionNoteResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListSubscriptionNotesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves a list of notes associated with a subscription. The response will be an array of Notes.
    /// </remarks>
    public Task<IReadOnlyList<SubscriptionNoteResponse>> ListSubscriptionNotes(ListSubscriptionNotesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/notes.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [new Param("page", request.Page), new Param("per_page", request.PerPage)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<SubscriptionNoteResponse>>(),
            ListSubscriptionNotesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Subscription Note
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionNoteResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves a specific note attached to a subscription.
    /// </remarks>
    public Task<SubscriptionNoteResponse> ReadSubscriptionNote(ReadSubscriptionNoteRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/notes/{note_id}.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("note_id", request.NoteId),
            ],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<SubscriptionNoteResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Subscription Note
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionNoteResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateSubscriptionNoteError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates a note for a subscription.
    /// </remarks>
    public Task<SubscriptionNoteResponse> UpdateSubscriptionNote(UpdateSubscriptionNoteOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/notes/{note_id}.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("note_id", request.NoteId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionNoteResponse>(),
            UpdateSubscriptionNoteError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
