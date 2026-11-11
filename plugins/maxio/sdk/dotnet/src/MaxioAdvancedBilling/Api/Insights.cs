using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.Insights;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Insights endpoints provide aggregate site-level statistics, such as customer and subscription counts, giving a high-level snapshot of account activity.
/// </summary>
public sealed class Insights
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Insights(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// List MRR Movements
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListMrrResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists your site's MRR movements.
    /// <para>
    /// ## Understanding MRR movements
    /// </para>
    /// <para>
    /// This endpoint will aid in accessing your site's <see href="https://maxio.zendesk.com/hc/en-us/articles/24285894587021-MRR-Analytics">MRR Report</see> data.
    /// </para>
    /// <para>
    /// Whenever a subscription event occurs that causes your site's MRR to change (such as a signup or upgrade), we record an MRR movement. These records are accessible via the MRR Movements endpoint.
    /// </para>
    /// <para>
    /// Each MRR Movement belongs to a subscription and contains a timestamp, category, and an amount. <c>line_items</c> represent the subscription's product configuration at the time of the movement.
    /// </para>
    /// <para>
    /// ### Plan &amp; Usage Breakouts
    /// </para>
    /// <para>
    /// In the MRR Report UI, we support a setting to <see href="https://maxio.zendesk.com/hc/en-us/articles/24285894587021-MRR-Analytics#displaying-component-based-metered-usage-in-mrr">include or exclude</see> usage revenue. In the MRR APIs, responses include <c>plan</c> and <c>usage</c> breakouts.
    /// </para>
    /// <para>
    /// Plan includes revenue from:
    /// * Products
    /// * Quantity-Based Components
    /// * On/Off Components
    /// </para>
    /// <para>
    /// Usage includes revenue from:
    /// * Metered Components
    /// * Prepaid Usage Components
    /// </para>
    /// </remarks>
    public Task<ListMrrResponse> ListMrrMovements(ListMrrMovementsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/mrr_movements.json"),
            [],
            [
                new Param("subscription_id", request.SubscriptionId),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("direction", request.Direction),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListMrrResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List MRR per subscription
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionMrrResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListMrrPerSubscriptionError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists your site's current MRR, including plan and usage breakouts split per subscription.
    /// </remarks>
    public Task<SubscriptionMrrResponse> ListMrrPerSubscription(ListMrrPerSubscriptionRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions_mrr.json"),
            [],
            [
                new Param("filter", request.Filter),
                new Param("at_time", request.AtTime),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("direction", request.Direction),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<SubscriptionMrrResponse>(),
            ListMrrPerSubscriptionError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read MRR
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="MrrResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns your site's current MRR, including plan and usage breakouts.
    /// </remarks>
    public Task<MrrResponse> ReadMrr(ReadMrrRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/mrr.json"),
            [],
            [new Param("at_time", request.AtTime?.ToIso8601()), new Param("subscription_id", request.SubscriptionId)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<MrrResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Site Stats
    /// </summary>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SiteSummary"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns basic site-level stats. This API call only answers with JSON responses. An XML version is not provided.
    /// <para>
    /// ## Stats Documentation
    /// </para>
    /// <para>
    /// There currently is not a complimentary matching set of documentation that compliments this endpoint. However, each Site's dashboard will reflect the summary of information provided in the Stats response.
    /// </para>
    /// <code>
    /// https://subdomain.chargify.com/dashboard
    /// </code>
    /// </remarks>
    public Task<SiteSummary> ReadSiteStats(RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/stats.json"),
            [],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<SiteSummary>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
