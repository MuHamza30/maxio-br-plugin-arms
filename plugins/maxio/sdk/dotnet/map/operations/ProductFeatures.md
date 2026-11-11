<!-- Generated file — do not edit; regenerated with the SDK. -->

# ProductFeatures — operations

Accessor: `client.ProductFeatures` · Source: `src/MaxioAdvancedBilling/Api/ProductFeatures.cs` · 6 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateProductFeature

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateProductFeature(CreateProductFeatureRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`
- **Returns**: `FeatureCatalogItemResponse`
- **Error**: `ApiException<CreateProductFeatureError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [403, 422] · `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateProductFeatureRequest` | `src/MaxioAdvancedBilling/Requests/ProductFeatures/CreateProductFeatureRequest.cs` |
| `CreateFeatureCatalogItemRequest` | `src/MaxioAdvancedBilling/Models/CreateFeatureCatalogItemRequest.cs` |
| `FeatureCatalogItemResponse` | `src/MaxioAdvancedBilling/Models/FeatureCatalogItemResponse.cs` |
| `CreateProductFeatureError` | `src/MaxioAdvancedBilling/Errors/CreateProductFeatureError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListProductFeatures

- **Auth**: `options.BasicAuth`
- **Signature**: `ListProductFeatures(ListProductFeaturesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`
- **Returns**: `FeatureCatalogItemsListResponse`
- **Error**: `ApiException<ListProductFeaturesError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [403] · `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListProductFeaturesRequest` | `src/MaxioAdvancedBilling/Requests/ProductFeatures/ListProductFeaturesRequest.cs` |
| `FeatureCatalogItemsListResponse` | `src/MaxioAdvancedBilling/Models/FeatureCatalogItemsListResponse.cs` |
| `ListProductFeaturesError` | `src/MaxioAdvancedBilling/Errors/ListProductFeaturesError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReadProductFeature

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadProductFeature(ReadProductFeatureRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`, `Id`
- **Returns**: `FeatureCatalogItemResponse`
- **Error**: `ApiException<ReadProductFeatureError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [403] · `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadProductFeatureRequest` | `src/MaxioAdvancedBilling/Requests/ProductFeatures/ReadProductFeatureRequest.cs` |
| `FeatureCatalogItemResponse` | `src/MaxioAdvancedBilling/Models/FeatureCatalogItemResponse.cs` |
| `ReadProductFeatureError` | `src/MaxioAdvancedBilling/Errors/ReadProductFeatureError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### RemoveProductFeature

- **Auth**: `options.BasicAuth`
- **Signature**: `RemoveProductFeature(RemoveProductFeatureRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`, `Id`
- **Query params (wire ← C#)**: `destroy_entitlements` ← `DestroyEntitlements`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RemoveProductFeatureError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [403] · `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `RemoveProductFeatureRequest` | `src/MaxioAdvancedBilling/Requests/ProductFeatures/RemoveProductFeatureRequest.cs` |
| `RemoveProductFeatureError` | `src/MaxioAdvancedBilling/Errors/RemoveProductFeatureError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### RestoreProductFeature

- **Auth**: `options.BasicAuth`
- **Signature**: `RestoreProductFeature(RestoreProductFeatureRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`, `Id`
- **Returns**: `FeatureCatalogItemResponse`
- **Error**: `ApiException<RestoreProductFeatureError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [403, 422] · `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `RestoreProductFeatureRequest` | `src/MaxioAdvancedBilling/Requests/ProductFeatures/RestoreProductFeatureRequest.cs` |
| `FeatureCatalogItemResponse` | `src/MaxioAdvancedBilling/Models/FeatureCatalogItemResponse.cs` |
| `RestoreProductFeatureError` | `src/MaxioAdvancedBilling/Errors/RestoreProductFeatureError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### UpdateProductFeature

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateProductFeature(UpdateProductFeatureRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`, `Id`
- **Returns**: `FeatureCatalogItemResponse`
- **Error**: `ApiException<UpdateProductFeatureError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [403, 422] · `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateProductFeatureRequest` | `src/MaxioAdvancedBilling/Requests/ProductFeatures/UpdateProductFeatureRequest.cs` |
| `UpdateFeatureCatalogItemRequest` | `src/MaxioAdvancedBilling/Models/UpdateFeatureCatalogItemRequest.cs` |
| `FeatureCatalogItemResponse` | `src/MaxioAdvancedBilling/Models/FeatureCatalogItemResponse.cs` |
| `UpdateProductFeatureError` | `src/MaxioAdvancedBilling/Errors/UpdateProductFeatureError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

