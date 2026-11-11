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
