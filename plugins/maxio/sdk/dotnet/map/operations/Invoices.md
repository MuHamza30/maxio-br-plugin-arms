<!-- Generated file — do not edit; regenerated with the SDK. -->

# Invoices — operations

Accessor: `client.Invoices` · Source: `src/MaxioAdvancedBilling/Api/Invoices.cs` · 19 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateInvoice(CreateInvoiceOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `InvoiceResponse`
- **Error**: `ApiException<CreateInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateInvoiceOperationRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/CreateInvoiceOperationRequest.cs` |
| `CreateInvoiceRequest` | `src/MaxioAdvancedBilling/Models/CreateInvoiceRequest.cs` |
| `InvoiceResponse` | `src/MaxioAdvancedBilling/Models/InvoiceResponse.cs` |
| `CreateInvoiceError` | `src/MaxioAdvancedBilling/Errors/CreateInvoiceError.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### DeleteInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `DeleteInvoice(DeleteInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `Uid`
- **Returns**: `void` (Task)
- **Error**: `ApiException<DeleteInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `DeleteInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/DeleteInvoiceRequest.cs` |
| `DeleteInvoiceError` | `src/MaxioAdvancedBilling/Errors/DeleteInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### IssueInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `IssueInvoice(IssueInvoiceOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `Invoice`
- **Error**: `ApiException<IssueInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `IssueInvoiceOperationRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/IssueInvoiceOperationRequest.cs` |
| `IssueInvoiceRequest` | `src/MaxioAdvancedBilling/Models/IssueInvoiceRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |
| `IssueInvoiceError` | `src/MaxioAdvancedBilling/Errors/IssueInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListConsolidatedInvoiceSegments

- **Auth**: `options.BasicAuth`
- **Signature**: `ListConsolidatedInvoiceSegments(ListConsolidatedInvoiceSegmentsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `InvoiceUid`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `direction` ← `Direction`
- **Returns**: `ConsolidatedInvoice`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListConsolidatedInvoiceSegmentsRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/ListConsolidatedInvoiceSegmentsRequest.cs` |
| `Direction` | `src/MaxioAdvancedBilling/Models/Enums/Direction.cs` |
| `ConsolidatedInvoice` | `src/MaxioAdvancedBilling/Models/ConsolidatedInvoice.cs` |

### ListCreditNotes

- **Auth**: `options.BasicAuth`
- **Signature**: `ListCreditNotes(ListCreditNotesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `subscription_id` ← `SubscriptionId`, `date_field` ← `DateField`, `start_date` ← `StartDate`, `end_date` ← `EndDate`, `start_datetime` ← `StartDatetime`, `end_datetime` ← `EndDatetime`, `page` ← `Page`, `per_page` ← `PerPage`, `direction` ← `Direction`, `line_items` ← `LineItems`, `discounts` ← `Discounts`, `taxes` ← `Taxes`, `refunds` ← `Refunds`, `applications` ← `Applications`
- **Returns**: `ListCreditNotesResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListCreditNotesRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/ListCreditNotesRequest.cs` |
| `CreditNoteDateField` | `src/MaxioAdvancedBilling/Models/Enums/CreditNoteDateField.cs` |
| `Direction` | `src/MaxioAdvancedBilling/Models/Enums/Direction.cs` |
| `ListCreditNotesResponse` | `src/MaxioAdvancedBilling/Models/ListCreditNotesResponse.cs` |

### ListInvoiceEvents

- **Auth**: `options.BasicAuth`
- **Signature**: `ListInvoiceEvents(ListInvoiceEventsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `since_date` ← `SinceDate`, `since_id` ← `SinceId`, `page` ← `Page`, `per_page` ← `PerPage`, `invoice_uid` ← `InvoiceUid`, `with_change_invoice_status` ← `WithChangeInvoiceStatus`, `event_types` ← `EventTypes`
- **Returns**: `ListInvoiceEventsResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListInvoiceEventsRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/ListInvoiceEventsRequest.cs` |
| `InvoiceEventType` | `src/MaxioAdvancedBilling/Models/Enums/InvoiceEventType.cs` |
| `ListInvoiceEventsResponse` | `src/MaxioAdvancedBilling/Models/ListInvoiceEventsResponse.cs` |

### ListInvoices

- **Auth**: `options.BasicAuth`
- **Signature**: `ListInvoices(ListInvoicesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `start_date` ← `StartDate`, `end_date` ← `EndDate`, `status` ← `Status`, `subscription_id` ← `SubscriptionId`, `subscription_group_uid` ← `SubscriptionGroupUid`, `consolidation_level` ← `ConsolidationLevel`, `page` ← `Page`, `per_page` ← `PerPage`, `direction` ← `Direction`, `line_items` ← `LineItems`, `discounts` ← `Discounts`, `taxes` ← `Taxes`, `credits` ← `Credits`, `payments` ← `Payments`, `custom_fields` ← `CustomFields`, `refunds` ← `Refunds`, `date_field` ← `DateField`, `start_datetime` ← `StartDatetime`, `end_datetime` ← `EndDatetime`, `customer_ids` ← `CustomerIds`, `number` ← `Number`, `product_ids` ← `ProductIds`, `sort` ← `Sort`
- **Returns**: `ListInvoicesResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListInvoicesRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/ListInvoicesRequest.cs` |
| `InvoiceStatus` | `src/MaxioAdvancedBilling/Models/Enums/InvoiceStatus.cs` |
| `Direction` | `src/MaxioAdvancedBilling/Models/Enums/Direction.cs` |
| `InvoiceDateField` | `src/MaxioAdvancedBilling/Models/Enums/InvoiceDateField.cs` |
| `InvoiceSortField` | `src/MaxioAdvancedBilling/Models/Enums/InvoiceSortField.cs` |
| `ListInvoicesResponse` | `src/MaxioAdvancedBilling/Models/ListInvoicesResponse.cs` |

### PreviewCustomerInformationChanges

- **Auth**: `options.BasicAuth`
- **Signature**: `PreviewCustomerInformationChanges(PreviewCustomerInformationChangesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `CustomerChangesPreviewResponse`
- **Error**: `ApiException<PreviewCustomerInformationChangesError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `PreviewCustomerInformationChangesRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/PreviewCustomerInformationChangesRequest.cs` |
| `CustomerChangesPreviewResponse` | `src/MaxioAdvancedBilling/Models/CustomerChangesPreviewResponse.cs` |
| `PreviewCustomerInformationChangesError` | `src/MaxioAdvancedBilling/Errors/PreviewCustomerInformationChangesError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReadCreditNote

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadCreditNote(ReadCreditNoteRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `CreditNote`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadCreditNoteRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/ReadCreditNoteRequest.cs` |
| `CreditNote` | `src/MaxioAdvancedBilling/Models/CreditNote.cs` |

### ReadInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadInvoice(ReadInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `Invoice`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/ReadInvoiceRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |

### RecordPaymentForInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `RecordPaymentForInvoice(RecordPaymentForInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `Invoice`
- **Error**: `ApiException<RecordPaymentForInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `RecordPaymentForInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/RecordPaymentForInvoiceRequest.cs` |
| `CreateInvoicePaymentRequest` | `src/MaxioAdvancedBilling/Models/CreateInvoicePaymentRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |
| `RecordPaymentForInvoiceError` | `src/MaxioAdvancedBilling/Errors/RecordPaymentForInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### RecordPaymentForMultipleInvoices

- **Auth**: `options.BasicAuth`
- **Signature**: `RecordPaymentForMultipleInvoices(RecordPaymentForMultipleInvoicesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `MultiInvoicePaymentResponse`
- **Error**: `ApiException<RecordPaymentForMultipleInvoicesError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `RecordPaymentForMultipleInvoicesRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/RecordPaymentForMultipleInvoicesRequest.cs` |
| `CreateMultiInvoicePaymentRequest` | `src/MaxioAdvancedBilling/Models/CreateMultiInvoicePaymentRequest.cs` |
| `MultiInvoicePaymentResponse` | `src/MaxioAdvancedBilling/Models/MultiInvoicePaymentResponse.cs` |
| `RecordPaymentForMultipleInvoicesError` | `src/MaxioAdvancedBilling/Errors/RecordPaymentForMultipleInvoicesError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### RecordPaymentForSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `RecordPaymentForSubscription(RecordPaymentForSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `RecordPaymentResponse`
- **Error**: `ApiException<RecordPaymentForSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `RecordPaymentForSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/RecordPaymentForSubscriptionRequest.cs` |
| `RecordPaymentRequest` | `src/MaxioAdvancedBilling/Models/RecordPaymentRequest.cs` |
| `RecordPaymentResponse` | `src/MaxioAdvancedBilling/Models/RecordPaymentResponse.cs` |
| `RecordPaymentForSubscriptionError` | `src/MaxioAdvancedBilling/Errors/RecordPaymentForSubscriptionError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### RefundInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `RefundInvoice(RefundInvoiceOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `Invoice`
- **Error**: `ApiException<RefundInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `RefundInvoiceOperationRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/RefundInvoiceOperationRequest.cs` |
| `RefundInvoiceRequest` | `src/MaxioAdvancedBilling/Models/RefundInvoiceRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |
| `RefundInvoiceError` | `src/MaxioAdvancedBilling/Errors/RefundInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReopenInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `ReopenInvoice(ReopenInvoiceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `Invoice`
- **Error**: `ApiException<ReopenInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetObject(out object?)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReopenInvoiceRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/ReopenInvoiceRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |
| `ReopenInvoiceError` | `src/MaxioAdvancedBilling/Errors/ReopenInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### SendInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `SendInvoice(SendInvoiceOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `void` (Task)
- **Error**: `ApiException<SendInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `SendInvoiceOperationRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/SendInvoiceOperationRequest.cs` |
| `SendInvoiceRequest` | `src/MaxioAdvancedBilling/Models/SendInvoiceRequest.cs` |
| `SendInvoiceError` | `src/MaxioAdvancedBilling/Errors/SendInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### UpdateCustomerInformation

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateCustomerInformation(UpdateCustomerInformationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `Invoice`
- **Error**: `ApiException<UpdateCustomerInformationError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateCustomerInformationRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/UpdateCustomerInformationRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |
| `UpdateCustomerInformationError` | `src/MaxioAdvancedBilling/Errors/UpdateCustomerInformationError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### UpdateInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateInvoice(UpdateInvoiceOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `Uid`
- **Returns**: `InvoiceResponse`
- **Error**: `ApiException<UpdateInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [404] · `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateInvoiceOperationRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/UpdateInvoiceOperationRequest.cs` |
| `UpdateInvoiceRequest` | `src/MaxioAdvancedBilling/Models/UpdateInvoiceRequest.cs` |
| `InvoiceResponse` | `src/MaxioAdvancedBilling/Models/InvoiceResponse.cs` |
| `UpdateInvoiceError` | `src/MaxioAdvancedBilling/Errors/UpdateInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### VoidInvoice

- **Auth**: `options.BasicAuth`
- **Signature**: `VoidInvoice(VoidInvoiceOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `Invoice`
- **Error**: `ApiException<VoidInvoiceError>` — **Case A (typed)**
- **Error accessors**: `TryGetObject(out object?)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `VoidInvoiceOperationRequest` | `src/MaxioAdvancedBilling/Requests/Invoices/VoidInvoiceOperationRequest.cs` |
| `VoidInvoiceRequest` | `src/MaxioAdvancedBilling/Models/VoidInvoiceRequest.cs` |
| `Invoice` | `src/MaxioAdvancedBilling/Models/Invoice.cs` |
| `VoidInvoiceError` | `src/MaxioAdvancedBilling/Errors/VoidInvoiceError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

