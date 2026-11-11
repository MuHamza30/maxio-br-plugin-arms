<!-- Generated file — do not edit; regenerated with the SDK. -->

# ReasonCodes — operations

Accessor: `client.ReasonCodes` · Source: `src/MaxioAdvancedBilling/Api/ReasonCodes.cs` · 5 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateReasonCode

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateReasonCode(CreateReasonCodeOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `ReasonCodeResponse`
- **Error**: `ApiException<CreateReasonCodeError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateReasonCodeOperationRequest` | `src/MaxioAdvancedBilling/Requests/ReasonCodes/CreateReasonCodeOperationRequest.cs` |
| `CreateReasonCodeRequest` | `src/MaxioAdvancedBilling/Models/CreateReasonCodeRequest.cs` |
| `ReasonCodeResponse` | `src/MaxioAdvancedBilling/Models/ReasonCodeResponse.cs` |
| `CreateReasonCodeError` | `src/MaxioAdvancedBilling/Errors/CreateReasonCodeError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### DeleteReasonCode

- **Auth**: `options.BasicAuth`
- **Signature**: `DeleteReasonCode(DeleteReasonCodeRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ReasonCodeId`
- **Returns**: `OkResponse`
- **Error**: `ApiException<DeleteReasonCodeError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `DeleteReasonCodeRequest` | `src/MaxioAdvancedBilling/Requests/ReasonCodes/DeleteReasonCodeRequest.cs` |
| `OkResponse` | `src/MaxioAdvancedBilling/Models/OkResponse.cs` |
| `DeleteReasonCodeError` | `src/MaxioAdvancedBilling/Errors/DeleteReasonCodeError.cs` |

### ListReasonCodes

- **Auth**: `options.BasicAuth`
- **Signature**: `ListReasonCodes(ListReasonCodesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`
- **Returns**: `IReadOnlyList<ReasonCodeResponse>`
- **Error**: `ApiException<ListReasonCodesError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListReasonCodesRequest` | `src/MaxioAdvancedBilling/Requests/ReasonCodes/ListReasonCodesRequest.cs` |
| `ReasonCodeResponse` | `src/MaxioAdvancedBilling/Models/ReasonCodeResponse.cs` |
| `ListReasonCodesError` | `src/MaxioAdvancedBilling/Errors/ListReasonCodesError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReadReasonCode

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadReasonCode(ReadReasonCodeRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ReasonCodeId`
- **Returns**: `ReasonCodeResponse`
- **Error**: `ApiException<ReadReasonCodeError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadReasonCodeRequest` | `src/MaxioAdvancedBilling/Requests/ReasonCodes/ReadReasonCodeRequest.cs` |
| `ReasonCodeResponse` | `src/MaxioAdvancedBilling/Models/ReasonCodeResponse.cs` |
| `ReadReasonCodeError` | `src/MaxioAdvancedBilling/Errors/ReadReasonCodeError.cs` |

### UpdateReasonCode

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateReasonCode(UpdateReasonCodeOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ReasonCodeId`
- **Returns**: `ReasonCodeResponse`
- **Error**: `ApiException<UpdateReasonCodeError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateReasonCodeOperationRequest` | `src/MaxioAdvancedBilling/Requests/ReasonCodes/UpdateReasonCodeOperationRequest.cs` |
| `UpdateReasonCodeRequest` | `src/MaxioAdvancedBilling/Models/UpdateReasonCodeRequest.cs` |
| `ReasonCodeResponse` | `src/MaxioAdvancedBilling/Models/ReasonCodeResponse.cs` |
| `UpdateReasonCodeError` | `src/MaxioAdvancedBilling/Errors/UpdateReasonCodeError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

