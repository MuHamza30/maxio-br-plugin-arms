<!-- Generated file — do not edit; regenerated with the SDK. -->

# SubscriptionGroupInvoiceAccount — operations

Accessor: `client.SubscriptionGroupInvoiceAccount` · Source: `src/MaxioAdvancedBilling/Api/SubscriptionGroupInvoiceAccount.cs` · 4 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateSubscriptionGroupPrepayment

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateSubscriptionGroupPrepayment(CreateSubscriptionGroupPrepaymentRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `SubscriptionGroupPrepaymentResponse`
- **Error**: `ApiException<CreateSubscriptionGroupPrepaymentError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateSubscriptionGroupPrepaymentRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionGroupInvoiceAccount/CreateSubscriptionGroupPrepaymentRequest.cs` |
| `SubscriptionGroupPrepaymentRequest` | `src/MaxioAdvancedBilling/Models/SubscriptionGroupPrepaymentRequest.cs` |
| `SubscriptionGroupPrepaymentResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionGroupPrepaymentResponse.cs` |
| `CreateSubscriptionGroupPrepaymentError` | `src/MaxioAdvancedBilling/Errors/CreateSubscriptionGroupPrepaymentError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### DeductSubscriptionGroupServiceCredit

- **Auth**: `options.BasicAuth`
- **Signature**: `DeductSubscriptionGroupServiceCredit(DeductSubscriptionGroupServiceCreditRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `ServiceCredit`
- **Error**: `ApiException<DeductSubscriptionGroupServiceCreditError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `DeductSubscriptionGroupServiceCreditRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionGroupInvoiceAccount/DeductSubscriptionGroupServiceCreditRequest.cs` |
| `DeductServiceCreditRequest` | `src/MaxioAdvancedBilling/Models/DeductServiceCreditRequest.cs` |
| `ServiceCredit` | `src/MaxioAdvancedBilling/Models/ServiceCredit.cs` |
| `DeductSubscriptionGroupServiceCreditError` | `src/MaxioAdvancedBilling/Errors/DeductSubscriptionGroupServiceCreditError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### IssueSubscriptionGroupServiceCredit

- **Auth**: `options.BasicAuth`
- **Signature**: `IssueSubscriptionGroupServiceCredit(IssueSubscriptionGroupServiceCreditRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `ServiceCreditResponse`
- **Error**: `ApiException<IssueSubscriptionGroupServiceCreditError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `IssueSubscriptionGroupServiceCreditRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionGroupInvoiceAccount/IssueSubscriptionGroupServiceCreditRequest.cs` |
| `IssueServiceCreditRequest` | `src/MaxioAdvancedBilling/Models/IssueServiceCreditRequest.cs` |
| `ServiceCreditResponse` | `src/MaxioAdvancedBilling/Models/ServiceCreditResponse.cs` |
| `IssueSubscriptionGroupServiceCreditError` | `src/MaxioAdvancedBilling/Errors/IssueSubscriptionGroupServiceCreditError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListPrepaymentsForSubscriptionGroup

- **Auth**: `options.BasicAuth`
- **Signature**: `ListPrepaymentsForSubscriptionGroup(ListPrepaymentsForSubscriptionGroupRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `filter` ← `Filter`
- **Returns**: `ListSubscriptionGroupPrepaymentResponse`
- **Error**: `ApiException<ListPrepaymentsForSubscriptionGroupError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListPrepaymentsForSubscriptionGroupRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionGroupInvoiceAccount/ListPrepaymentsForSubscriptionGroupRequest.cs` |
| `ListPrepaymentsFilter` | `src/MaxioAdvancedBilling/Models/ListPrepaymentsFilter.cs` |
| `ListSubscriptionGroupPrepaymentResponse` | `src/MaxioAdvancedBilling/Models/ListSubscriptionGroupPrepaymentResponse.cs` |
| `ListPrepaymentsForSubscriptionGroupError` | `src/MaxioAdvancedBilling/Errors/ListPrepaymentsForSubscriptionGroupError.cs` |

