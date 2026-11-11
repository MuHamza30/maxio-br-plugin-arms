<!-- Generated file — do not edit; regenerated with the SDK. -->

# ProformaInvoices — operations

Accessor: `client.ProformaInvoices` · Source: `src/MaxioAdvancedBilling/Api/ProformaInvoices.cs` · 10 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateConsolidatedProformaInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateConsolidatedProformaInvoice(CreateConsolidatedProformaInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `void` (Task)
- **Error**: `ApiException<CreateConsolidatedProformaInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateConsolidatedProformaInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/ProformaInvoices/CreateConsolidatedProformaInvoiceRequest.cs` |
| `CreateConsolidatedProformaInvoiceError` | `src/MaxioAdvancedBilling/Errors/CreateConsolidatedProformaInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### CreateProformaInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateProformaInvoice(CreateProformaInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `ProformaInvoice`
- **Error**: `ApiException<CreateProformaInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateProformaInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/ProformaInvoices/CreateProformaInvoiceRequest.cs` |
| `ProformaInvoice` | `src/MaxioAdvancedBilling/Models/ProformaInvoice.cs` |
| `CreateProformaInvoiceError` | `src/MaxioAdvancedBilling/Errors/CreateProformaInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### CreateSignupProformaInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateSignupProformaInvoice(CreateSignupProformaInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `ProformaInvoice`
- **Error**: `ApiException<CreateSignupProformaInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetProformaBadRequestErrorResponse1(out ProformaBadRequestErrorResponse1)` [400] · `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateSignupProformaInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/ProformaInvoices/CreateSignupProformaInvoiceRequest.cs` |
| `CreateSubscriptionRequest` | `src/MaxioAdvancedBilling/Models/CreateSubscriptionRequest.cs` |
| `ProformaInvoice` | `src/MaxioAdvancedBilling/Models/ProformaInvoice.cs` |
| `CreateSignupProformaInvoiceError` | `src/MaxioAdvancedBilling/Errors/CreateSignupProformaInvoiceError.cs` |
| `ProformaBadRequestErrorResponse1` | `src/MaxioAdvancedBilling/Models/ProformaBadRequestErrorResponse1.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### DeliverProformaInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `DeliverProformaInvoice(DeliverProformaInvoiceOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProformaInvoiceUid`
- **Returns**: `ProformaInvoice`
- **Error**: `ApiException<DeliverProformaInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `DeliverProformaInvoiceOperationRequest` | `src/MaxioAdvancedBilling/Requests/ProformaInvoices/DeliverProformaInvoiceOperationRequest.cs` |
| `DeliverProformaInvoiceRequest` | `src/MaxioAdvancedBilling/Models/DeliverProformaInvoiceRequest.cs` |
| `ProformaInvoice` | `src/MaxioAdvancedBilling/Models/ProformaInvoice.cs` |
| `DeliverProformaInvoiceError` | `src/MaxioAdvancedBilling/Errors/DeliverProformaInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListProformaInvoices

- **Auth**: `options.BasicAuth`
- **Signature**: `ListProformaInvoices(ListProformaInvoicesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Query params (wire ← C#)**: `start_date` ← `StartDate`, `end_date` ← `EndDate`, `status` ← `Status`, `page` ← `Page`, `per_page` ← `PerPage`, `direction` ← `Direction`, `line_items` ← `LineItems`, `discounts` ← `Discounts`, `taxes` ← `Taxes`, `credits` ← `Credits`, `payments` ← `Payments`, `custom_fields` ← `CustomFields`
- **Returns**: `ListProformaInvoicesResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListProformaInvoicesRequest` | `src/MaxioAdvancedBilling/Requests/ProformaInvoices/ListProformaInvoicesRequest.cs` |
| `ProformaInvoiceStatus` | `src/MaxioAdvancedBilling/Models/Enums/ProformaInvoiceStatus.cs` |
| `Direction` | `src/MaxioAdvancedBilling/Models/Enums/Direction.cs` |
| `ListProformaInvoicesResponse` | `src/MaxioAdvancedBilling/Models/ListProformaInvoicesResponse.cs` |

### ListSubscriptionGroupProformaInvoices

- **Auth**: `options.BasicAuth`
- **Signature**: `ListSubscriptionGroupProformaInvoices(ListSubscriptionGroupProformaInvoicesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Query params (wire ← C#)**: `line_items` ← `LineItems`, `discounts` ← `Discounts`, `taxes` ← `Taxes`, `credits` ← `Credits`, `payments` ← `Payments`, `custom_fields` ← `CustomFields`
- **Returns**: `ListProformaInvoicesResponse`
- **Error**: `ApiException<ListSubscriptionGroupProformaInvoicesError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListSubscriptionGroupProformaInvoicesRequest` | `src/MaxioAdvancedBilling/Requests/ProformaInvoices/ListSubscriptionGroupProformaInvoicesRequest.cs` |
| `ListProformaInvoicesResponse` | `src/MaxioAdvancedBilling/Models/ListProformaInvoicesResponse.cs` |
| `ListSubscriptionGroupProformaInvoicesError` | `src/MaxioAdvancedBilling/Errors/ListSubscriptionGroupProformaInvoicesError.cs` |

### PreviewProformaInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `PreviewProformaInvoice(PreviewProformaInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `ProformaInvoice`
- **Error**: `ApiException<PreviewProformaInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `PreviewProformaInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/ProformaInvoices/PreviewProformaInvoiceRequest.cs` |
| `ProformaInvoice` | `src/MaxioAdvancedBilling/Models/ProformaInvoice.cs` |
| `PreviewProformaInvoiceError` | `src/MaxioAdvancedBilling/Errors/PreviewProformaInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### PreviewSignupProformaInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `PreviewSignupProformaInvoice(PreviewSignupProformaInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `include` ← `Include`
- **Returns**: `SignupProformaPreviewResponse`
- **Error**: `ApiException<PreviewSignupProformaInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetProformaBadRequestErrorResponse1(out ProformaBadRequestErrorResponse1)` [400] · `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `PreviewSignupProformaInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/ProformaInvoices/PreviewSignupProformaInvoiceRequest.cs` |
| `CreateSignupProformaPreviewInclude` | `src/MaxioAdvancedBilling/Models/Enums/CreateSignupProformaPreviewInclude.cs` |
| `CreateSubscriptionRequest` | `src/MaxioAdvancedBilling/Models/CreateSubscriptionRequest.cs` |
| `SignupProformaPreviewResponse` | `src/MaxioAdvancedBilling/Models/SignupProformaPreviewResponse.cs` |
| `PreviewSignupProformaInvoiceError` | `src/MaxioAdvancedBilling/Errors/PreviewSignupProformaInvoiceError.cs` |
| `ProformaBadRequestErrorResponse1` | `src/MaxioAdvancedBilling/Models/ProformaBadRequestErrorResponse1.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### ReadProformaInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadProformaInvoice(ReadProformaInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProformaInvoiceUid`
- **Returns**: `ProformaInvoice`
- **Error**: `ApiException<ReadProformaInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadProformaInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/ProformaInvoices/ReadProformaInvoiceRequest.cs` |
| `ProformaInvoice` | `src/MaxioAdvancedBilling/Models/ProformaInvoice.cs` |
| `ReadProformaInvoiceError` | `src/MaxioAdvancedBilling/Errors/ReadProformaInvoiceError.cs` |

### VoidProformaInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `VoidProformaInvoice(VoidProformaInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ProformaInvoiceUid`
- **Returns**: `ProformaInvoice`
- **Error**: `ApiException<VoidProformaInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `VoidProformaInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/ProformaInvoices/VoidProformaInvoiceRequest.cs` |
| `VoidInvoiceRequest` | `src/MaxioAdvancedBilling/Models/VoidInvoiceRequest.cs` |
| `ProformaInvoice` | `src/MaxioAdvancedBilling/Models/ProformaInvoice.cs` |
| `VoidProformaInvoiceError` | `src/MaxioAdvancedBilling/Errors/VoidProformaInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

