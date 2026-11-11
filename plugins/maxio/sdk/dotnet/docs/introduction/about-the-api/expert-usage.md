# Expert Usage

The topics below come up in more advanced integrations. Which topics apply depends on the features you use and how you integrate with the API.

## Advanced Signup Examples

The following examples cover signups that go beyond creating a basic subscription. For the full list of supported fields, see [Create Subscription]($e/Subscriptions/createSubscription).

1. Import as a new subscription

Send the existing billing date and payment profile with the signup:

```json
// POST /subscriptions.json
{
  "subscription": {
    "product_handle": "basic",
    "next_billing_at": "2030-08-29T12:00:00-04:00",
    "customer_attributes": {
      "first_name": "John",
      "last_name": "Doe",
      "email": "john.doe@example.com",
      "reference": "123",
      "organization": "Acme Widgets"
    },
    "payment_profile_attributes": {
      "vault_token": "12345",
      "customer_vault_token": "67890",
      "current_vault": "authorizenet",
      "expiration_year": "2030",
      "expiration_month": "12",
      "card_type": "visa",
      "last_four": "1111"
    }
  }
}
```

2. New subscription with a coupon, trial, or components

Apply coupons, custom trial periods, and components when the subscription is first created:

```json
{
  "subscription": {
    "product_handle": "basic",
    "customer_attributes": {
      "first_name": "John",
      "last_name": "Smith",
      "email": "john.smith@example.com"
    },
    "credit_card_attributes": {
      "masked_card_number": "XXXX-XXXX-XXXX-1111",
      "expiration_month": "10",
      "expiration_year": "2030"
    },
    "coupon_code": "SUB111",
    "next_billing_at": "2030-06-01",
    "components": [
      {
        "component_id": 123456,
        "unit_balance": 20
      }
    ]
  }
}
```

3. New subscription with an existing payment profile

## Metafields and Metadata

Metafields are custom fields that store information on a customer or subscription resource. Metafields and metadata appear as [Custom Fields](https://docs.maxio.com/hc/en-us/articles/24178993762317-Custom-Fields) in the application and in the product documentation.

The following example creates a metafield and then sets a metadata value for that metafield.

First, create the metafield. This example applies a `Color` field to the customers resource:

```json
// POST /customers/metafields.json
{
  "metafields": {
    "name": "Color",
    "scope": {
      "csv": "1",
      "invoices": "1",
      "portal": "1"
    }
  }
}
```

The `scope` values set here show the metafield on Public Signup Pages, show the metadata on invoices, and allow the metadata to be exported to CSV.

Next, set the color for a single customer:

```json
// POST /customers/{customer_id}/metadata.json
{
  "metadata": {
    "name": "Color",
    "value": "Blue"
  }
}
```

That customer now has a `Color` metadata value of `Blue`.

For more API information about metafields, which are the containers for your metadata, see [Create Metafields]($e/Custom%20Fields/createMetafields).

## Communication

You can communicate with subscribers in several ways. For details on each method, see the corresponding help article:

- [Email Templates](https://maxio.zendesk.com/hc/en-us/articles/24266086168461-Email-Templates)
- [Liquid Examples](https://maxio.zendesk.com/hc/en-us/articles/24266086916877-Liquid-Examples), for use in emails and other communications
- [HTML Emails](https://maxio.zendesk.com/hc/en-us/articles/24266101040781-HTML-Emails)
- [Bulk Email](https://maxio.zendesk.com/hc/en-us/articles/24266110860045-Bulk-Email-Mass-Emailing-Subscribers), for mass emailing subscribers
- [Email Archives](https://maxio.zendesk.com/hc/en-us/articles/24266109347853-Email-Archives)

## Dunning

Dunning is the process of communicating with customers about failed credit card transactions and expiring credit cards.

Advanced Billing manages the dunning process for you. On gateways such as Authorize.net and PayPal, each failed transaction has to be addressed manually as it arises, which is time consuming and does not scale to a large number of transactions.

For more information, including how to set up your dunning plans, see the [Dunning Overview](https://maxio.zendesk.com/hc/en-us/articles/24287076583565-Dunning-Overview) help article.

## Referrals

Referrals reward your customers for sharing information about your application with potential new users. A referral code is generated for each subscription, and both the new customer and the referrer are rewarded when a signup uses the code.

When referrals are enabled, every subscription includes a `referral_code` in the subscription API response. The customer on that subscription shares this code to refer new signups.

```json
// GET /subscriptions/{subscription_id}.{format}
{
  "subscription": {
    "id": "123456789",
    "state": "active",
    // ...
    "referral_code": "trdgzp"
  }
}
```

To validate a referral code before using it, send the following request:

HTTP GET: `https://{subdomain}.chargify.com/referral_codes/validate.{format}?code={referral_code}`

A valid referral code returns `200 OK`. An invalid referral code returns `404 NOT FOUND`.

For more information, see the [Referrals Overview](https://maxio.zendesk.com/hc/en-us/articles/24287008467725-Referrals-Overview) help article.

## Notes

Notes keep unstructured data associated with an individual subscription. For structured data, use metafields instead. See [Create Metafields]($e/Custom%20Fields/createMetafields).

To create a simple note on a subscription:

```json
// POST /subscriptions/{subscription_id}/notes.{format}
{
  "note": {
    "body": "Customer requested annual invoicing starting at the next renewal.",
    "sticky": true
  }
}
```

Setting `sticky` to `true` shows the note prominently when viewing the subscription.

For more information, see [Create Subscription Note]($e/Subscription%20Notes/createSubscriptionNote).
