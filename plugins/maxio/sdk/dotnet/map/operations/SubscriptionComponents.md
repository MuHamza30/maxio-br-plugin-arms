<!-- Generated file — do not edit; regenerated with the SDK. -->

# SubscriptionComponents — operations

Accessor: `client.SubscriptionComponents` · Source: `src/MaxioAdvancedBilling/Api/SubscriptionComponents.cs` · 17 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ActivateEventBasedComponent

- **Auth**: `options.BasicAuth`
- **Signature**: `ActivateEventBasedComponent(ActivateEventBasedComponentRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `ComponentId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ActivateEventBasedComponentRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/ActivateEventBasedComponentRequest.cs` |
| `ActivateEventBasedComponent` | `src/MaxioAdvancedBilling/Models/ActivateEventBasedComponent.cs` |

### AllocateComponent

- **Auth**: `options.BasicAuth`
- **Signature**: `AllocateComponent(AllocateComponentRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `ComponentId`
- **Returns**: `AllocationResponse`
- **Error**: `ApiException<AllocateComponentError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `AllocateComponentRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/AllocateComponentRequest.cs` |
| `CreateAllocationRequest` | `src/MaxioAdvancedBilling/Models/CreateAllocationRequest.cs` |
| `AllocationResponse` | `src/MaxioAdvancedBilling/Models/AllocationResponse.cs` |
| `AllocateComponentError` | `src/MaxioAdvancedBilling/Errors/AllocateComponentError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### AllocateComponents

- **Auth**: `options.BasicAuth`
- **Signature**: `AllocateComponents(AllocateComponentsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `IReadOnlyList<AllocationResponse>`
- **Error**: `ApiException<AllocateComponentsError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `AllocateComponentsRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/AllocateComponentsRequest.cs` |
| `AllocateComponents` | `src/MaxioAdvancedBilling/Models/AllocateComponents.cs` |
| `AllocationResponse` | `src/MaxioAdvancedBilling/Models/AllocationResponse.cs` |
| `AllocateComponentsError` | `src/MaxioAdvancedBilling/Errors/AllocateComponentsError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### BulkRecordEvents

- **Server group**: `Ebb`
- **Auth**: `options.BasicAuth`
- **Signature**: `BulkRecordEvents(BulkRecordEventsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ApiHandle`
- **Query params (wire ← C#)**: `store_uid` ← `StoreUid`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `BulkRecordEventsRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/BulkRecordEventsRequest.cs` |
| `EbbEvent` | `src/MaxioAdvancedBilling/Models/EbbEvent.cs` |

### BulkResetSubscriptionComponentsPricePoints

- **Auth**: `options.BasicAuth`
- **Signature**: `BulkResetSubscriptionComponentsPricePoints(BulkResetSubscriptionComponentsPricePointsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `BulkResetSubscriptionComponentsPricePointsRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/BulkResetSubscriptionComponentsPricePointsRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |

### BulkUpdateSubscriptionComponentsPricePoints

- **Auth**: `options.BasicAuth`
- **Signature**: `BulkUpdateSubscriptionComponentsPricePoints(BulkUpdateSubscriptionComponentsPricePointsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `BulkComponentsPricePointAssignment`
- **Error**: `ApiException<BulkUpdateSubscriptionComponentsPricePointsError>` — **Case A (typed)**
- **Error accessors**: `TryGetComponentPricePointError1(out ComponentPricePointError1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `BulkUpdateSubscriptionComponentsPricePointsRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/BulkUpdateSubscriptionComponentsPricePointsRequest.cs` |
| `BulkComponentsPricePointAssignment` | `src/MaxioAdvancedBilling/Models/BulkComponentsPricePointAssignment.cs` |
| `BulkUpdateSubscriptionComponentsPricePointsError` | `src/MaxioAdvancedBilling/Errors/BulkUpdateSubscriptionComponentsPricePointsError.cs` |
| `ComponentPricePointError1` | `src/MaxioAdvancedBilling/Models/ComponentPricePointError1.cs` |

### CreateUsage

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateUsage(CreateUsageOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionIdOrReference`, `ComponentId`
- **Returns**: `UsageResponse`
- **Error**: `ApiException<CreateUsageError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateUsageOperationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/CreateUsageOperationRequest.cs` |
| `SubscriptionIdOrReference` | `src/MaxioAdvancedBilling/Models/AnyOf/SubscriptionIdOrReference.cs` |
| `ComponentIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ComponentIdModel.cs` |
| `CreateUsageRequest` | `src/MaxioAdvancedBilling/Models/CreateUsageRequest.cs` |
| `UsageResponse` | `src/MaxioAdvancedBilling/Models/UsageResponse.cs` |
| `CreateUsageError` | `src/MaxioAdvancedBilling/Errors/CreateUsageError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### DeactivateEventBasedComponent

- **Auth**: `options.BasicAuth`
- **Signature**: `DeactivateEventBasedComponent(DeactivateEventBasedComponentRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `ComponentId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `DeactivateEventBasedComponentRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/DeactivateEventBasedComponentRequest.cs` |

### DeletePrepaidUsageAllocation

- **Auth**: `options.BasicAuth`
- **Signature**: `DeletePrepaidUsageAllocation(DeletePrepaidUsageAllocationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `ComponentId`, `AllocationId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<DeletePrepaidUsageAllocationError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetSubscriptionComponentAllocationError1(out SubscriptionComponentAllocationError1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `DeletePrepaidUsageAllocationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/DeletePrepaidUsageAllocationRequest.cs` |
| `CreditSchemeRequest` | `src/MaxioAdvancedBilling/Models/CreditSchemeRequest.cs` |
| `DeletePrepaidUsageAllocationError` | `src/MaxioAdvancedBilling/Errors/DeletePrepaidUsageAllocationError.cs` |
| `SubscriptionComponentAllocationError1` | `src/MaxioAdvancedBilling/Models/SubscriptionComponentAllocationError1.cs` |

### ListAllocations

- **Auth**: `options.BasicAuth`
- **Signature**: `ListAllocations(ListAllocationsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `ComponentId`
- **Query params (wire ← C#)**: `page` ← `Page`
- **Returns**: `IReadOnlyList<AllocationResponse>`
- **Error**: `ApiException<ListAllocationsError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListAllocationsRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/ListAllocationsRequest.cs` |
| `AllocationResponse` | `src/MaxioAdvancedBilling/Models/AllocationResponse.cs` |
| `ListAllocationsError` | `src/MaxioAdvancedBilling/Errors/ListAllocationsError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListSubscriptionComponents

- **Auth**: `options.BasicAuth`
- **Signature**: `ListSubscriptionComponents(ListSubscriptionComponentsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Query params (wire ← C#)**: `date_field` ← `DateField`, `direction` ← `Direction`, `filter` ← `Filter`, `end_date` ← `EndDate`, `end_datetime` ← `EndDatetime`, `price_point_ids` ← `PricePointIds`, `product_family_ids` ← `ProductFamilyIds`, `sort` ← `Sort`, `start_date` ← `StartDate`, `start_datetime` ← `StartDatetime`, `include` ← `Include`, `in_use` ← `InUse`
- **Returns**: `IReadOnlyList<SubscriptionComponentResponse>`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListSubscriptionComponentsRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/ListSubscriptionComponentsRequest.cs` |
| `SubscriptionListDateField` | `src/MaxioAdvancedBilling/Models/Enums/SubscriptionListDateField.cs` |
| `SortingDirection` | `src/MaxioAdvancedBilling/Models/Enums/SortingDirection.cs` |
| `ListSubscriptionComponentsFilter` | `src/MaxioAdvancedBilling/Models/ListSubscriptionComponentsFilter.cs` |
| `IncludeNotNull` | `src/MaxioAdvancedBilling/Models/Enums/IncludeNotNull.cs` |
| `ListSubscriptionComponentsSort` | `src/MaxioAdvancedBilling/Models/Enums/ListSubscriptionComponentsSort.cs` |
| `ListSubscriptionComponentsInclude` | `src/MaxioAdvancedBilling/Models/Enums/ListSubscriptionComponentsInclude.cs` |
| `SubscriptionComponentResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionComponentResponse.cs` |

### ListSubscriptionComponentsForSite

- **Auth**: `options.BasicAuth`
- **Signature**: `ListSubscriptionComponentsForSite(ListSubscriptionComponentsForSiteRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `sort` ← `Sort`, `direction` ← `Direction`, `filter` ← `Filter`, `date_field` ← `DateField`, `start_date` ← `StartDate`, `start_datetime` ← `StartDatetime`, `end_date` ← `EndDate`, `end_datetime` ← `EndDatetime`, `subscription_ids` ← `SubscriptionIds`, `price_point_ids` ← `PricePointIds`, `product_family_ids` ← `ProductFamilyIds`, `include` ← `Include`
- **Returns**: `ListSubscriptionComponentsResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListSubscriptionComponentsForSiteRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/ListSubscriptionComponentsForSiteRequest.cs` |
| `ListSubscriptionComponentsSort` | `src/MaxioAdvancedBilling/Models/Enums/ListSubscriptionComponentsSort.cs` |
| `SortingDirection` | `src/MaxioAdvancedBilling/Models/Enums/SortingDirection.cs` |
| `ListSubscriptionComponentsForSiteFilter` | `src/MaxioAdvancedBilling/Models/ListSubscriptionComponentsForSiteFilter.cs` |
| `SubscriptionListDateField` | `src/MaxioAdvancedBilling/Models/Enums/SubscriptionListDateField.cs` |
| `IncludeNotNull` | `src/MaxioAdvancedBilling/Models/Enums/IncludeNotNull.cs` |
| `ListSubscriptionComponentsInclude` | `src/MaxioAdvancedBilling/Models/Enums/ListSubscriptionComponentsInclude.cs` |
| `ListSubscriptionComponentsResponse` | `src/MaxioAdvancedBilling/Models/ListSubscriptionComponentsResponse.cs` |

### ListUsages

- **Auth**: `options.BasicAuth`
- **Signature**: `ListUsages(ListUsagesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionIdOrReference`, `ComponentId`
- **Query params (wire ← C#)**: `since_id` ← `SinceId`, `max_id` ← `MaxId`, `since_date` ← `SinceDate`, `until_date` ← `UntilDate`, `page` ← `Page`, `per_page` ← `PerPage`
- **Returns**: `IReadOnlyList<UsageResponse>`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListUsagesRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/ListUsagesRequest.cs` |
| `SubscriptionIdOrReference` | `src/MaxioAdvancedBilling/Models/AnyOf/SubscriptionIdOrReference.cs` |
| `ComponentIdModel` | `src/MaxioAdvancedBilling/Models/AnyOf/ComponentIdModel.cs` |
| `UsageResponse` | `src/MaxioAdvancedBilling/Models/UsageResponse.cs` |

### PreviewAllocations

- **Auth**: `options.BasicAuth`
- **Signature**: `PreviewAllocations(PreviewAllocationsOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `AllocationPreviewResponse`
- **Error**: `ApiException<PreviewAllocationsError>` — **Case A (typed)**
- **Error accessors**: `TryGetComponentAllocationError1(out ComponentAllocationError1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `PreviewAllocationsOperationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/PreviewAllocationsOperationRequest.cs` |
| `PreviewAllocationsRequest` | `src/MaxioAdvancedBilling/Models/PreviewAllocationsRequest.cs` |
| `AllocationPreviewResponse` | `src/MaxioAdvancedBilling/Models/AllocationPreviewResponse.cs` |
| `PreviewAllocationsError` | `src/MaxioAdvancedBilling/Errors/PreviewAllocationsError.cs` |
| `ComponentAllocationError1` | `src/MaxioAdvancedBilling/Models/ComponentAllocationError1.cs` |

### ReadSubscriptionComponent

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadSubscriptionComponent(ReadSubscriptionComponentRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `ComponentId`
- **Returns**: `SubscriptionComponentResponse`
- **Error**: `ApiException<ReadSubscriptionComponentError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadSubscriptionComponentRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/ReadSubscriptionComponentRequest.cs` |
| `SubscriptionComponentResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionComponentResponse.cs` |
| `ReadSubscriptionComponentError` | `src/MaxioAdvancedBilling/Errors/ReadSubscriptionComponentError.cs` |

### RecordEvent

- **Server group**: `Ebb`
- **Auth**: `options.BasicAuth`
- **Signature**: `RecordEvent(RecordEventRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ApiHandle`
- **Query params (wire ← C#)**: `store_uid` ← `StoreUid`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `RecordEventRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/RecordEventRequest.cs` |
| `EbbEvent` | `src/MaxioAdvancedBilling/Models/EbbEvent.cs` |

### UpdatePrepaidUsageAllocationExpirationDate

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdatePrepaidUsageAllocationExpirationDate(UpdatePrepaidUsageAllocationExpirationDateRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `ComponentId`, `AllocationId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<UpdatePrepaidUsageAllocationExpirationDateError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetSubscriptionComponentAllocationError1(out SubscriptionComponentAllocationError1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdatePrepaidUsageAllocationExpirationDateRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionComponents/UpdatePrepaidUsageAllocationExpirationDateRequest.cs` |
| `UpdateAllocationExpirationDate` | `src/MaxioAdvancedBilling/Models/UpdateAllocationExpirationDate.cs` |
| `UpdatePrepaidUsageAllocationExpirationDateError` | `src/MaxioAdvancedBilling/Errors/UpdatePrepaidUsageAllocationExpirationDateError.cs` |
| `SubscriptionComponentAllocationError1` | `src/MaxioAdvancedBilling/Models/SubscriptionComponentAllocationError1.cs` |

