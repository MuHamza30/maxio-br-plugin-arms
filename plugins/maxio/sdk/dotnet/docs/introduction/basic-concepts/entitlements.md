# Entitlements

Entitlements let you define features once (for example, Single Sign-On, a monthly API call limit, or a support tier) and grant them to subscribers through the products and components they already have.

At runtime, Entitlements answers a simple question: **what is this subscription actually allowed to do, and how much of it?**

---

There are three steps to using Entitlements:

- Defining a [feature template](#feature-templates)
- Attaching that feature template to a [product or component](#attaching-features-to-products-and-components)
- Reading a subscriber's [aggregated entitlements](#reading-a-subscribers-entitlements)

## Feature Templates

A feature template is defined once, at the site level, and describes a feature you might want to grant to subscribers. Every feature template has a `kind`, which determines how the feature's value behaves:

- **`access_right`**: a boolean entitlement. A subscriber either has access or does not (for example, Single Sign-On).
- **`usage_limit`**: a quantified allowance measured over a recurring period (for example, "10,000 API calls per month").
- **`service_right`**: a free-form value (text, boolean, or number) that isn't a simple access flag or a metered limit (for example, a support tier of `"gold"`).

```json
// POST /features.json
{
  "feature": {
    "key": "sso",
    "name": "Single Sign-On",
    "kind": "access_right"
  }
}
```

That data is posted to the [Create Feature Template]($e/Feature%20Templates/createFeatureTemplate) endpoint. `key` is immutable once set, and `kind` can't be changed once the feature template has been attached to any product or component.

## Attaching Features to Products and Components

A feature template isn't granted to anyone on its own. The template has to be attached to a product or component (or to one of their price points) as a **feature catalog item**, with a concrete value:

```json
// POST /products/{product_id}/features.json
{
  "feature": {
    "feature_template_id": 1001,
    "value": "true"
  }
}
```

That data is posted to the [Create Product Feature Catalog Item]($e/Product%20Features/createProductFeature) endpoint (or [Create Component Feature Catalog Item]($e/Component%20Features/createComponentFeature) for a component). Once attached, subscribers on that product/component are provisioned an entitlement automatically the next time their subscription changes (for example, on signup, a plan change, or a component allocation).

You can scope a feature catalog item to a single price point instead of the whole product/component by passing `price_point_type` and `price_point_id`. For details, see [Create Product Feature Catalog Item]($e/Product%20Features/createProductFeature).

Removing a feature catalog item defaults to a soft removal: the feature is taken out of the catalog, but subscribers who already have it keep their existing entitlement (they're "grandfathered in"). Pass `destroy_entitlements=true` to instead revoke access immediately. Archiving a feature template with [Archive Feature Template]($e/Feature%20Templates/archiveFeatureTemplate) has the same default and override shape, using its own query parameter, `remove_from_catalog=true`. Restoring an archived feature template never restores entitlements removed this way.

## Reading a Subscriber's Entitlements

To find out what a specific subscription is entitled to, call [Read Subscription Entitlements]($e/Entitlements/readSubscriptionEntitlements). That endpoint collapses every product and component on the subscription into one entry per feature key and periodicity window. A `usage_limit` feature granted monthly by one product and daily by another therefore comes back as two entries, each identified by its own `periodicity_key`:

```json
// GET /subscriptions/{subscription_id}/entitlements.json
{
  "subscription_id": 12345,
  "customer_id": 678,
  "status": "active",
  "entitlements": [
    {
      "feature_key": "feature.sso",
      "periodicity_key": "feature.sso",
      "name": "SSO",
      "type": "access_right",
      "value": true,
      "enabled": true,
      "periodicity": null,
      "source_products": ["Gold Plan"]
    },
    {
      "feature_key": "usage.api_calls",
      "periodicity_key": "usage.api_calls:1:month",
      "name": "API Calls",
      "type": "usage_limit",
      "value": 50000,
      "enabled": true,
      "periodicity": { "interval": 1, "unit": "month" },
      "source_products": ["Gold Plan"]
    }
  ]
}
```

`enabled` reflects both the aggregated value and the subscription's state. The field is `false` whenever the subscription isn't in a live state (`active`, `trialing`, `assessing`, `past_due`, or `soft_failure`), regardless of the value. Note that entitlements stay enabled while a subscription is in dunning. Most integrations should check `enabled` before granting access in their own application.

If you're already fetching products or components and want their features embedded directly in that response instead of making a separate call, pass `include_features=true` to [List Products]($e/Products/listProducts), [Read Product]($e/Products/readProduct), or [Read Component]($e/Components/readComponent). [List Components]($e/Components/listComponents) does not support `include_features`.
