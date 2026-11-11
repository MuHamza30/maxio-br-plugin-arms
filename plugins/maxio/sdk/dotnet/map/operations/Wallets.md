<!-- Generated file — do not edit; regenerated with the SDK. -->

# Wallets — operations

Accessor: `client.Wallets` · Source: `src/MaxioAdvancedBilling/Api/Wallets.cs` · 5 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ArchiveWallet

- **Auth**: `options.BasicAuth`
- **Signature**: `ArchiveWallet(ArchiveWalletRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `WalletResponse`
- **Error**: `ApiException<ArchiveWalletError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ArchiveWalletRequest` | `src/MaxioAdvancedBilling/Requests/Wallets/ArchiveWalletRequest.cs` |
| `WalletResponse` | `src/MaxioAdvancedBilling/Models/WalletResponse.cs` |
| `ArchiveWalletError` | `src/MaxioAdvancedBilling/Errors/ArchiveWalletError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### CreateWallet

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateWallet(CreateWalletOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `WalletResponse`
- **Error**: `ApiException<CreateWalletError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateWalletOperationRequest` | `src/MaxioAdvancedBilling/Requests/Wallets/CreateWalletOperationRequest.cs` |
| `CreateWalletRequest` | `src/MaxioAdvancedBilling/Models/CreateWalletRequest.cs` |
| `WalletResponse` | `src/MaxioAdvancedBilling/Models/WalletResponse.cs` |
| `CreateWalletError` | `src/MaxioAdvancedBilling/Errors/CreateWalletError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### ListWallets

- **Auth**: `options.BasicAuth`
- **Signature**: `ListWallets(ListWalletsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `subscription_id` ← `SubscriptionId`, `status` ← `Status`
- **Returns**: `WalletsListResponse`
- **Error**: `ApiException<ListWalletsError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListWalletsRequest` | `src/MaxioAdvancedBilling/Requests/Wallets/ListWalletsRequest.cs` |
| `Status2` | `src/MaxioAdvancedBilling/Models/Enums/Status2.cs` |
| `WalletsListResponse` | `src/MaxioAdvancedBilling/Models/WalletsListResponse.cs` |
| `ListWalletsError` | `src/MaxioAdvancedBilling/Errors/ListWalletsError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### ReadWallet

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadWallet(ReadWalletRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `WalletResponse`
- **Error**: `ApiException<ReadWalletError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ReadWalletRequest` | `src/MaxioAdvancedBilling/Requests/Wallets/ReadWalletRequest.cs` |
| `WalletResponse` | `src/MaxioAdvancedBilling/Models/WalletResponse.cs` |
| `ReadWalletError` | `src/MaxioAdvancedBilling/Errors/ReadWalletError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### UpdateWallet

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateWallet(UpdateWalletOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `Id`
- **Returns**: `WalletResponse`
- **Error**: `ApiException<UpdateWalletError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateWalletOperationRequest` | `src/MaxioAdvancedBilling/Requests/Wallets/UpdateWalletOperationRequest.cs` |
| `UpdateWalletRequest` | `src/MaxioAdvancedBilling/Models/UpdateWalletRequest.cs` |
| `WalletResponse` | `src/MaxioAdvancedBilling/Models/WalletResponse.cs` |
| `UpdateWalletError` | `src/MaxioAdvancedBilling/Errors/UpdateWalletError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

