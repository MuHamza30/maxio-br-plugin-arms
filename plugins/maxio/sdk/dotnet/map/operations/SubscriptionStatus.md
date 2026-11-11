<!-- Generated file — do not edit; regenerated with the SDK. -->

# SubscriptionStatus — operations

Accessor: `client.SubscriptionStatus` · Source: `src/MaxioAdvancedBilling/Api/SubscriptionStatus.cs` · 10 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CancelDelayedCancellation

- **Auth**: `options.BasicAuth`
- **Signature**: `CancelDelayedCancellation(CancelDelayedCancellationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `DelayedCancellationResponse`
- **Error**: `ApiException<CancelDelayedCancellationError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CancelDelayedCancellationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionStatus/CancelDelayedCancellationRequest.cs` |
| `DelayedCancellationResponse` | `src/MaxioAdvancedBilling/Models/DelayedCancellationResponse.cs` |
| `CancelDelayedCancellationError` | `src/MaxioAdvancedBilling/Errors/CancelDelayedCancellationError.cs` |

### CancelDunning

- **Auth**: `options.BasicAuth`
- **Signature**: `CancelDunning(CancelDunningRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<CancelDunningError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CancelDunningRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionStatus/CancelDunningRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `CancelDunningError` | `src/MaxioAdvancedBilling/Errors/CancelDunningError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### CancelSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `CancelSubscription(CancelSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<CancelSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetCancelSubscriptionErrorResponse(out CancelSubscriptionErrorResponse)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CancelSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionStatus/CancelSubscriptionRequest.cs` |
| `CancellationRequest` | `src/MaxioAdvancedBilling/Models/CancellationRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `CancelSubscriptionError` | `src/MaxioAdvancedBilling/Errors/CancelSubscriptionError.cs` |
| `CancelSubscriptionErrorResponse` | `src/MaxioAdvancedBilling/Models/AnyOf/CancelSubscriptionErrorResponse.cs` |

### InitiateDelayedCancellation

- **Auth**: `options.BasicAuth`
- **Signature**: `InitiateDelayedCancellation(InitiateDelayedCancellationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `DelayedCancellationResponse`
- **Error**: `ApiException<InitiateDelayedCancellationError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `InitiateDelayedCancellationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionStatus/InitiateDelayedCancellationRequest.cs` |
| `CancellationRequest` | `src/MaxioAdvancedBilling/Models/CancellationRequest.cs` |
| `DelayedCancellationResponse` | `src/MaxioAdvancedBilling/Models/DelayedCancellationResponse.cs` |
| `InitiateDelayedCancellationError` | `src/MaxioAdvancedBilling/Errors/InitiateDelayedCancellationError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### PauseSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `PauseSubscription(PauseSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<PauseSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `PauseSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionStatus/PauseSubscriptionRequest.cs` |
| `PauseRequest` | `src/MaxioAdvancedBilling/Models/PauseRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `PauseSubscriptionError` | `src/MaxioAdvancedBilling/Errors/PauseSubscriptionError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### PreviewRenewal

- **Auth**: `options.BasicAuth`
- **Signature**: `PreviewRenewal(PreviewRenewalRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `RenewalPreviewResponse`
- **Error**: `ApiException<PreviewRenewalError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `PreviewRenewalRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionStatus/PreviewRenewalRequest.cs` |
| `RenewalPreviewRequest` | `src/MaxioAdvancedBilling/Models/RenewalPreviewRequest.cs` |
| `RenewalPreviewResponse` | `src/MaxioAdvancedBilling/Models/RenewalPreviewResponse.cs` |
| `PreviewRenewalError` | `src/MaxioAdvancedBilling/Errors/PreviewRenewalError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReactivateSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `ReactivateSubscription(ReactivateSubscriptionOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<ReactivateSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReactivateSubscriptionOperationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionStatus/ReactivateSubscriptionOperationRequest.cs` |
| `ReactivateSubscriptionRequest` | `src/MaxioAdvancedBilling/Models/ReactivateSubscriptionRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `ReactivateSubscriptionError` | `src/MaxioAdvancedBilling/Errors/ReactivateSubscriptionError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ResumeSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `ResumeSubscription(ResumeSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Query params (wire ← C#)**: `calendar_billing['resumption_charge']` ← `CalendarBillingResumptionCharge`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<ResumeSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ResumeSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionStatus/ResumeSubscriptionRequest.cs` |
| `ResumptionCharge` | `src/MaxioAdvancedBilling/Models/Enums/ResumptionCharge.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `ResumeSubscriptionError` | `src/MaxioAdvancedBilling/Errors/ResumeSubscriptionError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### RetrySubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `RetrySubscription(RetrySubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<RetrySubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `RetrySubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionStatus/RetrySubscriptionRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `RetrySubscriptionError` | `src/MaxioAdvancedBilling/Errors/RetrySubscriptionError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### UpdateAutomaticSubscriptionResumption

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateAutomaticSubscriptionResumption(UpdateAutomaticSubscriptionResumptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<UpdateAutomaticSubscriptionResumptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateAutomaticSubscriptionResumptionRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionStatus/UpdateAutomaticSubscriptionResumptionRequest.cs` |
| `PauseRequest` | `src/MaxioAdvancedBilling/Models/PauseRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `UpdateAutomaticSubscriptionResumptionError` | `src/MaxioAdvancedBilling/Errors/UpdateAutomaticSubscriptionResumptionError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

