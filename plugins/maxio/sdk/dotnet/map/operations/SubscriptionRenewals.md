<!-- Generated file — do not edit; regenerated with the SDK. -->

# SubscriptionRenewals — operations

Accessor: `client.SubscriptionRenewals` · Source: `src/MaxioAdvancedBilling/Api/SubscriptionRenewals.cs` · 11 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CancelScheduledRenewalConfiguration

- **Auth**: `options.BasicAuth`
- **Signature**: `CancelScheduledRenewalConfiguration(CancelScheduledRenewalConfigurationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `Id`
- **Returns**: `ScheduledRenewalConfigurationResponse`
- **Error**: `ApiException<CancelScheduledRenewalConfigurationError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CancelScheduledRenewalConfigurationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/CancelScheduledRenewalConfigurationRequest.cs` |
| `ScheduledRenewalConfigurationResponse` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationResponse.cs` |
| `CancelScheduledRenewalConfigurationError` | `src/MaxioAdvancedBilling/Errors/CancelScheduledRenewalConfigurationError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### CreateScheduledRenewalConfiguration

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateScheduledRenewalConfiguration(CreateScheduledRenewalConfigurationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `ScheduledRenewalConfigurationResponse`
- **Error**: `ApiException<CreateScheduledRenewalConfigurationError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateScheduledRenewalConfigurationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/CreateScheduledRenewalConfigurationRequest.cs` |
| `ScheduledRenewalConfigurationRequest` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationRequest.cs` |
| `ScheduledRenewalConfigurationResponse` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationResponse.cs` |
| `CreateScheduledRenewalConfigurationError` | `src/MaxioAdvancedBilling/Errors/CreateScheduledRenewalConfigurationError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### CreateScheduledRenewalConfigurationItem

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateScheduledRenewalConfigurationItem(CreateScheduledRenewalConfigurationItemRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `ScheduledRenewalsConfigurationId`
- **Returns**: `ScheduledRenewalConfigurationItemResponse`
- **Error**: `ApiException<CreateScheduledRenewalConfigurationItemError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateScheduledRenewalConfigurationItemRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/CreateScheduledRenewalConfigurationItemRequest.cs` |
| `ScheduledRenewalConfigurationItemRequest` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationItemRequest.cs` |
| `ScheduledRenewalConfigurationItemResponse` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationItemResponse.cs` |
| `CreateScheduledRenewalConfigurationItemError` | `src/MaxioAdvancedBilling/Errors/CreateScheduledRenewalConfigurationItemError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### DeleteScheduledRenewalConfigurationItem

- **Auth**: `options.BasicAuth`
- **Signature**: `DeleteScheduledRenewalConfigurationItem(DeleteScheduledRenewalConfigurationItemRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `ScheduledRenewalsConfigurationId`, `Id`
- **Returns**: `void` (Task)
- **Error**: `ApiException<DeleteScheduledRenewalConfigurationItemError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `DeleteScheduledRenewalConfigurationItemRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/DeleteScheduledRenewalConfigurationItemRequest.cs` |
| `DeleteScheduledRenewalConfigurationItemError` | `src/MaxioAdvancedBilling/Errors/DeleteScheduledRenewalConfigurationItemError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListScheduledRenewalConfigurations

- **Auth**: `options.BasicAuth`
- **Signature**: `ListScheduledRenewalConfigurations(ListScheduledRenewalConfigurationsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Query params (wire ← C#)**: `status` ← `Status`
- **Returns**: `ScheduledRenewalConfigurationsResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListScheduledRenewalConfigurationsRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/ListScheduledRenewalConfigurationsRequest.cs` |
| `Status` | `src/MaxioAdvancedBilling/Models/Enums/Status.cs` |
| `ScheduledRenewalConfigurationsResponse` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationsResponse.cs` |

### LockInScheduledRenewalImmediately

- **Auth**: `options.BasicAuth`
- **Signature**: `LockInScheduledRenewalImmediately(LockInScheduledRenewalImmediatelyRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `Id`
- **Returns**: `ScheduledRenewalConfigurationResponse`
- **Error**: `ApiException<LockInScheduledRenewalImmediatelyError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `LockInScheduledRenewalImmediatelyRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/LockInScheduledRenewalImmediatelyRequest.cs` |
| `ScheduledRenewalConfigurationResponse` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationResponse.cs` |
| `LockInScheduledRenewalImmediatelyError` | `src/MaxioAdvancedBilling/Errors/LockInScheduledRenewalImmediatelyError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReadScheduledRenewalConfiguration

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadScheduledRenewalConfiguration(ReadScheduledRenewalConfigurationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `Id`
- **Returns**: `ScheduledRenewalConfigurationResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadScheduledRenewalConfigurationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/ReadScheduledRenewalConfigurationRequest.cs` |
| `ScheduledRenewalConfigurationResponse` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationResponse.cs` |

### ScheduleScheduledRenewalLockIn

- **Auth**: `options.BasicAuth`
- **Signature**: `ScheduleScheduledRenewalLockIn(ScheduleScheduledRenewalLockInRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `Id`
- **Returns**: `ScheduledRenewalConfigurationResponse`
- **Error**: `ApiException<ScheduleScheduledRenewalLockInError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ScheduleScheduledRenewalLockInRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/ScheduleScheduledRenewalLockInRequest.cs` |
| `ScheduledRenewalLockInRequest` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalLockInRequest.cs` |
| `ScheduledRenewalConfigurationResponse` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationResponse.cs` |
| `ScheduleScheduledRenewalLockInError` | `src/MaxioAdvancedBilling/Errors/ScheduleScheduledRenewalLockInError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### UnpublishScheduledRenewalConfiguration

- **Auth**: `options.BasicAuth`
- **Signature**: `UnpublishScheduledRenewalConfiguration(UnpublishScheduledRenewalConfigurationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `Id`
- **Returns**: `ScheduledRenewalConfigurationResponse`
- **Error**: `ApiException<UnpublishScheduledRenewalConfigurationError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UnpublishScheduledRenewalConfigurationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/UnpublishScheduledRenewalConfigurationRequest.cs` |
| `ScheduledRenewalConfigurationResponse` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationResponse.cs` |
| `UnpublishScheduledRenewalConfigurationError` | `src/MaxioAdvancedBilling/Errors/UnpublishScheduledRenewalConfigurationError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### UpdateScheduledRenewalConfiguration

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateScheduledRenewalConfiguration(UpdateScheduledRenewalConfigurationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `Id`
- **Returns**: `ScheduledRenewalConfigurationResponse`
- **Error**: `ApiException<UpdateScheduledRenewalConfigurationError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateScheduledRenewalConfigurationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/UpdateScheduledRenewalConfigurationRequest.cs` |
| `ScheduledRenewalConfigurationRequest` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationRequest.cs` |
| `ScheduledRenewalConfigurationResponse` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationResponse.cs` |
| `UpdateScheduledRenewalConfigurationError` | `src/MaxioAdvancedBilling/Errors/UpdateScheduledRenewalConfigurationError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### UpdateScheduledRenewalConfigurationItem

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateScheduledRenewalConfigurationItem(UpdateScheduledRenewalConfigurationItemRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `ScheduledRenewalsConfigurationId`, `Id`
- **Returns**: `ScheduledRenewalConfigurationItemResponse`
- **Error**: `ApiException<UpdateScheduledRenewalConfigurationItemError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateScheduledRenewalConfigurationItemRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionRenewals/UpdateScheduledRenewalConfigurationItemRequest.cs` |
| `ScheduledRenewalUpdateRequest` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalUpdateRequest.cs` |
| `ScheduledRenewalConfigurationItemResponse` | `src/MaxioAdvancedBilling/Models/ScheduledRenewalConfigurationItemResponse.cs` |
| `UpdateScheduledRenewalConfigurationItemError` | `src/MaxioAdvancedBilling/Errors/UpdateScheduledRenewalConfigurationItemError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

