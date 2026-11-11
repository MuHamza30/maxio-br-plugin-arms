# Background: entitlement-provisioning

- **silent**, order/side-effects (`background:entitlement-provisioning`, `client.ProductFeatures.CreateProductFeature`, `client.ComponentFeatures.CreateComponentFeature`): A feature is attached to a product or component that subscribers already have → Subscribers are provisioned the entitlement automatically only the next time their subscription changes (for example signup, plan change or component allocation). **Do:** Do not expect existing subscribers to get a new feature until their subscription next changes. *(`maxio-c4:background.72`)*
