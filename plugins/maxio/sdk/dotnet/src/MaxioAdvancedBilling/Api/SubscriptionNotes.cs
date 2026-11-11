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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The note body is blank → Maxio answers 422 with "Body: cannot be blank.". Send a non-empty note body.</description></item>
    ///   <item><description>A note is created or updated on a subscription → The note is internal only and is never exposed to the customer. Do not use notes to communicate with customers.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>page is below 1 (for example -1) → Maxio answers 422 with "invalid page: -1". Start paging at page 1.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>per_page is omitted or above 200 → 20 records per page by default; values over 200 are changed to 200. Page through results and do not expect more than 200 records per page.</description></item>
    ///   <item><description>The requested page is past the last page of results → An empty result set is returned. Stop paging when a page comes back empty.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The note body is blank → Maxio answers 422 with "Body: cannot be blank.". Send a non-empty note body.</description></item>
    ///   <item><description>A note is created or updated on a subscription → The note is internal only and is never exposed to the customer. Do not use notes to communicate with customers.</description></item>
    /// </list>
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
