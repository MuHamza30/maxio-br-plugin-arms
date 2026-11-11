using System;
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
using MaxioAdvancedBilling.Requests.ComponentFeatures;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Component Feature object attaches a Feature Template to a Component as a feature catalog item with a concrete value, granting that feature to subscribers through the component.
/// <para>
/// A component feature applies to the whole component by default, or can be scoped to a single component price point as an override. For more information, see <see href="https://docs.maxio.com/billing/catalog/entitlements/configure-entitlements">Configure Entitlements</see>.
/// </para>
/// </summary>
public sealed class ComponentFeatures
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal ComponentFeatures(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Component Feature Catalog Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureCatalogItemResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateComponentFeatureError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Attaches a feature template to this component with a concrete value. Pass <c>price_point_type: "PricePoint"</c> and <c>price_point_id</c> to create an override scoped to a single component price point instead of the whole component.
    /// </remarks>
    public Task<FeatureCatalogItemResponse> CreateComponentFeature(CreateComponentFeatureRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/features.json"),
            [new TemplateParam("component_id", request.ComponentId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<FeatureCatalogItemResponse>(),
            CreateComponentFeatureError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Component Feature Catalog Items
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureCatalogItemsListResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListComponentFeaturesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the feature catalog items attached to this component, including price-point-specific overrides.
    /// </remarks>
    public Task<FeatureCatalogItemsListResponse> ListComponentFeatures(ListComponentFeaturesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/features.json"),
            [new TemplateParam("component_id", request.ComponentId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<FeatureCatalogItemsListResponse>(),
            ListComponentFeaturesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Component Feature Catalog Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureCatalogItemResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadComponentFeatureError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a single feature catalog item attached to this component.
    /// </remarks>
    public Task<FeatureCatalogItemResponse> ReadComponentFeature(ReadComponentFeatureRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/features/{id}.json"),
            [new TemplateParam("component_id", request.ComponentId), new TemplateParam("id", request.Id)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<FeatureCatalogItemResponse>(),
            ReadComponentFeatureError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Remove Component Feature Catalog Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RemoveComponentFeatureError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Removes a feature catalog item from this component.
    /// </remarks>
    public Task RemoveComponentFeature(RemoveComponentFeatureRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/features/{id}.json"),
            [new TemplateParam("component_id", request.ComponentId), new TemplateParam("id", request.Id)],
            [new Param("destroy_entitlements", request.DestroyEntitlements)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            RemoveComponentFeatureError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Restore Component Feature Catalog Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureCatalogItemResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RestoreComponentFeatureError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Clears the archived state of a feature catalog item attached to this component. Returns <c>422</c> if the parent feature template is still archived. Restore the feature template first.
    /// </remarks>
    public Task<FeatureCatalogItemResponse> RestoreComponentFeature(RestoreComponentFeatureRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/features/{id}/restore.json"),
            [new TemplateParam("component_id", request.ComponentId), new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<FeatureCatalogItemResponse>(),
            RestoreComponentFeatureError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Component Feature Catalog Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureCatalogItemResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateComponentFeatureError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the value or periodicity of a feature catalog item attached to this component.
    /// </remarks>
    public Task<FeatureCatalogItemResponse> UpdateComponentFeature(UpdateComponentFeatureRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/features/{id}.json"),
            [new TemplateParam("component_id", request.ComponentId), new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<FeatureCatalogItemResponse>(),
            UpdateComponentFeatureError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
