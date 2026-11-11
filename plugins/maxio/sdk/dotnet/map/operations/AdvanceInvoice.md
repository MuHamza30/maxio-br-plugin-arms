<!-- Generated file — do not edit; regenerated with the SDK. -->

# AdvanceInvoice — operations

Accessor: `client.AdvanceInvoice` · Source: `src/MaxioAdvancedBilling/Api/AdvanceInvoice.cs` · 3 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### IssueAdvanceInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `IssueAdvanceInvoice(IssueAdvanceInvoiceOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `Invoice`
- **Error**: `ApiException<IssueAdvanceInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `IssueAdvanceInvoiceOperationRequest` | `src/MaxioAdvancedBilling/Requests/AdvanceInvoice/IssueAdvanceInvoiceOperationRequest.cs` |
| `IssueAdvanceInvoiceRequest` | `src/MaxioAdvancedBilling/Models/IssueAdvanceInvoiceRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |
| `IssueAdvanceInvoiceError` | `src/MaxioAdvancedBilling/Errors/IssueAdvanceInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReadAdvanceInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadAdvanceInvoice(ReadAdvanceInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `Invoice`
- **Error**: `ApiException<ReadAdvanceInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadAdvanceInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/AdvanceInvoice/ReadAdvanceInvoiceRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |
| `ReadAdvanceInvoiceError` | `src/MaxioAdvancedBilling/Errors/ReadAdvanceInvoiceError.cs` |

### VoidAdvanceInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `VoidAdvanceInvoice(VoidAdvanceInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `Invoice`
- **Error**: `ApiException<VoidAdvanceInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `VoidAdvanceInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/AdvanceInvoice/VoidAdvanceInvoiceRequest.cs` |
| `VoidInvoiceRequest` | `src/MaxioAdvancedBilling/Models/VoidInvoiceRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |
| `VoidAdvanceInvoiceError` | `src/MaxioAdvancedBilling/Errors/VoidAdvanceInvoiceError.cs` |

