<!-- Generated file — do not edit; regenerated with the SDK. -->

# ProductPricePoints — operations

Accessor: `client.ProductPricePoints` · Source: `src/MaxioAdvancedBilling/Api/ProductPricePoints.cs` · 11 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ArchiveProductPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `ArchiveProductPricePoint(ArchiveProductPricePointRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`, `PricePointId`
- **Returns**: `ProductPricePointResponse`
- **Error**: `ApiException<ArchiveProductPricePointError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ArchiveProductPricePointRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/ArchiveProductPricePointRequest.cs` |
| `ProductIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ProductIdModel.cs` |
| `PricePointIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/PricePointIdModel.cs` |
| `ProductPricePointResponse` | `src/MaxioAdvancedBilling/Models/ProductPricePointResponse.cs` |
| `ArchiveProductPricePointError` | `src/MaxioAdvancedBilling/Errors/ArchiveProductPricePointError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### BulkCreateProductPricePoints

- **Auth**: `options.BasicAuth`
- **Signature**: `BulkCreateProductPricePoints(BulkCreateProductPricePointsOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`
- **Returns**: `BulkCreateProductPricePointsResponse`
- **Error**: `ApiException<BulkCreateProductPricePointsError>` — **Case A (typed)**
- **Error accessors**: `TryGetMapOfJsonElement(out IReadOnlyDictionary<string, JsonElement>)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `BulkCreateProductPricePointsOperationRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/BulkCreateProductPricePointsOperationRequest.cs` |
| `BulkCreateProductPricePointsRequest` | `src/MaxioAdvancedBilling/Models/BulkCreateProductPricePointsRequest.cs` |
| `BulkCreateProductPricePointsResponse` | `src/MaxioAdvancedBilling/Models/BulkCreateProductPricePointsResponse.cs` |
| `BulkCreateProductPricePointsError` | `src/MaxioAdvancedBilling/Errors/BulkCreateProductPricePointsError.cs` |

### CreateProductCurrencyPrices

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateProductCurrencyPrices(CreateProductCurrencyPricesOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductPricePointId`
- **Returns**: `CurrencyPricesResponse`
- **Error**: `ApiException<CreateProductCurrencyPricesError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateProductCurrencyPricesOperationRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/CreateProductCurrencyPricesOperationRequest.cs` |
| `CreateProductCurrencyPricesRequest` | `src/MaxioAdvancedBilling/Models/CreateProductCurrencyPricesRequest.cs` |
| `CurrencyPricesResponse` | `src/MaxioAdvancedBilling/Models/CurrencyPricesResponse.cs` |
| `CreateProductCurrencyPricesError` | `src/MaxioAdvancedBilling/Errors/CreateProductCurrencyPricesError.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### CreateProductPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateProductPricePoint(CreateProductPricePointOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`
- **Returns**: `ProductPricePointResponse`
- **Error**: `ApiException<CreateProductPricePointError>` — **Case A (typed)**
- **Error accessors**: `TryGetProductPricePointErrorResponse1(out ProductPricePointErrorResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateProductPricePointOperationRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/CreateProductPricePointOperationRequest.cs` |
| `ProductIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ProductIdModel.cs` |
| `CreateProductPricePointRequest` | `src/MaxioAdvancedBilling/Models/CreateProductPricePointRequest.cs` |
| `ProductPricePointResponse` | `src/MaxioAdvancedBilling/Models/ProductPricePointResponse.cs` |
| `CreateProductPricePointError` | `src/MaxioAdvancedBilling/Errors/CreateProductPricePointError.cs` |
| `ProductPricePointErrorResponse1` | `src/MaxioAdvancedBilling/Models/ProductPricePointErrorResponse1.cs` |

### ListAllProductPricePoints

- **Auth**: `options.BasicAuth`
- **Signature**: `ListAllProductPricePoints(ListAllProductPricePointsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `direction` ← `Direction`, `filter` ← `Filter`, `include` ← `Include`, `page` ← `Page`, `per_page` ← `PerPage`
- **Returns**: `ListProductPricePointsResponse`
- **Error**: `ApiException<ListAllProductPricePointsError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListAllProductPricePointsRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/ListAllProductPricePointsRequest.cs` |
| `SortingDirection` | `src/MaxioAdvancedBilling/Models/Enums/SortingDirection.cs` |
| `ListPricePointsFilter` | `src/MaxioAdvancedBilling/Models/ListPricePointsFilter.cs` |
| `ListProductsPricePointsInclude` | `src/MaxioAdvancedBilling/Models/Enums/ListProductsPricePointsInclude.cs` |
| `ListProductPricePointsResponse` | `src/MaxioAdvancedBilling/Models/ListProductPricePointsResponse.cs` |
| `ListAllProductPricePointsError` | `src/MaxioAdvancedBilling/Errors/ListAllProductPricePointsError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListProductPricePoints

- **Auth**: `options.BasicAuth`
- **Signature**: `ListProductPricePoints(ListProductPricePointsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `currency_prices` ← `CurrencyPrices`, `filter[type]` ← `FilterType`, `archived` ← `Archived`
- **Returns**: `ListProductPricePointsResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListProductPricePointsRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/ListProductPricePointsRequest.cs` |
| `ProductIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ProductIdModel.cs` |
| `PricePointType` | `src/MaxioAdvancedBilling/Models/Enums/PricePointType.cs` |
| `ListProductPricePointsResponse` | `src/MaxioAdvancedBilling/Models/ListProductPricePointsResponse.cs` |

### PromoteProductPricePointToDefault

- **Auth**: `options.BasicAuth`
- **Signature**: `PromoteProductPricePointToDefault(PromoteProductPricePointToDefaultRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`, `PricePointId`
- **Returns**: `ProductResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `PromoteProductPricePointToDefaultRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/PromoteProductPricePointToDefaultRequest.cs` |
| `ProductResponse` | `src/MaxioAdvancedBilling/Models/ProductResponse.cs` |

### ReadProductPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadProductPricePoint(ReadProductPricePointRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`, `PricePointId`
- **Query params (wire ← C#)**: `currency_prices` ← `CurrencyPrices`
- **Returns**: `ProductPricePointResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadProductPricePointRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/ReadProductPricePointRequest.cs` |
| `ProductIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ProductIdModel.cs` |
| `PricePointIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/PricePointIdModel.cs` |
| `ProductPricePointResponse` | `src/MaxioAdvancedBilling/Models/ProductPricePointResponse.cs` |

### UnarchiveProductPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `UnarchiveProductPricePoint(UnarchiveProductPricePointRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`, `PricePointId`
- **Returns**: `ProductPricePointResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `UnarchiveProductPricePointRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/UnarchiveProductPricePointRequest.cs` |
| `ProductPricePointResponse` | `src/MaxioAdvancedBilling/Models/ProductPricePointResponse.cs` |

### UpdateProductCurrencyPrices

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateProductCurrencyPrices(UpdateProductCurrencyPricesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductPricePointId`
- **Returns**: `CurrencyPricesResponse`
- **Error**: `ApiException<UpdateProductCurrencyPricesError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateProductCurrencyPricesRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/UpdateProductCurrencyPricesRequest.cs` |
| `UpdateCurrencyPricesRequest` | `src/MaxioAdvancedBilling/Models/UpdateCurrencyPricesRequest.cs` |
| `CurrencyPricesResponse` | `src/MaxioAdvancedBilling/Models/CurrencyPricesResponse.cs` |
| `UpdateProductCurrencyPricesError` | `src/MaxioAdvancedBilling/Errors/UpdateProductCurrencyPricesError.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### UpdateProductPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateProductPricePoint(UpdateProductPricePointOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`, `PricePointId`
- **Returns**: `ProductPricePointResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `UpdateProductPricePointOperationRequest` | `src/MaxioAdvancedBilling/Requests/ProductPricePoints/UpdateProductPricePointOperationRequest.cs` |
| `ProductIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ProductIdModel.cs` |
| `PricePointIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/PricePointIdModel.cs` |
| `UpdateProductPricePointRequest` | `src/MaxioAdvancedBilling/Models/UpdateProductPricePointRequest.cs` |
| `ProductPricePointResponse` | `src/MaxioAdvancedBilling/Models/ProductPricePointResponse.cs` |

