<!-- Generated file — do not edit; regenerated with the SDK. -->

# Products — operations

Accessor: `client.Products` · Source: `src/MaxioAdvancedBilling/Api/Products.cs` · 6 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ArchiveProduct

- **Auth**: `options.BasicAuth`
- **Signature**: `ArchiveProduct(ArchiveProductRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`
- **Returns**: `ProductResponse`
- **Error**: `ApiException<ArchiveProductError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ArchiveProductRequest` | `src/MaxioAdvancedBilling/Requests/Products/ArchiveProductRequest.cs` |
| `ProductResponse` | `src/MaxioAdvancedBilling/Models/ProductResponse.cs` |
| `ArchiveProductError` | `src/MaxioAdvancedBilling/Errors/ArchiveProductError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### CreateProduct

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateProduct(CreateProductRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductFamilyId`
- **Returns**: `ProductResponse`
- **Error**: `ApiException<CreateProductError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateProductRequest` | `src/MaxioAdvancedBilling/Requests/Products/CreateProductRequest.cs` |
| `CreateOrUpdateProductRequest` | `src/MaxioAdvancedBilling/Models/CreateOrUpdateProductRequest.cs` |
| `ProductResponse` | `src/MaxioAdvancedBilling/Models/ProductResponse.cs` |
| `CreateProductError` | `src/MaxioAdvancedBilling/Errors/CreateProductError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListProducts

- **Auth**: `options.BasicAuth`
- **Signature**: `ListProducts(ListProductsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `date_field` ← `DateField`, `filter` ← `Filter`, `end_date` ← `EndDate`, `end_datetime` ← `EndDatetime`, `start_date` ← `StartDate`, `start_datetime` ← `StartDatetime`, `page` ← `Page`, `per_page` ← `PerPage`, `include_archived` ← `IncludeArchived`, `include` ← `Include`, `include_features` ← `IncludeFeatures`
- **Returns**: `IReadOnlyList<ProductResponse>`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListProductsRequest` | `src/MaxioAdvancedBilling/Requests/Products/ListProductsRequest.cs` |
| `BasicDateField` | `src/MaxioAdvancedBilling/Models/Enums/BasicDateField.cs` |
| `ListProductsFilter` | `src/MaxioAdvancedBilling/Models/ListProductsFilter.cs` |
| `ListProductsInclude` | `src/MaxioAdvancedBilling/Models/Enums/ListProductsInclude.cs` |
| `ProductResponse` | `src/MaxioAdvancedBilling/Models/ProductResponse.cs` |

### ReadProduct

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadProduct(ReadProductRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`
- **Query params (wire ← C#)**: `include_features` ← `IncludeFeatures`
- **Returns**: `ProductResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadProductRequest` | `src/MaxioAdvancedBilling/Requests/Products/ReadProductRequest.cs` |
| `ProductResponse` | `src/MaxioAdvancedBilling/Models/ProductResponse.cs` |

### ReadProductByHandle

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadProductByHandle(ReadProductByHandleRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ApiHandle`
- **Returns**: `ProductResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadProductByHandleRequest` | `src/MaxioAdvancedBilling/Requests/Products/ReadProductByHandleRequest.cs` |
| `ProductResponse` | `src/MaxioAdvancedBilling/Models/ProductResponse.cs` |

### UpdateProduct

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateProduct(UpdateProductRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductId`
- **Returns**: `ProductResponse`
- **Error**: `ApiException<UpdateProductError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateProductRequest` | `src/MaxioAdvancedBilling/Requests/Products/UpdateProductRequest.cs` |
| `CreateOrUpdateProductRequest` | `src/MaxioAdvancedBilling/Models/CreateOrUpdateProductRequest.cs` |
| `ProductResponse` | `src/MaxioAdvancedBilling/Models/ProductResponse.cs` |
| `UpdateProductError` | `src/MaxioAdvancedBilling/Errors/UpdateProductError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

