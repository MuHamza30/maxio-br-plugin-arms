# Background: invoicing

- **silent**, account-config/values (`background:invoicing`, `client.ComponentPricePoints.CreateComponentPricePoint`, `client.ComponentPricePoints.UpdateComponentPricePoint`, `client.Components.CreateQuantityBasedComponent`): A hybrid price point is billed → The customer sees one invoice line item per billing period for the component, combining primary and secondary charges.. **Do:** Expect one combined line item, not separate primary and overage lines, for hybrid price points. *(`maxio-c4:background.71`)*
