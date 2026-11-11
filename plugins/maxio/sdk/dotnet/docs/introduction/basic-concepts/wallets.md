# Wallets

A wallet is a balance, denominated in the unit of a billing product, that you can create, fund, drain, and read entirely through the API. Wallets are useful for pre-paid allowances, credits, and consumption pools that don't map cleanly onto the regular usage-billing cycle.

At runtime, Wallets answers a simple question: **how much of this unit does this subscription have left, and where did it come from?**

---

There are five things to know about how a wallet works:

- Every wallet is [created for a subscription and a billing product](#creating-a-wallet), either directly or [provisioned automatically from a Feature](#provisioning-a-wallet-from-a-feature)
- Every balance change is recorded on an [append-only ledger](#the-ledger)
- Funding a wallet manually, or consuming it directly, both go through [one transaction endpoint](#funding-and-consuming-a-wallet)
- Subscribers can [purchase additional units](#purchasing-units), charged to their payment method
- Support and ops corrections use a [separate, more constrained adjustment endpoint](#correcting-a-grants-balance)

## Creating a Wallet

A wallet is owned by a subscription and denominated by a [Wallet Product](#setting-up-a-wallet-product): a billing product created specifically to back wallets. An ordinary component can't denominate a wallet. The Wallet Product is fixed for the life of the wallet. There's no way to move an existing wallet to a different one.

```json
// POST /wallets.json
{
  "wallet": {
    "subscription_id": 23,
    "wallet_product_id": 76
  }
}
```

That's posted to [Create Wallet]($e/Wallets/createWallet). The new wallet starts at a balance of `"0.0"` unless you pass a non-zero `initial_balance`, a free opening balance seeded atomically with the wallet and not reflected on any invoice.

After creation, only the wallet's `name` can be changed, with [Update Wallet]($e/Wallets/updateWallet). Everything else is fixed for the life of the wallet.

A prepaid subscription can't have a wallet: it already meters against its own balance, and a wallet would double that up. `subscription_id` naming one is rejected with `422`, and a subscription holding any active wallet, manual or provisioned, can't switch to prepaid collection either.

Two optional settings shape how the wallet behaves at its edges:

- **`hard_stop_overflow`** determines what happens at the zero boundary. `discard` (the default) caps a debit that would go below zero and rejects the excess. `allow_negative` always deducts the full amount, letting the balance go negative; the portion past zero is recorded as an overdraft ledger row.
- **`meter_id`** pairs the wallet with a metering meter, so its balance factors in live usage between ledger writes. Only a `sum` or `count` meter can be paired, and the pairing is set once, at creation. See [Reading a Wallet](#reading-a-wallet) for the fields a metered wallet reports.

## Provisioning a Wallet from a Feature

A wallet can also come into existence automatically, tied to a usage-limit [entitlement](page:introduction/basic-concepts/entitlements), instead of being created directly. This is configured on the Feature, not on the wallet: set `wallet_behavior` when creating or updating the Feature Template, optionally pairing it with a meter the same way `meter_id`/`conversion_rate` do on [Create Wallet]($e/Wallets/createWallet). `wallet_behavior` accepts `wallet_product_id`, `name`, `price_point_id`, `depletion_policy`, `hard_stop_overflow`, and the optional `meter_id` and `conversion_rate`, and the Feature Template reference doesn't list these fields yet.

```json
// PUT /features/{id}.json
{
  "feature": {
    "wallet_behavior": {
      "wallet_product_id": 88,
      "price_point_id": 140,
      "depletion_policy": "hard_stop",
      "hard_stop_overflow": "discard"
    }
  }
}
```

Once a subscription resolves an entitlement to that Feature, a wallet is provisioned for it automatically: `POST /wallets.json` is never called directly for this case. The parameter does exist there, though; sending `entitlement_id` to Create Wallet is read and always rejected (see its field description), rather than simply not existing. The wallet's `origin` is `feature`, and its `feature_key` names the Feature it was provisioned for. Provisioning waits for the subscription to actually go live; a subscription that's still pending or awaiting signup gets no wallet until then, so nothing is ever funded against a subscription that hasn't started billing yet.

The wallet reports the state of its funding schedule in a `recurring_funding` array:

```json
{
  "recurring_funding": [
    {
      "id": 30,
      "entitlement_id": 501,
      "allowance": "5000.0",
      "next_grant_at": "2026-09-01T00:00:00Z",
      "skipped_periods": 0,
      "status": "active",
      "reason": null
    }
  ]
}
```

Neither the allowance nor the cadence is copied onto the wallet. Both are read fresh from the entitlement each time, so lowering the entitlement's allowance takes effect on the very next scheduled grant (raising it is granted immediately, as described below). The first grant lands immediately, covering the period already in progress. After that, refills follow the entitlement's own periodicity interval, independent of the subscription's billing term: a monthly entitlement on an annual plan refills monthly, and a yearly entitlement on a monthly plan doesn't grant again at each month end. The first refill lands one entitlement interval after the period start, except when the entitlement's interval matches the billing cadence, or the period is a trial or prorated: those refill at the period's end. A refill due at a period boundary waits for the subscription's renewal to run first, so a delayed cancellation or product change is applied before anything is granted.

Raising the entitlement's allowance mid-period grants the difference immediately, as a grant with `origin: "allowance_increase"` rather than `"recurring"`. Lowering it does not claw anything back; the lower allowance only takes effect starting with the next scheduled refill.

`status` is one of three values, and each means something different for what you should do about it:

- **`active`**: refilling normally.
- **`erroring`**: the schedule is still due and the job keeps retrying, but the last attempt couldn't be written (see `reason`). For `allowance_not_representable`, correcting the underlying entitlement resumes funding on the next run. For `balance_at_capacity`, the entitlement is fine; the wallet's balance itself needs to come down (a debit, adjustment, or expiration) before granting can succeed again.
- **`stopped`**: funding has ended for good (see `reason`). Units already granted stay on the wallet and remain spendable; only future funding is affected.

Recurring funding also tracks the subscription's own state. `canceled`, `expired`, and `trial_ended` stop the schedule for good. A subscription that's `trialing` has its refills retried rather than granted, even though trialing subscriptions otherwise keep billing normally. Other non-live states (`on_hold`, `paused`, and the like) skip that period without stopping the schedule; `past_due`, `soft_failure`, and `unpaid` still grant normally. `skipped_periods` counts both kinds of gap: a period skipped because the subscription wasn't live, and a period a scheduled run was simply late to process (for example, during an outage).

Grants created this way carry `origin: "recurring"` on the grant objects returned when [reading a wallet](#reading-a-wallet), and `granted_for_period_start` records which funding period each one covers.

## The Ledger

Every balance change writes one or more append-only rows to the wallet's ledger: a `kind: grant`, `debit`, `adjustment`, `initial_balance`, or `expiration` row. A single API call can write more than one row: a debit writes one row per grant it drains, plus an overdraft row if it goes negative; a grant made while the balance is already negative writes an overdraft-repayment row before the grant itself. A purchase doesn't have its own kind; it writes a `kind: grant` row like any other grant, distinguished only by that grant's `origin: "purchase"`. Rows are never modified after they're written. A correction is always a new row, never an edit to an old one.

```json
// GET /wallets/{wallet_id}/transactions.json
{
  "wallet_transactions": [
    {
      "id": 41,
      "wallet_id": 5,
      "kind": "debit",
      "amount": "-30.0",
      "grant_id": 9,
      "source_transaction_id": null,
      "source_corrections_total": "0.0",
      "balance_after": "0.0",
      "metered_from_at": null,
      "metered_through_at": null,
      "conversion_rate_applied": null,
      "invoice_uid": null,
      "memo": null,
      "created_at": "2026-08-01T09:30:00Z"
    }
  ],
  "meta": {
    "total_count": 1,
    "current_page": 1,
    "total_pages": 1,
    "per_page": 20
  }
}
```

Call [List Wallet Transactions]($e/Wallet%20Transactions/listWalletTransactions) to read it, newest first. `amount` is signed: positive for grants, negative for debits and expirations, either sign for adjustments. `balance_after` is a snapshot of the wallet's posted balance immediately after that row was applied, so you can reconstruct the balance history without re-summing every row from the start. Reconcile it against the wallet's `posted_balance`, not its `balance`: on a metered wallet the two can differ by usage that isn't on the ledger yet. A row written by metered consumption also reports the usage window it closed (`metered_from_at`, `metered_through_at`) and the `conversion_rate_applied`; a row behind a purchase carries the paying `invoice_uid`.

All amounts on wallets, grants, and ledger rows are encoded as **decimal strings** (`"150.0"`, not `150.0`), to avoid the precision loss floating-point JSON numbers can introduce on the client side. Requests follow the same rule: send amounts as decimal strings (a whole-number JSON integer is also accepted). A JSON number with a decimal point is rejected with `422`, never rounded, because it may already have lost precision before it reached the API.

## Reading a Wallet

[Read Wallet]($e/Wallets/readWallet) and [List Wallets]($e/Wallets/listWallets) return the wallet's balance together with every grant on it, open or not: the balance broken down by funding tranche.

```json
// GET /wallets/{id}.json
{
  "wallet": {
    "id": 5,
    "owner_type": "subscription",
    "owner_id": 23,
    "name": "Onboarding credits",
    "wallet_product_id": 76,
    "wallet_product_name": "API Calls",
    "unit": "calls",
    "price_point_id": 140,
    "balance": "150.0",
    "posted_balance": "150.0",
    "meter": null,
    "metered_through_at": null,
    "metered_recheck_after_seconds": null,
    "metered_stale": null,
    "status": "active",
    "depletion_policy": "hard_stop",
    "hard_stop_overflow": "discard",
    "origin": "manual",
    "feature_key": null,
    "grants": [
      {
        "id": 9,
        "name": "Grant #9",
        "original": "100.0",
        "remaining": "50.0",
        "posted_remaining": "50.0",
        "expires_at": null,
        "expires_on": null,
        "expired_at": null,
        "expired_on": null,
        "origin": "manual",
        "granted_for_period_start": null,
        "invoice_uid": null,
        "created_at": "2026-07-15T10:00:00Z"
      }
    ],
    "recurring_funding": [],
    "created_at": "2026-07-15T10:00:00Z",
    "updated_at": "2026-08-01T09:30:00Z"
  }
}
```

`balance` is what you can spend now; `posted_balance` is what the ledger says. Grants follow the spendable balance, while transactions follow the ledger: a grant's `remaining` is what it can still spend, and its `posted_remaining` is what its written ledger entries have left, while each transaction's `balance_after` is a posted balance. The two pairs are equal everywhere except a live metered read.

Grants are listed in drain order: the order debits would consume the still-open ones in, with the soonest-expiring grant first and evergreen grants (`expires_at: null`) last. Spent and expired grants stay in the array too; nothing is filtered out of it. A grant that reaches its `expires_at` before being fully spent is automatically written off; the unconsumed remainder is recorded as a `kind: "expiration"` row on the ledger, and the grant's `expired_at`/`expired_on` fields are populated.

For a wallet paired with a meter, `meter` names the paired meter, but only [Read Wallet]($e/Wallets/readWallet) actually computes a live balance from it, and only for an active wallet on a site with metered consumption enabled. In that one response, `balance` reflects usage as of `metered_through_at`, including usage not yet posted to the ledger, and each grant's `remaining` is its `posted_remaining` less the pending usage drawn from it in drain order (never below zero, so the grants' `remaining` add up to `balance` whenever it is zero or more, and any overdraft belongs to no grant), `metered_stale` is `true` if live usage couldn't be read fresh (in which case `balance` falls back to the last known snapshot, and `posted_balance` stays exact), and `metered_recheck_after_seconds` suggests how long to wait before reading again. Every other response for the same wallet, including [List Wallets]($e/Wallets/listWallets), [Create Wallet]($e/Wallets/createWallet), [Update Wallet]($e/Wallets/updateWallet), and [Archive Wallet]($e/Wallets/archiveWallet), reports the stored balance as of the last ledger write (`balance` equal to `posted_balance`, and `remaining` equal to `posted_remaining`) and leaves the three `metered_*` fields `null`, even for a metered wallet.

A wallet is read the same way whether it's active or archived, but an archived wallet never gets a live metered balance even from Read Wallet; it always reports the stored balance. [Archive Wallet]($e/Wallets/archiveWallet) blocks further grants, debits, purchases, and adjustments, and stops recurring funding (with reason `wallet_archived`), but doesn't affect reads or hide the wallet from list results.

## Funding and Consuming a Wallet

Manual top-ups and direct consumption both go through [Create Wallet Transaction]($e/Wallet%20Transactions/createWalletTransaction), a single endpoint discriminated by `kind`:

```json
// POST /wallets/{wallet_id}/transactions.json
{
  "wallet_transaction": {
    "kind": "grant",
    "amount": "100.0",
    "memo": "Manual top-up"
  }
}
```

```json
// POST /wallets/{wallet_id}/transactions.json
{ "wallet_transaction": { "kind": "debit", "amount": "30.0" } }
```

`amount` is always sent as a positive number; `kind` decides the direction that ends up on the ledger. A `grant` can carry its own `expires_at`, independent of any recurring funding schedule on the wallet, useful for a one-time credit that should lapse after a set period. A `debit` never carries an expiration, and drains the wallet's grants earliest-expiration-first.

Any grant made while the wallet's balance is negative pays off that overdraft first: a `kind: grant` row with `grant_id: null` for the repaid amount, followed by the row that creates the grant itself, whose `amount` is only what's left after the repayment (possibly `"0.0"`). The grant is created even when the overdraft absorbs the whole top-up: its `original` is the full amount sent and its `remaining` is `0`, which lets a later correction of the usage it repaid be credited back to it. The same repay-first order applies to recurring refills, allowance increases, and purchases, since they all fund the wallet through the same mechanism as a manual grant.

Under the default `hard_stop_overflow: discard`, a debit larger than the available balance is capped: the response's `requested_amount`, `drained_amount`, and `rejected_amount` tell you exactly how much of the request was actually covered. For a metered wallet, that cap is the live metered balance rather than the stored one, so a debit can drain less than the stored balance would suggest, including draining nothing at all with a `201` (`drained_amount: "0.0"`) if live usage already outpaced the ledger. A debit against a wallet whose _stored_ balance is already at or below zero returns `422` rather than a zero-amount success; that failure can still leave new `kind: expiration` rows behind, from grants this same call expired before the depleted check ran.

## Purchasing Units

A subscriber can buy additional units for a wallet, charged to the subscription's payment method, with [Create Wallet Purchase]($e/Wallet%20Purchases/createWalletPurchase):

```json
// POST /wallets/{wallet_id}/purchases.json
{ "purchase": { "quantity": "500.0" } }
```

The purchase is priced at the wallet's `price_point_id` unless a different one is named, and writes a grant of origin `purchase` whose expiration follows the funding entitlement's grant expiration policy. "Charged" works differently depending on the subscription's collection method. For card/automatic collection, payment is attempted immediately, and a decline writes nothing to the wallet. For an invoice-billed (remittance) subscription, there's no immediate charge to decline at all: the units are credited to the wallet as soon as the invoice is drafted, and the invoice is issued afterward; if issuing fails, the invoice is left in draft for follow-up, but the purchase still returns successfully and the credited units are not reversed. If crediting fails after a card payment has already settled, the call returns `422` (`purchase_paid_but_uncredited`) with the paid invoice's uid at the end of the message, so you can reconcile it. A purchase into an overdrawn wallet repays the debt first, and still creates its grant (with `remaining` of `0` when the debt absorbs it all). Purchasing at a price point with no price in the subscription's currency is refused. To quote a price without charging anything, call [Preview Wallet Purchase]($e/Wallet%20Purchases/previewWalletPurchase) with the same parameters.

## Correcting a Grant's Balance

[Create Wallet Adjustment]($e/Wallet%20Adjustments/createWalletAdjustment) is a narrower, support-and-ops-oriented endpoint for correcting a specific grant, rather than funding or consuming the wallet in the ordinary flow:

```json
// POST /wallets/{wallet_id}/adjustments.json
{
  "adjustment": {
    "grant_id": 9,
    "amount": "10.0",
    "reason": "Support credit for outage on 2026-07-30"
  }
}
```

`amount` is signed and targets one grant directly: positive restores units to the grant, negative removes them. An adjustment works on what the ledger has recorded, so it can't push a grant's `posted_remaining` below zero or above the grant's `original` allocation; out-of-bounds requests are rejected outright rather than clamped. `reason` is required and becomes the ledger row's `memo`, so every correction is self-documenting in the ledger.

A `debit` row with no `grant_id`, written when an `allow_negative` wallet consumes past its grants, is an overdraft entry. It can be corrected without a `grant_id`: omit it and the correction credits the wallet's outstanding overdraft, bounded by the usage the entry still records and by the overdraft the wallet still owes. Once a later grant has repaid the overdraft, correct that grant's balance instead.

If you're deriving the adjustment `amount` from a value you read earlier (the grant's `posted_remaining`, or a debit row's `source_corrections_total` via `source_transaction_id`), pass that value back as `expected_remaining` or `expected_source_corrections`. If it's moved since you read it, the request is rejected with `409` instead of silently applying a correction computed from stale data.

Send `posted_remaining`, not `remaining`, as `expected_remaining`. On a metered wallet read through Read Wallet, `remaining` also subtracts usage that hasn't been posted yet, so it can differ from what the adjustment checks against, and sending it while usage is pending returns `409`.

## Setting Up a Wallet Product

A Wallet Product is a billing product created specifically to denominate wallets: a component with no product family (sending `product_family_id` is rejected, not ignored), priced with one or more price points, purchasable through [Purchasing Units](#purchasing-units) and pairable with a Feature's `wallet_behavior`.

```json
// POST /wallet_products.json
{
  "wallet_product": {
    "name": "Token pack",
    "unit_name": "tokens",
    "price_point": {
      "name": "Standard",
      "pricing_scheme": "per_unit",
      "unit_price": "0.05"
    }
  }
}
```

[Create Wallet Product]($e/Wallet%20Products/createWalletProduct) returns the full Component field set under a `wallet_product` root, since a Wallet Product is a Component under the hood; its `kind` is `"wallet_product"`, and its product-family fields are always `null`. Only `name`, `description`, `taxable`, `tax_code`, `accounting_code`, and `item_category` can be changed afterward with [Update Wallet Product]($e/Wallet%20Products/updateWalletProduct); the unit name, handle, and price points are fixed once created. [List Wallet Products]($e/Wallet%20Products/listWalletProducts) additionally reports `used_by` (an object: `wallet_configurations`, the number of wallet templates billing through this product, and `active_wallets`, the number of active wallets created from them) and `price_point_summary`, a rollup of the default price point's scheme and bracket range. [Archive Wallet Product]($e/Wallet%20Products/archiveWalletProduct) refuses to archive one that's still in use by an active Feature or a scheduled renewal, but that only protects Feature-provisioned wallets; a manually created wallet can be left pointing at an archived Wallet Product, and purchases against it aren't blocked either.

A **wallet template** is the underlying configuration a wallet or a Feature's `wallet_behavior` is built from: which Wallet Product and price point it bills through, and its depletion settings. For a full breakdown of what's drawing on a Wallet Product, call [Read Wallet Product Usage]($e/Wallet%20Products/readWalletProductUsage): every wallet template billing through it, split by whether it uses the default price point or overrides it (one row per Feature and assigned plan/component sharing a template, so the same template can appear more than once), plus a sample of the active wallets actually created from them.

## Handling Errors

Every wallet error response (`404`, `409`, `422`, `500`, and `503`) is a JSON body with two fields: `errors`, an array of human-readable messages, and `code`, a stable machine-readable reason.

```json
{
  "errors": ["Wallet is depleted"],
  "code": "wallet_depleted"
}
```

Match on `code` rather than the message text, which may be reworded. Failures that come from record validation report `validation_failed`. A `403` is the exception: it is the plain text `403 Forbidden`, not JSON, and means the Wallets feature isn't enabled for the site, or that you sent `meter_id` or `conversion_rate` without metered wallet consumption enabled.
