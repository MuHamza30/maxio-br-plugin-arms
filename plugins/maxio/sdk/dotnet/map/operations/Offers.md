<!-- Generated file — do not edit; regenerated with the SDK. -->

# Offers — operations

Accessor: `client.Offers` · Source: `src/MaxioAdvancedBilling/Api/Offers.cs` · 5 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ArchiveOffer

- **Auth**: `options.BasicAuth`
- **Signature**: `ArchiveOffer(ArchiveOfferRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `OfferId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ArchiveOfferRequest` | `src/MaxioAdvancedBilling/Requests/Offers/ArchiveOfferRequest.cs` |

### CreateOffer

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateOffer(CreateOfferOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `OfferResponse`
- **Error**: `ApiException<CreateOfferError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateOfferOperationRequest` | `src/MaxioAdvancedBilling/Requests/Offers/CreateOfferOperationRequest.cs` |
| `CreateOfferRequest` | `src/MaxioAdvancedBilling/Models/CreateOfferRequest.cs` |
| `OfferResponse` | `src/MaxioAdvancedBilling/Models/OfferResponse.cs` |
| `CreateOfferError` | `src/MaxioAdvancedBilling/Errors/CreateOfferError.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### ListOffers

- **Auth**: `options.BasicAuth`
- **Signature**: `ListOffers(ListOffersRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `include_archived` ← `IncludeArchived`
- **Returns**: `ListOffersResponse`
- **Error**: `ApiException<ListOffersError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListOffersRequest` | `src/MaxioAdvancedBilling/Requests/Offers/ListOffersRequest.cs` |
| `ListOffersResponse` | `src/MaxioAdvancedBilling/Models/ListOffersResponse.cs` |
| `ListOffersError` | `src/MaxioAdvancedBilling/Errors/ListOffersError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReadOffer

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadOffer(ReadOfferRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `OfferId`
- **Returns**: `OfferResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadOfferRequest` | `src/MaxioAdvancedBilling/Requests/Offers/ReadOfferRequest.cs` |
| `OfferResponse` | `src/MaxioAdvancedBilling/Models/OfferResponse.cs` |

### UnarchiveOffer

- **Auth**: `options.BasicAuth`
- **Signature**: `UnarchiveOffer(UnarchiveOfferRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `OfferId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `UnarchiveOfferRequest` | `src/MaxioAdvancedBilling/Requests/Offers/UnarchiveOfferRequest.cs` |

