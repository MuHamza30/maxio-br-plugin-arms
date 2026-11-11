<!-- Generated file — do not edit; regenerated with the SDK. -->

# WalletAdjustments — operations

Accessor: `client.WalletAdjustments` · Source: `src/MaxioAdvancedBilling/Api/WalletAdjustments.cs` · 1 operation

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateWalletAdjustment

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateWalletAdjustment(CreateWalletAdjustmentOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `WalletId`
- **Returns**: `WalletGrantResultResponse`
- **Error**: `ApiException<CreateWalletAdjustmentError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [403] · `TryGetWalletErrorResponse1(out WalletErrorResponse1)` [404, 409, 422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateWalletAdjustmentOperationRequest` | `src/MaxioAdvancedBilling/Requests/WalletAdjustments/CreateWalletAdjustmentOperationRequest.cs` |
| `CreateWalletAdjustmentRequest` | `src/MaxioAdvancedBilling/Models/CreateWalletAdjustmentRequest.cs` |
| `WalletGrantResultResponse` | `src/MaxioAdvancedBilling/Models/WalletGrantResultResponse.cs` |
| `CreateWalletAdjustmentError` | `src/MaxioAdvancedBilling/Errors/CreateWalletAdjustmentError.cs` |
| `WalletErrorResponse1` | `src/MaxioAdvancedBilling/Models/WalletErrorResponse1.cs` |

