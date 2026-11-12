<!-- Generated file — do not edit; regenerated with the SDK. -->

# Entitlements — operations

Accessor: `client.Entitlements` · Source: `src/MaxioAdvancedBilling/Api/Entitlements.cs` · 1 operation

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ReadSubscriptionEntitlements

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadSubscriptionEntitlements(ReadSubscriptionEntitlementsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `AggregatedEntitlementsResponse`
- **Error**: `ApiException<ReadSubscriptionEntitlementsError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1([NotNullWhen(true)] out ErrorListResponse1? value)` [403] · `TryGetNoContent([NotNullWhen(true)] out RawError? value)` [404] · `TryGetRawError([NotNullWhen(true)] out RawError? error)` [fallback]

| Type | Source |
| --- | --- |
| `ReadSubscriptionEntitlementsRequest` | `src/MaxioAdvancedBilling/Requests/Entitlements/ReadSubscriptionEntitlementsRequest.cs` |
| `AggregatedEntitlementsResponse` | `src/MaxioAdvancedBilling/Models/AggregatedEntitlementsResponse.cs` |
| `ReadSubscriptionEntitlementsError` | `src/MaxioAdvancedBilling/Errors/ReadSubscriptionEntitlementsError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

