<!-- Generated file — do not edit; regenerated with the SDK. -->

# SubscriptionGroupStatus — operations

Accessor: `client.SubscriptionGroupStatus` · Source: `src/MaxioAdvancedBilling/Api/SubscriptionGroupStatus.cs` · 4 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CancelDelayedCancellationForGroup

- **Auth**: `options.BasicAuth`
- **Signature**: `CancelDelayedCancellationForGroup(CancelDelayedCancellationForGroupRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `void` (Task)
- **Error**: `ApiException<CancelDelayedCancellationForGroupError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CancelDelayedCancellationForGroupRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionGroupStatus/CancelDelayedCancellationForGroupRequest.cs` |
| `CancelDelayedCancellationForGroupError` | `src/MaxioAdvancedBilling/Errors/CancelDelayedCancellationForGroupError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### CancelSubscriptionsInGroup

- **Auth**: `options.BasicAuth`
- **Signature**: `CancelSubscriptionsInGroup(CancelSubscriptionsInGroupRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `void` (Task)
- **Error**: `ApiException<CancelSubscriptionsInGroupError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CancelSubscriptionsInGroupRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionGroupStatus/CancelSubscriptionsInGroupRequest.cs` |
| `CancelGroupedSubscriptionsRequest` | `src/MaxioAdvancedBilling/Models/CancelGroupedSubscriptionsRequest.cs` |
| `CancelSubscriptionsInGroupError` | `src/MaxioAdvancedBilling/Errors/CancelSubscriptionsInGroupError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### InitiateDelayedCancellationForGroup

- **Auth**: `options.BasicAuth`
- **Signature**: `InitiateDelayedCancellationForGroup(InitiateDelayedCancellationForGroupRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `void` (Task)
- **Error**: `ApiException<InitiateDelayedCancellationForGroupError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `InitiateDelayedCancellationForGroupRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionGroupStatus/InitiateDelayedCancellationForGroupRequest.cs` |
| `InitiateDelayedCancellationForGroupError` | `src/MaxioAdvancedBilling/Errors/InitiateDelayedCancellationForGroupError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReactivateSubscriptionGroup

- **Auth**: `options.BasicAuth`
- **Signature**: `ReactivateSubscriptionGroup(ReactivateSubscriptionGroupOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`
- **Returns**: `ReactivateSubscriptionGroupResponse`
- **Error**: `ApiException<ReactivateSubscriptionGroupError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReactivateSubscriptionGroupOperationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionGroupStatus/ReactivateSubscriptionGroupOperationRequest.cs` |
| `ReactivateSubscriptionGroupRequest` | `src/MaxioAdvancedBilling/Models/ReactivateSubscriptionGroupRequest.cs` |
| `ReactivateSubscriptionGroupResponse` | `src/MaxioAdvancedBilling/Models/ReactivateSubscriptionGroupResponse.cs` |
| `ReactivateSubscriptionGroupError` | `src/MaxioAdvancedBilling/Errors/ReactivateSubscriptionGroupError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

