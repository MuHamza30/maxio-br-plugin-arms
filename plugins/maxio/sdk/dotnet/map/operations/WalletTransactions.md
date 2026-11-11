<!-- Generated file — do not edit; regenerated with the SDK. -->

# WalletTransactions — operations

Accessor: `client.WalletTransactions` · Source: `src/MaxioAdvancedBilling/Api/WalletTransactions.cs` · 2 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateWalletTransaction

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateWalletTransaction(CreateWalletTransactionOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `WalletId`
- **Returns**: `WalletTransactionResultResponse`
- **Error**: `ApiException<CreateWalletTransactionError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404, 422, 500, 503] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateWalletTransactionOperationRequest` | `src/MaxioAdvancedBilling/Requests/WalletTransactions/CreateWalletTransactionOperationRequest.cs` |
| `CreateWalletTransactionRequest` | `src/MaxioAdvancedBilling/Models/CreateWalletTransactionRequest.cs` |
| `WalletTransactionResultResponse` | `src/MaxioAdvancedBilling/Models/WalletTransactionResultResponse.cs` |
| `CreateWalletTransactionError` | `src/MaxioAdvancedBilling/Errors/CreateWalletTransactionError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### ListWalletTransactions

- **Auth**: `options.BasicAuth`
- **Signature**: `ListWalletTransactions(ListWalletTransactionsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `WalletId`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`
- **Returns**: `WalletTransactionsListResponse`
- **Error**: `ApiException<ListWalletTransactionsError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListWalletTransactionsRequest` | `src/MaxioAdvancedBilling/Requests/WalletTransactions/ListWalletTransactionsRequest.cs` |
| `WalletTransactionsListResponse` | `src/MaxioAdvancedBilling/Models/WalletTransactionsListResponse.cs` |
| `ListWalletTransactionsError` | `src/MaxioAdvancedBilling/Errors/ListWalletTransactionsError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

