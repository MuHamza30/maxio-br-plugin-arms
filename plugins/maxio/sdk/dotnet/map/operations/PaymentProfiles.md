<!-- Generated file — do not edit; regenerated with the SDK. -->

# PaymentProfiles — operations

Accessor: `client.PaymentProfiles` · Source: `src/MaxioAdvancedBilling/Api/PaymentProfiles.cs` · 12 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ChangeSubscriptionDefaultPaymentProfile

- **Auth**: `options.BasicAuth`
- **Signature**: `ChangeSubscriptionDefaultPaymentProfile(ChangeSubscriptionDefaultPaymentProfileRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `PaymentProfileId`
- **Returns**: `PaymentProfileResponse`
- **Error**: `ApiException<ChangeSubscriptionDefaultPaymentProfileError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ChangeSubscriptionDefaultPaymentProfileRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/ChangeSubscriptionDefaultPaymentProfileRequest.cs` |
| `PaymentProfileResponse` | `src/MaxioAdvancedBilling/Models/PaymentProfileResponse.cs` |
| `ChangeSubscriptionDefaultPaymentProfileError` | `src/MaxioAdvancedBilling/Errors/ChangeSubscriptionDefaultPaymentProfileError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ChangeSubscriptionGroupDefaultPaymentProfile

- **Auth**: `options.BasicAuth`
- **Signature**: `ChangeSubscriptionGroupDefaultPaymentProfile(ChangeSubscriptionGroupDefaultPaymentProfileRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`, `PaymentProfileId`
- **Returns**: `PaymentProfileResponse`
- **Error**: `ApiException<ChangeSubscriptionGroupDefaultPaymentProfileError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ChangeSubscriptionGroupDefaultPaymentProfileRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/ChangeSubscriptionGroupDefaultPaymentProfileRequest.cs` |
| `PaymentProfileResponse` | `src/MaxioAdvancedBilling/Models/PaymentProfileResponse.cs` |
| `ChangeSubscriptionGroupDefaultPaymentProfileError` | `src/MaxioAdvancedBilling/Errors/ChangeSubscriptionGroupDefaultPaymentProfileError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### CreatePaymentProfile

- **Auth**: `options.BasicAuth`
- **Signature**: `CreatePaymentProfile(CreatePaymentProfileOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `PaymentProfileResponse`
- **Error**: `ApiException<CreatePaymentProfileError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreatePaymentProfileOperationRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/CreatePaymentProfileOperationRequest.cs` |
| `CreatePaymentProfileRequest` | `src/MaxioAdvancedBilling/Models/CreatePaymentProfileRequest.cs` |
| `PaymentProfileResponse` | `src/MaxioAdvancedBilling/Models/PaymentProfileResponse.cs` |
| `CreatePaymentProfileError` | `src/MaxioAdvancedBilling/Errors/CreatePaymentProfileError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### DeleteSubscriptionGroupPaymentProfile

- **Auth**: `options.BasicAuth`
- **Signature**: `DeleteSubscriptionGroupPaymentProfile(DeleteSubscriptionGroupPaymentProfileRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Uid`, `PaymentProfileId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `DeleteSubscriptionGroupPaymentProfileRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/DeleteSubscriptionGroupPaymentProfileRequest.cs` |

### DeleteSubscriptionsPaymentProfile

- **Auth**: `options.BasicAuth`
- **Signature**: `DeleteSubscriptionsPaymentProfile(DeleteSubscriptionsPaymentProfileRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `PaymentProfileId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `DeleteSubscriptionsPaymentProfileRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/DeleteSubscriptionsPaymentProfileRequest.cs` |

### DeleteUnusedPaymentProfile

- **Auth**: `options.BasicAuth`
- **Signature**: `DeleteUnusedPaymentProfile(DeleteUnusedPaymentProfileRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `PaymentProfileId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<DeleteUnusedPaymentProfileError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `DeleteUnusedPaymentProfileRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/DeleteUnusedPaymentProfileRequest.cs` |
| `DeleteUnusedPaymentProfileError` | `src/MaxioAdvancedBilling/Errors/DeleteUnusedPaymentProfileError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ListPaymentProfiles

- **Auth**: `options.BasicAuth`
- **Signature**: `ListPaymentProfiles(ListPaymentProfilesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `customer_id` ← `CustomerId`
- **Returns**: `IReadOnlyList<PaymentProfileResponse>`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListPaymentProfilesRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/ListPaymentProfilesRequest.cs` |
| `PaymentProfileResponse` | `src/MaxioAdvancedBilling/Models/PaymentProfileResponse.cs` |

### ReadOneTimeToken

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadOneTimeToken(ReadOneTimeTokenRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `ChargifyToken`
- **Returns**: `GetOneTimeTokenRequest`
- **Error**: `ApiException<ReadOneTimeTokenError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadOneTimeTokenRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/ReadOneTimeTokenRequest.cs` |
| `GetOneTimeTokenRequest` | `src/MaxioAdvancedBilling/Models/GetOneTimeTokenRequest.cs` |
| `ReadOneTimeTokenError` | `src/MaxioAdvancedBilling/Errors/ReadOneTimeTokenError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReadPaymentProfile

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadPaymentProfile(ReadPaymentProfileRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `PaymentProfileId`
- **Returns**: `PaymentProfileResponse`
- **Error**: `ApiException<ReadPaymentProfileError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadPaymentProfileRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/ReadPaymentProfileRequest.cs` |
| `PaymentProfileResponse` | `src/MaxioAdvancedBilling/Models/PaymentProfileResponse.cs` |
| `ReadPaymentProfileError` | `src/MaxioAdvancedBilling/Errors/ReadPaymentProfileError.cs` |

### SendRequestUpdatePaymentEmail

- **Auth**: `options.BasicAuth`
- **Signature**: `SendRequestUpdatePaymentEmail(SendRequestUpdatePaymentEmailRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<SendRequestUpdatePaymentEmailError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `SendRequestUpdatePaymentEmailRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/SendRequestUpdatePaymentEmailRequest.cs` |
| `SendRequestUpdatePaymentEmailError` | `src/MaxioAdvancedBilling/Errors/SendRequestUpdatePaymentEmailError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### UpdatePaymentProfile

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdatePaymentProfile(UpdatePaymentProfileOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `PaymentProfileId`
- **Returns**: `PaymentProfileResponse`
- **Error**: `ApiException<UpdatePaymentProfileError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorStringMapResponse1(out ErrorStringMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdatePaymentProfileOperationRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/UpdatePaymentProfileOperationRequest.cs` |
| `UpdatePaymentProfileRequest` | `src/MaxioAdvancedBilling/Models/UpdatePaymentProfileRequest.cs` |
| `PaymentProfileResponse` | `src/MaxioAdvancedBilling/Models/PaymentProfileResponse.cs` |
| `UpdatePaymentProfileError` | `src/MaxioAdvancedBilling/Errors/UpdatePaymentProfileError.cs` |
| `ErrorStringMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorStringMapResponse1.cs` |

### VerifyBankAccount

- **Auth**: `options.BasicAuth`
- **Signature**: `VerifyBankAccount(VerifyBankAccountRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `BankAccountId`
- **Returns**: `BankAccountResponse`
- **Error**: `ApiException<VerifyBankAccountError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `VerifyBankAccountRequest` | `src/MaxioAdvancedBilling/Requests/PaymentProfiles/VerifyBankAccountRequest.cs` |
| `BankAccountVerificationRequest` | `src/MaxioAdvancedBilling/Models/BankAccountVerificationRequest.cs` |
| `BankAccountResponse` | `src/MaxioAdvancedBilling/Models/BankAccountResponse.cs` |
| `VerifyBankAccountError` | `src/MaxioAdvancedBilling/Errors/VerifyBankAccountError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

