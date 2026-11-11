<!-- Generated file — do not edit; regenerated with the SDK. -->

# Insights — operations

Accessor: `client.Insights` · Source: `src/MaxioAdvancedBilling/Api/Insights.cs` · 4 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ListMrrMovements

- **Auth**: `options.BasicAuth`
- **Signature**: `ListMrrMovements(ListMrrMovementsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `subscription_id` ← `SubscriptionId`, `page` ← `Page`, `per_page` ← `PerPage`, `direction` ← `Direction`
- **Returns**: `ListMrrResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListMrrMovementsRequest` | `src/MaxioAdvancedBilling/Requests/Insights/ListMrrMovementsRequest.cs` |
| `SortingDirection` | `src/MaxioAdvancedBilling/Models/Enums/SortingDirection.cs` |
| `ListMrrResponse` | `src/MaxioAdvancedBilling/Models/ListMrrResponse.cs` |

### ListMrrPerSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `ListMrrPerSubscription(ListMrrPerSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `filter` ← `Filter`, `at_time` ← `AtTime`, `page` ← `Page`, `per_page` ← `PerPage`, `direction` ← `Direction`
- **Returns**: `SubscriptionMrrResponse`
- **Error**: `ApiException<ListMrrPerSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetSubscriptionsMrrErrorResponse1(out SubscriptionsMrrErrorResponse1)` [400] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListMrrPerSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/Insights/ListMrrPerSubscriptionRequest.cs` |
| `ListMrrFilter` | `src/MaxioAdvancedBilling/Models/ListMrrFilter.cs` |
| `Direction` | `src/MaxioAdvancedBilling/Models/Enums/Direction.cs` |
| `SubscriptionMrrResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionMrrResponse.cs` |
| `ListMrrPerSubscriptionError` | `src/MaxioAdvancedBilling/Errors/ListMrrPerSubscriptionError.cs` |
| `SubscriptionsMrrErrorResponse1` | `src/MaxioAdvancedBilling/Models/SubscriptionsMrrErrorResponse1.cs` |

### ReadMrr

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadMrr(ReadMrrRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `at_time` ← `AtTime`, `subscription_id` ← `SubscriptionId`
- **Returns**: `MrrResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadMrrRequest` | `src/MaxioAdvancedBilling/Requests/Insights/ReadMrrRequest.cs` |
| `MrrResponse` | `src/MaxioAdvancedBilling/Models/MrrResponse.cs` |

### ReadSiteStats

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadSiteStats(RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `SiteSummary`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `SiteSummary` | `src/MaxioAdvancedBilling/Models/SiteSummary.cs` |

