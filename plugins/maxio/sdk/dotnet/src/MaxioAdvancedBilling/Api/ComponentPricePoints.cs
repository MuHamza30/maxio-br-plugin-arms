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
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.ComponentPricePoints;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Component Price Point object defines a specific pricing configuration for a Component, including the price point's rate structure and billing interval.
/// <para>
/// A single Component can have multiple price points, allowing different subscriptions to be billed for the same component under different terms.
/// </para>
/// </summary>
public sealed class ComponentPricePoints
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal ComponentPricePoints(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Archive Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ArchiveComponentPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Archives a component price point. Subscriptions using a price point that has been archived will continue using it until they're moved to another price point.
    /// </remarks>
    public Task<ComponentPricePointResponse> ArchiveComponentPricePoint(ArchiveComponentPricePointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentPricePointResponse>(),
            ArchiveComponentPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Bulk Create Component Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="BulkCreateComponentPricePointsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates multiple component price points in one request.
    /// </remarks>
    public Task<ComponentPricePointsResponse> BulkCreateComponentPricePoints(BulkCreateComponentPricePointsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/bulk.json"),
            [new TemplateParam("component_id", request.ComponentId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentPricePointsResponse>(),
            BulkCreateComponentPricePointsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Clone Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointCurrencyOverageResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CloneComponentPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Clones a component price point. Custom price points (tied to a specific subscription) cannot be cloned. The following attributes are copied from the source price point:
    /// - Pricing scheme
    /// - All price tiers (with starting/ending quantities and unit prices)
    /// - Tax included setting
    /// - Currency prices (if definitive pricing is set)
    /// - Overage pricing (for prepaid usage components)
    /// - Interval settings (if multi-frequency is enabled)
    /// - Event-based billing segments (if applicable)
    /// </remarks>
    public Task<ComponentPricePointCurrencyOverageResponse> CloneComponentPricePoint(CloneComponentPricePointOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/clone.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentPricePointCurrencyOverageResponse>(),
            CloneComponentPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateComponentPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a price point for an existing component.
    /// </remarks>
    public Task<ComponentPricePointResponse> CreateComponentPricePoint(CreateComponentPricePointOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points.json"),
            [new TemplateParam("component_id", request.ComponentId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentPricePointResponse>(),
            CreateComponentPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Currency Prices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentCurrencyPricesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateCurrencyPricesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates currency prices for a given currency defined at the site level.
    /// <para>
    /// When creating currency prices, they need to mirror the structure of your primary pricing. For each price level defined on the component price point, there should be a matching price level created in the given currency.
    /// </para>
    /// <para>
    /// Note: Currency Prices are not able to be created for custom price points.
    /// </para>
    /// </remarks>
    public Task<ComponentCurrencyPricesResponse> CreateCurrencyPrices(CreateCurrencyPricesOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/price_points/{price_point_id}/currency_prices.json"),
            [new TemplateParam("price_point_id", request.PricePointId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentCurrencyPricesResponse>(),
            CreateCurrencyPricesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List All Components Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListComponentsPricePointsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListAllComponentPricePointsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists all component price points belonging to a site.
    /// </remarks>
    public Task<ListComponentsPricePointsResponse> ListAllComponentPricePoints(ListAllComponentPricePointsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components_price_points.json"),
            [],
            [
                new Param("include", request.Include),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("direction", request.Direction),
                new Param("filter", request.Filter),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListComponentsPricePointsResponse>(),
            ListAllComponentPricePointsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Component Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the price points associated with a component.
    /// <para>
    /// You may specify the component by using either the numeric id or the <c>handle:gold</c> syntax.
    /// </para>
    /// <para>
    /// If the price point is set to <c>use_site_exchange_rate: true</c>, it will return pricing based on the current exchange rate. If the flag is set to false, it will return all of the defined prices for each currency.
    /// </para>
    /// </remarks>
    public Task<ComponentPricePointsResponse> ListComponentPricePoints(ListComponentPricePointsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points.json"),
            [new TemplateParam("component_id", request.ComponentId)],
            [
                new Param("currency_prices", request.CurrencyPrices),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("filter[type]", request.FilterType),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentPricePointsResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Promote Price Point to Default
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Sets a new default price point for the component. This new default will apply to all new subscriptions going forward - existing subscriptions will remain on their current price point.
    /// <para>
    /// See <see href="https://maxio.zendesk.com/hc/en-us/articles/24261191737101-Price-Points-Components">Price Points Documentation</see> for more information on price points and moving subscriptions between price points.
    /// </para>
    /// <para>
    /// Note: Custom price points are not able to be set as the default for a component.
    /// </para>
    /// </remarks>
    public Task<ComponentResponse> PromoteComponentPricePointToDefault(PromoteComponentPricePointToDefaultRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/default.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointCurrencyOverageResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns details for a specific component price point. You can achieve this by using either the component price point ID or handle.
    /// </remarks>
    public Task<ComponentPricePointCurrencyOverageResponse> ReadComponentPricePoint(ReadComponentPricePointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [new Param("currency_prices", request.CurrencyPrices)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentPricePointCurrencyOverageResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Unarchive Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Unarchives a component price point.
    /// </remarks>
    public Task<ComponentPricePointResponse> UnarchiveComponentPricePoint(UnarchiveComponentPricePointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/unarchive.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentPricePointResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateComponentPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates a component price point and its associated prices.
    /// <para>
    /// Passing in a price bracket without an <c>id</c> will attempt to create a new price.
    /// </para>
    /// <para>
    /// Including an <c>id</c> will update the corresponding price, and including the <c>_destroy</c> flag set to true along with the <c>id</c> will remove that price.
    /// </para>
    /// <para>
    /// Note: Custom price points cannot be updated directly. They must be edited through the Subscription.
    /// </para>
    /// </remarks>
    public Task<ComponentPricePointResponse> UpdateComponentPricePoint(UpdateComponentPricePointOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentPricePointResponse>(),
            UpdateComponentPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Currency Prices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentCurrencyPricesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateCurrencyPricesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates currency prices for a given currency defined at the site level.
    /// <para>
    /// Note: Currency Prices are not able to be updated for custom price points.
    /// </para>
    /// </remarks>
    public Task<ComponentCurrencyPricesResponse> UpdateCurrencyPrices(UpdateCurrencyPricesOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/price_points/{price_point_id}/currency_prices.json"),
            [new TemplateParam("price_point_id", request.PricePointId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentCurrencyPricesResponse>(),
            UpdateCurrencyPricesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
