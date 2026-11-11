<!-- Generated file — do not edit; regenerated with the SDK. -->

# Customers — operations

Accessor: `client.Customers` · Source: `src/MaxioAdvancedBilling/Api/Customers.cs` · 7 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateCustomer

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateCustomer(CreateCustomerOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `CustomerResponse`
- **Error**: `ApiException<CreateCustomerError>` — **Case A (typed)**
- **Error accessors**: `TryGetCustomerErrorResponse1(out CustomerErrorResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateCustomerOperationRequest` | `src/MaxioAdvancedBilling/Requests/Customers/CreateCustomerOperationRequest.cs` |
| `CreateCustomerRequest` | `src/MaxioAdvancedBilling/Models/CreateCustomerRequest.cs` |
| `CustomerResponse` | `src/MaxioAdvancedBilling/Models/CustomerResponse.cs` |
| `CreateCustomerError` | `src/MaxioAdvancedBilling/Errors/CreateCustomerError.cs` |
| `CustomerErrorResponse1` | `src/MaxioAdvancedBilling/Models/CustomerErrorResponse1.cs` |

### DeleteCustomer

- **Auth**: `options.BasicAuth`
- **Signature**: `DeleteCustomer(DeleteCustomerRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `DeleteCustomerRequest` | `src/MaxioAdvancedBilling/Requests/Customers/DeleteCustomerRequest.cs` |

### ListCustomerSubscriptions

- **Auth**: `options.BasicAuth`
- **Signature**: `ListCustomerSubscriptions(ListCustomerSubscriptionsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `CustomerId`
- **Returns**: `IReadOnlyList<SubscriptionResponse>`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListCustomerSubscriptionsRequest` | `src/MaxioAdvancedBilling/Requests/Customers/ListCustomerSubscriptionsRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |

### ListCustomers

- **Auth**: `options.BasicAuth`
- **Signature**: `ListCustomers(ListCustomersRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `direction` ← `Direction`, `page` ← `Page`, `per_page` ← `PerPage`, `date_field` ← `DateField`, `start_date` ← `StartDate`, `end_date` ← `EndDate`, `start_datetime` ← `StartDatetime`, `end_datetime` ← `EndDatetime`, `q` ← `Q`
- **Returns**: `IReadOnlyList<CustomerResponse>`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListCustomersRequest` | `src/MaxioAdvancedBilling/Requests/Customers/ListCustomersRequest.cs` |
| `SortingDirection` | `src/MaxioAdvancedBilling/Models/Enums/SortingDirection.cs` |
| `BasicDateField` | `src/MaxioAdvancedBilling/Models/Enums/BasicDateField.cs` |
| `CustomerResponse` | `src/MaxioAdvancedBilling/Models/CustomerResponse.cs` |

### ReadCustomer

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadCustomer(ReadCustomerRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `CustomerResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadCustomerRequest` | `src/MaxioAdvancedBilling/Requests/Customers/ReadCustomerRequest.cs` |
| `CustomerResponse` | `src/MaxioAdvancedBilling/Models/CustomerResponse.cs` |

### ReadCustomerByReference

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadCustomerByReference(ReadCustomerByReferenceRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Reference`
- **Query params (wire ← C#)**: `reference` ← `Reference`
- **Returns**: `CustomerResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadCustomerByReferenceRequest` | `src/MaxioAdvancedBilling/Requests/Customers/ReadCustomerByReferenceRequest.cs` |
| `CustomerResponse` | `src/MaxioAdvancedBilling/Models/CustomerResponse.cs` |

### UpdateCustomer

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateCustomer(UpdateCustomerOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `CustomerResponse`
- **Error**: `ApiException<UpdateCustomerError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetCustomerErrorResponse1(out CustomerErrorResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateCustomerOperationRequest` | `src/MaxioAdvancedBilling/Requests/Customers/UpdateCustomerOperationRequest.cs` |
| `UpdateCustomerRequest` | `src/MaxioAdvancedBilling/Models/UpdateCustomerRequest.cs` |
| `CustomerResponse` | `src/MaxioAdvancedBilling/Models/CustomerResponse.cs` |
| `UpdateCustomerError` | `src/MaxioAdvancedBilling/Errors/UpdateCustomerError.cs` |
| `CustomerErrorResponse1` | `src/MaxioAdvancedBilling/Models/CustomerErrorResponse1.cs` |

