using System;
using System.Collections.Generic;
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
using MaxioAdvancedBilling.Requests.ReasonCodes;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Reason Code object defines a churn reason that can be presented to customers when they cancel a subscription, such as "too expensive" or "switching providers."
/// <para>
/// Reason codes give merchants visibility into why subscribers are canceling. For more information, see <see href="https://docs.maxio.com/configuration/advanced-billing/settings/churn-reason-codes">Configure Churn Reason Codes</see>.
/// </para>
/// </summary>
public sealed class ReasonCodes
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal ReasonCodes(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Reason Code
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ReasonCodeResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateReasonCodeError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a reason code for a given site.
    /// <para>
    /// Reason Codes are a way to gain a high-level view of why your customers are cancelling the subscription to your product or service.
    /// </para>
    /// <para>
    /// Add a set of churn reason codes to be displayed in-app and/or the Maxio Billing Portal. As your subscribers decide to cancel their subscription, learn why they decided to cancel.
    /// </para>
    /// <para>
    /// For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24286647554701-Churn-Reason-Codes">Churn Reason Codes</see>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A reason code is created or updated with a code another reason code on the site already uses → Maxio answers 422 with "Code: This code is already in use.". Use a code that no other reason code on the site has.</description></item>
    ///   <item><description>A reason code is created with a blank code or description → Maxio answers 422 with "Code: cannot be blank. / Description: cannot be blank.". Send a non-empty code and description.</description></item>
    ///   <item><description>Reason codes are listed or shown → Codes appear in the order given by their position. Set position on each reason code to control the order in which it appears.</description></item>
    /// </list>
    /// </remarks>
    public Task<ReasonCodeResponse> CreateReasonCode(CreateReasonCodeOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/reason_codes.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ReasonCodeResponse>(),
            CreateReasonCodeError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Reason Code
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="OkResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeleteReasonCodeError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes a reason code from the Churn Reason Codes. This code will be immediately removed. This action is not reversible.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A reason code is deleted → The code is removed immediately and the deletion cannot be reversed. Confirm before deleting a reason code; recreate it if needed rather than expecting a restore.</description></item>
    /// </list>
    /// </remarks>
    public Task<OkResponse> DeleteReasonCode(DeleteReasonCodeRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/reason_codes/{reason_code_id}.json"),
            [new TemplateParam("reason_code_id", request.ReasonCodeId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<OkResponse>(),
            DeleteReasonCodeError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Reason Codes
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="ReasonCodeResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListReasonCodesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists all current churn codes for a given site.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>page is sent as 0 or a negative number → Maxio answers 422 with "invalid page: 0". Start paging at page 1.</description></item>
    ///   <item><description>per_page is sent above 200 → per_page is changed to 200. Page with per_page of at most 200 and expect no more than 200 records per page.</description></item>
    ///   <item><description>page asks for a page past the last page of results → An empty result set is returned. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>Reason codes are listed or shown → Codes appear in the order given by their position. Set position on each reason code to control the order in which it appears.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<ReasonCodeResponse>> ListReasonCodes(ListReasonCodesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/reason_codes.json"),
            [],
            [new Param("page", request.Page), new Param("per_page", request.PerPage)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<ReasonCodeResponse>>(),
            ListReasonCodesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Reason Code
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ReasonCodeResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadReasonCodeError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a particular churn reason code for a given site by its unique ID.
    /// </remarks>
    public Task<ReasonCodeResponse> ReadReasonCode(ReadReasonCodeRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/reason_codes/{reason_code_id}.json"),
            [new TemplateParam("reason_code_id", request.ReasonCodeId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ReasonCodeResponse>(),
            ReadReasonCodeError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Reason Code
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ReasonCodeResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateReasonCodeError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates an existing reason code for a given site.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A reason code is created or updated with a code another reason code on the site already uses → Maxio answers 422 with "Code: This code is already in use.". Use a code that no other reason code on the site has.</description></item>
    ///   <item><description>Reason codes are listed or shown → Codes appear in the order given by their position. Set position on each reason code to control the order in which it appears.</description></item>
    /// </list>
    /// </remarks>
    public Task<ReasonCodeResponse> UpdateReasonCode(UpdateReasonCodeOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/reason_codes/{reason_code_id}.json"),
            [new TemplateParam("reason_code_id", request.ReasonCodeId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ReasonCodeResponse>(),
            UpdateReasonCodeError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
