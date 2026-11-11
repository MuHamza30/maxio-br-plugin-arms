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
using MaxioAdvancedBilling.Requests.SalesCommissions;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Sales Commissions allow sellers with financial access to retrieve commission data for a site with Advanced Analytics enabled.
/// <para>
/// These endpoints use a different, user-level authentication method than the rest of the API, since commission data is tied to the seller rather than an individual site.
/// </para>
/// </summary>
public sealed class SalesCommissions
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal SalesCommissions(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// List Sales Commission Settings
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="SaleRepSettings"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists subscriptions with associated sales reps.
    /// <para>
    /// ## Modified Authentication Process
    /// </para>
    /// <para>
    /// The Sales Commission API differs from other Chargify API endpoints. This resource is associated with the seller itself. Up to now all available resources were at the level of the site, therefore creating the API Key per site was a sufficient solution. To share resources at the seller level, a new authentication method was introduced, which is user authentication. Creating an API Key for a user is a required step to correctly use the Sales Commission API, more details <see href="https://developers.chargify.com/docs/developer-docs/ZG9jOjMyNzk5NTg0-2020-04-20-new-api-authentication">here</see>.
    /// </para>
    /// <para>
    /// Access to the Sales Commission API endpoints is available to users with financial access, where the seller has the Advanced Analytics component enabled. For further information on getting access to Advanced Analytics contact Maxio support.
    /// </para>
    /// <para>
    /// &gt; Note: The request is at seller level, it means <c>&lt;&lt;subdomain&gt;&gt;</c> variable will be replaced by <c>app</c>.
    /// </para>
    /// </remarks>
    public Task<IReadOnlyList<SaleRepSettings>> ListSalesCommissionSettings(ListSalesCommissionSettingsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/sellers/{seller_id}/sales_commission_settings.json"),
            [new TemplateParam("seller_id", request.SellerId)],
            [
                new Param("live_mode", request.LiveMode),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
            ],
            [new HeaderParam("Authorization", request.Authorization)],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<SaleRepSettings>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Sales Reps
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="ListSaleRepItem"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists sales reps with details.
    /// <para>
    /// ## Modified Authentication Process
    /// </para>
    /// <para>
    /// The Sales Commission API differs from other Chargify API endpoints. This resource is associated with the seller itself. Up to now all available resources were at the level of the site, therefore creating the API Key per site was a sufficient solution. To share resources at the seller level, a new authentication method was introduced, which is user authentication. Creating an API Key for a user is a required step to correctly use the Sales Commission API, more details <see href="https://developers.chargify.com/docs/developer-docs/ZG9jOjMyNzk5NTg0-2020-04-20-new-api-authentication">here</see>.
    /// </para>
    /// <para>
    /// Access to the Sales Commission API endpoints is available to users with financial access, where the seller has the Advanced Analytics component enabled. For further information on getting access to Advanced Analytics contact Maxio support.
    /// </para>
    /// <para>
    /// &gt; Note: The request is at seller level, it means <c>&lt;&lt;subdomain&gt;&gt;</c> variable will be replaced by <c>app</c>.
    /// </para>
    /// </remarks>
    public Task<IReadOnlyList<ListSaleRepItem>> ListSalesReps(ListSalesRepsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/sellers/{seller_id}/sales_reps.json"),
            [new TemplateParam("seller_id", request.SellerId)],
            [
                new Param("live_mode", request.LiveMode),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
            ],
            [new HeaderParam("Authorization", request.Authorization)],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<ListSaleRepItem>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Sales Rep
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SaleRep"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a sales rep and attached subscription details.
    /// <para>
    /// ## Modified Authentication Process
    /// </para>
    /// <para>
    /// The Sales Commission API differs from other Chargify API endpoints. This resource is associated with the seller itself. Up to now all available resources were at the level of the site, therefore creating the API Key per site was a sufficient solution. To share resources at the seller level, a new authentication method was introduced, which is user authentication. Creating an API Key for a user is a required step to correctly use the Sales Commission API, more details <see href="https://developers.chargify.com/docs/developer-docs/ZG9jOjMyNzk5NTg0-2020-04-20-new-api-authentication">here</see>.
    /// </para>
    /// <para>
    /// Access to the Sales Commission API endpoints is available to users with financial access, where the seller has the Advanced Analytics component enabled. For further information on getting access to Advanced Analytics contact Maxio support.
    /// </para>
    /// <para>
    /// &gt; Note: The request is at seller level, it means <c>&lt;&lt;subdomain&gt;&gt;</c> variable will be replaced by <c>app</c>.
    /// </para>
    /// </remarks>
    public Task<SaleRep> ReadSalesRep(ReadSalesRepRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/sellers/{seller_id}/sales_reps/{sales_rep_id}.json"),
            [new TemplateParam("seller_id", request.SellerId), new TemplateParam("sales_rep_id", request.SalesRepId)],
            [
                new Param("live_mode", request.LiveMode),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
            ],
            [new HeaderParam("Authorization", request.Authorization)],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<SaleRep>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
