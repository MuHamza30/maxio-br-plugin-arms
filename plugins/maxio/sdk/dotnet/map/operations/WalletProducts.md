<!-- Generated file — do not edit; regenerated with the SDK. -->

# WalletProducts — operations

Accessor: `client.WalletProducts` · Source: `src/MaxioAdvancedBilling/Api/WalletProducts.cs` · 7 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ArchiveWalletProduct

- **Auth**: `options.BasicAuth`
- **Signature**: `ArchiveWalletProduct(ArchiveWalletProductRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `WalletProductResponse`
- **Error**: `ApiException<ArchiveWalletProductError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ArchiveWalletProductRequest` | `src/MaxioAdvancedBilling/Requests/WalletProducts/ArchiveWalletProductRequest.cs` |
| `WalletProductResponse` | `src/MaxioAdvancedBilling/Models/WalletProductResponse.cs` |
| `ArchiveWalletProductError` | `src/MaxioAdvancedBilling/Errors/ArchiveWalletProductError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### CreateWalletProduct

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateWalletProduct(CreateWalletProductOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `WalletProductResponse`
- **Error**: `ApiException<CreateWalletProductError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateWalletProductOperationRequest` | `src/MaxioAdvancedBilling/Requests/WalletProducts/CreateWalletProductOperationRequest.cs` |
| `CreateWalletProductRequest` | `src/MaxioAdvancedBilling/Models/CreateWalletProductRequest.cs` |
| `WalletProductResponse` | `src/MaxioAdvancedBilling/Models/WalletProductResponse.cs` |
| `CreateWalletProductError` | `src/MaxioAdvancedBilling/Errors/CreateWalletProductError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### ListWalletProducts

- **Auth**: `options.BasicAuth`
- **Signature**: `ListWalletProducts(ListWalletProductsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `include_archived` ← `IncludeArchived`, `status` ← `Status`, `q` ← `Q`, `sort_by` ← `SortBy`, `sort_direction` ← `SortDirection`, `page` ← `Page`, `per_page` ← `PerPage`
- **Returns**: `WalletProductsListResponse`
- **Error**: `ApiException<ListWalletProductsError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListWalletProductsRequest` | `src/MaxioAdvancedBilling/Requests/WalletProducts/ListWalletProductsRequest.cs` |
| `Status1` | `src/MaxioAdvancedBilling/Models/Enums/Status1.cs` |
| `SortBy1` | `src/MaxioAdvancedBilling/Models/Enums/SortBy1.cs` |
| `SortDirection` | `src/MaxioAdvancedBilling/Models/Enums/SortDirection.cs` |
| `WalletProductsListResponse` | `src/MaxioAdvancedBilling/Models/WalletProductsListResponse.cs` |
| `ListWalletProductsError` | `src/MaxioAdvancedBilling/Errors/ListWalletProductsError.cs` |

### ReadWalletProduct

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadWalletProduct(ReadWalletProductRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `WalletProductResponse`
- **Error**: `ApiException<ReadWalletProductError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadWalletProductRequest` | `src/MaxioAdvancedBilling/Requests/WalletProducts/ReadWalletProductRequest.cs` |
| `WalletProductResponse` | `src/MaxioAdvancedBilling/Models/WalletProductResponse.cs` |
| `ReadWalletProductError` | `src/MaxioAdvancedBilling/Errors/ReadWalletProductError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### ReadWalletProductUsage

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadWalletProductUsage(ReadWalletProductUsageRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `WalletProductUsageResponse`
- **Error**: `ApiException<ReadWalletProductUsageError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadWalletProductUsageRequest` | `src/MaxioAdvancedBilling/Requests/WalletProducts/ReadWalletProductUsageRequest.cs` |
| `WalletProductUsageResponse` | `src/MaxioAdvancedBilling/Models/WalletProductUsageResponse.cs` |
| `ReadWalletProductUsageError` | `src/MaxioAdvancedBilling/Errors/ReadWalletProductUsageError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### UnarchiveWalletProduct

- **Auth**: `options.BasicAuth`
- **Signature**: `UnarchiveWalletProduct(UnarchiveWalletProductRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `WalletProductResponse`
- **Error**: `ApiException<UnarchiveWalletProductError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UnarchiveWalletProductRequest` | `src/MaxioAdvancedBilling/Requests/WalletProducts/UnarchiveWalletProductRequest.cs` |
| `WalletProductResponse` | `src/MaxioAdvancedBilling/Models/WalletProductResponse.cs` |
| `UnarchiveWalletProductError` | `src/MaxioAdvancedBilling/Errors/UnarchiveWalletProductError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### UpdateWalletProduct

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateWalletProduct(UpdateWalletProductOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `WalletProductResponse`
- **Error**: `ApiException<UpdateWalletProductError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateWalletProductOperationRequest` | `src/MaxioAdvancedBilling/Requests/WalletProducts/UpdateWalletProductOperationRequest.cs` |
| `UpdateWalletProductRequest` | `src/MaxioAdvancedBilling/Models/UpdateWalletProductRequest.cs` |
| `WalletProductResponse` | `src/MaxioAdvancedBilling/Models/WalletProductResponse.cs` |
| `UpdateWalletProductError` | `src/MaxioAdvancedBilling/Errors/UpdateWalletProductError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

