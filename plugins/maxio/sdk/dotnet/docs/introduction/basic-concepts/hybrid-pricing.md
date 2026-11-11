# Hybrid Pricing

Hybrid Pricing lets a single Component bill a primary tiered, volume, or stairstep pricing model together with a secondary pricing model for usage above an included threshold, as a single invoice line item.

---

## How it works

Hybrid Pricing combines a Component's primary pricing model with a secondary pricing model, and bills both together as one invoice line item instead of multiple. The primary model covers usage up to an included threshold, and the secondary model takes over for usage beyond that threshold.

## Requirements

Hybrid Pricing only applies when all of the following are true for a given Price Point:

| Requirement              | Details                                                                                                                                                                                |
| ------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Site feature             | Hybrid Pricing must be enabled for the Site, and requires Invoice-Centric Billing to also be enabled. This is not a self-service toggle. Contact your Maxio account team to enable it. |
| Component type           | Only Quantity-Based and Metered Components support Hybrid Pricing. Metered Components configured for event-based billing (metric, meter, or formula) are not eligible.                 |
| Primary pricing model    | Must be `volume`, `tiered`, or `stairstep`. `per_unit` cannot be the primary model.                                                                                                    |
| Primary pricing brackets | The primary model's highest bracket must have a finite `ending_quantity` (the included threshold). An open-ended top bracket disqualifies the Price Point from Hybrid Pricing.         |
| Secondary pricing model  | A secondary pricing model must be configured on the Price Point (the `overage_pricing_scheme` and `overage_pricing` parameters).                                                       |

There is no explicit `hybrid` flag anywhere in the API. A Price Point becomes a hybrid Price Point automatically once the requirements above are satisfied. Configure it the same way you would configure any Component with a secondary pricing model.

## Configuring Hybrid Pricing via the API

Hybrid Pricing is configured through the existing Components and Price Points endpoints. There is no dedicated Hybrid Pricing endpoint or parameter.

### Creating the Component

Create a Quantity-Based or Metered Component with a bracketed primary `pricing_scheme` and a secondary pricing block (the `overage_pricing` parameter):

```json
// POST /product_families/{product_family_id}/quantity_based_components.json
{
  "quantity_based_component": {
    "name": "Seats",
    "unit_name": "seat",
    "pricing_scheme": "stairstep",
    "prices": [
      { "starting_quantity": 1, "ending_quantity": 10, "unit_price": 500 }
    ],
    "overage_pricing": {
      "pricing_scheme": "per_unit",
      "prices": [{ "starting_quantity": 1, "unit_price": 8 }]
    }
  }
}
```

This creates a Component whose default Price Point charges a flat $500 for up to 10 seats, then $8 per seat beyond that. Post this to the [Create Quantity Based Component]($e/Components/createQuantityBasedComponent) endpoint. Since the site has Hybrid Pricing enabled and the primary model (`stairstep`) has a finite included threshold, this Price Point is a hybrid Price Point.

`overage_pricing.prices` is its own self-contained bracket set: `starting_quantity` always starts at `1`, not at the primary model's `ending_quantity`. The secondary model still only takes effect once usage crosses the primary model's included threshold.

### Adding or updating a Price Point

The same secondary pricing structure (the `overage_pricing` parameter) applies when creating or updating additional Price Points on an existing Component:

```json
// POST /components/{component_id}/price_points.json
{
  "price_point": {
    "name": "Enterprise",
    "pricing_scheme": "tiered",
    "prices": [
      { "starting_quantity": 1, "ending_quantity": 50, "unit_price": 4 }
    ],
    "overage_pricing_scheme": "per_unit",
    "overage_pricing": {
      "prices": [{ "starting_quantity": 1, "unit_price": 2 }]
    }
  }
}
```

See [Create Component Price Point]($e/Component%3A%20Price%20Points/createComponentPricePoint) and [Update Component Price Point]($e/Component%3A%20Price%20Points/updateComponentPricePoint) for the complete input/output schema.

### Common validation errors

| Error                                                                             | Cause                                                                                                                                                                                                        |
| --------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `Pricing scheme cannot be per_unit for hybrid pricing`                            | The primary `pricing_scheme` was set to `per_unit` while a secondary pricing model was also configured on a hybrid-eligible Component. Use `volume`, `tiered`, or `stairstep` for the primary model instead. |
| `Prices primary pricing must have a finite included threshold for hybrid pricing` | The primary model's highest bracket did not specify an `ending_quantity`. Add one to define where the secondary model takes over.                                                                            |

`overage_pricing_scheme` is required for Hybrid Pricing to take effect, but omitting it does not raise a validation error: the entire `overage_pricing` block is silently ignored and the Price Point is created as a normal, non-hybrid Price Point using only the primary pricing. Always confirm `overage_pricing_scheme` is present in your request when you expect a Price Point to be hybrid.

## Invoicing

Hybrid Price Points bill through the same Invoices you already use. No separate resource is introduced. The customer sees one line item per billing period for the Component, combining the primary and secondary charges instead of billing them as separate line items.

## Best Practices

- **Confirm Invoice-Centric Billing and the Hybrid Pricing feature are both enabled for the Site** before configuring a hybrid Price Point. Otherwise the Price Point falls back to billing the primary and secondary pricing as separate invoice line items, even with an identical `overage_pricing` configuration.
- **Always set a finite `ending_quantity`** on the primary model's top bracket to indicate where the primary model ends and the secondary model begins.
- **Cache pricing structure in your application** rather than re-fetching it on every request, consistent with our general guidance for [Components](page:introduction/basic-concepts/product-catalog#components).
