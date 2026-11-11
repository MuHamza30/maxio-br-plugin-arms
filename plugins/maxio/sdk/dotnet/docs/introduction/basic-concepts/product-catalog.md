# Product Catalog

Learn how to setup products and components for use when creating subscriptions.

Products control what is charged and how often charges are assessed/billed to a subscription. If you need help after reading this, [let us know](page:help/getting-support) so we can help and also improve this documentation.

---

With regards to products, there are three important aspects that are required for using products when interacting with the API:

- Creating the [product family](#product-family)
- Creating the [product](#product)

Before delving into this section, we recommend reviewing our [Products Introduction](https://maxio.zendesk.com/hc/en-us/articles/24261090117645-Products-Overview) help article.

## Product Family

Products have to belong to a product family. Think of them as a logical grouping of products. In our Acme, Inc. example - one possible product family would be "Acme Projects".

To create a product family using the API you need to do the following:

Input attributes:

- `name` (required) - The product family name. For example, if your app had two levels of service, "Basic" and "Premium" then these might be the product names.
- `handle` (optional) - The handle of the product family. This is generated automatically if not specified.
- `description` (optional) - A quick description of what the product family is.

An example of our input attributes might look like the following:

```json
// product_family.json
{
  "product_family": {
    "name": "Acme Projects",
    "description": "Amazing project management tool"
  }
}
```

That data should be posted to the [Create Product Family]($e/Product%20Families/createProductFamily) endpoint.

A simple curl example would be the following:

```perl
curl -u <API_KEY>:X -H Accept:application/json -d @product_family.json -X POST https://<SUBDOMAIN>.chargify.com/product_families.json
```

To create a product family using the application, refer to the [Creating Product Families](https://maxio.zendesk.com/hc/en-us/articles/24261098936205-Product-Families) help article for more information.

See [Create Product Family]($e/Product%20Families/createProductFamily) for a complete listing of input/output schema, along with code examples in multiple programming languages.

## Product

In Advanced Billing, you sell Subscriptions to your Products. You must first create and configure a Product before you can sell anything to a Customer.

In your app or business, you might call these products your “Plans” or “Feature Levels”. For example, if you have “Basic”, “Pro”, and “Max” plans, each of these would be a separate Product within Advanced Billing.

You can create a product using the Create Product endpoint:

```json
{
  "product": {
    "name": "Basic Plan",
    "handle": "basic",
    "description": "This is our basic plan.",
    "accounting_code": "123",
    "request_credit_card": true,
    "price_in_cents": 1000,
    "interval": 1,
    "interval_unit": "month",
    "auto_create_signup_page": true
  }
}
```

That data is posted to the [Create Product]($e/Products/createProduct) endpoint.

## Product Price Points

Product price points allow you to charge customers different amounts and at different frequencies for the same product.

See the [Product Price Points](https://maxio.zendesk.com/hc/en-us/articles/24261111947789-Product-Price-Points) help article.

## Components

Components are a great way to customize how your customers can use your products or services, and provide an excellent mechanism for increasing the [MRR](https://www.maxio.com/saaspedia#saaspedia_mrr-articles) per subscription through new features you might develop.

---

Components allow you to introduce additional line items to your products that are often expressed as add-ons, premium features, or pay-per-use items. There are two basic concepts needed to use components that we will discuss:

1.  [Creating](#creating-components) components
2.  The [usage/allocation](#usage-allocation) of components

For more information about components, see our [Component](https://maxio.zendesk.com/hc/en-us/articles/24261141522189-Components-Overview) help article.

### Creating Components

To use components, you must first create them. You can do this in a number of ways: by creating them via the Advanced Billing user interface, or by creating them via the API. In the following example, let's create a component called "Text Messages" that costs $0.0075 per message:

```json
{
  "metered_component": {
    "name": "Text messages",
    "unit_name": "text message",
    "taxable": true,
    "pricing_scheme": "per_unit",
    "unit_price": 0.0075
  }
}
```

The response for the creation of this component would provide you the ID necessary to use the component in all further subsequent API usage requests.

If you need to display component pricing to your customers, we recommend caching this information in your application rather than making repeated API calls for it, since the pricing structure of a component does not usually change very often.

For more information on components, see the following:

- About [Components](https://maxio.zendesk.com/hc/en-us/articles/24261141522189-Components-Overview) help article
- Creating components [via the API]($e/Components/createMeteredComponent)

### Usage/Allocation

Associating components with a subscription is done by allocating (or adding usage, depending on the type of component).

- For metered components which reset to zero at each billing period, you would be adding "usage". For example, if your customer sent 10 text messages today, you would add usage for 10 units of the "text message" component (see above).

- For quantity components, you would be "allocating" use. For example, if you had a component that represented the number of seats covered under their license, you would allocate that amount: i.e., the customer is allocated 10 seats covered by their license to use your software.

- For "on/off" components, you would be turning them on or off. For example, let's say your customer could have "premium support" for an extra $25/month. That component, "premium support" could be turned on or off at will during the life time of the subscription - including prorating it during changes to the subscription plan.

The following is an example that adds 5 text messages as "usage":

```json
{
  "usage": {
    "quantity": 5,
    "memo": "Extra text messages"
  }
}
```

Components can be used in a huge number of varying ways to cover your business model - it's just up to you on how you want it to work.

### Coupons

Are you looking to offer current or potential customers a discount? Advanced Billing handles all of your promotional codes, discounts, and coupons with ease. Simply name the promotion, set your desired promo code, and enter the discount. You even have the power to control the expiration date and how long the promotion runs for in conjunction with your products.

Let's create a coupon that we can then use when creating our next subscription.

```json
// POST /coupons.json
{
  "coupon": {
    "name": "15% off",
    "code": "15OFF",
    "description": "15% off for life",
    "percentage": "15",
    "allow_negative_balance": "false",
    "recurring": "false",
    "end_date": "2012-08-29T12:00:00-04:00",
    "product_family_id": "2"
  }
}
```

For more information on coupons, see the following:

- Create a coupon [via the API]($e/Coupons/createCoupon).
- Use a coupon when creating a new subscription [via the API]($e/Subscriptions/createSubscription).
