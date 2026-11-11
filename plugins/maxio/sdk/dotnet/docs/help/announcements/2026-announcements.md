# 2026 Announcements

## Hybrid Pricing for Quantity-Based and Metered Components

Quantity-Based and Metered Components can now combine a primary tiered, volume, or stairstep pricing model with a secondary pricing model for usage above an included threshold, billed together as a single invoice line item instead of multiple. See the [Hybrid Pricing](page:introduction/basic-concepts/hybrid-pricing) guide for requirements and API configuration.

Hybrid Pricing is configured through the existing Components and Price Points endpoints. No new endpoints or request parameters were introduced. It requires Invoice-Centric Billing and must be enabled for your Site; contact your Maxio account team to turn it on.

## NEW Catalog Experience and Terminology

We’ve redesigned the Maxio Product Catalog to [give you more control](https://docs.maxio.com/hc/en-us/articles/44628847587341-Understand-Planless-Subscriptions) over how you package your offerings.

As such, we’ve updated our terminology in the UI:

### UI vs API naming

When the new catalog experience is enabled, the UI uses updated labels:

- Objects that were previously called Products in the UI are now called Plans.

- Objects that were previously called Components in the UI are now called Products.

These changes are limited to the application UI. No API object names or endpoints are being renamed as part of this change, and existing integrations continue to work as they do today:

- UI Plans still map to the existing product objects in the API.

- UI Products still map to the existing component objects in the API.

## SDK releases

You can view release information on the package manager for a client library:

- [Python](https://pypi.org/project/maxio-advanced-billing-sdk/)
- [Ruby](https://rubygems.org/gems/maxio-advanced-billing-sdk/)
- [PHP](https://packagist.org/packages/maxio/advanced-billing-sdk)
- [C#/.NET](https://www.nuget.org/packages/Maxio.AdvancedBillingSdk/)
- [Typescript](https://www.npmjs.com/package/@maxio-com/advanced-billing-sdk/)
- [Java](https://central.sonatype.com/artifact/com.maxio/advanced-billing-sdk)
- [Go](https://pkg.go.dev/github.com/maxio-com/ab-golang-sdk)

Or you can visit the client library's release page in source control for release details:

## August SDK Release Notes

- C#/.Net: https://github.com/maxio-com/ab-dotnet-sdk/releases/tag/10.0.0
- Go: https://github.com/maxio-com/ab-golang-sdk/releases/tag/v0.10.0
- PHP: https://github.com/maxio-com/ab-php-sdk/releases/tag/10.0.0
- Python: https://github.com/maxio-com/ab-python-sdk/releases/tag/10.0.0
- Java: https://github.com/maxio-com/ab-java-sdk/releases/tag/10.0.0
- Ruby: https://github.com/maxio-com/ab-ruby-sdk/releases/tag/10.0.0
- Typescript: https://github.com/maxio-com/ab-typescript-sdk/releases/tag/10.0.0

## March SDK Release Notes

- C#/.Net: https://github.com/maxio-com/ab-dotnet-sdk/releases/tag/9.0.0
- Go: https://github.com/maxio-com/ab-golang-sdk/releases/tag/v0.9.0
- PHP: https://github.com/maxio-com/ab-php-sdk/releases/tag/9.0.0
- Python: https://github.com/maxio-com/ab-python-sdk/releases/tag/9.0.0
- Java: https://github.com/maxio-com/ab-java-sdk/releases/tag/9.0.0
- Ruby: https://github.com/maxio-com/ab-ruby-sdk/releases/tag/9.0.0
- Typescript: https://github.com/maxio-com/ab-typescript-sdk/releases/tag/9.0.0
