<!-- Generated file — do not edit; regenerated with the SDK. -->

# Subscriptions — operations

Accessor: `client.Subscriptions` · Source: `src/MaxioAdvancedBilling/Api/Subscriptions.cs` · 12 operations

**Type sources**: the file declaring each type an operation names (`RawError` excluded — see sdk-map.md).

### ActivateSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `ActivateSubscription(ActivateSubscriptionOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<ActivateSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorArrayMapResponse1(out ErrorArrayMapResponse1)` [400] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ActivateSubscriptionOperationRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/ActivateSubscriptionOperationRequest.cs` |
| `ActivateSubscriptionRequest` | `src/MaxioAdvancedBilling/Models/ActivateSubscriptionRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `ActivateSubscriptionError` | `src/MaxioAdvancedBilling/Errors/ActivateSubscriptionError.cs` |
| `ErrorArrayMapResponse1` | `src/MaxioAdvancedBilling/Models/ErrorArrayMapResponse1.cs` |

### ApplyCouponsToSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `ApplyCouponsToSubscription(ApplyCouponsToSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Query params (wire ← C#)**: `code` ← `Code`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<ApplyCouponsToSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetSubscriptionAddCouponError1(out SubscriptionAddCouponError1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `ApplyCouponsToSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/ApplyCouponsToSubscriptionRequest.cs` |
| `AddCouponsRequest` | `src/MaxioAdvancedBilling/Models/AddCouponsRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `ApplyCouponsToSubscriptionError` | `src/MaxioAdvancedBilling/Errors/ApplyCouponsToSubscriptionError.cs` |
| `SubscriptionAddCouponError1` | `src/MaxioAdvancedBilling/Models/SubscriptionAddCouponError1.cs` |

### CreateSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `CreateSubscription(CreateSubscriptionOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<CreateSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `CreateSubscriptionOperationRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/CreateSubscriptionOperationRequest.cs` |
| `CreateSubscriptionRequest` | `src/MaxioAdvancedBilling/Models/CreateSubscriptionRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `CreateSubscriptionError` | `src/MaxioAdvancedBilling/Errors/CreateSubscriptionError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

### FindSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `FindSubscription(FindSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `reference` ← `Reference`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<FindSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetNoContent(out RawError)` [404] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `FindSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/FindSubscriptionRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `FindSubscriptionError` | `src/MaxioAdvancedBilling/Errors/FindSubscriptionError.cs` |

### ListSubscriptions

- **Auth**: `options.BasicAuth`
- **Signature**: `ListSubscriptions(ListSubscriptionsRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Query params (wire ← C#)**: `page` ← `Page`, `per_page` ← `PerPage`, `sort` ← `Sort`, `direction` ← `Direction`, `state` ← `State`, `product` ← `Product`, `q` ← `Q`, `q_scope` ← `QScope`, `customer_id` ← `CustomerId`, `product_price_point_id` ← `ProductPricePointId`, `coupon` ← `Coupon`, `coupon_code` ← `CouponCode`, `collection_method` ← `CollectionMethod`, `branding_theme_id` ← `BrandingThemeId`, `date_field` ← `DateField`, `start_date` ← `StartDate`, `end_date` ← `EndDate`, `start_datetime` ← `StartDatetime`, `end_datetime` ← `EndDatetime`, `metadata` ← `Metadata`, `group_status` ← `GroupStatus`, `dunning_exemption` ← `DunningExemption`, `payment_gateways` ← `PaymentGateways`, `currencies` ← `Currencies`, `include` ← `Include`
- **Returns**: `IReadOnlyList<SubscriptionResponse>`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ListSubscriptionsRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/ListSubscriptionsRequest.cs` |
| `SubscriptionSort` | `src/MaxioAdvancedBilling/Models/Enums/SubscriptionSort.cs` |
| `SortingDirection` | `src/MaxioAdvancedBilling/Models/Enums/SortingDirection.cs` |
| `SubscriptionStateFilter` | `src/MaxioAdvancedBilling/Models/Enums/SubscriptionStateFilter.cs` |
| `Product1` | `src/MaxioAdvancedBilling/Models/AnyOf/Product1.cs` |
| `QScope` | `src/MaxioAdvancedBilling/Models/Enums/QScope.cs` |
| `CollectionMethod1` | `src/MaxioAdvancedBilling/Models/Enums/CollectionMethod1.cs` |
| `SubscriptionDateField` | `src/MaxioAdvancedBilling/Models/Enums/SubscriptionDateField.cs` |
| `GroupStatus` | `src/MaxioAdvancedBilling/Models/Enums/GroupStatus.cs` |
| `SubscriptionListInclude` | `src/MaxioAdvancedBilling/Models/Enums/SubscriptionListInclude.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |

### OverrideSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `OverrideSubscription(OverrideSubscriptionOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `void` (Task)
- **Error**: `ApiException<OverrideSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetSingleErrorResponse1(out SingleErrorResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `OverrideSubscriptionOperationRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/OverrideSubscriptionOperationRequest.cs` |
| `OverrideSubscriptionRequest` | `src/MaxioAdvancedBilling/Models/OverrideSubscriptionRequest.cs` |
| `OverrideSubscriptionError` | `src/MaxioAdvancedBilling/Errors/OverrideSubscriptionError.cs` |
| `SingleErrorResponse1` | `src/MaxioAdvancedBilling/Models/SingleErrorResponse1.cs` |

### PreviewSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `PreviewSubscription(PreviewSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
- **Returns**: `SubscriptionPreviewResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `PreviewSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/PreviewSubscriptionRequest.cs` |
| `CreateSubscriptionRequest` | `src/MaxioAdvancedBilling/Models/CreateSubscriptionRequest.cs` |
| `SubscriptionPreviewResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionPreviewResponse.cs` |

### PurgeSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `PurgeSubscription(PurgeSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`, `Ack`
- **Query params (wire ← C#)**: `ack` ← `Ack`, `cascade` ← `Cascade`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<PurgeSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetSubscriptionResponse(out SubscriptionResponse)` [400] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `PurgeSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/PurgeSubscriptionRequest.cs` |
| `SubscriptionPurgeType` | `src/MaxioAdvancedBilling/Models/Enums/SubscriptionPurgeType.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `PurgeSubscriptionError` | `src/MaxioAdvancedBilling/Errors/PurgeSubscriptionError.cs` |

### ReadSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `ReadSubscription(ReadSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Query params (wire ← C#)**: `include` ← `Include`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<RawError>` — **Case B**

| Type | Source |
| --- | --- |
| `ReadSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/ReadSubscriptionRequest.cs` |
| `SubscriptionInclude` | `src/MaxioAdvancedBilling/Models/Enums/SubscriptionInclude.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |

### RemoveCouponFromSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `RemoveCouponFromSubscription(RemoveCouponFromSubscriptionRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Query params (wire ← C#)**: `coupon_code` ← `CouponCode`
- **Returns**: `string`
- **Error**: `ApiException<RemoveCouponFromSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetSubscriptionRemoveCouponErrors1(out SubscriptionRemoveCouponErrors1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `RemoveCouponFromSubscriptionRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/RemoveCouponFromSubscriptionRequest.cs` |
| `RemoveCouponFromSubscriptionError` | `src/MaxioAdvancedBilling/Errors/RemoveCouponFromSubscriptionError.cs` |
| `SubscriptionRemoveCouponErrors1` | `src/MaxioAdvancedBilling/Models/SubscriptionRemoveCouponErrors1.cs` |

### UpdatePrepaidSubscriptionConfiguration

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdatePrepaidSubscriptionConfiguration(UpdatePrepaidSubscriptionConfigurationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `PrepaidConfigurationResponse`
- **Error**: `ApiException<UpdatePrepaidSubscriptionConfigurationError>` — **Case A (typed)**
- **Error accessors**: `TryGetPrepaidConfigurationErrorResponse(out PrepaidConfigurationErrorResponse)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdatePrepaidSubscriptionConfigurationRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/UpdatePrepaidSubscriptionConfigurationRequest.cs` |
| `UpsertPrepaidConfigurationRequest` | `src/MaxioAdvancedBilling/Models/UpsertPrepaidConfigurationRequest.cs` |
| `PrepaidConfigurationResponse` | `src/MaxioAdvancedBilling/Models/PrepaidConfigurationResponse.cs` |
| `UpdatePrepaidSubscriptionConfigurationError` | `src/MaxioAdvancedBilling/Errors/UpdatePrepaidSubscriptionConfigurationError.cs` |
| `PrepaidConfigurationErrorResponse` | `src/MaxioAdvancedBilling/Models/AnyOf/PrepaidConfigurationErrorResponse.cs` |

### UpdateSubscription

- **Auth**: `options.BasicAuth`
- **Signature**: `UpdateSubscription(UpdateSubscriptionOperationRequest request, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)`
  - required: `SubscriptionId`
- **Returns**: `SubscriptionResponse`
- **Error**: `ApiException<UpdateSubscriptionError>` — **Case A (typed)**
- **Error accessors**: `TryGetErrorListResponse1(out ErrorListResponse1)` [422] · `TryGetRawError(out RawError)` [fallback]

| Type | Source |
| --- | --- |
| `UpdateSubscriptionOperationRequest` | `src/MaxioAdvancedBilling/Requests/Subscriptions/UpdateSubscriptionOperationRequest.cs` |
| `UpdateSubscriptionRequest` | `src/MaxioAdvancedBilling/Models/UpdateSubscriptionRequest.cs` |
| `SubscriptionResponse` | `src/MaxioAdvancedBilling/Models/SubscriptionResponse.cs` |
| `UpdateSubscriptionError` | `src/MaxioAdvancedBilling/Errors/UpdateSubscriptionError.cs` |
| `ErrorListResponse1` | `src/MaxioAdvancedBilling/Models/ErrorListResponse1.cs` |

