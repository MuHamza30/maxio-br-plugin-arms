## 2021-12-16 Current Vault Validation

Between approximately 3:30 AM on November 9th, 2021, and 2:00 PM on November 10, 2021,the value of `current_vault` was being validated to see if it matched one of the gateways configured for the site.

In most cases the value did match and there was no effect. For a few sites, we discovered that an incorrect value was being submitted.
For example, we were receiving `stripe` when the correct value is `stripe_connect`.

This change has been reverted, and we are reaching out to a limited number of merchants who experienced signup failures due to this validation.

We apologize for any disruption and are taking steps to ensure that notification is provided in advance if we must make a change that breaks backwards compatibility in the API.

This change will be re-introduced in the future.

## 2021-11-23 Issue Service Credit Response Body Change

As of November 23rd, 2021, the [Issue Service Credit API endpoint]($e/Subscription%20Invoice%20Account/issueServiceCredit) will respond with a new body:

```json
{
  "id": "Integer",
  "amount_in_cents": "Integer",
  "ending_balance_in_cents": "Integer",
  "entry_type": "String",
  "memo": "String"
}
```

Previously, this endpoint was returning just a status code without body.

## 2021-11-18 Renewal Preview Returns Free Non-Zero Components

As of November 18th, 2021, the renewal preview API endpoint will return additional component line items.
Previously, only components with a cost greater than 0 were returned.

With the new behavior, free components will be returned as line items so long as their recorded quantity is greater than 0.

## 2021-11-05 Payment Collection Validation Changes

As of October 26th, 2021, subscription's attribute "payment_collection_method" is being validated accordingly to site's architecture:

- on statement-based sites valid options are:
  - "automatic"
  - "invoice"
- on Relationship Invoicing architecture valid options are:
  - "automatic"
  - "remittance"
  - "prepaid"

## 2021-10-01 Update Price Point Prices Error Response Code Change

Currently, when [updating a price point]($e/Components/updateComponentPricePoint) with changes to its prices and referencing an id of a price which does not belong to the specified price point, the API returns a 404.
The response for this case is changing to a 422.

## 2021-09-13 Subscription Reactivation Payment Decline Error Change

The error messages returned when a subscription's reactivation payment has been declined will change on October 4, 2021, both within the API and the admin UI.

The current behavior returns one of two generic messages. If the payment profile was declined, the message is: “The payment method on file could not be charged.”

If no payment profile is on file, the message is: “The customer does not have a credit card on file.”

With the upcoming changes, reactivations that fail due to a payment decline will return the payment gateway’s decline message.
When no payment profile is on file, the message will be: “No payment method was on file for the X balance,” where X is the appropriate currency marker and amount due at time of reactivation.

For API users, failed reactivation calls will still return a 422, and the structure of the error hash remains the same.
Only the error messages in the case of a failed payment or missing payment profile will change to better reflect the cause of failure.
These errors will also begin to appear in the admin UI.

## 2021-02-12 Subscription Reactivation Behavior Change

As of February 22nd, 2021, reactivation of subscription within the current billing period will by default apply service credits and prepayments to the invoice.
There will be an optional checkbox to reactivate the subscription in the old way without using service credits and prepayments.

In the API the behavior also will change - service credits and prepayments will be applied by default, to block that action the `use_credits_and_prepayments` attribute must be set to `false`.

Previously, service credits and prepayments haven't been considered during reactivation with resuming the current billing period.
