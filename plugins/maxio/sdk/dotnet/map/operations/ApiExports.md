<!-- Generated file — do not edit; regenerated with the SDK. -->

# ApiExports — operations

Accessor: `client.ApiExports` · Source: `src/MaxioAdvancedBilling/Api/ApiExports.cs` · 9 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ExportInvoices

- **Auth**: `options.BasicAuth`
- **Signature**: `ExportInvoices(RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `BatchJobResponse`
- **Error**: `ApiException<ExportInvoicesError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetSingleErrorResponse1(out SingleErrorResponse1)` [409] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `BatchJobResponse` | `src/MaxioAdvancedBilling/Models/BatchJobResponse.cs` |
| `ExportInvoicesError` | `src/MaxioAdvancedBilling/Errors/ExportInvoicesError.cs` |
| `SingleErrorResponse1` | `src/MaxioAdvancedBilling/Models/SingleErrorResponse1.cs` |

### ExportProformaInvoices

- **Auth**: `options.BasicAuth`
- **Signature**: `ExportProformaInvoices(RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `BatchJobResponse`
- **Error**: `ApiException<ExportProformaInvoicesError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetSingleErrorResponse1(out SingleErrorResponse1)` [409] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `BatchJobResponse` | `src/MaxioAdvancedBilling/Models/BatchJobResponse.cs` |
| `ExportProformaInvoicesError` | `src/MaxioAdvancedBilling/Errors/ExportProformaInvoicesError.cs` |
| `SingleErrorResponse1` | `src/MaxioAdvancedBilling/Models/SingleErrorResponse1.cs` |

### ExportSubscriptions

- **Auth**: `options.BasicAuth`
- **Signature**: `ExportSubscriptions(RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `BatchJobResponse`
- **Error**: `ApiException<ExportSubscriptionsError>` — **Case A (typed)**
- **Error accessors**: `TryGetSingleErrorResponse1(out SingleErrorResponse1)` [409] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `BatchJobResponse` | `src/MaxioAdvancedBilling/Models/BatchJobResponse.cs` |
| `ExportSubscriptionsError` | `src/MaxioAdvancedBilling/Errors/ExportSubscriptionsError.cs` |
| `SingleErrorResponse1` | `src/MaxioAdvancedBilling/Models/SingleErrorResponse1.cs` |

### ListExportedInvoices

- **Auth**: `options.BasicAuth`
- **Signature**: `ListExportedInvoices(ListExportedInvoicesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `BatchId`
- **Query params (wire ← C#)**: `per_page` ← `PerPage`, `page` ← `Page`
- **Returns**: `IReadOnlyList<Invoice>`
- **Error**: `ApiException<ListExportedInvoicesError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListExportedInvoicesRequest` | `src/MaxioAdvancedBilling/Requests/ApiExports/ListExportedInvoicesRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |
| `ListExportedInvoicesError` | `src/MaxioAdvancedBilling/Errors/ListExportedInvoicesError.cs` |

### ListExportedProformaInvoices

- **Auth**: `options.BasicAuth`
- **Signature**: `ListExportedProformaInvoices(ListExportedProformaInvoicesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `BatchId`
- **Query params (wire ← C#)**: `per_page` ← `PerPage`, `page` ← `Page`
- **Returns**: `IReadOnlyList<ProformaInvoice>`
- **Error**: `ApiException<ListExportedProformaInvoicesError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListExportedProformaInvoicesRequest` | `src/MaxioAdvancedBilling/Requests/ApiExports/ListExportedProformaInvoicesRequest.cs` |
| `ProformaInvoice` | `src/MaxioAdvancedBilling/Models/ProformaInvoice.cs` |
| `ListExportedProformaInvoicesError` | `src/MaxioAdvancedBilling/Errors/ListExportedProformaInvoicesError.cs` |

### ListExportedSubscriptions

- **Auth**: `options.BasicAuth`
- **Signature**: `ListExportedSubscriptions(ListExportedSubscriptionsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `BatchId`
- **Query params (wire ← C#)**: `per_page` ← `PerPage`, `page` ← `Page`
- **Returns**: `IReadOnlyList<Subscription>`
- **Error**: `ApiException<ListExportedSubscriptionsError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListExportedSubscriptionsRequest` | `src/MaxioAdvancedBilling/Requests/ApiExports/ListExportedSubscriptionsRequest.cs` |
| `Subscription` | `src/MaxioAdvancedBilling/Models/Subscription.cs` |
| `ListExportedSubscriptionsError` | `src/MaxioAdvancedBilling/Errors/ListExportedSubscriptionsError.cs` |

### ReadInvoicesExport

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadInvoicesExport(ReadInvoicesExportRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `BatchId`
- **Returns**: `BatchJobResponse`
- **Error**: `ApiException<ReadInvoicesExportError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadInvoicesExportRequest` | `src/MaxioAdvancedBilling/Requests/ApiExports/ReadInvoicesExportRequest.cs` |
| `BatchJobResponse` | `src/MaxioAdvancedBilling/Models/BatchJobResponse.cs` |
| `ReadInvoicesExportError` | `src/MaxioAdvancedBilling/Errors/ReadInvoicesExportError.cs` |

### ReadProformaInvoicesExport

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadProformaInvoicesExport(ReadProformaInvoicesExportRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `BatchId`
- **Returns**: `BatchJobResponse`
- **Error**: `ApiException<ReadProformaInvoicesExportError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadProformaInvoicesExportRequest` | `src/MaxioAdvancedBilling/Requests/ApiExports/ReadProformaInvoicesExportRequest.cs` |
| `BatchJobResponse` | `src/MaxioAdvancedBilling/Models/BatchJobResponse.cs` |
| `ReadProformaInvoicesExportError` | `src/MaxioAdvancedBilling/Errors/ReadProformaInvoicesExportError.cs` |

### ReadSubscriptionsExport

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadSubscriptionsExport(ReadSubscriptionsExportRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `BatchId`
- **Returns**: `BatchJobResponse`
- **Error**: `ApiException<ReadSubscriptionsExportError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadSubscriptionsExportRequest` | `src/MaxioAdvancedBilling/Requests/ApiExports/ReadSubscriptionsExportRequest.cs` |
| `BatchJobResponse` | `src/MaxioAdvancedBilling/Models/BatchJobResponse.cs` |
| `ReadSubscriptionsExportError` | `src/MaxioAdvancedBilling/Errors/ReadSubscriptionsExportError.cs` |

