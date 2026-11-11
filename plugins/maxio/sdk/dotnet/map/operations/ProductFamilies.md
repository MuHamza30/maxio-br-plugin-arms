<!-- Generated file — do not edit; regenerated with the SDK. -->

# ProductFamilies — operations

Accessor: `client.ProductFamilies` · Source: `src/MaxioAdvancedBilling/Api/ProductFamilies.cs` · 4 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateProductFamily

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateProductFamily(CreateProductFamilyOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `ProductFamilyResponse`
- **Error**: `ApiException<CreateProductFamilyError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateProductFamilyOperationRequest` | `src/MaxioAdvancedBilling/Requests/ProductFamilies/CreateProductFamilyOperationRequest.cs` |
| `CreateProductFamilyRequest` | `src/MaxioAdvancedBilling/Models/CreateProductFamilyRequest.cs` |
| `ProductFamilyResponse` | `src/MaxioAdvancedBilling/Models/ProductFamilyResponse.cs` |
| `CreateProductFamilyError` | `src/MaxioAdvancedBilling/Errors/CreateProductFamilyError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListProductFamilies

- **Auth**: `options.BasicAuth`
- **Signature**: `ListProductFamilies(ListProductFamiliesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `date_field` ← `DateField`, `start_date` ← `StartDate`, `end_date` ← `EndDate`, `start_datetime` ← `StartDatetime`, `end_datetime` ← `EndDatetime`
- **Returns**: `IReadOnlyList<ProductFamilyResponse>`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListProductFamiliesRequest` | `src/MaxioAdvancedBilling/Requests/ProductFamilies/ListProductFamiliesRequest.cs` |
| `BasicDateField` | `src/MaxioAdvancedBilling/Models/Enums/BasicDateField.cs` |
| `ProductFamilyResponse` | `src/MaxioAdvancedBilling/Models/ProductFamilyResponse.cs` |

### ListProductsForProductFamily

- **Auth**: `options.BasicAuth`
- **Signature**: `ListProductsForProductFamily(ListProductsForProductFamilyRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProductFamilyId`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `date_field` ← `DateField`, `filter` ← `Filter`, `start_date` ← `StartDate`, `end_date` ← `EndDate`, `start_datetime` ← `StartDatetime`, `end_datetime` ← `EndDatetime`, `include_archived` ← `IncludeArchived`, `include` ← `Include`
- **Returns**: `IReadOnlyList<ProductResponse>`
- **Error**: `ApiException<ListProductsForProductFamilyError>` — **Case A (typed)**
- **Error accessors**: `TryGetString(out string)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListProductsForProductFamilyRequest` | `src/MaxioAdvancedBilling/Requests/ProductFamilies/ListProductsForProductFamilyRequest.cs` |
| `BasicDateField` | `src/MaxioAdvancedBilling/Models/Enums/BasicDateField.cs` |
| `ListProductsFilter` | `src/MaxioAdvancedBilling/Models/ListProductsFilter.cs` |
| `ListProductsInclude` | `src/MaxioAdvancedBilling/Models/Enums/ListProductsInclude.cs` |
| `ProductResponse` | `src/MaxioAdvancedBilling/Models/ProductResponse.cs` |
| `ListProductsForProductFamilyError` | `src/MaxioAdvancedBilling/Errors/ListProductsForProductFamilyError.cs` |

### ReadProductFamily

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadProductFamily(ReadProductFamilyRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `ProductFamilyResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadProductFamilyRequest` | `src/MaxioAdvancedBilling/Requests/ProductFamilies/ReadProductFamilyRequest.cs` |
| `ProductFamilyResponse` | `src/MaxioAdvancedBilling/Models/ProductFamilyResponse.cs` |

