<!-- Generated file — do not edit; regenerated with the SDK. -->

# SubscriptionInvoiceAccount — operations

Accessor: `client.SubscriptionInvoiceAccount` · Source: `src/MaxioAdvancedBilling/Api/SubscriptionInvoiceAccount.cs` · 7 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreatePrepayment

- **Auth**: `options.BasicAuth`
- **Signature**: `CreatePrepayment(CreatePrepaymentOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `CreatePrepaymentResponse`
- **Error**: `ApiException<CreatePrepaymentError>` — **Case A (typed)**
- **Error accessors**: `TryGetCreatePrepaymentErrorResponse(out CreatePrepaymentErrorResponse)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreatePrepaymentOperationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionInvoiceAccount/CreatePrepaymentOperationRequest.cs` |
| `CreatePrepaymentRequest` | `src/MaxioAdvancedBilling/Models/CreatePrepaymentRequest.cs` |
| `CreatePrepaymentResponse` | `src/MaxioAdvancedBilling/Models/CreatePrepaymentResponse.cs` |
| `CreatePrepaymentError` | `src/MaxioAdvancedBilling/Errors/CreatePrepaymentError.cs` |
| `CreatePrepaymentErrorResponse` | `src/MaxioAdvancedBilling/Models/AnyOf/CreatePrepaymentErrorResponse.cs` |

### DeductServiceCredit

- **Auth**: `options.BasicAuth`
- **Signature**: `DeductServiceCredit(DeductServiceCreditOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<DeductServiceCreditError>` — **Case A (typed)**
- **Error accessors**: `TryGetDeductServiceCreditErrorResponse(out DeductServiceCreditErrorResponse)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `DeductServiceCreditOperationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionInvoiceAccount/DeductServiceCreditOperationRequest.cs` |
| `DeductServiceCreditRequest` | `src/MaxioAdvancedBilling/Models/DeductServiceCreditRequest.cs` |
| `DeductServiceCreditError` | `src/MaxioAdvancedBilling/Errors/DeductServiceCreditError.cs` |
| `DeductServiceCreditErrorResponse` | `src/MaxioAdvancedBilling/Models/AnyOf/DeductServiceCreditErrorResponse.cs` |

### IssueServiceCredit

- **Auth**: `options.BasicAuth`
- **Signature**: `IssueServiceCredit(IssueServiceCreditOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `ServiceCredit`
- **Error**: `ApiException<IssueServiceCreditError>` — **Case A (typed)**
- **Error accessors**: `TryGetIssueServiceCreditErrorResponse(out IssueServiceCreditErrorResponse)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `IssueServiceCreditOperationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionInvoiceAccount/IssueServiceCreditOperationRequest.cs` |
| `IssueServiceCreditRequest` | `src/MaxioAdvancedBilling/Models/IssueServiceCreditRequest.cs` |
| `ServiceCredit` | `src/MaxioAdvancedBilling/Models/ServiceCredit.cs` |
| `IssueServiceCreditError` | `src/MaxioAdvancedBilling/Errors/IssueServiceCreditError.cs` |
| `IssueServiceCreditErrorResponse` | `src/MaxioAdvancedBilling/Models/AnyOf/IssueServiceCreditErrorResponse.cs` |

### ListPrepayments

- **Auth**: `options.BasicAuth`
- **Signature**: `ListPrepayments(ListPrepaymentsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `filter` ← `Filter`
- **Returns**: `PrepaymentsResponse`
- **Error**: `ApiException<ListPrepaymentsError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListPrepaymentsRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionInvoiceAccount/ListPrepaymentsRequest.cs` |
| `ListPrepaymentsFilter` | `src/MaxioAdvancedBilling/Models/ListPrepaymentsFilter.cs` |
| `PrepaymentsResponse` | `src/MaxioAdvancedBilling/Models/PrepaymentsResponse.cs` |
| `ListPrepaymentsError` | `src/MaxioAdvancedBilling/Errors/ListPrepaymentsError.cs` |

### ListServiceCredits

- **Auth**: `options.BasicAuth`
- **Signature**: `ListServiceCredits(ListServiceCreditsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `direction` ← `Direction`
- **Returns**: `ListServiceCreditsResponse`
- **Error**: `ApiException<ListServiceCreditsError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListServiceCreditsRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionInvoiceAccount/ListServiceCreditsRequest.cs` |
| `SortingDirection` | `src/MaxioAdvancedBilling/Models/Enums/SortingDirection.cs` |
| `ListServiceCreditsResponse` | `src/MaxioAdvancedBilling/Models/ListServiceCreditsResponse.cs` |
| `ListServiceCreditsError` | `src/MaxioAdvancedBilling/Errors/ListServiceCreditsError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReadAccountBalances

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadAccountBalances(ReadAccountBalancesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `AccountBalances`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadAccountBalancesRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionInvoiceAccount/ReadAccountBalancesRequest.cs` |
| `AccountBalances` | `src/MaxioAdvancedBilling/Models/AccountBalances.cs` |

### RefundPrepayment

- **Auth**: `options.BasicAuth`
- **Signature**: `RefundPrepayment(RefundPrepaymentOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `PrepaymentId`
- **Returns**: `PrepaymentResponse`
- **Error**: `ApiException<RefundPrepaymentError>` — **Case A (typed)**
- **Error accessors**: `TryGetRefundPrepaymentBaseErrorsResponse1(out RefundPrepaymentBaseErrorsResponse1)` [400] · `TryGetString(out string)` [404] · `TryGetRefundPrepaymentErrorResponse(out RefundPrepaymentErrorResponse)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `RefundPrepaymentOperationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionInvoiceAccount/RefundPrepaymentOperationRequest.cs` |
| `RefundPrepaymentRequest` | `src/MaxioAdvancedBilling/Models/RefundPrepaymentRequest.cs` |
| `PrepaymentResponse` | `src/MaxioAdvancedBilling/Models/PrepaymentResponse.cs` |
| `RefundPrepaymentError` | `src/MaxioAdvancedBilling/Errors/RefundPrepaymentError.cs` |
| `RefundPrepaymentBaseErrorsResponse1` | `src/MaxioAdvancedBilling/Models/RefundPrepaymentBaseErrorsResponse1.cs` |
| `RefundPrepaymentErrorResponse` | `src/MaxioAdvancedBilling/Models/AnyOf/RefundPrepaymentErrorResponse.cs` |

