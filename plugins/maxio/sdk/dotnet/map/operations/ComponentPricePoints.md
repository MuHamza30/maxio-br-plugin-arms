<!-- Generated file — do not edit; regenerated with the SDK. -->

# ComponentPricePoints — operations

Accessor: `client.ComponentPricePoints` · Source: `src/MaxioAdvancedBilling/Api/ComponentPricePoints.cs` · 12 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ArchiveComponentPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `ArchiveComponentPricePoint(ArchiveComponentPricePointRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`
- **Returns**: `ComponentPricePointResponse`
- **Error**: `ApiException<ArchiveComponentPricePointError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ArchiveComponentPricePointRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/ArchiveComponentPricePointRequest.cs` |
| `ComponentIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ComponentIdModel.cs` |
| `PricePointIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/PricePointIdModel.cs` |
| `ComponentPricePointResponse` | `src/MaxioAdvancedBilling/Models/ComponentPricePointResponse.cs` |
| `ArchiveComponentPricePointError` | `src/MaxioAdvancedBilling/Errors/ArchiveComponentPricePointError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### BulkCreateComponentPricePoints

- **Auth**: `options.BasicAuth`
- **Signature**: `BulkCreateComponentPricePoints(BulkCreateComponentPricePointsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`
- **Returns**: `ComponentPricePointsResponse`
- **Error**: `ApiException<BulkCreateComponentPricePointsError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `BulkCreateComponentPricePointsRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/BulkCreateComponentPricePointsRequest.cs` |
| `CreateComponentPricePointsRequest` | `src/MaxioAdvancedBilling/Models/CreateComponentPricePointsRequest.cs` |
| `ComponentPricePointsResponse` | `src/MaxioAdvancedBilling/Models/ComponentPricePointsResponse.cs` |
| `BulkCreateComponentPricePointsError` | `src/MaxioAdvancedBilling/Errors/BulkCreateComponentPricePointsError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### CloneComponentPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `CloneComponentPricePoint(CloneComponentPricePointOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`
- **Returns**: `ComponentPricePointCurrencyOverageResponse`
- **Error**: `ApiException<CloneComponentPricePointError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CloneComponentPricePointOperationRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/CloneComponentPricePointOperationRequest.cs` |
| `ComponentIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ComponentIdModel.cs` |
| `PricePointIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/PricePointIdModel.cs` |
| `CloneComponentPricePointRequest` | `src/MaxioAdvancedBilling/Models/CloneComponentPricePointRequest.cs` |
| `ComponentPricePointCurrencyOverageResponse` | `src/MaxioAdvancedBilling/Models/ComponentPricePointCurrencyOverageResponse.cs` |
| `CloneComponentPricePointError` | `src/MaxioAdvancedBilling/Errors/CloneComponentPricePointError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### CreateComponentPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateComponentPricePoint(CreateComponentPricePointOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`
- **Returns**: `ComponentPricePointResponse`
- **Error**: `ApiException<CreateComponentPricePointError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateComponentPricePointOperationRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/CreateComponentPricePointOperationRequest.cs` |
| `CreateComponentPricePointRequest` | `src/MaxioAdvancedBilling/Models/CreateComponentPricePointRequest.cs` |
| `ComponentPricePointResponse` | `src/MaxioAdvancedBilling/Models/ComponentPricePointResponse.cs` |
| `CreateComponentPricePointError` | `src/MaxioAdvancedBilling/Errors/CreateComponentPricePointError.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### CreateCurrencyPrices

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateCurrencyPrices(CreateCurrencyPricesOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `PricePointId`
- **Returns**: `ComponentCurrencyPricesResponse`
- **Error**: `ApiException<CreateCurrencyPricesError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateCurrencyPricesOperationRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/CreateCurrencyPricesOperationRequest.cs` |
| `CreateCurrencyPricesRequest` | `src/MaxioAdvancedBilling/Models/CreateCurrencyPricesRequest.cs` |
| `ComponentCurrencyPricesResponse` | `src/MaxioAdvancedBilling/Models/ComponentCurrencyPricesResponse.cs` |
| `CreateCurrencyPricesError` | `src/MaxioAdvancedBilling/Errors/CreateCurrencyPricesError.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### ListAllComponentPricePoints

- **Auth**: `options.BasicAuth`
- **Signature**: `ListAllComponentPricePoints(ListAllComponentPricePointsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `include` ← `Include`, `page` ← `Page`, `per_page` ← `PerPage`, `direction` ← `Direction`, `filter` ← `Filter`
- **Returns**: `ListComponentsPricePointsResponse`
- **Error**: `ApiException<ListAllComponentPricePointsError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListAllComponentPricePointsRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/ListAllComponentPricePointsRequest.cs` |
| `ListComponentsPricePointsInclude` | `src/MaxioAdvancedBilling/Models/Enums/ListComponentsPricePointsInclude.cs` |
| `SortingDirection` | `src/MaxioAdvancedBilling/Models/Enums/SortingDirection.cs` |
| `ListPricePointsFilter` | `src/MaxioAdvancedBilling/Models/ListPricePointsFilter.cs` |
| `ListComponentsPricePointsResponse` | `src/MaxioAdvancedBilling/Models/ListComponentsPricePointsResponse.cs` |
| `ListAllComponentPricePointsError` | `src/MaxioAdvancedBilling/Errors/ListAllComponentPricePointsError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListComponentPricePoints

- **Auth**: `options.BasicAuth`
- **Signature**: `ListComponentPricePoints(ListComponentPricePointsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`
- **Query params (wire ← C#)**: `currency_prices` ← `CurrencyPrices`, `page` ← `Page`, `per_page` ← `PerPage`, `filter[type]` ← `FilterType`
- **Returns**: `ComponentPricePointsResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListComponentPricePointsRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/ListComponentPricePointsRequest.cs` |
| `PricePointType` | `src/MaxioAdvancedBilling/Models/Enums/PricePointType.cs` |
| `ComponentPricePointsResponse` | `src/MaxioAdvancedBilling/Models/ComponentPricePointsResponse.cs` |

### PromoteComponentPricePointToDefault

- **Auth**: `options.BasicAuth`
- **Signature**: `PromoteComponentPricePointToDefault(PromoteComponentPricePointToDefaultRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`
- **Returns**: `ComponentResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `PromoteComponentPricePointToDefaultRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/PromoteComponentPricePointToDefaultRequest.cs` |
| `ComponentResponse` | `src/MaxioAdvancedBilling/Models/ComponentResponse.cs` |

### ReadComponentPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadComponentPricePoint(ReadComponentPricePointRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`
- **Query params (wire ← C#)**: `currency_prices` ← `CurrencyPrices`
- **Returns**: `ComponentPricePointCurrencyOverageResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadComponentPricePointRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/ReadComponentPricePointRequest.cs` |
| `ComponentIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ComponentIdModel.cs` |
| `PricePointIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/PricePointIdModel.cs` |
| `ComponentPricePointCurrencyOverageResponse` | `src/MaxioAdvancedBilling/Models/ComponentPricePointCurrencyOverageResponse.cs` |

### UnarchiveComponentPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `UnarchiveComponentPricePoint(UnarchiveComponentPricePointRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`
- **Returns**: `ComponentPricePointResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `UnarchiveComponentPricePointRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/UnarchiveComponentPricePointRequest.cs` |
| `ComponentPricePointResponse` | `src/MaxioAdvancedBilling/Models/ComponentPricePointResponse.cs` |

### UpdateComponentPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateComponentPricePoint(UpdateComponentPricePointOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`
- **Returns**: `ComponentPricePointResponse`
- **Error**: `ApiException<UpdateComponentPricePointError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateComponentPricePointOperationRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/UpdateComponentPricePointOperationRequest.cs` |
| `ComponentIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ComponentIdModel.cs` |
| `PricePointIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/PricePointIdModel.cs` |
| `UpdateComponentPricePointRequest` | `src/MaxioAdvancedBilling/Models/UpdateComponentPricePointRequest.cs` |
| `ComponentPricePointResponse` | `src/MaxioAdvancedBilling/Models/ComponentPricePointResponse.cs` |
| `UpdateComponentPricePointError` | `src/MaxioAdvancedBilling/Errors/UpdateComponentPricePointError.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### UpdateCurrencyPrices

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateCurrencyPrices(UpdateCurrencyPricesOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `PricePointId`
- **Returns**: `ComponentCurrencyPricesResponse`
- **Error**: `ApiException<UpdateCurrencyPricesError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateCurrencyPricesOperationRequest` | `src/MaxioAdvancedBilling/Requests/ComponentPricePoints/UpdateCurrencyPricesOperationRequest.cs` |
| `UpdateCurrencyPricesRequest` | `src/MaxioAdvancedBilling/Models/UpdateCurrencyPricesRequest.cs` |
| `ComponentCurrencyPricesResponse` | `src/MaxioAdvancedBilling/Models/ComponentCurrencyPricesResponse.cs` |
| `UpdateCurrencyPricesError` | `src/MaxioAdvancedBilling/Errors/UpdateCurrencyPricesError.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

