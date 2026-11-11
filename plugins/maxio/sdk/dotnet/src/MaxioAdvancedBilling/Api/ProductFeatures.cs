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
using MaxioAdvancedBilling.Requests.ProductFeatures;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Product Feature object attaches a Feature Template to a Product as a feature catalog item with a concrete value, granting that feature to subscribers through the product.
/// <para>
/// A product feature applies to the whole product by default, or can be scoped to a single product price point as an override. For more information, see <see href="https://docs.maxio.com/billing/catalog/entitlements/configure-entitlements">Configure Entitlements</see>.
/// </para>
/// </summary>
public sealed class ProductFeatures
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal ProductFeatures(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Product Feature Catalog Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureCatalogItemResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateProductFeatureError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Attaches a feature template to this product with a concrete value. Pass <c>price_point_type: "ProductPricePoint"</c> and <c>price_point_id</c> to create an override scoped to a single product price point instead of the whole product.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The request body of a feature catalog item create names the owning product or component → Maxio answers 422. Leave the owning product or component out of the body; it comes from the URL.</description></item>
    ///   <item><description>Only one of price_point_type and price_point_id is sent when creating a feature catalog item → Maxio answers 422. Send price_point_type and price_point_id together to scope an override, or neither.</description></item>
    ///   <item><description>A feature is attached to a product or component that subscribers already have → Subscribers are provisioned the entitlement automatically only the next time their subscription changes (for example signup, plan change or component allocation). Do not expect existing subscribers to get a new feature until their subscription next changes.</description></item>
    ///   <item><description>A feature catalog item is created without price_point_type → The item applies to every price point of the product or component. Omit price_point_type and price_point_id for a product-wide feature.</description></item>
    ///   <item><description>A feature catalog item is created without propagate_to_subscriptions=true → Existing subscribers on the product or component get the entitlement only at their next subscription change (signup, plan change, component allocation). Send propagate_to_subscriptions=true when existing subscribers must get the feature now.</description></item>
    ///   <item><description>A feature catalog item is created with propagate_to_subscriptions=true → Existing subscriptions on the product or component are granted the entitlement immediately. Check subscriptions_count on the list response before propagating to many subscriptions.</description></item>
    /// </list>
    /// </remarks>
    public Task<FeatureCatalogItemResponse> CreateProductFeature(CreateProductFeatureRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/features.json"),
            [new TemplateParam("product_id", request.ProductId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<FeatureCatalogItemResponse>(),
            CreateProductFeatureError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Product Feature Catalog Items
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureCatalogItemsListResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListProductFeaturesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the feature catalog items attached to this product, including price-point-specific overrides.
    /// </remarks>
    public Task<FeatureCatalogItemsListResponse> ListProductFeatures(ListProductFeaturesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/features.json"),
            [new TemplateParam("product_id", request.ProductId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<FeatureCatalogItemsListResponse>(),
            ListProductFeaturesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Product Feature Catalog Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureCatalogItemResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadProductFeatureError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a single feature catalog item attached to this product.
    /// </remarks>
    public Task<FeatureCatalogItemResponse> ReadProductFeature(ReadProductFeatureRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/features/{id}.json"),
            [new TemplateParam("product_id", request.ProductId), new TemplateParam("id", request.Id)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<FeatureCatalogItemResponse>(),
            ReadProductFeatureError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Remove Product Feature Catalog Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RemoveProductFeatureError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Removes a feature catalog item from this product.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A component feature catalog item is removed with destroy_entitlements=true → The item and every entitlement it created are permanently deleted and subscriber access is revoked immediately. Use destroy_entitlements=true only when immediate, permanent revocation is intended.</description></item>
    ///   <item><description>A feature catalog item is removed without destroy_entitlements=true → Removal is soft: the feature leaves the catalog but subscribers who already have it keep their entitlement. Pass destroy_entitlements=true when access must be revoked immediately.</description></item>
    ///   <item><description>A feature catalog item is removed without destroy_entitlements (or with false) → The item is archived and subscribers who already have the entitlement keep it. Expect existing subscribers to keep the feature after a default removal.</description></item>
    /// </list>
    /// </remarks>
    public Task RemoveProductFeature(RemoveProductFeatureRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/features/{id}.json"),
            [new TemplateParam("product_id", request.ProductId), new TemplateParam("id", request.Id)],
            [new Param("destroy_entitlements", request.DestroyEntitlements)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            RemoveProductFeatureError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Restore Product Feature Catalog Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureCatalogItemResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RestoreProductFeatureError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Clears the archived state of a feature catalog item attached to this product. Returns <c>422</c> if the parent feature template is still archived. Restore the feature template first.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>restoreProductFeature is called while the item's parent feature template is still archived → Maxio answers 422. Restore the feature template first, then the product feature catalog item.</description></item>
    /// </list>
    /// </remarks>
    public Task<FeatureCatalogItemResponse> RestoreProductFeature(RestoreProductFeatureRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/features/{id}/restore.json"),
            [new TemplateParam("product_id", request.ProductId), new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<FeatureCatalogItemResponse>(),
            RestoreProductFeatureError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Product Feature Catalog Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureCatalogItemResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateProductFeatureError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the value or periodicity of a feature catalog item attached to this product.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A component or product feature catalog item is updated with propagate_to_subscriptions true → The new value/periodicity is applied immediately to every existing entitlement created from the item. Send propagate_to_subscriptions=true only when existing entitlements must change now; check subscriptions_count first to see how many subscriptions it touches.</description></item>
    /// </list>
    /// </remarks>
    public Task<FeatureCatalogItemResponse> UpdateProductFeature(UpdateProductFeatureRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/products/{product_id}/features/{id}.json"),
            [new TemplateParam("product_id", request.ProductId), new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<FeatureCatalogItemResponse>(),
            UpdateProductFeatureError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
