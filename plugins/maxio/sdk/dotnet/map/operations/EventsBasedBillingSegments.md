<!-- Generated file — do not edit; regenerated with the SDK. -->

# EventsBasedBillingSegments — operations

Accessor: `client.EventsBasedBillingSegments` · Source: `src/MaxioAdvancedBilling/Api/EventsBasedBillingSegments.cs` · 6 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### BulkCreateSegments

- **Auth**: `options.BasicAuth`
- **Signature**: `BulkCreateSegments(BulkCreateSegmentsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`
- **Returns**: `ListSegmentsResponse`
- **Error**: `ApiException<BulkCreateSegmentsError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetEventBasedBillingSegment1(out EventBasedBillingSegment1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `BulkCreateSegmentsRequest` | `src/MaxioAdvancedBilling/Requests/EventsBasedBillingSegments/BulkCreateSegmentsRequest.cs` |
| `BulkCreateSegments` | `src/MaxioAdvancedBilling/Models/BulkCreateSegments.cs` |
| `ListSegmentsResponse` | `src/MaxioAdvancedBilling/Models/ListSegmentsResponse.cs` |
| `BulkCreateSegmentsError` | `src/MaxioAdvancedBilling/Errors/BulkCreateSegmentsError.cs` |
| `EventBasedBillingSegment1` | `src/MaxioAdvancedBilling/Models/EventBasedBillingSegment1.cs` |

### BulkUpdateSegments

- **Auth**: `options.BasicAuth`
- **Signature**: `BulkUpdateSegments(BulkUpdateSegmentsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`
- **Returns**: `ListSegmentsResponse`
- **Error**: `ApiException<BulkUpdateSegmentsError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetEventBasedBillingSegment1(out EventBasedBillingSegment1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `BulkUpdateSegmentsRequest` | `src/MaxioAdvancedBilling/Requests/EventsBasedBillingSegments/BulkUpdateSegmentsRequest.cs` |
| `BulkUpdateSegments` | `src/MaxioAdvancedBilling/Models/BulkUpdateSegments.cs` |
| `ListSegmentsResponse` | `src/MaxioAdvancedBilling/Models/ListSegmentsResponse.cs` |
| `BulkUpdateSegmentsError` | `src/MaxioAdvancedBilling/Errors/BulkUpdateSegmentsError.cs` |
| `EventBasedBillingSegment1` | `src/MaxioAdvancedBilling/Models/EventBasedBillingSegment1.cs` |

### CreateSegment

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateSegment(CreateSegmentOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`
- **Returns**: `SegmentResponse`
- **Error**: `ApiException<CreateSegmentError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetEventBasedBillingSegmentErrors1(out EventBasedBillingSegmentErrors1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateSegmentOperationRequest` | `src/MaxioAdvancedBilling/Requests/EventsBasedBillingSegments/CreateSegmentOperationRequest.cs` |
| `CreateSegmentRequest` | `src/MaxioAdvancedBilling/Models/CreateSegmentRequest.cs` |
| `SegmentResponse` | `src/MaxioAdvancedBilling/Models/SegmentResponse.cs` |
| `CreateSegmentError` | `src/MaxioAdvancedBilling/Errors/CreateSegmentError.cs` |
| `EventBasedBillingSegmentErrors1` | `src/MaxioAdvancedBilling/Models/EventBasedBillingSegmentErrors1.cs` |

### DeleteSegment

- **Auth**: `options.BasicAuth`
- **Signature**: `DeleteSegment(DeleteSegmentRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`, `Id`
- **Returns**: `void` (Task)
- **Error**: `ApiException<DeleteSegmentError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `DeleteSegmentRequest` | `src/MaxioAdvancedBilling/Requests/EventsBasedBillingSegments/DeleteSegmentRequest.cs` |
| `DeleteSegmentError` | `src/MaxioAdvancedBilling/Errors/DeleteSegmentError.cs` |

### ListSegmentsForPricePoint

- **Auth**: `options.BasicAuth`
- **Signature**: `ListSegmentsForPricePoint(ListSegmentsForPricePointRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `filter` ← `Filter`
- **Returns**: `ListSegmentsResponse`
- **Error**: `ApiException<ListSegmentsForPricePointError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetEventBasedBillingListSegmentsErrors1(out EventBasedBillingListSegmentsErrors1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListSegmentsForPricePointRequest` | `src/MaxioAdvancedBilling/Requests/EventsBasedBillingSegments/ListSegmentsForPricePointRequest.cs` |
| `ListSegmentsFilter` | `src/MaxioAdvancedBilling/Models/ListSegmentsFilter.cs` |
| `ListSegmentsResponse` | `src/MaxioAdvancedBilling/Models/ListSegmentsResponse.cs` |
| `ListSegmentsForPricePointError` | `src/MaxioAdvancedBilling/Errors/ListSegmentsForPricePointError.cs` |
| `EventBasedBillingListSegmentsErrors1` | `src/MaxioAdvancedBilling/Models/EventBasedBillingListSegmentsErrors1.cs` |

### UpdateSegment

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateSegment(UpdateSegmentOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ComponentId`, `PricePointId`, `Id`
- **Returns**: `SegmentResponse`
- **Error**: `ApiException<UpdateSegmentError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetEventBasedBillingSegmentErrors1(out EventBasedBillingSegmentErrors1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateSegmentOperationRequest` | `src/MaxioAdvancedBilling/Requests/EventsBasedBillingSegments/UpdateSegmentOperationRequest.cs` |
| `UpdateSegmentRequest` | `src/MaxioAdvancedBilling/Models/UpdateSegmentRequest.cs` |
| `SegmentResponse` | `src/MaxioAdvancedBilling/Models/SegmentResponse.cs` |
| `UpdateSegmentError` | `src/MaxioAdvancedBilling/Errors/UpdateSegmentError.cs` |
| `EventBasedBillingSegmentErrors1` | `src/MaxioAdvancedBilling/Models/EventBasedBillingSegmentErrors1.cs` |

