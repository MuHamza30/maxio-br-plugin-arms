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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The create request body names the owning product or component → the outcome is not stated. Leave the owning component out of the body; the server takes it from the URL.</description></item>
    ///   <item><description>Only one of price_point_type and price_point_id is sent → the outcome is not stated. Send price_point_type and price_point_id together, or neither.</description></item>
    ///   <item><description>A component feature override is scoped to a price point with a price_point_type other than PricePoint → the outcome is not stated. Use price_point_type "PricePoint" (a component price point) for component overrides; ProductPricePoint is for products.</description></item>
    ///   <item><description>The feature value does not fit the feature template's kind ("true"/"false" for access_right, a numeric string for usage_limit) → the outcome is not stated. Send value as "true"/"false" for access_right, a numeric string for usage_limit, and a string shaped by value_type for service_right.</description></item>
    ///   <item><description>The request body of a feature catalog item create names the owning product or component → Maxio answers 422. Leave the owning product or component out of the body; it comes from the URL.</description></item>
    ///   <item><description>Only one of price_point_type and price_point_id is sent when creating a feature catalog item → Maxio answers 422. Send price_point_type and price_point_id together to scope an override, or neither.</description></item>
    ///   <item><description>A feature is attached to a product or component that subscribers already have → Subscribers are provisioned the entitlement automatically only the next time their subscription changes (for example signup, plan change or component allocation). Do not expect existing subscribers to get a new feature until their subscription next changes.</description></item>
    ///   <item><description>A component feature catalog item is created without price_point_type (and price_point_id) → The item applies to every price point of the component. Send price_point_type and price_point_id only when the feature must be scoped to one component price point.</description></item>
    ///   <item><description>A feature is attached to a component without propagate_to_subscriptions (default false) → Existing subscriptions on the component get the entitlement only at their next subscription change (signup, plan change or component allocation), not immediately. Send propagate_to_subscriptions=true when existing subscribers must get the feature now.</description></item>
    ///   <item><description>The feature template's kind is not usage_limit → periodicity_interval and periodicity_unit are null on the feature catalog item. Send a periodicity only for usage_limit features and expect null periodicity for the other kinds.</description></item>
    ///   <item><description>A feature catalog item is created without price_point_type → The item applies to every price point of the product or component. Omit price_point_type and price_point_id for a product-wide feature.</description></item>
    ///   <item><description>A feature catalog item is created without propagate_to_subscriptions=true → Existing subscribers on the product or component get the entitlement only at their next subscription change (signup, plan change, component allocation). Send propagate_to_subscriptions=true when existing subscribers must get the feature now.</description></item>
    ///   <item><description>A feature catalog item is created with propagate_to_subscriptions=true → Existing subscriptions on the product or component are granted the entitlement immediately. Check subscriptions_count on the list response before propagating to many subscriptions.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The feature template's kind is not usage_limit → periodicity_interval and periodicity_unit are null on the feature catalog item. Send a periodicity only for usage_limit features and expect null periodicity for the other kinds.</description></item>
    ///   <item><description>The component has price-point-specific feature overrides → The list returns the overrides together with the component-wide items. Read price_point_type/price_point_id on each item to tell overrides from component-wide items.</description></item>
    ///   <item><description>The component's feature catalog items are listed → subscriptions_count is the number of subscriptions on the component that a change propagated with propagate_to_subscriptions=true would affect. Use subscriptions_count to judge the impact before propagating a feature change.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The feature template's kind is not usage_limit → periodicity_interval and periodicity_unit are null on the feature catalog item. Send a periodicity only for usage_limit features and expect null periodicity for the other kinds.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A component feature catalog item is removed without destroy_entitlements (default false) → The item is archived and existing subscribers keep their entitlement (grandfathered). Send destroy_entitlements=true if subscribers must lose the feature; otherwise expect existing entitlements to remain.</description></item>
    ///   <item><description>A component feature catalog item is removed with destroy_entitlements=true → The item and every entitlement it created are permanently deleted and subscriber access is revoked immediately. Use destroy_entitlements=true only when immediate, permanent revocation is intended.</description></item>
    ///   <item><description>A feature catalog item is removed without destroy_entitlements=true → Removal is soft: the feature leaves the catalog but subscribers who already have it keep their entitlement. Pass destroy_entitlements=true when access must be revoked immediately.</description></item>
    ///   <item><description>A feature catalog item is removed without destroy_entitlements (or with false) → The item is archived and subscribers who already have the entitlement keep it. Expect existing subscribers to keep the feature after a default removal.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The parent feature template of the archived component feature catalog item is still archived → Maxio answers 422. Restore the feature template before restoring the component feature catalog item.</description></item>
    ///   <item><description>An archived component feature catalog item is restored → The item's archived state is cleared. Call restore on an archived item to make it active again.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The feature value does not fit the feature template's kind ("true"/"false" for access_right, a numeric string for usage_limit) → the outcome is not stated. Send value as "true"/"false" for access_right, a numeric string for usage_limit, and a string shaped by value_type for service_right.</description></item>
    ///   <item><description>The feature template's kind is not usage_limit → periodicity_interval and periodicity_unit are null on the feature catalog item. Send a periodicity only for usage_limit features and expect null periodicity for the other kinds.</description></item>
    ///   <item><description>A component or product feature catalog item is updated with propagate_to_subscriptions true → The new value/periodicity is applied immediately to every existing entitlement created from the item. Send propagate_to_subscriptions=true only when existing entitlements must change now; check subscriptions_count first to see how many subscriptions it touches.</description></item>
    /// </list>
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
