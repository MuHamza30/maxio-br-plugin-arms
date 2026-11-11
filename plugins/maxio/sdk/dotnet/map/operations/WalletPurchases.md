<!-- Generated file — do not edit; regenerated with the SDK. -->

# WalletPurchases — operations

Accessor: `client.WalletPurchases` · Source: `src/MaxioAdvancedBilling/Api/WalletPurchases.cs` · 2 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateWalletPurchase

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateWalletPurchase(CreateWalletPurchaseOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `WalletId`
- **Returns**: `WalletPurchaseResultResponse`
- **Error**: `ApiException<CreateWalletPurchaseError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateWalletPurchaseOperationRequest` | `src/MaxioAdvancedBilling/Requests/WalletPurchases/CreateWalletPurchaseOperationRequest.cs` |
| `CreateWalletPurchaseRequest` | `src/MaxioAdvancedBilling/Models/CreateWalletPurchaseRequest.cs` |
| `WalletPurchaseResultResponse` | `src/MaxioAdvancedBilling/Models/WalletPurchaseResultResponse.cs` |
| `CreateWalletPurchaseError` | `src/MaxioAdvancedBilling/Errors/CreateWalletPurchaseError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

### PreviewWalletPurchase

- **Auth**: `options.BasicAuth`
- **Signature**: `PreviewWalletPurchase(PreviewWalletPurchaseRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `WalletId`
- **Returns**: `WalletPurchasePreviewResponse`
- **Error**: `ApiException<PreviewWalletPurchaseError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `PreviewWalletPurchaseRequest` | `src/MaxioAdvancedBilling/Requests/WalletPurchases/PreviewWalletPurchaseRequest.cs` |
| `CreateWalletPurchaseRequest` | `src/MaxioAdvancedBilling/Models/CreateWalletPurchaseRequest.cs` |
| `WalletPurchasePreviewResponse` | `src/MaxioAdvancedBilling/Models/WalletPurchasePreviewResponse.cs` |
| `PreviewWalletPurchaseError` | `src/MaxioAdvancedBilling/Errors/PreviewWalletPurchaseError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

