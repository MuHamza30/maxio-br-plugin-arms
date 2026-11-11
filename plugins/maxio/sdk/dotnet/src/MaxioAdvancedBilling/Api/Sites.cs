using System;
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
using MaxioAdvancedBilling.Requests.Sites;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Site object represents a single billing account and holds site-wide configuration, such as default currency, tax settings, net terms, and which billing features are enabled. For more information, see <see href="https://docs.maxio.com/configuration/advanced-billing/sites/sites-overview">Understand Advanced Billing Sites</see>.
/// </summary>
public sealed class Sites
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Sites(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Clear Site Data
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Clears all data from a test site asynchronously. This call is asynchronous and there may be a delay before the site data is fully deleted. If you are clearing site data for an automated test, you will need to build in a delay and/or check that there are no products, etc., in the site before proceeding.
    /// <para>
    /// <b>This functionality will only work on sites in TEST mode. Attempts to perform this on sites in “live” mode will result in a response of 403 FORBIDDEN.</b>
    /// </para>
    /// </remarks>
    public Task ClearSite(ClearSiteRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/sites/clear_data.json"),
            [],
            [new Param("cleanup_scope", request.CleanupScope)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            VoidResponse.Instance,
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Maxio.js (formerly Chargify.js) Public Keys
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListPublicKeysResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists public keys used for Maxio.js (formerly Chargify.js).
    /// </remarks>
    public Task<ListPublicKeysResponse> ListChargifyJsPublicKeys(ListChargifyJsPublicKeysRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/chargify_js_keys.json"),
            [],
            [new Param("page", request.Page), new Param("per_page", request.PerPage)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListPublicKeysResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Site
    /// </summary>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SiteResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves site data.
    /// <para>
    /// For more information, see <see href="https://maxio.zendesk.com/hc/en-us/sections/24250550707085-Sites">Sites</see> in the product documentation. Specifically, the <see href="https://maxio.zendesk.com/hc/en-us/articles/24250617028365-Clearing-Site-Data">Clearing Site Data</see> section is relevant to this endpoint.
    /// </para>
    /// <para>
    /// #### Relationship invoicing enabled
    /// If the site has Relationship invoicing enabled, additional properties are returned in the response:
    /// </para>
    /// <code>
    /// "customer_hierarchy_enabled": true,
    /// "whopays_enabled": true,
    /// "whopays_default_payer": "self"
    /// </code>
    /// <para>
    /// For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24252185211533-Customer-Hierarchies-WhoPays">Who Pays &amp; Customer Hierarchy</see>.
    /// </para>
    /// </remarks>
    public Task<SiteResponse> ReadSite(RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/site.json"),
            [],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<SiteResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
