<!-- Generated file — do not edit; regenerated with the SDK. -->

# WebhooksApi — operations

Accessor: `client.WebhooksApi` · Source: `src/MaxioAdvancedBilling/Api/WebhooksApi.cs` · 6 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateEndpoint

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateEndpoint(CreateEndpointRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `EndpointResponse`
- **Error**: `ApiException<CreateEndpointError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateEndpointRequest` | `src/MaxioAdvancedBilling/Requests/WebhooksApi/CreateEndpointRequest.cs` |
| `CreateOrUpdateEndpointRequest` | `src/MaxioAdvancedBilling/Models/CreateOrUpdateEndpointRequest.cs` |
| `EndpointResponse` | `src/MaxioAdvancedBilling/Models/EndpointResponse.cs` |
| `CreateEndpointError` | `src/MaxioAdvancedBilling/Errors/CreateEndpointError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### EnableWebhooks

- **Auth**: `options.BasicAuth`
- **Signature**: `EnableWebhooks(EnableWebhooksOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `EnableWebhooksResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `EnableWebhooksOperationRequest` | `src/MaxioAdvancedBilling/Requests/WebhooksApi/EnableWebhooksOperationRequest.cs` |
| `EnableWebhooksRequest` | `src/MaxioAdvancedBilling/Models/EnableWebhooksRequest.cs` |
| `EnableWebhooksResponse` | `src/MaxioAdvancedBilling/Models/EnableWebhooksResponse.cs` |

### ListEndpoints

- **Auth**: `options.BasicAuth`
- **Signature**: `ListEndpoints(RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `IReadOnlyList<Endpoint>`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `Endpoint` | `src/MaxioAdvancedBilling/Models/Endpoint.cs` |

### ListWebhooks

- **Auth**: `options.BasicAuth`
- **Signature**: `ListWebhooks(ListWebhooksRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `status` ← `Status`, `since_date` ← `SinceDate`, `until_date` ← `UntilDate`, `page` ← `Page`, `per_page` ← `PerPage`, `order` ← `Order`, `subscription` ← `Subscription`
- **Returns**: `IReadOnlyList<WebhookResponse>`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListWebhooksRequest` | `src/MaxioAdvancedBilling/Requests/WebhooksApi/ListWebhooksRequest.cs` |
| `WebhookStatus` | `src/MaxioAdvancedBilling/Models/Enums/WebhookStatus.cs` |
| `WebhookOrder` | `src/MaxioAdvancedBilling/Models/Enums/WebhookOrder.cs` |
| `WebhookResponse` | `src/MaxioAdvancedBilling/Models/WebhookResponse.cs` |

### ReplayWebhooks

- **Auth**: `options.BasicAuth`
- **Signature**: `ReplayWebhooks(ReplayWebhooksOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `ReplayWebhooksResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReplayWebhooksOperationRequest` | `src/MaxioAdvancedBilling/Requests/WebhooksApi/ReplayWebhooksOperationRequest.cs` |
| `ReplayWebhooksRequest` | `src/MaxioAdvancedBilling/Models/ReplayWebhooksRequest.cs` |
| `ReplayWebhooksResponse` | `src/MaxioAdvancedBilling/Models/ReplayWebhooksResponse.cs` |

### UpdateEndpoint

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateEndpoint(UpdateEndpointRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `EndpointId`
- **Returns**: `EndpointResponse`
- **Error**: `ApiException<UpdateEndpointError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateEndpointRequest` | `src/MaxioAdvancedBilling/Requests/WebhooksApi/UpdateEndpointRequest.cs` |
| `CreateOrUpdateEndpointRequest` | `src/MaxioAdvancedBilling/Models/CreateOrUpdateEndpointRequest.cs` |
| `EndpointResponse` | `src/MaxioAdvancedBilling/Models/EndpointResponse.cs` |
| `UpdateEndpointError` | `src/MaxioAdvancedBilling/Errors/UpdateEndpointError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

