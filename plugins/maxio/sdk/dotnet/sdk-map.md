<!-- Generated file — do not edit; regenerated with the SDK. -->

# SDK map — Maxio Advanced Billing (.NET)

> A generated table of contents for this SDK. Consult this map and its sub-pages to learn signatures, error types, and server/auth wiring **by lookup**. Model shapes and enum values are *not* duplicated here — the map names the file declaring each type; read the shape there. The compiler is the backstop: a wrong name fails to build.

|  |  |
| --- | --- |
| SDK display name | Maxio Advanced Billing |
| Root namespace | `MaxioAdvancedBilling` |
| Target framework | `netstandard2.0` (C# `LangVersion 14`, `Nullable enable`) |
| API spec version | `1.0` |
| Generator | APIMatic |

Staleness check: the API spec version above changes when the SDK is regenerated from a new spec. If a lookup here fails to compile, trust the compiler and re-read the source file named in the row.

All `Source` paths on this map and its sub-pages are relative to the **SDK root** — the directory holding this file — never to the page that carries them; the project file is `src/MaxioAdvancedBilling/MaxioAdvancedBilling.csproj`. Open them as-is from the SDK root, from any page; if the SDK sits under a subdirectory of a larger repo, prefix that subdirectory.

---

## Getting a client

```csharp
var httpClient = new HttpClient();
// TODO: configure more client options here
var options = new MaxioAdvancedBillingClientOptions
{
    BasicAuth = new BasicAuthCredentials { Username = "YOUR_USERNAME", Password = "YOUR_PASSWORD" },
    Environment = ServerEnvironment.Us,
};
var client = new MaxioAdvancedBillingClient(httpClient, options);
```

DI alternative (`services.AddMaxioAdvancedBillingClient`):

```csharp
services.AddMaxioAdvancedBillingClient(options =>
{
    options.BasicAuth = new BasicAuthCredentials { Username = "YOUR_USERNAME", Password = "YOUR_PASSWORD" };
    options.Environment = ServerEnvironment.Us;
    // TODO: configure more client options here
});
```

Every API group is a property on the client (e.g. `client.ApiExports`). Source: `src/MaxioAdvancedBilling/MaxioAdvancedBillingClient.cs`. The only constructor is `MaxioAdvancedBillingClient(HttpClient httpClient, MaxioAdvancedBillingClientOptions options)`.

All `MaxioAdvancedBillingClientOptions` properties (source: `src/MaxioAdvancedBilling/MaxioAdvancedBillingClientOptions.cs`):

| Property | Type |
| --- | --- |
| `Environment` | `ServerEnvironment` |
| `Retry` | `RetryOptions` |
| `Logging` | `LoggingOptions` |
| `TimeProvider` | `TimeProvider` |
| `Server` | `ServerOptions` |
| `StreamReadTimeout` | `TimeSpan?` |
| `Hooks` | `IReadOnlyList<SdkHook>` |
| `BasicAuth` | `BasicAuthCredentials?` |
| `BasicAuthProvider` | `CredentialProvider<BasicAuthCredentials>?` |

`ServerEnvironment` (source: `src/MaxioAdvancedBilling/Servers/ServerEnvironment.cs`, namespace `MaxioAdvancedBilling.Servers`)
`BasicAuthCredentials` (source: `src/MaxioAdvancedBilling/Core/Authentication/Basic/BasicAuthCredentials.cs`, namespace `MaxioAdvancedBilling.Core.Authentication.Basic`)

`RetryOptions` members (namespace `MaxioAdvancedBilling.Core.Configuration` — add `using MaxioAdvancedBilling.Core.Configuration;`; source: `src/MaxioAdvancedBilling/Core/Configuration/RetryOptions.cs`; all members are `required`, so build a full instance or start from `RetryOptions.Default()`):

| Member | Type |
| --- | --- |
| `StatusCodesToRetry` | `IReadOnlyList<HttpStatusCode>` |
| `HttpMethodsToRetry` | `IReadOnlyList<HttpMethod>` |
| `MaxRetries` | `int` |
| `Delay` | `TimeSpan` |
| `Timeout` | `TimeSpan?` |
| `BackOffFactor` | `int` |
| `UseExponentialBackoff` | `bool` |
| `MaxJitter` | `TimeSpan` |
| `OnRetry` | `Action<RetryAttempt>?` |

---

## Error-handling model (read once — applies to every operation)

Operations are **throw-based**. On an error status the SDK throws `ApiException<TError>` (`src/MaxioAdvancedBilling/Core/Exceptions/ApiException.cs`, namespace `MaxioAdvancedBilling.Core.Exceptions`) exposing `.Error` of type `TError` beside the `StatusCode`, `Headers` and `ContentType` of the response. There are two cases:

- **Case A — typed error.** `TError` is a generated `…Error : ApiError` class (namespace `MaxioAdvancedBilling.Errors`) with status-specific `TryGet…(out …)` accessors (each returns `true` when that shape is present) plus the inherited `TryGetRawError(out RawError)` fallback. The operation blocks name the exact `TryGet…` methods and the HTTP status each maps to.
- **Case B — raw error.** `TError` is `RawError` (`src/MaxioAdvancedBilling/Core/ErrorResponse/RawError.cs`, namespace `MaxioAdvancedBilling.Core.ErrorResponse`): `StatusCode: HttpStatusCode` · `ReadAsBytes(): ReadOnlyMemory<byte>` · `ReadAsString(): string` · `ReadAsJson<T>(): T?`.

⚠ Each of the three lives in its own namespace. `src/MaxioAdvancedBilling/Core/` holds several namespaces, so a catch block naming `ApiException<T>`, a typed `{Operation}Error` and `RawError` together needs a `using` for each.

`ApiException<TError>` is one leaf of the `SdkException` family (`src/MaxioAdvancedBilling/Core/Exceptions/SdkException.cs`): a request that produced no usable response surfaces as `SdkConnectionException` or `SdkTimeoutException`, a body that does not match its declared type as `ResponseDeserializationException`, and a required credential that is not configured, or one that could not be applied, as `AuthSchemeException`. Every one of them names the failed call. The full table is in [README → Error Handling](README.md#error-handling).

Core error types (`src/MaxioAdvancedBilling/Core/ErrorResponse/`) — public members with their **declared types**, verbatim from source:

| Type | Public members | Source |
| --- | --- | --- |
| `ApiError` — abstract base of the 201 typed error classes in `src/MaxioAdvancedBilling/Errors/` | `TryGetRawError(out RawError error): bool` | `src/MaxioAdvancedBilling/Core/ErrorResponse/ApiError.cs` |
| `RawError` | `StatusCode: HttpStatusCode` · `ReadAsBytes(): ReadOnlyMemory<byte>` · `ReadAsString(): string` · `ReadAsJson<T>(): T?` | `src/MaxioAdvancedBilling/Core/ErrorResponse/RawError.cs` |

Typed-error payload shapes (the `out` types in each operation page's error-accessor cells) are ordinary records/unions — no special handling. The operation's **Type sources** table gives the file that declares each one; read field names, declared types, and JSON wire names there, as for any other model.

```csharp
try
{
    var response = await client.ApiExports.ExportInvoices();
}
catch (ApiException<ExportInvoicesError> ex)
{
    // Case A — typed error
    if (ex.Error.TryGetNoContent(out var error))
    {
        // Handle 404
    }
    else if (ex.Error.TryGetRawError(out var raw))
    {
        // Any other error status
    }
}
catch (ApiException<RawError> ex)
{
    // Case B — raw error
    // ex.Error.StatusCode, ex.Error.ReadAsString(), ex.Error.ReadAsJson<T>()
}
```

**No-throw (`…Result`) variants: absent across this SDK** — every operation is throw-only. Of **285 operations**, **201 are Case A (typed)** and **84 are Case B (raw)**.

---

## Operations — by controller (42 groups, 285 operations)

Each links to a sub-page with one row per operation: signature, the request record's required members, query-param wire names, return type, error Case A/B, and Case A's typed accessors with their statuses. Each operation also carries a **Type sources** table — every type it names, with the file that declares it — so resolving a body, return, or error payload to its source is a lookup, never a search. `RawError` is excluded there (its members and path are above); an operation with no table names nothing but primitives and `RawError`.

**Each row states what is specific to its operation. Everything below holds for EVERY operation unless that operation's row says otherwise, so a row silent on one of these points is telling you the default here applies — take it and move on rather than opening the source to confirm it.**

| Applies to every operation | Stated where | A row appears only when |
| --- | --- | --- |
| **Throw-only** — no `…Result`/no-throw variant exists anywhere in this SDK | this page, Error-handling model | a no-throw sibling exists (none do at this SDK version) |
| **No pagination** — the operation returns a single response, not a `Pageable` | here | pagination is offered — the block carries a **Pagination** bullet naming the posture (page-, offset-, cursor- or link-based, or the `page`-without-page-size case) |
| **Case B error accessors are always these four** — `StatusCode: HttpStatusCode` · `ReadAsBytes(): ReadOnlyMemory<byte>` · `ReadAsString(): string` · `ReadAsJson<T>(): T?` | the `RawError` row above | never — a `Case B` label always implies exactly these four; Case A rows list their own typed accessors |
| **Server group `Production`** — base URL per Servers & auth below | here | the operation is on another group — its block carries a **Server group** bullet |
| **Inputs are one request record** — an operation that takes input takes a single request record as its first parameter; build it with an object initializer, using the property names in the record's source (its file is in the operation's **Type sources**). A member the row does not list as required is optional or already carries the spec's default | here | never — it always holds |

**The HTTP verb and route live on the operation itself**, in the source file named at the top of its operations page. This map is method-first: the C# method is the interface you call. When something wire-level needs the route — reproducing a raw request, pointing the client at a mock, reading a provider-side log — read it from that file; do not reconstruct it from memory or infer it from the method name.

**The endpoint's behavioural prose lives there too**, as the XML `<remarks>` on the method. Rows here give you the contract — names, types, shapes, errors. Where an operation's *semantics* decide what you must pass — a parameter whose value changes server-side behaviour, an ordering or exclusivity rule between fields — that is what `<remarks>` settles; read it there rather than filling it in from memory.

| Controller (`client.X`) | Ops | Page |
| --- | --- | --- |
| `ApiExports` | 9 | [map/operations/ApiExports.md](map/operations/ApiExports.md) |
| `AdvanceInvoice` | 3 | [map/operations/AdvanceInvoice.md](map/operations/AdvanceInvoice.md) |
| `BillingPortal` | 4 | [map/operations/BillingPortal.md](map/operations/BillingPortal.md) |
| `ComponentFeatures` | 6 | [map/operations/ComponentFeatures.md](map/operations/ComponentFeatures.md) |
| `ComponentPricePoints` | 12 | [map/operations/ComponentPricePoints.md](map/operations/ComponentPricePoints.md) |
| `Components` | 12 | [map/operations/Components.md](map/operations/Components.md) |
| `Coupons` | 14 | [map/operations/Coupons.md](map/operations/Coupons.md) |
| `CustomFields` | 9 | [map/operations/CustomFields.md](map/operations/CustomFields.md) |
| `Customers` | 7 | [map/operations/Customers.md](map/operations/Customers.md) |
| `Entitlements` | 1 | [map/operations/Entitlements.md](map/operations/Entitlements.md) |
| `Events` | 3 | [map/operations/Events.md](map/operations/Events.md) |
| `EventsBasedBillingSegments` | 6 | [map/operations/EventsBasedBillingSegments.md](map/operations/EventsBasedBillingSegments.md) |
| `FeatureTemplates` | 6 | [map/operations/FeatureTemplates.md](map/operations/FeatureTemplates.md) |
| `Insights` | 4 | [map/operations/Insights.md](map/operations/Insights.md) |
| `Invoices` | 19 | [map/operations/Invoices.md](map/operations/Invoices.md) |
| `Offers` | 5 | [map/operations/Offers.md](map/operations/Offers.md) |
| `PaymentProfiles` | 12 | [map/operations/PaymentProfiles.md](map/operations/PaymentProfiles.md) |
| `ProductFamilies` | 4 | [map/operations/ProductFamilies.md](map/operations/ProductFamilies.md) |
| `ProductFeatures` | 6 | [map/operations/ProductFeatures.md](map/operations/ProductFeatures.md) |
| `ProductPricePoints` | 11 | [map/operations/ProductPricePoints.md](map/operations/ProductPricePoints.md) |
| `Products` | 6 | [map/operations/Products.md](map/operations/Products.md) |
| `ProformaInvoices` | 10 | [map/operations/ProformaInvoices.md](map/operations/ProformaInvoices.md) |
| `ReasonCodes` | 5 | [map/operations/ReasonCodes.md](map/operations/ReasonCodes.md) |
| `ReferralCodes` | 1 | [map/operations/ReferralCodes.md](map/operations/ReferralCodes.md) |
| `SalesCommissions` | 3 | [map/operations/SalesCommissions.md](map/operations/SalesCommissions.md) |
| `Sites` | 3 | [map/operations/Sites.md](map/operations/Sites.md) |
| `SubscriptionComponents` | 17 | [map/operations/SubscriptionComponents.md](map/operations/SubscriptionComponents.md) |
| `SubscriptionGroupInvoiceAccount` | 4 | [map/operations/SubscriptionGroupInvoiceAccount.md](map/operations/SubscriptionGroupInvoiceAccount.md) |
| `SubscriptionGroupStatus` | 4 | [map/operations/SubscriptionGroupStatus.md](map/operations/SubscriptionGroupStatus.md) |
| `SubscriptionGroups` | 9 | [map/operations/SubscriptionGroups.md](map/operations/SubscriptionGroups.md) |
| `SubscriptionInvoiceAccount` | 7 | [map/operations/SubscriptionInvoiceAccount.md](map/operations/SubscriptionInvoiceAccount.md) |
| `SubscriptionNotes` | 5 | [map/operations/SubscriptionNotes.md](map/operations/SubscriptionNotes.md) |
| `SubscriptionProducts` | 2 | [map/operations/SubscriptionProducts.md](map/operations/SubscriptionProducts.md) |
| `SubscriptionRenewals` | 11 | [map/operations/SubscriptionRenewals.md](map/operations/SubscriptionRenewals.md) |
| `SubscriptionStatus` | 10 | [map/operations/SubscriptionStatus.md](map/operations/SubscriptionStatus.md) |
| `Subscriptions` | 12 | [map/operations/Subscriptions.md](map/operations/Subscriptions.md) |
| `WalletAdjustments` | 1 | [map/operations/WalletAdjustments.md](map/operations/WalletAdjustments.md) |
| `WalletProducts` | 7 | [map/operations/WalletProducts.md](map/operations/WalletProducts.md) |
| `WalletPurchases` | 2 | [map/operations/WalletPurchases.md](map/operations/WalletPurchases.md) |
| `WalletTransactions` | 2 | [map/operations/WalletTransactions.md](map/operations/WalletTransactions.md) |
| `Wallets` | 5 | [map/operations/Wallets.md](map/operations/Wallets.md) |
| `WebhooksApi` | 6 | [map/operations/WebhooksApi.md](map/operations/WebhooksApi.md) |

---

## Models — where they live, how to build them

**Shapes live only in the source.** Every file under `src/MaxioAdvancedBilling/Models/`, `src/MaxioAdvancedBilling/Errors/` and `src/MaxioAdvancedBilling/Requests/` declares exactly one public type, named after the file, and no two share a name — so a type name *is* its path. Take it from the operation's **Type sources** table, or build it from the kind's directory below. Never grep for a type.

| Group | Count | Directory (file = `<TypeName>.cs`) |
| --- | --- | --- |
| Records (plain `record` data models) | 617 | `src/MaxioAdvancedBilling/Models/` |
| Unions (`OneOf`) — variant factories + `TryGet…` | 8 | `src/MaxioAdvancedBilling/Models/OneOf/` |
| Unions (`AnyOf`) — variant factories + `TryGet…` | 95 | `src/MaxioAdvancedBilling/Models/AnyOf/` |
| Enums (`OpenStringEnum<T>` / `OpenIntEnum<T>`) — C# member names + wire values | 121 | `src/MaxioAdvancedBilling/Models/Enums/` |
| Typed error classes (`: ApiError`, one per Case A operation) | 201 | `src/MaxioAdvancedBilling/Errors/` |
| Request records (one per operation that takes input; never serialized) | 279 | `src/MaxioAdvancedBilling/Requests/` — a controller's records sit in a folder named after it (an operation with several tags is filed under the first tag it declares) |

Conventions: records are immutable, `init`-only; `required` properties must be set in the object initializer; `T?` is optional. A field's wire name is its `[JsonPropertyName]` and often differs from the C# name (`AmountInCents` ↔ `amount_in_cents`) — read it off the property, don't derive it. `OneOf`/`AnyOf` unions wrap `Optional<T>` variants — build via static factory or implicit conversion, read via `TryGet…(out …)`; `AllOf` compositions are not unions — every constituent is a `required` property, so set them all, and those constituent properties carry no `[JsonPropertyName]` and have no wire name of their own, because the generated converter flattens each constituent's own fields directly into the one parent JSON object. Enums are **not** C# enums — use the static members, whose names are PascalCase even when the wire value isn't (`CollectionMethod.Invoice`, not `.invoice`); there is no public factory, so resolve a raw value with `TryGetKnownValue` and branch with the generated `Match`, whose `otherwise` arm receives any value the SDK does not declare.

Namespaces by content type (add `using` accordingly):

| Contents | Namespace |
| --- | --- |
| Client & options (root) | `MaxioAdvancedBilling` |
| Operation controllers (`src/MaxioAdvancedBilling/Api/`) | `MaxioAdvancedBilling.Api` |
| Records (`src/MaxioAdvancedBilling/Models/`) | `MaxioAdvancedBilling.Models` |
| Enums (`src/MaxioAdvancedBilling/Models/Enums/`) | `MaxioAdvancedBilling.Models.Enums` |
| OneOf unions (`src/MaxioAdvancedBilling/Models/OneOf/`) | `MaxioAdvancedBilling.Models.OneOf` |
| AnyOf unions (`src/MaxioAdvancedBilling/Models/AnyOf/`) | `MaxioAdvancedBilling.Models.AnyOf` |
| Error classes (`src/MaxioAdvancedBilling/Errors/`) | `MaxioAdvancedBilling.Errors` |
| Request records (`src/MaxioAdvancedBilling/Requests/`) | `MaxioAdvancedBilling.Requests` — plus `.<Controller>` for a controller's records (an operation with several tags is filed under the first tag it declares) |

---

## Servers & auth

**Basic auth.** Set `options.BasicAuth = new BasicAuthCredentials { Username = …, Password = … }`, or `options.BasicAuthProvider` to a delegate asked on every request. The `username` is a Maxio Chargify API key. The `password` is `x`.

Operation blocks name their credential in an **Auth** bullet; an operation whose spec declares no scheme carries no such bullet.

- `AND` — every credential listed must be configured; the call fails before anything is sent, naming each one that is not.
- `OR` — the first configured credential that applies is sent, in the order listed; a configured one that fails falls through to the next; when none is configured the call fails before anything is sent, naming all of them.
- `OR anonymous` — the operation accepts anonymous callers: it is authenticated when a whole alternative listed before it is configured (every credential in an AND group), and sent without credentials otherwise.
- `(scopes: …)` — the OAuth2 scopes the operation's token is requested with; a client-credentials scheme mints one token per scope set.

A required credential you never configured fails the call with `AuthSchemeException` before anything is sent, so a `401` on an operation that requires a credential means the server rejected the one that was sent. A credential option left `null` is read from an environment variable when the client is constructed; the README's **Credentials from environment variables** table names them. A value set in code always wins, including the placeholder assignments in the **Getting a client** sample; delete the line for any credential you supply through an environment variable.

**Environments.** `options.Environment` selects the target environment (`src/MaxioAdvancedBilling/Servers/ServerEnvironment.cs`):

| Environment | Value | Hosting |
| --- | --- | --- |
| `ServerEnvironment.Us` *(default)* | `US` | Default Advanced Billing environment hosted in US. Valid for the majority of our customers. |
| `ServerEnvironment.Eu` | `EU` | Advanced Billing environment hosted in EU. Use only when you requested EU hosting for your AB account. |

**2 server groups.** Base-URL templates and override points (`options.Server.…`):

| Group | `Us` base URL | `Eu` base URL | Override point |
| --- | --- | --- | --- |
| `Production` | `https://{site}.chargify.com` | `https://{site}.ebilling.maxio.com` | `options.Server.Production.Us.BaseUrl` (and the other environments) |
| `Ebb` | `https://events.chargify.com/{site}` | `https://events.chargify.com/{site}` | `options.Server.Ebb.Us.BaseUrl` (and the other environments) |

`Production` · `Us` template variables: `{site}` defaults to `"subdomain"` — override `options.Server.Production.Us.Site`.

`Production` · `Eu` template variables: `{site}` defaults to `"subdomain"` — override `options.Server.Production.Eu.Site`.

`Ebb` · `Us` template variables: `{site}` defaults to `"subdomain"` — override `options.Server.Ebb.Us.Site`.

`Ebb` · `Eu` template variables: `{site}` defaults to `"subdomain"` — override `options.Server.Ebb.Eu.Site`.

Retry/resilience is configurable via `options.Retry` (`RetryOptions`, backed by Polly).

