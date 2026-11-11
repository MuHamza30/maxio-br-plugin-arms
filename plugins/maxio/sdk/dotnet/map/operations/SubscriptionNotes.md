<!-- Generated file — do not edit; regenerated with the SDK. -->

# SubscriptionNotes — operations

Accessor: `client.SubscriptionNotes` · Source: `src/MaxioAdvancedBilling/Api/SubscriptionNotes.cs` · 5 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### CreateSubscriptionNote

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateSubscriptionNote(CreateSubscriptionNoteRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `SubscriptionNoteResponse`
- **Error**: `ApiException<CreateSubscriptionNoteError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateSubscriptionNoteRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionNotes/CreateSubscriptionNoteRequest.cs` |
| `UpdateSubscriptionNoteRequest` | `src/MaxioAdvancedBilling/Models/UpdateSubscriptionNoteRequest.cs` |
| `SubscriptionNoteResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionNoteResponse.cs` |
| `CreateSubscriptionNoteError` | `src/MaxioAdvancedBilling/Errors/CreateSubscriptionNoteError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### DeleteSubscriptionNote

- **Auth**: `options.BasicAuth`
- **Signature**: `DeleteSubscriptionNote(DeleteSubscriptionNoteRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `NoteId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `DeleteSubscriptionNoteRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionNotes/DeleteSubscriptionNoteRequest.cs` |

### ListSubscriptionNotes

- **Auth**: `options.BasicAuth`
- **Signature**: `ListSubscriptionNotes(ListSubscriptionNotesRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`
- **Returns**: `IReadOnlyList<SubscriptionNoteResponse>`
- **Error**: `ApiException<ListSubscriptionNotesError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ListSubscriptionNotesRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionNotes/ListSubscriptionNotesRequest.cs` |
| `SubscriptionNoteResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionNoteResponse.cs` |
| `ListSubscriptionNotesError` | `src/MaxioAdvancedBilling/Errors/ListSubscriptionNotesError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### ReadSubscriptionNote

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadSubscriptionNote(ReadSubscriptionNoteRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `NoteId`
- **Returns**: `SubscriptionNoteResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadSubscriptionNoteRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionNotes/ReadSubscriptionNoteRequest.cs` |
| `SubscriptionNoteResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionNoteResponse.cs` |

### UpdateSubscriptionNote

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateSubscriptionNote(UpdateSubscriptionNoteOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `NoteId`
- **Returns**: `SubscriptionNoteResponse`
- **Error**: `ApiException<UpdateSubscriptionNoteError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateSubscriptionNoteOperationRequest` | `src/MaxioAdvancedBilling/Requests/SubscriptionNotes/UpdateSubscriptionNoteOperationRequest.cs` |
| `UpdateSubscriptionNoteRequest` | `src/MaxioAdvancedBilling/Models/UpdateSubscriptionNoteRequest.cs` |
| `SubscriptionNoteResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionNoteResponse.cs` |
| `UpdateSubscriptionNoteError` | `src/MaxioAdvancedBilling/Errors/UpdateSubscriptionNoteError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

