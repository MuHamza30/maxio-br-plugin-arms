You can create signups (also called subscriptions) by signing up customers to products on your site.

This guide focuses on the basics of creating subscriptions, though Advanced Billing can almost handle any scenario using API integration.

- Advanced Billing [signup methods](#signup-methods)
- The [payment methods](#payment-methods) available for subscriptions
- How to handle customers with [multiple subscriptions](#multiple-subscriptions)
- Component [quantities](#components) and how they can be used to customize billing

---

You can review our product documentation for more details:

- [Subscriptions Reference](https://maxio.zendesk.com/hc/en-us/articles/24251526991757-Subscription-Overview)
- [Subscriptions Actions](https://maxio.zendesk.com/hc/en-us/articles/24251983024653-Subscription-Actions-Overview)
- [Subscription Cancellation](https://maxio.zendesk.com/hc/en-us/articles/24251957778829-Cancel-Subscriptions)
- [Subscription Reactivation](https://maxio.zendesk.com/hc/en-us/articles/24252109503629-Reactivating-and-Resuming)
- [Subscription Import](https://maxio.zendesk.com/hc/en-us/articles/24251489107213-Imports)
- [Product Options](https://maxio.zendesk.com/hc/en-us/articles/24261076617869-Product-Editing).

## Signup Methods

There are a number of methods of actually signing up customers to your business. Explore the following help articles and see how they might be used in your business:

- [Manually (within Advanced Billing)](https://maxio.zendesk.com/hc/en-us/articles/24181202779149-Create-Subscriptions-Inside-Advanced-Billing)
- [With Public Signup Pages (PSP)](https://maxio.zendesk.com/hc/en-us/articles/24181172242957-Accept-Signups-with-Public-Signup-Pages?method=themes)
- [Via the API](#api)

### Manually (within Advanced Billing)

The easiest way to create simple subscriptions is directly within your Advanced Billing account. Before you begin, ensure that you have at least one Product available for use in the example below. For step-by-step instructions, see the [Create Subscriptions Inside Advanced Billing quick start guide](https://maxio.zendesk.com/hc/en-us/articles/24181202779149-Create-Subscriptions-Inside-Advanced-Billing).

This sign-up method is ideal for businesses with a low volume of subscriptions. It is the fastest way to get started, as it requires no integration.

While this method is simple and effective for initial setup, manually signing up customers is not scalable. Fortunately, there are more robust solutions available to automate and streamline the process.

### Public Signup Pages (PSP)

Public Signup Pages are fully customizable, white-labeled pages that serve as the public-facing side of your subscription business. They provide a fast, code-free way to integrate with Advanced Billing without handling payment information or building a custom integration.

All Advanced Billing plans include access to two types of Public Signup Pages:

1. A Public Signup Page is automatically created for each new product and allows people to sign up for any of your current active products.
2. A [Self-Service Page](https://maxio.zendesk.com/hc/en-us/articles/24261425318541-Self-Service-Pages#example-self-service-page) is automatically created for each active subscription and allows the customer to manage payment methods.

For details on configuring the appearance and behavior of your Public Page, see [Public Page Default Settings](https://maxio.zendesk.com/hc/en-us/articles/24261337051789-Default-Page-Settings) and [Individual Page Settings](https://maxio.zendesk.com/hc/en-us/articles/24261368332557-Individual-Page-Settings).

When using Public Signup Pages, you have a specific URL to which customers can be sent that will allow them to sign themselves up - creating the subscription that is then added to your site.

We recommend reviewing how [Public Signup Pages work](https://maxio.zendesk.com/hc/en-us/articles/24181202779149-Create-Subscriptions-Inside-Advanced-Billing) to better understand the many ways Advanced Billing can be integrated with your systems. Public Signup Pages can also be a useful tool during development to test a simple signup with our pre-made forms versus your form in order to troubleshoot.

In some cases, the Public Signup Pages can't quite handle the specific scenario that you might need in your integration with Advanced Billing - that's why we expose a public API for you to consume by your application.

### API

You can create a basic subscription through the Advanced Billing API by providing just a few key details:

1. The **product** - A subscription links a customer to a product available on your site, so it needs to be specified when creating a subscription.
2. The **customer** - A customer is the person who is consuming your product/service. This can either be a reference to an existing customer in your site, or a completely new customer.
3. The **payment method** - Required for paid Products or any product with a billable component. This specifies how payment is collected.

> **Note:**
> Do not use real card information for testing. See the Sites articles that cover [testing your site setup](https://docs.maxio.com/hc/en-us/articles/24250712113165-Testing-Overview#testing-overview-0-0) for more details on testing in your sandbox.

Note that collecting and sending raw card details in production requires [PCI compliance](https://docs.maxio.com/hc/en-us/articles/24183956938381-PCI-Compliance#pci-compliance-0-0) on your end. If your business is not PCI compliant, use [Maxio.js (formerly Chargify.js)](https://docs.maxio.com/hc/en-us/articles/38163190843789-Chargify-js-Overview#chargify-js-overview-0-0) to collect credit card or bank account information.

- For [automatic](#payment-methods) billing, payments are collected through a credit card or Automated Clearing House (ACH) details.

> **Note:**
> Use [Maxio.js (formerly Chargify.js)]($e/development-tools/chargify-js-overview) to easily construct signup and payment profile update forms directly on your existing sites. This approach ensures that you meet the latest [PCI compliance requirements](https://docs.maxio.com/hc/en-us/articles/24183956938381-PCI-Compliance).

- For [invoice](https://maxio.zendesk.com/hc/en-us/articles/24302160124173-Invoices-Overview-Statements) billing, the payment does not happen automatically but can still be done manually either through: non-electronic means and marked manually, or by using a credit card.

For example, the following `POST` to the Create Subscription API endpoint creates a subscription:

```json
{
  "subscription": {
    "product_handle": "pro-plan",
    "customer_attributes": {
      "first_name": "Joe",
      "last_name": "Smith",
      "email": "j.smith@example.com"
    },
    "credit_card_attributes": {
      "chargify_token": "tok_cwhvpfcnbtgkd8nfkzf9dnjn",
      "payment_type": "credit_card"
    }
  }
}
```

For more information, see the [Create Subscription endpoint documentation]($e/Subscriptions/createSubscription).

For advanced subscription creation scenarios, see [Advanced Subscription Creation Examples](page:introduction/about-the-api/expert-usage#advanced-signup-examples).

## Payment Methods

The payment method for a customer can be either Automatic or Remittance. With Automatic billing, the customer is automatically charged when a subscription renews. With Remittance, the customer is not automatically charged. Instead, an invoice is generated at renewal and can be sent to the customer. You can then record the payment manually once it is received.

For more information, see the [Payment Methods](https://maxio.zendesk.com/hc/en-us/articles/24181238764685-Overview-Subscription-Management?method=paymenttype) help article.

## Taxes

If you intend to charge your subscribers tax via [Avalara taxes](https://maxio.zendesk.com/hc/en-us/articles/24287008131853-Advanced-Billing-Managed-Sales-Tax) or [custom taxes](https://maxio.zendesk.com/hc/en-us/articles/24287044212749-Custom-Taxes), there are a few considerations regarding collecting subscription data. For subscribers to be eligible to be taxed, the following information for the `customer` object or `payment_profile` object must be supplied:

- A subscription to a [taxable product](https://maxio.zendesk.com/hc/en-us/articles/24261076617869-Product-Editing#tax-settings)
- [Full valid billing or shipping address](https://maxio.zendesk.com/hc/en-us/articles/24287008131853-Advanced-Billing-Managed-Sales-Tax#full-address-required-for-taxable-subscriptions) to identify the tax locale
- The portion of the address that houses the [state information](https://maxio.zendesk.com/hc/en-us/articles/24287008131853-Advanced-Billing-Managed-Sales-Tax#required-state-format-for-taxable-subscriptions) of either address must adhere to the ISO standard of a 2-3 character limit/format.
  The portion of the address that houses the [country information](https://maxio.zendesk.com/hc/en-us/articles/24287008131853-Advanced-Billing-Managed-Sales-Tax#required-country-format-for-taxable-subscriptions) must adhere to the ISO standard of a 2 character limit/format.

## Multiple Subscriptions

Advanced Billing doesn't limit you to only allowing one single subscription per customer, you can have multiple subscriptions for a single customer using separate or linked payment methods.

In the following example, the existing customer with `reference` (shown as `customer_reference` below) value `1234-AB` will be subscribed to the product specified. You may also specify the customer_id, but it's far more useful to map a user on your system to a customer on Advanced Billing using this reference value. It's commonly filled with the user's unique identifier (i.e., the userID), which makes referencing the customer in Advanced Billing very simple as there are customer reference value filters in many methods.

```json
{
  "subscription": {
    "product_handle": "basic",
    "customer_reference": "1234-AB",
    "credit_card_attributes": {
      "chargify_token": "tok_cwhvpfcnbtgkd8nfkzf9dnjn",
      "payment_type": "credit_card"
    }
  }
}
```

For more information about the `customer_reference` and `customer_id` values, see [Create Subscription]($e/Subscriptions/createSubscription).

## Subscription in a Customer Hierarchy

For sites using the [Relationship Billing](https://maxio.zendesk.com/hc/en-us/articles/24252287829645-Advanced-Billing-Invoices-Overview) and [Customer Hierarchy](https://maxio.zendesk.com/hc/en-us/articles/24252185211533-Customer-Hierarchies-WhoPays) features, it is possible to create subscriptions within a customer hierarchy. This functionality is available through the API by including `group` parameters in the create subscription request.

The `group` parameters are optional and consist of the required `target` parameter and the optional `billing` parameter.

When the `target` parameter specifies a customer that is already part of a hierarchy, the new subscription becomes a member of the customer hierarchy. If the target customer is not part of a hierarchy, Maxio creates a new customer hierarchy, and both the target customer and the new subscription become part of the hierarchy with the specified target customer designated as the responsible payer for all subscriptions in that hierarchy.

Rather than specifying a customer, the `target` parameter can a value of `self`, which indicates the subscription is paid for by the subscribing customer. This is true whether the customer is being created new, already part of a hierarchy, or already exists outside a hierarchy. A valid payment method must also be specified in the subscription parameters.

When creating subscriptions in a customer hierarchy, if the hierarchy does not already have a payment method, passing a payment method makes that payment method the default for the customer hierarchy, regardless of the responsible payer.

## Subscription in a Subscription Group

For sites making use of [relationship billing](https://maxio.zendesk.com/hc/en-us/articles/24252287829645-Advanced-Billing-Invoices-Overview) you can create a subscription as part of a [subscription group](https://maxio.zendesk.com/hc/en-us/articles/24252172565005-Subscription-Groups-Overview) to use [invoice consolidation](https://maxio.zendesk.com/hc/en-us/articles/24252269909389-Invoice-Consolidation). You can achieve this through the API by passing group parameters in the create subscription request. The `group` parameters are optional and consist of the required `target` and optional `billing` parameters.

The `target` parameters specify an existing subscription with which the newly created subscription should be grouped. If the target subscription is already part of a group, the new subscription becomes a member of the group as well. If the target subscription is not part of a group, a new group is created and both the target and the new subscription become part of the group with the target as the group's primary subscription.

### Billing Parameters for Group and Customer Hierarchy Subscriptions

The optional `billing` parameters control how billing is handled for new subscription in a customer hierarchy or group:

- Use the `accrue` parameter to defer payment capture and accrue charges until the next assessment date.

- Use the `align_date` parameter to align the billing date of the new Subscription with the target Subscription.

- When aligning dates, you can also specify the `prorate` parameter so that charges for the new Subscription are prorated according to the target Subscription’s billing period.

## Components

A common first step during signup is to allocate one or more components that match the initial state of the customer’s subscription. Consider a subscription service that ships a set number of widgets each month. If the Customer signs up for the “5 widgets per month” Product, you would allocate five units of the widget component, as shown below:

```json
{
  "subscription": {
    "product_handle": "basic",
    "customer_attributes": {
      "first_name": "Alysa",
      "last_name": "Test",
      "email": "alysa@example.com",
      "reference": "1234-AB"
    },
    "credit_card_attributes": {
      "chargify_token": "tok_cwhvpfcnbtgkd8nfkzf9dnjn",
      "payment_type": "credit_card"
    },
    "components": [
      {
        "component_id": 1,
        "allocated_quantity": 5
      }
    ]
  }
}
```

For more information about components and how to use this great feature to customize your signup process - see [components](page:core-concepts/product-catalog#components).

See the following articles for a deep dive into how components function within Advanced Billing:

- [Setting component allocations](https://maxio.zendesk.com/hc/en-us/articles/24251883961485-Component-Allocations-Overview)
- [Building components in Advanced Billing](https://maxio.zendesk.com/hc/en-us/articles/24261141522189-Components-Overview)

# Managing Subscriptions

After a Subscription is created, you or your customer will likely need to manage it in various ways. The following sections outline common subscription management tasks.

---

## One-Time Charges

Advanced Billing allows you to add charges to a subscription outside of the regular recurring billing cycle. This is called a ["one-time" charge](https://maxio.zendesk.com/hc/en-us/articles/24302079003533-One-time-Charges) A one-time charge is a charge that happens once either by submitting the charge via the API or by creating the charge manually in the app.

This example posts a $1 charge to a subscription:

```json
// POST /subscriptions/{subscription_id}/charges.json
{
  "charge": {
    "amount_in_cents": 100,
    "memo": "This is the description of the reason for the $1 charge."
  }
}
```

For more information, see [the API details for creating charges](https://developers.maxio.com/legacy/http/api-endpoints/legacy-subscription-balance/create-subscription-charge).

## Billing Dates

It is common for a subscription’s billing date to change. The billing date is the next date the subscription is processed or assessed, and when charges may be captured from its payment method. Changes to the billing date are typically made to extend or shorten trials, process a Subscription immediately, or adjust the date for [calendar billing](https://maxio.zendesk.com/hc/en-us/articles/24286596359949-Calendar-Billing) scenarios.

This example updates the billing date for a subscription:

```json
/// POST /subscriptions/{subscription_id}.json
{
  "subscription": {
    "next_billing_at": "2016-08-29T12:00:00-04:00"
  }
}
```

See the full API documentation for [updating subscription assessment date]($e/Subscriptions/updateSubscription) for more information.

## Updating Payment Details

Updating the payment details allows you to change the card used for a subscription or update the card’s expiration date.

> If your customer pays taxes on their purchased product, and you are attempting to update the `payment_profile`, complete address information is required. For information on required address formatting to allow your subscriber to be taxed, see the section on [sign-up taxes](./Signups.md#taxes).

You can update the payment details via:

- [Self-Service Pages](#updating-via-self-service-pages)
- [API](#updating-via-api)
- [Maxio.js (formerly Chargify.js)](https://docs.maxio.com/hc/en-us/articles/38163190843789-Chargify-js-Overview)

### Updating via Self-Service Pages

You can allow your users to update their information themselves, using the self-service public hosted pages or even the new billing portal.

For the public service page card update, you merely direct them to a specific URL:

`https://{subdomain}.chargify.com/update_payment/{subscription_id}/{token}`

- The `subdomain` is just your subdomain. Our imaginary company "Acme"'s URL would start like the following: `https://acme.chargify.com`

- The `subscription_id` would be the integer ID of the subscription as it is in the Advanced Billing site/subdomain.

- The `token` is calculated using the first 10 characters of the SHA-1 hex digest of this message:

```
message = "update_payment--{subscription_id}--{shared_key}"
token = SHA1(message)[0..9]
```

For more information about the self-service card update public page, see the [Obtaining the Self-Service Page URL](https://maxio.zendesk.com/hc/en-us/articles/24261425318541-Self-Service-Pages#obtaining-the-self-service-page-url) article.

Your users can also self-service update their payment method if using the Advanced Billing Portal feature, see [Updating Payment Information via the Billing Portal](https://maxio.zendesk.com/hc/en-us/articles/24261425318541-Self-Service-Pages#updating-payment-information-via-the-billing-portal) for more information.

### Updating via API

Updating payment profiles via the API is useful in situations where you are more directly integrating with Advanced Billing.

There are many methods of performing this action via the API, you can:

1. Update the payment profile indirectly through a subscription update
2. Update the payment profile directly

See [Update Payment Profile]($e/Payment%20Profiles/updatePaymentProfile) and [Update Subscription]($e/Subscriptions/updateSubscription) for more complete documentation about updating payment profiles via the API.

## Cancelling

Cancelling subscriptions is another common task that customers can perform, or that is performed on their behalf when payment cannot be captured.

This example cancels a subscription:

```json
// DELETE /subscriptions/{subscription_id}.json
{
  "subscription": {
    "cancellation_message": "Canceling the subscription via the API"
  }
}
```

You can also cancel a subscription at the end of the current billing period, which is called a delayed cancellation.

This is an example of a delayed cancellation request:

```json
// DELETE /subscriptions/{subscription_id}.json
{
  "subscription": {
    "cancel_at_end_of_period": 1,
    "cancellation_message": "Canceling the subscription via the API"
  }
}
```

For information about cancelling using the API, see [Cancelling via API]($e/Subscription%20Status/cancelSubscription).

For information about cancelling subscriptions in general, see the [Cancellations](https://maxio.zendesk.com/hc/en-us/articles/24252133729165-Cancellations) help article.

## Refunds

With Advanced Billing you have the ability to apply a refund to payments that have been processed at the gateway. Refunds are only supported for the gateways listed in the [Issuing Refunds in Statement-Based Sites](https://maxio.zendesk.com/hc/en-us/articles/24302120115213-Refunds) help article.

For gateways like Bambora, you need to perform a "manual refund" in that you record the refund as a transaction directly after you perform the actual refund in your gateway account.

You can perform a non-manual refund using the API, as shown in the following example:

```json
{
  "refund": {
    "payment_id": "{payment_id}",
    "amount": "4.00",
    "memo": "Your memo here."
  }
}
```

You will substitute values for `payment_id`, `amount` and `memo` in this example. The `payment_id` is the ID of the payment transaction that the credit will be applied to.

For more information, see [API refunds](https://developers.maxio.com/legacy/http/api-endpoints/legacy-subscription-balance/create-refund).

For a manual or external refund, you also supply a value for `external`:

```json
{
  "refund": {
    "payment_id": "{payment_id}",
    "amount": "4.00",
    "memo": "Your memo here."
  },
  "external": 1
}
```

For a manual or external refund, there is nothing passed through to your gateway - it is simply added to the subscription, modifying the balance and adding a transaction record.

For more information, see [API Refunds (External)](https://developers.maxio.com/legacy/http/api-endpoints/legacy-subscription-balance/create-refund).

## Subscription Updates via Billing Portal

Subscriptions can also be updated by the subscriber via the Billing Portal. The Billing Portal allows your subscribers to perform certain managerial actions on their current subscription. As a merchant, you have the ability to also restrict what actions can be performed by a subscriber.

Here are a few examples of actions that can be performed via the Billing Portal:

- Plan changes
- Subscription cancellation
- Credit card updates
- Component purchase / allocation updates

For more information on the Advanced Billing Portal, see the [Billing Portal](https://maxio.zendesk.com/hc/en-us/articles/24252412965133-Billing-Portal-Overview) help articles.
