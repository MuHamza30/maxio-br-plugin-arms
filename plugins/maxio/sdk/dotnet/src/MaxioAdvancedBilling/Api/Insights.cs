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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>page points past the last page of results → An empty result set is returned. Stop paging when a page comes back empty; do not wait for an error.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>MRR is reported with breakouts → The plan breakout covers revenue from Products, Quantity-Based Components and On/Off Components. Read plan revenue from the plan breakout and do not expect metered or prepaid revenue in it.</description></item>
    ///   <item><description>MRR is reported with breakouts → The usage breakout covers revenue from Metered Components and Prepaid Usage Components. Read metered and prepaid usage revenue from the usage breakout.</description></item>
    ///   <item><description>The site's MRR Report UI is set to exclude usage revenue → The MRR API responses still include both plan and usage breakouts; the UI setting does not apply. Subtract the usage breakout yourself if you want MRR without usage revenue.</description></item>
    ///   <item><description>An MRR movement is listed → line_items show the subscription's product configuration at the time of the movement, not its current configuration. Read current configuration from the subscription, not from movement line_items.</description></item>
    ///   <item><description>per_page is above 50 when listing MRR movements → per_page is changed to 50; without per_page, 10 records are returned. Page MRR movements with per_page of at most 50.</description></item>
    ///   <item><description>A no-charge component allocation was made on a subscription in a currency other than the site's primary currency before the fix announced on 2022-02-09 → The MRR changes from those past allocation events were wrong and are not recalculated automatically; only later events use the proper amount. Ask Maxio support to update MRR for those past events if your reporting depends on them.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>at_time is not a valid ISO 8601 timestamp → Maxio answers 400 with "supplied value is invalid, expected ISO 8601 format". Send at_time as an ISO 8601 timestamp, e.g. 2022-01-10T10:00:00-05:00.</description></item>
    ///   <item><description>per_page is above 200 → per_page is changed to 200; no error is returned. Without per_page, 20 records are returned. Page with per_page of at most 200 and do not assume a larger page was honoured.</description></item>
    ///   <item><description>page points past the last page of results → An empty result set is returned. Stop paging when a page comes back empty; do not wait for an error.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>MRR is reported with breakouts → The plan breakout covers revenue from Products, Quantity-Based Components and On/Off Components. Read plan revenue from the plan breakout and do not expect metered or prepaid revenue in it.</description></item>
    ///   <item><description>MRR is reported with breakouts → The usage breakout covers revenue from Metered Components and Prepaid Usage Components. Read metered and prepaid usage revenue from the usage breakout.</description></item>
    ///   <item><description>The site's MRR Report UI is set to exclude usage revenue → The MRR API responses still include both plan and usage breakouts; the UI setting does not apply. Subtract the usage breakout yourself if you want MRR without usage revenue.</description></item>
    ///   <item><description>direction is not sent → Records are ordered by subscription_id ascending. Send direction=desc for descending subscription_id order.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>MRR is reported with breakouts → The plan breakout covers revenue from Products, Quantity-Based Components and On/Off Components. Read plan revenue from the plan breakout and do not expect metered or prepaid revenue in it.</description></item>
    ///   <item><description>MRR is reported with breakouts → The usage breakout covers revenue from Metered Components and Prepaid Usage Components. Read metered and prepaid usage revenue from the usage breakout.</description></item>
    ///   <item><description>The site's MRR Report UI is set to exclude usage revenue → The MRR API responses still include both plan and usage breakouts; the UI setting does not apply. Subtract the usage breakout yourself if you want MRR without usage revenue.</description></item>
    ///   <item><description>A no-charge component allocation was made on a subscription in a currency other than the site's primary currency before the fix announced on 2022-02-09 → The MRR changes from those past allocation events were wrong and are not recalculated automatically; only later events use the proper amount. Ask Maxio support to update MRR for those past events if your reporting depends on them.</description></item>
    ///   <item><description>at_time is omitted → MRR is returned for the current time; with at_time it is returned for that historic time. Send at_time in ISO 8601 when you need MRR at a past moment.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Site stats are requested in XML → the outcome is not stated. Request site stats as JSON.</description></item>
    /// </list>
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
