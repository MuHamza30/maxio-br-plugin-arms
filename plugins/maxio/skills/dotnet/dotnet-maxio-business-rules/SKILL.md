---
name: "dotnet-maxio-business-rules"
description: "Maxio Advanced Billing provider rules that no single operation page states: the rules for every request, for jobs Maxio runs on its own (renewals, retries, webhooks), and the rules that span several operations or fix their order, grouped by flow. Load it while planning a Maxio Advanced Billing .NET integration, before the PRODUCTION READINESS table, and record each flow's rules there."
---

# Maxio Advanced Billing: provider rules by flow

513 rules from Maxio's own spec and portal docs that apply across operations. A **loud** rule makes Maxio refuse the request; a **silent** one gives a result with no error. Each line: the SDK calls it concerns, when it applies → what Maxio does, then what the app must do. Record how the plan honours each rule of every flow the scope touches in row 12 of the plan's PRODUCTION READINESS table.

Each flow's rules are in its own file beside this one. Read **Every request** always, and the file of every other flow whose operations your scope calls.

| Flow | File | Rules |
|---|---|---|
| Every request | `flows/every-request.md` | 44 |
| Background: dunning | `flows/background-dunning.md` | 2 |
| Background: entitlement-provisioning | `flows/background-entitlement-provisioning.md` | 1 |
| Background: exports | `flows/background-exports.md` | 1 |
| Background: invoice-issuance | `flows/background-invoice-issuance.md` | 3 |
| Background: invoicing | `flows/background-invoicing.md` | 1 |
| Background: renewal | `flows/background-renewal.md` | 8 |
| Background: wallet-funding | `flows/background-wallet-funding.md` | 11 |
| Background: wallet-grant-expiration | `flows/background-wallet-grant-expiration.md` | 1 |
| Background: wallet-provisioning | `flows/background-wallet-provisioning.md` | 2 |
| Background: webhook-delivery | `flows/background-webhook-delivery.md` | 57 |
| Subscriptions and their lifecycle | `flows/subscriptions-and-their-lifecycle.md` | 67 |
| Subscription components, usage and events | `flows/subscription-components-usage-and-events.md` | 64 |
| The product catalog | `flows/the-product-catalog.md` | 62 |
| Invoicing | `flows/invoicing.md` | 68 |
| Customers, payment profiles, coupons and offers | `flows/customers-payment-profiles-coupons-and-offers.md` | 54 |
| Subscription groups, wallets and features | `flows/subscription-groups-wallets-and-features.md` | 52 |
| Site-wide, every-request and background rules | `flows/site-wide-every-request-and-background-rules.md` | 15 |
