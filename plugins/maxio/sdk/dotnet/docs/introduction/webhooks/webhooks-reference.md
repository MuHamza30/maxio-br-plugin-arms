# Webhooks reference

Webhook events are triggered by key actions and changes within Advanced Billing, providing notifications about billing, subscription, payment, and account activities. Each event includes a structured payload with relevant data, allowing you to automate workflows, synchronize systems, or log activity for auditing and analysis.

If you’re using the [enhanced Catalog experience](page:help/announcements/2026-announcements#new-catalog-experience-and-terminology), you’ll see updated naming in webhook events and messages.

Event name changes:

- subscription_product_change → subscription_plan_change
- component_allocation_change → allocation_change
- component_billing_date_change → product_billing_date_change

Message updates:

- “Successful payment for allocation changes to Product on Subscription”
- “Failed payment for allocation changes to Product on Subscription”
- “Plan changed on Subscription from previous plan to new plan”

## Payloads

The resource objects sent as payloads typically contain the same information as the corresponding API resource. For example, Site payload objects include the site’s id and subdomain. However, not all values may be populated in the payload. Use discretion to determine whether the data is available and how to handle missing information. For example, the reason_code data is not delivered in a signup_success webhook.

Webhooks retain their event and payload data. Refer to the [webhook API]($e/Webhooks/listWebhooks) for detailed attribute documentation.

### Webhook metadata

Webhook records include metadata about their acceptance or rejection by your application, as well as details about errors encountered during delivery attempts. Metadata attributes include:

- **id**: Unique identifier for the webhook, consistent across retries/replays.
- **successful**: Boolean indicating whether the webhook was accepted on its last attempt.
- **created_at**: Timestamp for when the webhook was created.
- **accepted_at**: Timestamp for when the webhook was successfully accepted Once a webhook is accepted, the accepted_at timestamp will be filled. This timestamp can be viewed via the Webhooks API or in the Webhooks panel, if available for your plan.
- **last_sent_at**: Timestamp for the most recent delivery attempt.
- **last_error_at**: Timestamp for the last failed delivery attempt.
- **last_error**: Description of the error from the last failed attempt.

## Events

<Accordion title="billing_date_change">

**Trigger:** Any change to the billing date that is initiated explicitly by altering the billing date through the application or the API. This will not be triggered by a normal renewal and period advancement, or a migration.

**Payload Fields:** event_id, site, subscription (with previous_billing_date)
The subscription object also contains information on the Customer and Product.

> Note
> Multiple webhook events may be triggered by a single system event. For example, the creation of a new Subscription will typically fire both a signup_success and payment_success event (if a payment was necessary to start the Subscription).

<Accordion title="Full payload example">

```json
{"site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "subscription"=>
  {"id"=>16372192,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Mon, 13 Feb 2025 11:50:57 EST -05:00,
   "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 13:39:20 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>Thu, 13 Apr 2025 14:28:00 EDT -04:00,
   "next_assessment_at"=>Thu, 13 Apr 2025 14:28:00 EDT -04:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 13 Feb 2025 13:28:05 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>173961106,
   "signup_revenue"=>"60.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>22000,
   "product_price_in_cents"=>5000,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"cz8wdq",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "customer"=>
    {"id"=>15826583,
     "first_name"=>"Doris",
     "last_name"=>"Tester",
     "organization"=>"Acme",
     "email"=>"doris@example.com",
     "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
     "updated_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "reference"=>"123456789",
     "address"=>"123 Anywhere Street",
     "address_2"=>"",
     "city"=>"Boston",
     "state"=>"MA",
     "zip"=>"02120",
     "country"=>"US",
     "phone"=>"555-555-1212",
     "portal_invite_last_sent_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>nil,
     "portal_customer_created_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil},
   "product"=>
    {"id"=>4442358,
     "name"=>"Gold Product",
     "handle"=>"gold-product",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Thu, 15 Dec 2024 09:32:36 EST -05:00,
     "updated_at"=>Thu, 15 Dec 2024 09:32:36 EST -05:00,
     "price_in_cents"=>5000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>306012,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/7dbsnjd8t8cx/gold-product"},
       {"id"=>310598,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/ksjh9py5fn5h/gold-product"},
       {"id"=>311132,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/kjmks49g8d3d/gold-product"}]},
   "credit_card"=>
    {"id"=>10914352,
     "first_name"=>"Doris",
     "last_name"=>"Tester",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
     "card_type"=>"visa",
     "expiration_month"=>2,
     "expiration_year"=>2018,
     "customer_id"=>15826583,
     "current_vault"=>"bogus",
     "vault_token"=>"1",
     "billing_address"=>"",
     "billing_city"=>"",
     "billing_state"=>"",
     "billing_zip"=>"",
     "billing_country"=>"",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"",
     "payment_type"=>"credit_card"},
   "previous_billing_date"=>Mon, 13 Mar 2025 14:28:05 EDT -04:00},
 "event_id"=>377604301}
```

</Accordion>
</Accordion>

<Accordion title="component_allocation_change">

**Trigger:** Any change to the Subscription quantity-based component allocation, enabled status of an on/off component, or a purchase of a prepaid component allocation that is made _after_ signup. This webhook does not fire if allocations are configured during the initial Subscription creation (signup). `previous_allocation` and `new_allocation` give the allocation values before and after the change. These will be either `0` or `1` for On/Off Components to represent `off` and `on`, respectively. `timestamp` provides the date and time the allocation was recorded and is listed in ISO8601 format in the UTC timezone.

**Payload Fields:** event_id, site, component, subscription, product, previous_allocation, new_allocation, memo, timestamp
<Accordion title="Full payload example for On/Off Component Change">

```json
 "component"=>
  {"id"=>375117,
   "kind"=>"on_off_component",
   "name"=>"Cancellation fee",
   "unit_name"=>"on/off"},
 "subscription"=>{"id"=>16372192, "name"=>"Doris Tester"},
 "product"=>{"id"=>4443536, "name"=>"Business Monthly"},
 "memo"=>"Adding cancellation fee for Doris",
 "timestamp"=>"2025-02-13T18:46:49Z",
 "previous_allocation"=>0,
 "new_allocation"=>1,
 "payment"=>nil,
 "event_id"=>377608723}
```

</Accordion>
<Accordion title="Full payload example for Quantity-based Component Change">

```json
 "component"=>
  {"id"=>375250,
   "kind"=>"quantity_based_component",
   "name"=>"Quantity Component",
   "unit_name"=>"Quantity Component"},
 "subscription"=>{"id"=>16372192, "name"=>"Doris Tester"},
 "product"=>{"id"=>4443536, "name"=>"Business Monthly"},
 "memo"=>"Adding 90 components for Doris",
 "timestamp"=>"2025-02-13T18:49:58Z",
 "previous_allocation"=>10,
 "new_allocation"=>90,
 "payment"=>
   {"id"=>1078634,
    "success"=>true,
    "amount_in_cents"=>800,
    "memo"=>"Payment for: Full-price component allocation changes."},
 "event_id"=>377609562}
```

</Accordion>
<Accordion title="Full payload example for Prepaid Component Change">

```json
"component"=>
  {"id"=>957864,
   "kind"=>"prepaid_usage_component",
   "name"=>"Minutes",
   "unit_name"=>"minutes",
   "handle"=>nil},
 "subscription"=>
  {"id"=>32304660,
   "name"=>"Mario Smith",
   "organization"=>"",
   "state"=>"active",
   "product"=>
    {"id"=>4607690,
     "product_price_point_id"=>322106,
     "product_price_point_handle"=>"uuid:8e1f3840-c44a-0136-fd09-02d0e580fd36",
     "name"=>"v-1",
     "interval"=>1,
     "interval_unit"=>"month"}},
 "product"=>
  {"id"=>4607690,
   "name"=>"Silver Plan",
   "interval"=>1,
   "interval_unit"=>"month"},
 "allocation"=>
  {"id"=>571895482,
   "proration_upgrade_scheme"=>"full-price-attempt-capture",
   "proration_downgrade_scheme"=>"no-prorate"},
 "previous_allocation"=>0,
 "new_allocation"=>10,
 "memo"=>"",
 "timestamp"=>"2025-04-28T18:34:28Z",
 "price_point_id"=>821647,
 "payment"=>
  {"id"=>381732923,
   "success"=>true,
   "amount_in_cents"=>2000,
   "memo"=>"Payment for: Full-price component allocation changes."},
 "allocated_quantity"=>10,
 "expires_at"=>nil,
 "renews"=>true,
 "total_used"=>0,
 "total_overage"=>0,
 "event_id"=>1064815277}
```

</Accordion>

</Accordion>

<Accordion title="chjs_tokenization_failure">

**Trigger:** Maxio.js (formerly Chargify.js) failed to tokenize a payment profile at the payment gateway (for example, a declined test card or a gateway error). The event is recorded when tokenization is attempted through `POST /js/tokens` and the gateway returns an error. Failures that occur before tokenization (such as reCAPTCHA, fraud blocking, or an invalid security token) do not trigger this event.

**Payload Fields:** event_id, site, errors, payment_profile_params

<Accordion title="Full payload example">

```json
{
  "site": {
    "id": 1,
    "subdomain": "acme"
  },
  "errors": "Card was declined",
  "payment_profile_params": {
    "first_name": "John",
    "last_name": "Doe",
    "card_type": "visa"
  },
  "event_id": 12346
}
```

</Accordion>

</Accordion>

<Accordion title="chjs_tokenization_success">

**Trigger:** Maxio.js successfully tokenized a payment profile at the payment gateway. The event is recorded when tokenization completes through `POST /js/tokens` during public signup pages, self-service flows, or other Maxio.js integrations.

**Payload Fields:** event_id, site, payment_profile, gateway_customer_id

<Accordion title="Full payload example">

```json
{
  "site": {
    "id": 1,
    "subdomain": "acme"
  },
  "payment_profile": {
    "id": 987,
    "vault_token": "1",
    "gateway_handle": "default",
    "customer_vault_token": "cus_example"
  },
  "gateway_customer_id": 456,
  "event_id": 12345
}
```

</Accordion>

</Accordion>

<Accordion title="custom_field_value_change">

**Trigger:** A change to any custom field value, whether adding a custom field at signup, or updating a custom field on an existing Subscription or Customer record.

**Payload Fields:** See full payload example.
<Accordion title="Full payload example">

```json
{"site"=>{"id"=>60050, "subdomain"=>"chargify-sandbox-141"},
 "metafield"=>
  {"event_type"=>"created",
   "metafield_name"=>"Device ID",
   "metafield_id"=>31483,
   "old_value"=>nil,
   "new_value"=>"dev_7hbd)jh27gyJ",
   "resource_type"=>"Subscription",
   "resource_id"=>63081463},
 "event_id"=>2946240555}
```

</Accordion>
</Accordion>

<Accordion title="customer_create">

**Trigger:** A new customer is created.

**Payload Fields:** event_id, site, customer
<Accordion title="Full payload example">

```json
  {"address"=>"123 Main St",
   "address_2"=>"Apt 123",
   "city"=>"Pleasantville",
   "country"=>"US",
   "created_at"=>"2025-09-09 11:38:32 -0400",
   "email"=>"john@example.com",
   "first_name"=>"John",
   "id"=>"0",
   "last_name"=>"Doe",
   "organization"=>"Acme, Inc.",
   "phone"=>"555-555-1234",
   "reference"=>"johndoe",
   "state"=>"NC",
   "updated_at"=>"2025-09-09 11:38:32 -0400",
   "zip"=>"12345",
   "parent_id"=>nil},
 "site"=>{"id"=>2, "subdomain"=>"acme"},
 "event_id"=>855147013}
```

</Accordion>
</Accordion>

<Accordion title="customer_delete">

**Trigger:** When a customer is deleted. _Note_: This webhook is **not** triggered by the deletion of a Subscription and Customer simultaneously; it is only triggered by explicitly deleting the customer as a single action.

**Payload Fields:** See full payload example.
<Accordion title="Full payload example">

```json
{"customer"=>
  {"id"=>59200671,
   "first_name"=>"Harrison",
   "last_name"=>"Ankunding",
   "organization"=>"Jacobs and Sons",
   "email"=>"Mellie.Lindgren80@example.org",
   "created_at"=>Fri, 07 Oct 2025 12:53:42 PDT -07:00,
   "updated_at"=>Fri, 07 Oct 2025 12:53:58 PDT -07:00,
   "reference"=>nil,
   "address"=>"303 Mozell Summit",
   "address_2"=>"Suite 602",
   "city"=>"East Emie",
   "state"=>"CO",
   "zip"=>"25924",
   "country"=>"US",
   "phone"=>"484-328-6314",
   "portal_invite_last_sent_at"=>nil,
   "portal_invite_last_accepted_at"=>nil,
   "verified"=>false,
   "portal_customer_created_at"=>Fri, 07 Oct 2025 12:53:58 PDT -07:00,
   "vat_number"=>nil,
   "cc_emails"=>nil,
   "tax_exempt"=>false,
   "parent_id"=>nil,
   "locale"=>nil},
 "site"=>{"id"=>60050, "subdomain"=>"chargify-sandbox-141"},
 "event_id"=>2946257113}
```

</Accordion>
</Accordion>

<Accordion title="customer_update">

**Trigger:** Any change to the following Customer fields: first_name, last_name, organization, email, reference, address, address 2, city, state, zip, country, phone, vat_number, parent_id, cc_email.

**Payload Fields:** event_id, site, customer
<Accordion title="Full payload example">

```json
  {"id"=>15826583,
   "first_name"=>"Doris",
   "last_name"=>"Tester",
   "organization"=>"Acme",
   "email"=>"doristester@example.com",
   "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 13:42:07 EST -05:00,
   "reference"=>"123456789",
   "address"=>"123 Anywhere Street",
   "address_2"=>"",
   "city"=>"Boston",
   "state"=>"MA",
   "zip"=>"02120",
   "country"=>"US",
   "phone"=>"555-555-1212",
   "portal_invite_last_sent_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
   "portal_invite_last_accepted_at"=>Mon, 13 Feb 2025 12:50:55 EST -05:00,
   "verified"=>false,
   "portal_customer_created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
   "vat_number"=>737291901,
   "cc_emails"=>nil,
   "tax_exempt"=>false,
   "parent_id"=>7
   },
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "event_id"=>377607274}
```

</Accordion>
</Accordion>

<Accordion title="delayed_subscription_creation_failure">

**Trigger:** A failure to create a delayed Subscription.

**Payload Fields:** event_id, site, subscription
<Accordion title="Full payload example">

```json
{"subscription"=>
  {"id"=>63126445,
   "state"=>"pending",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>nil,
   "created_at"=>Fri, 10 Mar 2025 09:19:03 PST -08:00,
   "updated_at"=>Fri, 10 Mar 2025 09:19:03 PST -08:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>nil,
   "next_assessment_at"=>nil,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "next_product_handle"=>nil,
   "cancel_at_end_of_period"=>nil,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Fri, 10 Mar 2025 09:19:03 PST -08:00,
   "previous_state"=>"pending",
   "signup_payment_id"=>0,
   "signup_revenue"=>"0.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>0,
   "product_price_in_cents"=>10000,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"4t9cnw",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>nil,
   "payer_id"=>64300587,
   "receives_invoice_emails"=>nil,
   "product_price_point_id"=>898846,
   "next_product_price_point_id"=>nil,
   "credit_balance_in_cents"=>0,
   "prepayment_balance_in_cents"=>0,
   "net_terms"=>nil,
   "stored_credential_transaction_id"=>nil,
   "locale"=>nil,
   "reference"=>nil,
   "currency"=>"USD",
   "on_hold_at"=>nil,
   "scheduled_cancellation_at"=>nil,
   "prepaid_dunning"=>false,
   "product_price_point_type"=>"default",
   "dunning_communication_delay_enabled"=>false,
   "dunning_communication_delay_time_zone"=>nil,
   "customer"=>
    {"id"=>64300587,
     "first_name"=>"Summer",
     "last_name"=>"Bayer",
     "organization"=>"Fay - Rowe",
     "email"=>"Carolyn.Gutmann@example.net",
     "created_at"=>Fri, 10 Mar 2025 09:18:29 PST -08:00,
     "updated_at"=>Fri, 10 Mar 2025 09:18:29 PST -08:00,
     "reference"=>nil,
     "address"=>"407 Bogisich Roads",
     "address_2"=>"Suite 406",
     "city"=>"New Daniella",
     "state"=>"IA",
     "state_name"=>"Iowa",
     "zip"=>"17933",
     "country"=>"US",
     "country_name"=>"United States",
     "phone"=>"964-327-5176",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>nil,
     "vat_number"=>nil,
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil,
     "locale"=>nil},
   "product"=>
    {"id"=>5209515,
     "name"=>"Bronze Plan",
     "handle"=>"bronze_plan",
     "description"=>"my description",
     "accounting_code"=>nil,
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>nil,
     "created_at"=>Fri, 13 Mar 2025 14:32:26 PDT -07:00,
     "updated_at"=>Fri, 13 Mar 2025 14:32:26 PDT -07:00,
     "price_in_cents"=>10000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>nil,
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>nil,
     "taxable"=>false,
     "update_return_url"=>nil,
     "tax_code"=>nil,
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>nil,
     "default_product_price_point_id"=>898846,
     "request_billing_address"=>false,
     "require_billing_address"=>false,
     "require_shipping_address"=>false,
     "use_site_exchange_rate"=>true,
     "item_category"=>nil,
     "product_price_point_id"=>898846,
     "product_price_point_name"=>"Original",
     "product_price_point_handle"=>"uuid:0edd1580-47a0-0138-6907-060451d6a6ae",
     "product_family"=>
      {"id"=>1513518,
       "name"=>"import test",
       "description"=>"",
       "handle"=>"import-test",
       "accounting_code"=>nil,
       "created_at"=>Wed, 11 Mar 2025 09:45:14 PDT -07:00,
       "updated_at"=>Wed, 11 Mar 2025 09:45:14 PDT -07:00},
     "public_signup_pages"=>[]},
   "credit_card"=>
    {"id"=>50327544,
     "first_name"=>"Gus",
     "last_name"=>"Terry",
     "masked_card_number"=>"XXXX-XXXX-XXXX-3",
     "card_type"=>"bogus",
     "expiration_month"=>3,
     "expiration_year"=>2024,
     "customer_id"=>64300587,
     "current_vault"=>nil,
     "vault_token"=>nil,
     "billing_address"=>"172 Abernathy Mountain",
     "billing_city"=>"North Lornachester",
     "billing_state"=>"CA",
     "billing_zip"=>"63011-5354",
     "billing_country"=>"US",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"Suite 227",
     "payment_type"=>"credit_card",
     "disabled"=>false,
     "site_gateway_setting_id"=>nil,
     "gateway_handle"=>nil},
   "prepaid_configuration"=>nil,
   "group"=>nil,
   "initial_billing_at"=>Sat, 01 Apr 2025 05:00:00 PDT -07:00},
 "site"=>{"id"=>60050, "subdomain"=>"chargify-sandbox-141"},
 "event_id"=>2950983304}
```

</Accordion>

</Accordion>

<Accordion title="delayed_subscription_creation_success">

**Trigger:** Successful creation of a delayed Subscription in an awaiting signup state.

**Payload Fields:** event_id, site, subscription
<Accordion title="Full payload example">

```json
{"subscription"=>
  {"id"=>63126383,
   "state"=>"awaiting_signup",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>nil,
   "created_at"=>Fri, 10 Mar 2025 09:16:28 PST -08:00,
   "updated_at"=>Fri, 10 Mar 2025 09:16:28 PST -08:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>Sat, 01 Apr 2025 05:00:00 PDT -07:00,
   "next_assessment_at"=>Sat, 01 Apr 2025 05:00:00 PDT -07:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "next_product_handle"=>nil,
   "cancel_at_end_of_period"=>nil,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Fri, 10 Mar 2025 09:16:28 PST -08:00,
   "previous_state"=>"awaiting_signup",
   "signup_payment_id"=>0,
   "signup_revenue"=>"0.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>0,
   "product_price_in_cents"=>10000,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"w376jg",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>nil,
   "payer_id"=>64300481,
   "receives_invoice_emails"=>nil,
   "product_price_point_id"=>898846,
   "next_product_price_point_id"=>nil,
   "credit_balance_in_cents"=>0,
   "prepayment_balance_in_cents"=>0,
   "net_terms"=>nil,
   "stored_credential_transaction_id"=>nil,
   "locale"=>nil,
   "reference"=>nil,
   "currency"=>"USD",
   "on_hold_at"=>nil,
   "scheduled_cancellation_at"=>nil,
   "prepaid_dunning"=>false,
   "product_price_point_type"=>"default",
   "dunning_communication_delay_enabled"=>false,
   "dunning_communication_delay_time_zone"=>nil,
   "customer"=>
    {"id"=>64300481,
     "first_name"=>"Leonie",
     "last_name"=>"Hickle",
     "organization"=>"McCullough, Bogisich and Bayer",
     "email"=>"Cortney.Gulgowski59@example.com",
     "created_at"=>Fri, 10 Mar 2025 09:15:24 PST -08:00,
     "updated_at"=>Fri, 10 Mar 2025 09:15:24 PST -08:00,
     "reference"=>nil,
     "address"=>"064 Gislason Hollow",
     "address_2"=>"Suite 099",
     "city"=>"Lindberg",
     "state"=>"NC",
     "state_name"=>"North Carolina",
     "zip"=>"39203-7738",
     "country"=>"US",
     "country_name"=>"United States",
     "phone"=>"308-857-8585",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>nil,
     "vat_number"=>nil,
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil,
     "locale"=>nil},
   "product"=>
    {"id"=>5209515,
     "name"=>"Bronze Plan",
     "handle"=>"bronze_plan",
     "description"=>"my description",
     "accounting_code"=>nil,
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>nil,
     "created_at"=>Fri, 13 Mar 2025 14:32:26 PDT -07:00,
     "updated_at"=>Fri, 13 Mar 2025 14:32:26 PDT -07:00,
     "price_in_cents"=>10000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>nil,
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>nil,
     "taxable"=>false,
     "update_return_url"=>nil,
     "tax_code"=>nil,
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>nil,
     "default_product_price_point_id"=>898846,
     "request_billing_address"=>false,
     "require_billing_address"=>false,
     "require_shipping_address"=>false,
     "use_site_exchange_rate"=>true,
     "item_category"=>nil,
     "product_price_point_id"=>898846,
     "product_price_point_name"=>"Original",
     "product_price_point_handle"=>"uuid:0edd1580-47a0-0138-6907-060451d6a6ae",
     "product_family"=>
      {"id"=>1513518,
       "name"=>"import test",
       "description"=>"",
       "handle"=>"import-test",
       "accounting_code"=>nil,
       "created_at"=>Wed, 11 Mar 2025 09:45:14 PDT -07:00,
       "updated_at"=>Wed, 11 Mar 2025 09:45:14 PDT -07:00},
     "public_signup_pages"=>[]},
   "credit_card"=>
    {"id"=>50327479,
     "first_name"=>"Romaine",
     "last_name"=>"Grimes",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
     "card_type"=>"visa",
     "expiration_month"=>3,
     "expiration_year"=>2024,
     "customer_id"=>64300481,
     "current_vault"=>"bogus",
     "vault_token"=>"1",
     "billing_address"=>"8193 Abner Greens",
     "billing_city"=>"Westland",
     "billing_state"=>"NM",
     "billing_zip"=>"48592",
     "billing_country"=>"US",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"Suite 544",
     "payment_type"=>"credit_card",
     "disabled"=>false,
     "site_gateway_setting_id"=>nil,
     "gateway_handle"=>nil},
   "prepaid_configuration"=>nil,
   "group"=>nil,
   "initial_billing_at"=>Sat, 01 Apr 2025 05:00:00 PDT -07:00,
   "referred_by"=>nil},
 "site"=>{"id"=>60050, "subdomain"=>"chargify-sandbox-141"},
 "event_id"=>2950981186}
```

</Accordion>
</Accordion>

<Accordion title="direct_debit_payment_paid_out">

**Trigger:** When Direct Debit Payment was successfully processed in the gateway (currently, only Stripe and GoCardless are supported).

**Payload Fields:** event_id, site, subscription, transaction

<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>102485,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Thu, 25 Feb 2021 03:33:01 EST -05:00,
   "created_at"=>Thu, 25 Feb 2021 03:32:54 EST -05:00,
   "updated_at"=>Thu, 25 Feb 2021 03:33:10 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>Thu, 25 Mar 2021 04:32:54 EDT -04:00,
   "next_assessment_at"=>Thu, 25 Mar 2021 04:32:54 EDT -04:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "next_product_handle"=>nil,
   "cancel_at_end_of_period"=>nil,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Thu, 25 Feb 2021 03:32:54 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>34793,
   "signup_revenue"=>"30,00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>3000,
   "product_price_in_cents"=>3000,
   "product_version_number"=>1,
   "payment_type"=>"bank_account",
   "referral_code"=>nil,
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>nil,
   "payer_id"=>nil,
   "receives_invoice_emails"=>nil,
   "product_price_point_id"=>466,
   "next_product_price_point_id"=>nil,
   "credit_balance_in_cents"=>0,
   "prepayment_balance_in_cents"=>0,
   "net_terms"=>nil,
   "stored_credential_transaction_id"=>nil,
   "locale"=>nil,
   "reference"=>nil,
   "currency"=>"EUR",
   "on_hold_at"=>nil,
   "scheduled_cancellation_at"=>nil,
   "customer"=>
    {"id"=>4260,
     "first_name"=>"Test",
     "last_name"=>"Test",
     "organization"=>"",
     "email"=>"test@example.com",
     "created_at"=>Thu, 25 Feb 2021 03:32:53 EST -05:00,
     "updated_at"=>Thu, 25 Feb 2021 03:32:53 EST -05:00,
     "reference"=>nil,
     "address"=>nil,
     "address_2"=>nil,
     "city"=>nil,
     "state"=>nil,
     "state_name"=>nil,
     "zip"=>nil,
     "country"=>nil,
     "country_name"=>nil,
     "phone"=>"",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>nil,
     "vat_number"=>nil,
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil,
     "locale"=>nil},
   "product"=>
    {"id"=>372,
     "name"=>"test",
     "handle"=>"test",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Wed, 09 Dec 2020 05:06:07 EST -05:00,
     "updated_at"=>Thu, 21 Jan 2021 09:28:12 EST -05:00,
     "price_in_cents"=>3000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"",
     "tax_code"=>"",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"",
     "default_product_price_point_id"=>466,
     "request_billing_address"=>false,
     "require_billing_address"=>false,
     "require_shipping_address"=>false,
     "product_price_point_id"=>466,
     "product_price_point_name"=>"Original",
     "product_price_point_handle"=>"uuid:14078c50-1c34-0139-f2b7-08e9fe639598",
     "product_family"=>
      {"id"=>131,
       "name"=>"Billing Plans",
       "description"=>nil,
       "handle"=>"test-billing-plans",
       "accounting_code"=>nil,
       "created_at"=>Wed, 09 Dec 2020 04:37:03 EST -05:00,
       "updated_at"=>Wed, 09 Dec 2020 04:37:03 EST -05:00},
     "public_signup_pages"=>
      [{"id"=>199,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://test-71.chargifypay.test/subscribe/3gx9xh5z8gj2/test"}]},
   "bank_account"=>
    {"id"=>5847,
     "first_name"=>"Test",
     "last_name"=>"Test",
     "customer_id"=>4260,
     "current_vault"=>"stripe_connect",
     "vault_token"=>"cus_J0gewnaw35SKLd",
     "billing_address"=>nil,
     "billing_city"=>nil,
     "billing_state"=>nil,
     "billing_zip"=>nil,
     "billing_country"=>nil,
     "customer_vault_token"=>nil,
     "billing_address_2"=>nil,
     "bank_name"=>nil,
     "masked_bank_routing_number"=>nil,
     "masked_bank_account_number"=>"XXXX3000",
     "bank_account_type"=>"checking",
     "bank_account_holder_type"=>"personal",
     "payment_type"=>"bank_account",
     "verified"=>true,
     "site_gateway_setting_id"=>402},
   "group"=>nil},
 "transaction"=>
  {"id"=>34793,
   "subscription_id"=>102485,
   "type"=>"Payment",
   "kind"=>nil,
   "transaction_type"=>"payment",
   "success"=>true,
   "amount_in_cents"=>3000,
   "memo"=>"Test Test - test: Signup payment",
   "created_at"=>Thu, 25 Feb 2021 03:33:00 EST -05:00,
   "starting_balance_in_cents"=>3000,
   "ending_balance_in_cents"=>0,
   "gateway_used"=>"stripe_connect",
   "gateway_transaction_id"=>"pi_1IOfHLKajOcZzbwkfsXCKj1P",
   "gateway_order_id"=>nil,
   "payment_id"=>nil,
   "product_id"=>372,
   "tax_id"=>nil,
   "component_id"=>nil,
   "statement_id"=>24750,
   "customer_id"=>4260,
   "item_name"=>nil,
   "period_range_start"=>nil,
   "period_range_end"=>nil,
   "currency"=>"EUR",
   "exchange_rate"=>1,
   "component_handle"=>nil,
   "component_price_point_id"=>nil,
   "component_price_point_handle"=>nil,
   "parent_id"=>nil,
   "role"=>nil,
   "card_number"=>nil,
   "card_expiration"=>nil,
   "card_type"=>nil,
   "refunded_amount_in_cents"=>0,
   "invoice_id"=>nil},
 "event_id"=>51601}
```

</Accordion>
</Accordion>

<Accordion title="direct_debit_payment_pending">

**Trigger:** When Direct Debit Payment was created in the gateway and is pending processing (currently, only Stripe and GoCardless are supported).

**Payload Fields:** event_id, site, subscription, transaction
<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>102485,
   "state"=>"pending",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>nil,
   "created_at"=>Thu, 25 Feb 2021 03:32:54 EST -05:00,
   "updated_at"=>Thu, 25 Feb 2021 03:33:00 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>nil,
   "next_assessment_at"=>nil,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "next_product_handle"=>nil,
   "cancel_at_end_of_period"=>nil,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Thu, 25 Feb 2021 03:32:54 EST -05:00,
   "previous_state"=>"pending",
   "signup_payment_id"=>34793,
   "signup_revenue"=>"30,00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>3000,
   "product_price_in_cents"=>3000,
   "product_version_number"=>1,
   "payment_type"=>"bank_account",
   "referral_code"=>nil,
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>nil,
   "payer_id"=>nil,
   "receives_invoice_emails"=>nil,
   "product_price_point_id"=>466,
   "next_product_price_point_id"=>nil,
   "credit_balance_in_cents"=>0,
   "prepayment_balance_in_cents"=>0,
   "net_terms"=>nil,
   "stored_credential_transaction_id"=>nil,
   "locale"=>nil,
   "reference"=>nil,
   "currency"=>"EUR",
   "on_hold_at"=>nil,
   "scheduled_cancellation_at"=>nil,
   "customer"=>
    {"id"=>4260,
     "first_name"=>"Test",
     "last_name"=>"Test",
     "organization"=>"",
     "email"=>"test@example.com",
     "created_at"=>Thu, 25 Feb 2021 03:32:53 EST -05:00,
     "updated_at"=>Thu, 25 Feb 2021 03:32:53 EST -05:00,
     "reference"=>nil,
     "address"=>nil,
     "address_2"=>nil,
     "city"=>nil,
     "state"=>nil,
     "state_name"=>nil,
     "zip"=>nil,
     "country"=>nil,
     "country_name"=>nil,
     "phone"=>"",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>nil,
     "vat_number"=>nil,
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil,
     "locale"=>nil},
   "product"=>
    {"id"=>372,
     "name"=>"test",
     "handle"=>"test",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Wed, 09 Dec 2020 05:06:07 EST -05:00,
     "updated_at"=>Thu, 21 Jan 2021 09:28:12 EST -05:00,
     "price_in_cents"=>3000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"",
     "tax_code"=>"",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"",
     "default_product_price_point_id"=>466,
     "request_billing_address"=>false,
     "require_billing_address"=>false,
     "require_shipping_address"=>false,
     "product_price_point_id"=>466,
     "product_price_point_name"=>"Original",
     "product_price_point_handle"=>"uuid:14078c50-1c34-0139-f2b7-08e9fe639598",
     "product_family"=>
      {"id"=>131,
       "name"=>"Billing Plans",
       "description"=>nil,
       "handle"=>"test-billing-plans",
       "accounting_code"=>nil,
       "created_at"=>Wed, 09 Dec 2020 04:37:03 EST -05:00,
       "updated_at"=>Wed, 09 Dec 2020 04:37:03 EST -05:00},
     "public_signup_pages"=>
      [{"id"=>199,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://test-71.chargifypay.test/subscribe/3gx9xh5z8gj2/test"}]},
   "bank_account"=>
    {"id"=>5847,
     "first_name"=>"Test",
     "last_name"=>"Test",
     "customer_id"=>4260,
     "current_vault"=>"stripe_connect",
     "vault_token"=>"cus_J0gewnaw35SKLd",
     "billing_address"=>nil,
     "billing_city"=>nil,
     "billing_state"=>nil,
     "billing_zip"=>nil,
     "billing_country"=>nil,
     "customer_vault_token"=>nil,
     "billing_address_2"=>nil,
     "bank_name"=>nil,
     "masked_bank_routing_number"=>nil,
     "masked_bank_account_number"=>"XXXX3000",
     "bank_account_type"=>"checking",
     "bank_account_holder_type"=>"personal",
     "payment_type"=>"bank_account",
     "verified"=>true,
     "site_gateway_setting_id"=>402},
   "group"=>nil},
 "transaction"=>
  {"id"=>34793,
   "subscription_id"=>102485,
   "type"=>"Payment",
   "kind"=>nil,
   "transaction_type"=>"payment",
   "success"=>true,
   "amount_in_cents"=>3000,
   "memo"=>"Test Test - test: Signup payment",
   "created_at"=>Thu, 25 Feb 2021 03:33:00 EST -05:00,
   "starting_balance_in_cents"=>3000,
   "ending_balance_in_cents"=>0,
   "gateway_used"=>"stripe_connect",
   "gateway_transaction_id"=>"pi_1IOfHLKajOcZzbwkfsXCKj1P",
   "gateway_order_id"=>nil,
   "payment_id"=>nil,
   "product_id"=>372,
   "tax_id"=>nil,
   "component_id"=>nil,
   "statement_id"=>24750,
   "customer_id"=>4260,
   "item_name"=>nil,
   "period_range_start"=>nil,
   "period_range_end"=>nil,
   "currency"=>"EUR",
   "exchange_rate"=>1,
   "component_handle"=>nil,
   "component_price_point_id"=>nil,
   "component_price_point_handle"=>nil,
   "parent_id"=>nil,
   "role"=>nil,
   "card_number"=>nil,
   "card_expiration"=>nil,
   "card_type"=>nil,
   "refunded_amount_in_cents"=>0,
   "invoice_id"=>nil},
 "event_id"=>51595}
```

</Accordion>

</Accordion>

<Accordion title="direct_debit_payment_rejected">

**Trigger:** When Direct Debit Payment was rejected in the gateway—for example, due to insufficient funds (currently, only Stripe and GoCardless are supported).

**Payload Fields:** event_id, site, subscription, transaction
<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>102493,
   "state"=>"past_due",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Thu, 25 Feb 2021 05:27:47 EST -05:00,
   "created_at"=>Thu, 25 Feb 2021 05:27:38 EST -05:00,
   "updated_at"=>Thu, 25 Feb 2021 05:28:06 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>3000,
   "current_period_ends_at"=>Thu, 25 Mar 2021 06:27:38 EDT -04:00,
   "next_assessment_at"=>Fri, 26 Feb 2021 05:28:06 EST -05:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "next_product_handle"=>nil,
   "cancel_at_end_of_period"=>nil,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Thu, 25 Feb 2021 05:27:38 EST -05:00,
   "previous_state"=>"past_due",
   "signup_payment_id"=>34807,
   "signup_revenue"=>"30,00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>0,
   "product_price_in_cents"=>3000,
   "product_version_number"=>1,
   "payment_type"=>"bank_account",
   "referral_code"=>nil,
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>nil,
   "payer_id"=>nil,
   "receives_invoice_emails"=>nil,
   "product_price_point_id"=>466,
   "next_product_price_point_id"=>nil,
   "credit_balance_in_cents"=>0,
   "prepayment_balance_in_cents"=>0,
   "net_terms"=>nil,
   "stored_credential_transaction_id"=>nil,
   "locale"=>nil,
   "reference"=>nil,
   "currency"=>"EUR",
   "on_hold_at"=>nil,
   "scheduled_cancellation_at"=>nil,
   "customer"=>
    {"id"=>4268,
     "first_name"=>"Test",
     "last_name"=>"Test",
     "organization"=>"",
     "email"=>"test@example.com",
     "created_at"=>Thu, 25 Feb 2021 05:27:38 EST -05:00,
     "updated_at"=>Thu, 25 Feb 2021 05:27:38 EST -05:00,
     "reference"=>nil,
     "address"=>nil,
     "address_2"=>nil,
     "city"=>nil,
     "state"=>nil,
     "state_name"=>nil,
     "zip"=>nil,
     "country"=>nil,
     "country_name"=>nil,
     "phone"=>"",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>nil,
     "vat_number"=>nil,
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil,
     "locale"=>nil},
   "product"=>
    {"id"=>372,
     "name"=>"test",
     "handle"=>"test",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Wed, 09 Dec 2020 05:06:07 EST -05:00,
     "updated_at"=>Thu, 21 Jan 2021 09:28:12 EST -05:00,
     "price_in_cents"=>3000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"",
     "tax_code"=>"",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"",
     "default_product_price_point_id"=>466,
     "request_billing_address"=>false,
     "require_billing_address"=>false,
     "require_shipping_address"=>false,
     "product_price_point_id"=>466,
     "product_price_point_name"=>"Original",
     "product_price_point_handle"=>"uuid:14078c50-1c34-0139-f2b7-08e9fe639598",
     "product_family"=>
      {"id"=>131,
       "name"=>"Billing Plans",
       "description"=>nil,
       "handle"=>"test-billing-plans",
       "accounting_code"=>nil,
       "created_at"=>Wed, 09 Dec 2020 04:37:03 EST -05:00,
       "updated_at"=>Wed, 09 Dec 2020 04:37:03 EST -05:00},
     "public_signup_pages"=>
      [{"id"=>199,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://test-71.chargifypay.test/subscribe/3gx9xh5z8gj2/test"}]},
   "bank_account"=>
    {"id"=>5855,
     "first_name"=>"Test",
     "last_name"=>"Test",
     "customer_id"=>4268,
     "current_vault"=>"stripe_connect",
     "vault_token"=>"cus_J0iV6HHEeYiAX4",
     "billing_address"=>nil,
     "billing_city"=>nil,
     "billing_state"=>nil,
     "billing_zip"=>nil,
     "billing_country"=>nil,
     "customer_vault_token"=>nil,
     "billing_address_2"=>nil,
     "bank_name"=>nil,
     "masked_bank_routing_number"=>nil,
     "masked_bank_account_number"=>"XXXX3001",
     "bank_account_type"=>"checking",
     "bank_account_holder_type"=>"personal",
     "payment_type"=>"bank_account",
     "verified"=>true,
     "site_gateway_setting_id"=>402},
   "group"=>nil},
 "transaction"=>
  {"id"=>34807,
   "subscription_id"=>102493,
   "type"=>"Payment",
   "kind"=>nil,
   "transaction_type"=>"payment",
   "success"=>false,
   "amount_in_cents"=>3000,
   "memo"=>"Test Test - test: Signup payment",
   "created_at"=>Thu, 25 Feb 2021 05:27:45 EST -05:00,
   "starting_balance_in_cents"=>3000,
   "ending_balance_in_cents"=>0,
   "gateway_used"=>"stripe_connect",
   "gateway_transaction_id"=>"pi_1IOh4NKajOcZzbwkC2xrThzC",
   "gateway_order_id"=>nil,
   "payment_id"=>nil,
   "product_id"=>372,
   "tax_id"=>nil,
   "component_id"=>nil,
   "statement_id"=>24757,
   "customer_id"=>4268,
   "item_name"=>nil,
   "period_range_start"=>nil,
   "period_range_end"=>nil,
   "currency"=>"EUR",
   "exchange_rate"=>1,
   "component_handle"=>nil,
   "component_price_point_id"=>nil,
   "component_price_point_handle"=>nil,
   "parent_id"=>nil,
   "role"=>nil,
   "card_number"=>nil,
   "card_expiration"=>nil,
   "card_type"=>nil,
   "refunded_amount_in_cents"=>0,
   "invoice_id"=>nil},
 "event_id"=>51655}
```

</Accordion>
</Accordion>

<Accordion title="dunning_step_reached">

**Trigger:** When a subscription reaches any step of the dunning process.

**Payload Fields:** event_id, site, subscription, product, dunner, current_step, next_step

<Accordion title="Full payload example">

```json
"subscription"  =>  {
    "id" => 37,
    "state" => "past_due",
    "trial_started_at" => Wed, 25 Jan 2025 15:48:56 EST -05:00,
    "trial_ended_at" => Wed, 25 Jan 2025 15:49:35 EST -05:00,
    "activated_at" => nil,
    "created_at" => Wed, 25 Jan 2025 15:48:56 EST -05:00,
    "updated_at" => Wed, 25 Jan 2025 15:49:40 EST -05:00,
    "expires_at" => nil,
    "balance_in_cents" => 2400,
    "current_period_ends_at" => Sat, 25 Feb 2025 15:49:35 EST -05:00,
    "next_assessment_at" => Wed, 25 Jan 2025 15:49:35 EST -05:00,
    "canceled_at" => nil,
    "cancellation_message" => nil,
    "next_product_id" => nil,
    "cancel_at_end_of_period" => false,
    "payment_collection_method" => "automatic",
    "snap_day" => nil,
    "cancellation_method" => nil,
    "current_period_started_at" => Wed, 25 Jan 2025 15:49:35 EST -05:00,
    "previous_state" => "past_due",
    "signup_payment_id" => 101,
    "signup_revenue" => "0.00",
    "delayed_cancel_at" => nil,
    "coupon_code" => nil,
    "total_revenue_in_cents" => 0,
    "product_price_in_cents" => 2400,
    "product_version_number" => 1,
    "payment_type" => nil,
    "referral_code" => nil,
    "coupon_use_count" => nil,
    "coupon_uses_allowed" => nil,
    "product_price_point_id"=>1,
    "next_product_price_point_id"=>nil,
    "customer" =>  {
      "id" => 37,
      "first_name" => "Ella",
      "last_name" => "Gertrude",
      "organization" => "",
      "email" => "ella@example.com",
      "created_at" => Wed, 25 Jan 2025 15:48:56 EST -05:00,
      "updated_at" => Wed, 25 Jan 2025 15:48:56 EST -05:00,
      "reference" => nil,
      "address" => "",
      "address_2" => "",
      "city" => "",
      "state" => "",
      "zip" => "",
      "country" => "",
      "phone" => "",
      "portal_invite_last_sent_at" => nil,
      "portal_invite_last_accepted_at" => nil,
      "verified" => false,
      "portal_customer_created_at" => nil,
      "vat_number"=>"123456789"
      "cc_emails" => nil,
      "tax_exempt" => false,
      "parent_id" => nil},
    "product" => {
      "id" => 24,
      "name" => "Basic",
      "handle" => "basic",
      "description" => "Magni porro sint iste illum.  Qui aut reiciendis voluptatum.  Est et aut.",
      "accounting_code" => nil,
      "request_credit_card" => true,
      "expiration_interval" => nil,
      "expiration_interval_unit" => nil,
      "created_at" => Mon, 23 Jan 2025 15:33:40 EST -05:00,
      "updated_at" => Mon, 23 Jan 2025 15:33:40 EST -05:00,
      "price_in_cents" => 2400,
      "interval" => 1,
      "interval_unit" => "month",
      "initial_charge_in_cents" => nil,
      "trial_price_in_cents" => 0,
      "trial_interval" => 1,
      "trial_interval_unit" => "month",
      "archived_at" => nil,
      "require_credit_card" => true,
      "return_params" => nil,
      "taxable" => false,
      "update_return_url"=>"http://www.example.com",
      "initial_charge_after_trial" => false,
      "version_number" => 1,
      "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
      "default_product_price_point_id"=>1,
      "product_price_point_id"=>1,
      "product_price_point_handle"=>nil,
      "product_family" => {
        "id" => 4,
        "name" => "Acme Online",
        "description" => nil,
        "handle" => "acme-online",
        "accounting_code" => nil},
      "public_signup_pages" => [ {
        "id" => 5,
        "return_url" => nil,
        "return_params" => nil,
        "url" => "http://acme-test.chargify.dev/subscribe/y884dzrgd9r9/basic"}]}},
  "dunner" => {
    "subscription_id" => 37,
    "state" => "active",
    "attempts" => 1,
    "last_attempted_at" => Wed, 25 Jan 2025 15:49:42 EST -05:00,
    "created_at" => Wed, 25 Jan 2025 15:49:42 EST -05:00,
    "revenue_at_risk_in_cents" => nil},
  "current_step" => {
    "day_threshold" => 1,
    "action" => "retry",
    "email_body" => "Hello ,\n\nWe just tried to process your current amount due of \nfor , but  there is no credit card on file for this subscription. \n\nPlease use the following link to quickly and easily update your billing information:\n\n\nWe will attempt to retry the card on file prior to cancellation. If we are unable to successfully charge the outstanding balance of , your account will be canceled in 27 days.\n\nThank you for choosing us!\n\nSincerely,\n\n\n",
    "email_subject" => "NOTICE: ",
    "send_email" => true,
    "send_bcc_email" => false,
    "send_sms" => false,
    "sms_body" => nil},
  "next_step" => {
    "day_threshold" => 28,
    "action" => "cancel",
    "email_body" => "Hello ,\n\n\nWe've made several attempts to bill you for your  account but you have not yet provided us with a valid credit card profile.\n\nAs a result, your account has been canceled.\n\nIf you did not intend to cancel your account, please contact us to reactivate your account and avoid further interruption of your service.\n\nThank you for choosing us!\n\nSincerely,\n\n\n",
    "email_subject" => "NOTICE:  Account Canceled",
    "send_email" => true,
    "send_bcc_email" => false,
    "send_sms" => false,
    "sms_body" => nil},
  "event_id" => 165}
```

</Accordion>

</Accordion>

<Accordion title="expiration_date_change">

**Trigger:** Any change to an existing `expiration_date` for a subscription.

**Payload Fields:** event_id, site, subscription
<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>16945349,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Fri, 03 Mar 2025 13:00:08 EST -05:00,
   "created_at"=>Fri, 03 Mar 2025 13:00:02 EST -05:00,
   "updated_at"=>Fri, 03 Mar 2025 13:03:55 EST -05:00,
   "expires_at"=>Sun, 15 Oct 2025 00:00:00 EDT -04:00,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>Sun, 05 Mar 2025 13:00:02 EST -05:00,
   "next_assessment_at"=>Sat, 04 Mar 2025 13:00:02 EST -05:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Fri, 03 Mar 2025 13:00:02 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>176501894,
   "signup_revenue"=>"1.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>100,
   "product_price_in_cents"=>100,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"968js9",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "reason_code"=>nil,
   "customer"=>
    {"id"=>16428076,
     "first_name"=>"Expiring ",
     "last_name"=>"Subscription",
     "organization"=>"",
     "email"=>"expiring@example.com",
     "created_at"=>Fri, 03 Mar 2025 13:00:02 EST -05:00,
     "updated_at"=>Fri, 03 Mar 2025 13:00:02 EST -05:00,
     "reference"=>nil,
     "address"=>"",
     "address_2"=>"",
     "city"=>"",
     "state"=>"",
     "zip"=>"",
     "country"=>"",
     "phone"=>"",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>nil,
     "portal_customer_created_at"=>nil,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil},
   "product"=>
    {"id"=>4464208,
     "name"=>"Product that expires",
     "handle"=>"product-that-expires",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>1,
     "expiration_interval_unit"=>"day",
     "created_at"=>Fri, 03 Mar 2025 12:02:38 EST -05:00,
     "updated_at"=>Fri, 03 Mar 2025 12:02:38 EST -05:00,
     "price_in_cents"=>100,
     "interval"=>2,
     "interval_unit"=>"day",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>314126,
        "return_url"=>nil,
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/zs8352q9qtyz/product-that-expires"}]},
   "credit_card"=>
    {"id"=>11380641,
     "first_name"=>"Expiring ",
     "last_name"=>"Subscription",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1",
     "card_type"=>"bogus",
     "expiration_month"=>1,
     "expiration_year"=>2027,
     "customer_id"=>16428076,
     "current_vault"=>"bogus",
     "vault_token"=>"1",
     "billing_address"=>"",
     "billing_city"=>"",
     "billing_state"=>"",
     "billing_zip"=>"",
     "billing_country"=>"",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"",
     "payment_type"=>"credit_card"},
   "previous_expires_at"=>Wed, 15 Jul 2015 00:00:00 EDT -04:00},
 "event_id"=>384446169}
```

</Accordion>
</Accordion>

<Accordion title="expiring_card">

**Trigger:** A periodic event sent by Advanced Billing on the 1st, 15th, and 7 days before the end of the month. These webhooks identify cards expiring in the current month.

**Payload Fields:** event_id, site, subscription
The subscription object also contains information on the Customer and Product. The expiring_card webhook is sent on the 1st, 15th and 7 days before the end of the month. This will identify all cards expiring in the current month.
<Accordion title="Full payload example">

```json
{"id"=>16474328,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Mon, 20 Feb 2025 18:28:48 EST -05:00,
   "created_at"=>Mon, 20 Feb 2025 18:28:47 EST -05:00,
   "updated_at"=>Mon, 20 Feb 2025 18:32:14 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>Mon, 20 Mar 2025 19:28:47 EDT -04:00,
   "next_assessment_at"=>Mon, 20 Mar 2025 19:28:47 EDT -04:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 20 Feb 2025 18:28:47 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>174876340,
   "signup_revenue"=>"0.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>0,
   "product_price_in_cents"=>0,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"ww6mq5",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "customer"=>
    {"id"=>15102905,
     "first_name"=>"Amelia",
     "last_name"=>"Example",
     "organization"=>"",
     "email"=>"amelia@example.com",
     "created_at"=>Thu, 15 Dec 2024 09:33:15 EST -05:00,
     "updated_at"=>Thu, 15 Dec 2024 09:34:09 EST -05:00,
     "reference"=>nil,
     "address"=>"",
     "address_2"=>"",
     "city"=>"",
     "state"=>"",
     "zip"=>"",
     "country"=>"",
     "phone"=>"",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>nil,
     "portal_customer_created_at"=>Thu, 15 Dec 2024 09:33:21 EST -05:00,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false},
   "product"=>
    {"id"=>4461042,
     "name"=>"Gold Product",
     "handle"=>"gold-product",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Mon, 20 Feb 2025 18:28:18 EST -05:00,
     "updated_at"=>Mon, 20 Feb 2025 18:28:18 EST -05:00,
     "price_in_cents"=>0,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>312984,
        "return_url"=>nil,
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/yt4ybcgk7998/zero-dollar-product"}]},
   "credit_card"=>
    {"id"=>10548239,
     "first_name"=>"Amelia",
     "last_name"=>"Example",
     "masked_card_number"=>"XXXX-XXXX-XXXX-2",
     "card_type"=>"bogus",
     "expiration_month"=>2,
     "expiration_year"=>2025,
     "customer_id"=>15102905,
     "current_vault"=>"bogus",
     "vault_token"=>"2",
     "billing_address"=>"",
     "billing_city"=>"",
     "billing_state"=>"",
     "billing_zip"=>"",
     "billing_country"=>"",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"",
     "payment_type"=>"credit_card"}},
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "event_id"=>380080310}
```

</Accordion>
</Accordion>

<Accordion title="invoice_issued">

**Trigger:** Invoices issued towards a subscription on Relationship Invoicing site.

**Payload Fields:** event_id, site, subscription, invoice
The subscription object also contains information on the Customer and Product.
<Accordion title="Full payload example">

```json
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "subscription"=>
  {"id"=>28224225,
   "current_period_ends_at"=>Wed, 31 Jul 2025 15:25:54 CDT -05:00},
 "invoice"=>
  {"uid"=>"inv_4ffwqn13mppt7"
   "role"=>"renewal",
   "due_date"=>"2025-07-31",
   "issue_date"=>"2025-07-31",
   "paid_date"=>"",
   "due_amount"=>"$3,000.00",
   "paid_amount"=>"$0.00",
   "refund_amount"=>"$0.00",
   "tax_amount"=>"$0.00",
   "total_amount"=>"$3,000.00",
   "status_amount"=>"$3,000.00",
   "product_name"=>"2000",
   "line_items"=>
    {"0"=>
      {"uid"=>"li_8tjw5rjzyt287",
       "title"=>"2000",
       "description"=>"31 Jul 2025 - 31 Aug 2025",
       "quantity"=>1,
       "unit_price"=>"$3,000.00",
       "period_range_start"=>"2025-07-31",
       "period_range_end"=>"2025-08-31",
       "amount"=>"$3,000.00",
       "line_references"=>"",
       "pricing_details_index"=>nil,
       "pricing_details"=>{},
       "tax_code"=>nil,
       "tax_amount"=>"0.0"}}},
 "event_id"=>854499834}
```

</Accordion>
</Accordion>

<Accordion title="invoice_pending">

**Trigger:** Any time an invoice transitions into the `pending` state.

**Payload Fields:** event_id, site, invoice, customer, subscription (if applicable), previous_state, new_state, timestamp
This event follows the standard webhook behavior for event subscriptions, signed payload delivery, retries, and replay.
<Accordion title="Full payload example with subscription">

```json
"site"=>{"id"=>31111, "subdomain"=>"general-goods"},
"invoice"=>
 {"id"=>76439765,
  "uid"=>"inv_4fffff13mppt5",
  "status"=>"pending"},
"customer"=>
 {"id"=>15822222,
  "reference"=>"cust-000123"},
"subscription"=>
 {"id"=>22224225},
"previous_state"=>"draft",
"new_state"=>"pending",
"timestamp"=>"2025-07-31T20:45:00Z",
"event_id"=>854500001}
```

</Accordion>
<Accordion title="Full payload example without subscription">

```json
"site"=>{"id"=>31111, "subdomain"=>"general-goods"},
"invoice"=>
 {"id"=>76222,
  "uid"=>"inv_4fffff16mppt6",
  "status"=>"pending"},
"customer"=>
 {"id"=>15822222,
  "reference"=>"cust-000123"},
"subscription"=>nil,
"previous_state"=>"open",
"new_state"=>"pending",
"timestamp"=>"2025-07-31T21:10:00Z",
"event_id"=>854500000}
```

</Accordion>
</Accordion>

<Accordion title="metered_usage">

**Trigger:** Any reported usage for a Subscription’s metered components. This webhook will not fire when the unit balance is reset to `0` at renewal. `timestamp` is in ISO8601 format in UTC.

**Payload Fields:** event_id, site, component, subscription, product, previous_unit_balance, new_unit_balance, usage_quantity, memo, timestamp
<Accordion title="Full payload example">

```json
"component"=>
  {"id"=>375042,
   "kind"=>"metered_component",
   "name"=>"Test Metered",
   "unit_name"=>"Test Metered"},
 "subscription"=>{"id"=>16372192, "name"=>"Doris Tester"},
 "product"=>{"id"=>4443536, "name"=>"Business Monthly"},
 "memo"=>"Recording Metered Component Usage",
 "timestamp"=>"2025-02-13T18:50:23Z",
 "previous_unit_balance"=>#<BigDecimal:7f097c4b7d28,'0.0',9(18)>,
 "new_unit_balance"=>1000,
 "usage_quantity"=>1000,
 "event_id"=>377609676}
```

</Accordion>
</Accordion>

<Accordion title="payment_failure">

**Trigger:** Any failed payment attempt.payment_success or payment_failure are triggered for every payment attempted, whether it is for a normal renewal, a One-time Charge, a retry after failure, or a payment applied to an Invoice. Note that in some cases the payment may fail later, most commonly with ACH/eCheck and Direct Debit.

**Payload Fields:** event_id, site, subscription, transaction
<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>16090357,
   "state"=>"past_due",
   "trial_started_at"=>Mon, 23 Jan 2025 09:59:26 EST -05:00,
   "trial_ended_at"=>Mon, 23 Jan 2025 09:59:49 EST -05:00,
   "activated_at"=>nil,
   "created_at"=>Mon, 23 Jan 2025 09:59:26 EST -05:00,
   "updated_at"=>Mon, 30 Jan 2025 12:07:40 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>1000,
   "current_period_ends_at"=>Thu, 23 Feb 2025 09:59:49 EST -05:00,
   "next_assessment_at"=>Tue, 31 Jan 2025 12:07:40 EST -05:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 23 Jan 2025 09:59:49 EST -05:00,
   "previous_state"=>"past_due",
   "signup_payment_id"=>171203419,
   "signup_revenue"=>"0.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>0,
   "product_price_in_cents"=>1000,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"4nvrr2",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "currency"=>"USD",
   "customer"=>
    {"id"=>15547334,
     "first_name"=>"No obligation",
     "last_name"=>"Bad Card",
     "organization"=>"",
     "email"=>"nobbad@example.com",
     "created_at"=>Mon, 23 Jan 2025 09:59:26 EST -05:00,
     "updated_at"=>Mon, 23 Jan 2025 09:59:28 EST -05:00,
     "reference"=>nil,
     "address"=>"",
     "address_2"=>"",
     "city"=>"",
     "state"=>"",
     "zip"=>"",
     "country"=>"",
     "phone"=>"",
     "portal_invite_last_sent_at"=>Mon, 23 Jan 2025 09:59:28 EST -05:00,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>nil,
     "portal_customer_created_at"=>Mon, 23 Jan 2025 09:59:28 EST -05:00,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil},
   "product"=>
    {"id"=>4453830,
     "name"=>"Trial No obligation",
     "handle"=>"trial-no-obligation",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Mon, 23 Jan 2025 09:53:23 EST -05:00,
     "updated_at"=>Thu, 26 Jan 2025 13:35:29 EST -05:00,
     "price_in_cents"=>1000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>0,
     "trial_interval"=>1,
     "trial_interval_unit"=>"day",
     "archived_at"=>nil,
     "require_credit_card"=>false,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>310559,
        "return_url"=>nil,
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/ngbsvxv4hq7q/trial-no-obligation"}]},
   "credit_card"=>
    {"id"=>10734525,
     "first_name"=>"Chester",
     "last_name"=>"Tester",
     "masked_card_number"=>"XXXX-XXXX-XXXX-2",
     "card_type"=>"bogus",
     "expiration_month"=>1,
     "expiration_year"=>2027,
     "customer_id"=>15547334,
     "current_vault"=>"bogus",
     "vault_token"=>"2",
     "billing_address"=>"",
     "billing_city"=>"",
     "billing_state"=>"",
     "billing_zip"=>"",
     "billing_country"=>"",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"",
     "payment_type"=>"credit_card"}},
 "transaction"=>
  {"id"=>172084191,
   "subscription_id"=>16090357,
   "type"=>"Payment",
   "kind"=>nil,
   "transaction_type"=>"payment",
   "success"=>false,
   "amount_in_cents"=>1000,
   "memo"=>"Bogus Gateway: Forced failure",
   "created_at"=>Mon, 30 Jan 2025 12:07:39 EST -05:00,
   "starting_balance_in_cents"=>1000,
   "ending_balance_in_cents"=>1000,
   "gateway_used"=>"bogus",
   "gateway_transaction_id"=>nil,
   "gateway_order_id"=>nil,
   "payment_id"=>nil,
   "product_id"=>4453830,
   "tax_id"=>nil,
   "component_id"=>nil,
   "statement_id"=>84491989,
   "customer_id"=>15547334,
   "card_number"=>"XXXX-XXXX-XXXX-2",
   "card_expiration"=>"01/2027",
   "card_type"=>"bogus",
   "refunded_amount_in_cents"=>0,
   "invoice_uids"=>["inv_9hchn3xc84mgm"],
   "invoice_id"=>nil,
   "currency"=>"USD"},
 "event_id"=>372846073}
```

</Accordion>
</Accordion>

<Accordion title="payment_success">

**Trigger:** Any payment attempt that does not result in an immediate failure.payment_success or payment_failure are triggered for every payment attempted, whether it is for a normal renewal, a One-time Charge, a retry after failure, or a payment applied to an Invoice. Note that in some cases the payment may fail later, most commonly with ACH/eCheck and Direct Debit.

**Payload Fields:** event_id, site, subscription, transaction
<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>16327080,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Thu, 09 Feb 2025 11:42:28 EST -05:00,
   "created_at"=>Thu, 09 Feb 2025 11:42:26 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 12:03:41 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>Wed, 08 Mar 2025 12:00:00 EST -05:00,
   "next_assessment_at"=>Wed, 08 Mar 2025 12:00:00 EST -05:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>"8",
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 13 Feb 2025 12:00:00 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>0,
   "signup_revenue"=>"0.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>5667,
   "product_price_in_cents"=>6900,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"6zcsxw",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "payer_id"=>15780191,
   "currency"=>"USD",
   "customer"=>
    {"id"=>15780191,
     "first_name"=>"Test "
     "last_name"=>"Test",
     "organization"=>"",
     "email"=>"test@example.com",
     "created_at"=>Thu, 09 Feb 2025 11:42:26 EST -05:00,
     "updated_at"=>Thu, 09 Feb 2025 11:42:29 EST -05:00,
     "reference"=>nil,
     "address"=>nil,
     "address_2"=>nil,
     "city"=>nil,
     "state"=>nil,
     "zip"=>nil,
     "country"=>nil,
     "phone"=>"",
     "portal_invite_last_sent_at"=>Thu, 09 Feb 2025 11:42:29 EST -05:00,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>Thu, 09 Feb 2025 11:42:29 EST -05:00,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil},
   "product"=>
    {"id"=>4458204,
     "name"=>"Monthly product ",
     "handle"=>"monthly-product",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Wed, 08 Feb 2025 16:48:54 EST -05:00,
     "updated_at"=>Wed, 08 Feb 2025 16:48:54 EST -05:00,
     "price_in_cents"=>6900,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>312067,
        "return_url"=>nil,
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/5x8sbq29vgkn/monthly-product"},
       {"id"=>312068,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/vsmzkwvmfqqx/monthly-product"}]},
   "credit_card"=>
    {"id"=>10881632,
     "first_name"=>"Test",
     "last_name"=>"Page",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1",
     "card_type"=>"bogus",
     "expiration_month"=>1,
     "expiration_year"=>2027,
     "customer_id"=>15780191,
     "current_vault"=>"bogus",
     "vault_token"=>"1",
     "billing_address"=>nil,
     "billing_city"=>nil,
     "billing_state"=>nil,
     "billing_zip"=>"",
     "billing_country"=>nil,
     "customer_vault_token"=>nil,
     "billing_address_2"=>nil,
     "payment_type"=>"credit_card"}},
 "transaction"=>
  {"id"=>173963435,
   "subscription_id"=>16327080,
   "type"=>"Payment",
   "kind"=>nil,
   "transaction_type"=>"payment",
   "success"=>true,
   "amount_in_cents"=>5667,
   "memo"=>"Test Monthly - Monthly product : Renewal payment",
   "created_at"=>Mon, 13 Feb 2025 12:03:40 EST -05:00,
   "starting_balance_in_cents"=>5667,
   "ending_balance_in_cents"=>0,
   "gateway_used"=>"bogus",
   "gateway_transaction_id"=>"53433",
   "gateway_order_id"=>nil,
   "payment_id"=>nil,
   "product_id"=>4458204,
   "tax_id"=>nil,
   "component_id"=>nil,
   "statement_id"=>85701746,
   "customer_id"=>15780191,
   "card_number"=>"XXXX-XXXX-XXXX-1",
   "card_expiration"=>"01/2027",
   "card_type"=>"bogus",
   "refunded_amount_in_cents"=>0,
   "invoice_uids"=>["inv_9hchn3xc84mgm"],
   "invoice_id"=>nil,
   "currency"=>"USD"},
 "event_id"=>377575306}
```

Note that if you are using Authorize.Net for your payment gateway, you will receive extra payload information in the form of gateway_response and approval_code:

```json
{
   "site": { ... },
   "subscription": { ... },
   "transaction": { ... },
   "gateway_response": {
      "approval_code": "AB1CDO"
   },
   "event_id": 1
}
```

</Accordion>

</Accordion>

<Accordion title="pending_cancellation_change">

**Trigger:** When a subscription is canceled with delay (cancel at end of period) or when pending cancellation is cleared.

**Payload Fields:** event_id, site, subscription, cancellation_state, cancels_at
The subscription object also contains information on the Customer and Product.
<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>13,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Sat, 01 Feb 2020 10:38:50 EST -05:00,
   "created_at"=>Sat, 01 Feb 2020 10:38:49 EST -05:00,
   "updated_at"=>Fri, 10 Apr 2020 04:22:55 EDT -04:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>Thu, 07 May 2020 04:33:39 EDT -04:00,
   "next_assessment_at"=>Thu, 07 May 2020 04:33:39 EDT -04:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "next_product_handle"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Tue, 07 Apr 2020 04:33:39 EDT -04:00,
   "previous_state"=>"canceled",
   "signup_payment_id"=>0,
   "signup_revenue"=>"0.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>31918,
   "product_price_in_cents"=>29900,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>nil,
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>nil,
   "payer_id"=>8,
   "receives_invoice_emails"=>nil,
   "product_price_point_id"=>114,
   "next_product_price_point_id"=>nil,
   "credit_balance_in_cents"=>0,
   "prepayment_balance_in_cents"=>0,
   "net_terms"=>2,
   "stored_credential_transaction_id"=>nil,
   "locale"=>nil,
   "reference"=>nil,
   "currency"=>"USD",
   "customer"=>
    {"id"=>8,
     "first_name"=>"Marty",
     "last_name"=>"McFly",
     "organization"=>nil,
     "email"=>"timetraveller1985@example.com",
     "created_at"=>Sat, 01 Feb 2020 10:40:43 EST -05:00,
     "updated_at"=>Fri, 03 Apr 2020 05:58:47 EDT -04:00,
     "reference"=>nil,
     "address"=>"100 Shipping St.",
     "address_2"=>"Apt 200",
     "city"=>"Pleasantville",
     "state"=>"NC",
     "zip"=>"12345",
     "country"=>"US",
     "phone"=>nil,
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>nil,
     "vat_number"=>nil,
     "cc_emails"=>"john@example.com, sue@example.com",
     "tax_exempt"=>false,
     "parent_id"=>nil,
     "locale"=>nil},
   "product"=>
    {"id"=>114,
     "name"=>"Professional Plan",
     "handle"=>"server-professional",
     "description"=>
      "Voluptatem et quod delectus ut.  Reiciendis repudiandae nemo et doloribus.  Maxime velit ut.",
     "accounting_code"=>nil,
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>nil,
     "created_at"=>Sat, 01 Feb 2020 10:40:43 EST -05:00,
     "updated_at"=>Sat, 01 Feb 2020 10:40:43 EST -05:00,
     "price_in_cents"=>29900,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>nil,
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>nil,
     "taxable"=>true,
     "update_return_url"=>nil,
     "tax_code"=>nil,
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>nil,
     "default_product_price_point_id"=>114,
     "product_price_point_id"=>114,
     "product_price_point_name"=>"Default",
     "product_price_point_handle"=>"uuid:213b0f81-2737-0138-4a11-2cde48001122",
     "product_family"=>
      {"id"=>29,
       "name"=>"Cloud Compute Servers",
       "description"=>nil,
       "handle"=>"cloud-compute-servers",
       "accounting_code"=>nil},
     "public_signup_pages"=>[]},
   "credit_card"=>
    {"id"=>8,
     "first_name"=>"Marty",
     "last_name"=>"McFly",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
     "card_type"=>"visa",
     "expiration_month"=>12,
     "expiration_year"=>2022,
     "customer_id"=>8,
     "current_vault"=>"",
     "vault_token"=>"1",
     "billing_address"=>"200 Billing Rd.",
     "billing_city"=>"Needham",
     "billing_state"=>"MA",
     "billing_zip"=>"02494",
     "billing_country"=>"US",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"Suite 100",
     "payment_type"=>"credit_card",
     "disabled"=>false}},
 "cancellation_state"=>"cleared",
 "cancels_at"=>nil,
 "event_id"=>315}
```

</Accordion>
</Accordion>

<Accordion title="pending_payment_created">

**Trigger:** When a Pending Payment was created in the gateway and is pending processing.

**Payload Fields:** event_id, site, subscription, transaction
<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>784,
   "state"=>"pending",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>nil,
   "created_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
   "updated_at"=>Wed, 18 Aug 2021 05:58:43 EDT -04:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>nil,
   "next_assessment_at"=>nil,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "next_product_handle"=>nil,
   "cancel_at_end_of_period"=>nil,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
   "previous_state"=>"pending",
   "signup_payment_id"=>3556,
   "signup_revenue"=>"500.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>50000,
   "product_price_in_cents"=>50000,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>nil,
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>nil,
   "payer_id"=>nil,
   "receives_invoice_emails"=>nil,
   "product_price_point_id"=>168,
   "next_product_price_point_id"=>nil,
   "credit_balance_in_cents"=>0,
   "prepayment_balance_in_cents"=>0,
   "net_terms"=>nil,
   "stored_credential_transaction_id"=>nil,
   "locale"=>nil,
   "reference"=>nil,
   "currency"=>"USD",
   "on_hold_at"=>nil,
   "scheduled_cancellation_at"=>nil,
   "customer"=>
    {"id"=>708,
     "first_name"=>"John",
     "last_name"=>"Doe",
     "organization"=>"",
     "email"=>"test@example.com",
     "created_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
     "updated_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
     "reference"=>nil,
     "address"=>"Street",
     "address_2"=>"",
     "city"=>"San Francisco",
     "state"=>"CA",
     "state_name"=>"California",
     "zip"=>"66785",
     "country"=>"US",
     "country_name"=>"United States",
     "phone"=>"123456789",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>nil,
     "vat_number"=>nil,
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil,
     "locale"=>nil},
   "product"=>
    {"id"=>154,
     "name"=>"golder",
     "handle"=>"golderrr",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Mon, 05 Jul 2021 08:32:52 EDT -04:00,
     "updated_at"=>Tue, 20 Jul 2021 08:08:49 EDT -04:00,
     "price_in_cents"=>50000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>true,
     "update_return_url"=>"",
     "tax_code"=>"4512.100",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"",
     "default_product_price_point_id"=>168,
     "request_billing_address"=>true,
     "require_billing_address"=>true,
     "require_shipping_address"=>true,
     "product_price_point_id"=>168,
     "product_price_point_name"=>"Original",
     "product_price_point_handle"=>"uuid:0a6d2640-bfbb-0139-c94b-2cde48001122",
     "product_family"=>
      {"id"=>65,
       "name"=>"Billing Plans",
       "description"=>nil,
       "handle"=>"chargify-billing-plans",
       "accounting_code"=>nil,
       "created_at"=>Mon, 21 Jun 2021 08:22:16 EDT -04:00,
       "updated_at"=>Mon, 21 Jun 2021 08:22:16 EDT -04:00},
     "public_signup_pages"=>
      [{"id"=>101,
        "return_url"=>nil,
        "return_params"=>"",
        "url"=>
         "https://some-subdomain.chargifypay.test/subscribe/nmxwp8gsjh3t/golderrr"}]},
   "credit_card"=>
    {"id"=>885,
     "first_name"=>"John",
     "last_name"=>"Doe",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
     "card_type"=>"visa",
     "expiration_month"=>1,
     "expiration_year"=>2029,
     "customer_id"=>708,
     "current_vault"=>"digital_river",
     "vault_token"=>"b0aeda54-7576-4457-a36b-e775d2730df8",
     "billing_address"=>"Street",
     "billing_city"=>"San Francisco",
     "billing_state"=>"CA",
     "billing_zip"=>"66785",
     "billing_country"=>"US",
     "customer_vault_token"=>"541159940336",
     "billing_address_2"=>"",
     "payment_type"=>"credit_card",
     "disabled"=>false,
     "site_gateway_setting_id"=>71},
   "group"=>nil},
 "transaction"=>
  {"id"=>3556,
   "subscription_id"=>784,
   "type"=>"Payment",
   "kind"=>nil,
   "transaction_type"=>"payment",
   "success"=>true,
   "amount_in_cents"=>50000,
   "memo"=>"John Doe - golder: Signup payment",
   "created_at"=>Wed, 18 Aug 2021 05:58:43 EDT -04:00,
   "starting_balance_in_cents"=>50000,
   "ending_balance_in_cents"=>0,
   "gateway_used"=>"digital_river",
   "gateway_transaction_id"=>"3daa894a-77e5-4bd9-88b9-8e6dd1e84185",
   "gateway_order_id"=>nil,
   "payment_id"=>nil,
   "product_id"=>154,
   "tax_id"=>nil,
   "component_id"=>nil,
   "statement_id"=>1553,
   "customer_id"=>708,
   "item_name"=>nil,
   "period_range_start"=>nil,
   "period_range_end"=>nil,
   "currency"=>"USD",
   "exchange_rate"=>1,
   "component_handle"=>nil,
   "component_price_point_id"=>nil,
   "component_price_point_handle"=>nil,
   "parent_id"=>nil,
   "role"=>nil,
   "card_number"=>"XXXX-XXXX-XXXX-1111",
   "card_expiration"=>"01/2029",
   "card_type"=>"visa",
   "refunded_amount_in_cents"=>0,
   "invoice_id"=>nil},
 "event_id"=>5432}
```

</Accordion>
</Accordion>

<Accordion title="pending_payment_completed">

**Trigger:** When a Pending Payment was successfully processed in the gateway.

**Payload Fields:** event_id, site, subscription, transaction
<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>784,
   "state"=>"pending",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>nil,
   "created_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
   "updated_at"=>Wed, 18 Aug 2021 05:58:43 EDT -04:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>nil,
   "next_assessment_at"=>nil,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "next_product_handle"=>nil,
   "cancel_at_end_of_period"=>nil,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
   "previous_state"=>"pending",
   "signup_payment_id"=>3556,
   "signup_revenue"=>"500.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>50000,
   "product_price_in_cents"=>50000,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>nil,
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>nil,
   "payer_id"=>nil,
   "receives_invoice_emails"=>nil,
   "product_price_point_id"=>168,
   "next_product_price_point_id"=>nil,
   "credit_balance_in_cents"=>0,
   "prepayment_balance_in_cents"=>0,
   "net_terms"=>nil,
   "stored_credential_transaction_id"=>nil,
   "locale"=>nil,
   "reference"=>nil,
   "currency"=>"USD",
   "on_hold_at"=>nil,
   "scheduled_cancellation_at"=>nil,
   "customer"=>
    {"id"=>708,
     "first_name"=>"John",
     "last_name"=>"Doe",
     "organization"=>"",
     "email"=>"test@example.com",
     "created_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
     "updated_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
     "reference"=>nil,
     "address"=>"Street",
     "address_2"=>"",
     "city"=>"San Francisco",
     "state"=>"CA",
     "state_name"=>"California",
     "zip"=>"66785",
     "country"=>"US",
     "country_name"=>"United States",
     "phone"=>"123456789",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>nil,
     "vat_number"=>nil,
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil,
     "locale"=>nil},
   "product"=>
    {"id"=>154,
     "name"=>"golder",
     "handle"=>"golderrr",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Mon, 05 Jul 2021 08:32:52 EDT -04:00,
     "updated_at"=>Tue, 20 Jul 2021 08:08:49 EDT -04:00,
     "price_in_cents"=>50000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>true,
     "update_return_url"=>"",
     "tax_code"=>"4512.100",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"",
     "default_product_price_point_id"=>168,
     "request_billing_address"=>true,
     "require_billing_address"=>true,
     "require_shipping_address"=>true,
     "product_price_point_id"=>168,
     "product_price_point_name"=>"Original",
     "product_price_point_handle"=>"uuid:0a6d2640-bfbb-0139-c94b-2cde48001122",
     "product_family"=>
      {"id"=>65,
       "name"=>"Billing Plans",
       "description"=>nil,
       "handle"=>"chargify-billing-plans",
       "accounting_code"=>nil,
       "created_at"=>Mon, 21 Jun 2021 08:22:16 EDT -04:00,
       "updated_at"=>Mon, 21 Jun 2021 08:22:16 EDT -04:00},
     "public_signup_pages"=>
      [{"id"=>101,
        "return_url"=>nil,
        "return_params"=>"",
        "url"=>
         "https://some-subdomain.chargifypay.test/subscribe/nmxwp8gsjh3t/golderrr"}]},
   "credit_card"=>
    {"id"=>885,
     "first_name"=>"John",
     "last_name"=>"Doe",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
     "card_type"=>"visa",
     "expiration_month"=>1,
     "expiration_year"=>2029,
     "customer_id"=>708,
     "current_vault"=>"digital_river",
     "vault_token"=>"b0aeda54-7576-4457-a36b-e775d2730df8",
     "billing_address"=>"Street",
     "billing_city"=>"San Francisco",
     "billing_state"=>"CA",
     "billing_zip"=>"66785",
     "billing_country"=>"US",
     "customer_vault_token"=>"541159940336",
     "billing_address_2"=>"",
     "payment_type"=>"credit_card",
     "disabled"=>false,
     "site_gateway_setting_id"=>71},
   "group"=>nil},
 "transaction"=>
  {"id"=>3556,
   "subscription_id"=>784,
   "type"=>"Payment",
   "kind"=>nil,
   "transaction_type"=>"payment",
   "success"=>true,
   "amount_in_cents"=>50000,
   "memo"=>"John Doe - golder: Signup payment",
   "created_at"=>Wed, 18 Aug 2021 05:58:43 EDT -04:00,
   "starting_balance_in_cents"=>50000,
   "ending_balance_in_cents"=>0,
   "gateway_used"=>"digital_river",
   "gateway_transaction_id"=>"3daa894a-77e5-4bd9-88b9-8e6dd1e84185",
   "gateway_order_id"=>nil,
   "payment_id"=>nil,
   "product_id"=>154,
   "tax_id"=>nil,
   "component_id"=>nil,
   "statement_id"=>1553,
   "customer_id"=>708,
   "item_name"=>nil,
   "period_range_start"=>nil,
   "period_range_end"=>nil,
   "currency"=>"USD",
   "exchange_rate"=>1,
   "component_handle"=>nil,
   "component_price_point_id"=>nil,
   "component_price_point_handle"=>nil,
   "parent_id"=>nil,
   "role"=>nil,
   "card_number"=>"XXXX-XXXX-XXXX-1111",
   "card_expiration"=>"01/2029",
   "card_type"=>"visa",
   "refunded_amount_in_cents"=>0,
   "invoice_id"=>nil},
 "event_id"=>5432}
```

</Accordion>
</Accordion>

<Accordion title="pending_payment_failed">

**Trigger:** When a Pending Payment was rejected in the gateway.

**Payload Fields:** event_id, site, subscription, transaction
<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>784,
   "state"=>"pending",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>nil,
   "created_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
   "updated_at"=>Wed, 18 Aug 2021 05:58:43 EDT -04:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>nil,
   "next_assessment_at"=>nil,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "next_product_handle"=>nil,
   "cancel_at_end_of_period"=>nil,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
   "previous_state"=>"pending",
   "signup_payment_id"=>3556,
   "signup_revenue"=>"500.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>50000,
   "product_price_in_cents"=>50000,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>nil,
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>nil,
   "payer_id"=>nil,
   "receives_invoice_emails"=>nil,
   "product_price_point_id"=>168,
   "next_product_price_point_id"=>nil,
   "credit_balance_in_cents"=>0,
   "prepayment_balance_in_cents"=>0,
   "net_terms"=>nil,
   "stored_credential_transaction_id"=>nil,
   "locale"=>nil,
   "reference"=>nil,
   "currency"=>"USD",
   "on_hold_at"=>nil,
   "scheduled_cancellation_at"=>nil,
   "customer"=>
    {"id"=>708,
     "first_name"=>"John",
     "last_name"=>"Doe",
     "organization"=>"",
     "email"=>"test@example.com",
     "created_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
     "updated_at"=>Wed, 18 Aug 2021 05:58:29 EDT -04:00,
     "reference"=>nil,
     "address"=>"Street",
     "address_2"=>"",
     "city"=>"San Francisco",
     "state"=>"CA",
     "state_name"=>"California",
     "zip"=>"66785",
     "country"=>"US",
     "country_name"=>"United States",
     "phone"=>"123456789",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>nil,
     "vat_number"=>nil,
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil,
     "locale"=>nil},
   "product"=>
    {"id"=>154,
     "name"=>"golder",
     "handle"=>"golderrr",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Mon, 05 Jul 2021 08:32:52 EDT -04:00,
     "updated_at"=>Tue, 20 Jul 2021 08:08:49 EDT -04:00,
     "price_in_cents"=>50000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>true,
     "update_return_url"=>"",
     "tax_code"=>"4512.100",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"",
     "default_product_price_point_id"=>168,
     "request_billing_address"=>true,
     "require_billing_address"=>true,
     "require_shipping_address"=>true,
     "product_price_point_id"=>168,
     "product_price_point_name"=>"Original",
     "product_price_point_handle"=>"uuid:0a6d2640-bfbb-0139-c94b-2cde48001122",
     "product_family"=>
      {"id"=>65,
       "name"=>"Billing Plans",
       "description"=>nil,
       "handle"=>"chargify-billing-plans",
       "accounting_code"=>nil,
       "created_at"=>Mon, 21 Jun 2021 08:22:16 EDT -04:00,
       "updated_at"=>Mon, 21 Jun 2021 08:22:16 EDT -04:00},
     "public_signup_pages"=>
      [{"id"=>101,
        "return_url"=>nil,
        "return_params"=>"",
        "url"=>
         "https://some-subdomain.chargifypay.test/subscribe/nmxwp8gsjh3t/golderrr"}]},
   "credit_card"=>
    {"id"=>885,
     "first_name"=>"John",
     "last_name"=>"Doe",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
     "card_type"=>"visa",
     "expiration_month"=>1,
     "expiration_year"=>2029,
     "customer_id"=>708,
     "current_vault"=>"digital_river",
     "vault_token"=>"b0aeda54-7576-4457-a36b-e775d2730df8",
     "billing_address"=>"Street",
     "billing_city"=>"San Francisco",
     "billing_state"=>"CA",
     "billing_zip"=>"66785",
     "billing_country"=>"US",
     "customer_vault_token"=>"541159940336",
     "billing_address_2"=>"",
     "payment_type"=>"credit_card",
     "disabled"=>false,
     "site_gateway_setting_id"=>71},
   "group"=>nil},
 "transaction"=>
  {"id"=>3556,
   "subscription_id"=>784,
   "type"=>"Payment",
   "kind"=>nil,
   "transaction_type"=>"payment",
   "success"=>true,
   "amount_in_cents"=>50000,
   "memo"=>"John Doe - golder: Signup payment",
   "created_at"=>Wed, 18 Aug 2021 05:58:43 EDT -04:00,
   "starting_balance_in_cents"=>50000,
   "ending_balance_in_cents"=>0,
   "gateway_used"=>"digital_river",
   "gateway_transaction_id"=>"3daa894a-77e5-4bd9-88b9-8e6dd1e84185",
   "gateway_order_id"=>nil,
   "payment_id"=>nil,
   "product_id"=>154,
   "tax_id"=>nil,
   "component_id"=>nil,
   "statement_id"=>1553,
   "customer_id"=>708,
   "item_name"=>nil,
   "period_range_start"=>nil,
   "period_range_end"=>nil,
   "currency"=>"USD",
   "exchange_rate"=>1,
   "component_handle"=>nil,
   "component_price_point_id"=>nil,
   "component_price_point_handle"=>nil,
   "parent_id"=>nil,
   "role"=>nil,
   "card_number"=>"XXXX-XXXX-XXXX-1111",
   "card_expiration"=>"01/2029",
   "card_type"=>"visa",
   "refunded_amount_in_cents"=>0,
   "invoice_id"=>nil},
 "event_id"=>5432}
```

</Accordion>
</Accordion>

<Accordion title="prepaid_subscription_balance_change">

**Trigger:** Any change to a prepaid Subscription’s usage or prepayment balance.

**Payload Fields:** event_id, site, subscription, prepaid_configuration, customer, product, product_family, credit_card, group
<Accordion title="Full payload example">

```json
"subscription"=>
  {"id"=>129431,
   "state"=>"expired",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Mon, 09 Nov 2020 11:33:10 MST -07:00,
   "created_at"=>Mon, 09 Nov 2020 11:33:09 MST -07:00,
   "updated_at"=>Mon, 09 Nov 2020 11:57:26 MST -07:00,
   "expires_at"=>Mon, 09 Nov 2020 11:37:00 MST -07:00,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>nil,
   "next_assessment_at"=>nil,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "next_product_handle"=>nil,
   "cancel_at_end_of_period"=>nil,
   "payment_collection_method"=>"prepaid",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>nil,
   "previous_state"=>"active",
   "signup_payment_id"=>3129166,
   "signup_revenue"=>"30.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>22000,
   "product_price_in_cents"=>0,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>nil,
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>nil,
   "payer_id"=>nil,
   "receives_invoice_emails"=>nil,
   "product_price_point_id"=>16695,
   "next_product_price_point_id"=>nil,
   "credit_balance_in_cents"=>0,
   "prepayment_balance_in_cents"=>22000,
   "net_terms"=>nil,
   "stored_credential_transaction_id"=>nil,
   "locale"=>nil,
   "reference"=>nil,
   "currency"=>"USD",
   "on_hold_at"=>nil,
   "scheduled_cancellation_at"=>nil,
   "customer"=>
    {"id"=>214718,
     "first_name"=>"Baylee",
     "last_name"=>"Brekke",
     "organization"=>"Hickle, Strosin and Rice",
     "email"=>"Caden20@example.com",
     "created_at"=>Mon, 09 Nov 2020 11:32:39 MST -07:00,
     "updated_at"=>Mon, 09 Nov 2020 11:33:11 MST -07:00,
     "reference"=>nil,
     "address"=>"7563 Gutkowski Crescent",
     "address_2"=>"Apt. 499",
     "city"=>"West Michael",
     "state"=>"AZ",
     "zip"=>"44635-5049",
     "country"=>"US",
     "phone"=>"270-305-7838",
     "portal_invite_last_sent_at"=>Mon, 09 Nov 2020 11:33:11 MST -07:00,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>Mon, 09 Nov 2020 11:33:11 MST -07:00,
     "vat_number"=>nil,
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil,
     "locale"=>nil},
   "product"=>
    {"id"=>17900,
     "name"=>"expires",
     "handle"=>nil,
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>1,
     "expiration_interval_unit"=>"month",
     "created_at"=>Mon, 09 Nov 2020 11:29:50 MST -07:00,
     "updated_at"=>Mon, 09 Nov 2020 11:29:50 MST -07:00,
     "price_in_cents"=>0,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>false,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"",
     "tax_code"=>"",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"",
     "default_product_price_point_id"=>16695,
     "request_billing_address"=>false,
     "require_billing_address"=>false,
     "require_shipping_address"=>false,
     "product_price_point_id"=>16695,
     "product_price_point_name"=>"Original",
     "product_price_point_handle"=>"uuid:79d63630-04e7-0139-d7d7-029b6d08343c",
     "product_family"=>
      {"id"=>10712,
       "name"=>"Billing Plans",
       "description"=>nil,
       "handle"=>"acme-inc-billing-plans",
       "accounting_code"=>nil,
       "created_at"=>Mon, 27 Apr 2020 07:36:52 MDT -06:00,
       "updated_at"=>Mon, 27 Apr 2020 07:36:52 MDT -06:00},
     "public_signup_pages"=>[]},
   "credit_card"=>
    {"id"=>124033,
     "first_name"=>"Ciara",
     "last_name"=>"Heathcote",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1",
     "card_type"=>"bogus",
     "expiration_month"=>11,
     "expiration_year"=>2021,
     "customer_id"=>214718,
     "current_vault"=>"bogus",
     "vault_token"=>"1",
     "billing_address"=>"1816 Brekke Wall",
     "billing_city"=>"East Milo",
     "billing_state"=>"IN",
     "billing_zip"=>"00451-1348",
     "billing_country"=>"US",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"Apt. 650",
     "payment_type"=>"credit_card",
     "disabled"=>false,
     "site_gateway_setting_id"=>nil},
   "prepaid_configuration"=>
    {"id"=>624,
     "initial_funding_amount_in_cents"=>3000,
     "auto_replenish"=>true,
     "replenish_to_amount_in_cents"=>8000,
     "replenish_threshold_amount_in_cents"=>3000},
   "group"=>nil},
 "reason"=>"usage changed",
 "current_account_balance_in_cents"=>22000,
 "event_id"=>4917496}
```

</Accordion>
</Accordion>

<Accordion title="prepaid_usage">

**Trigger:** Any recorded usage for a Subscription’s prepaid component. Changes in allocation are reflected in `component_allocation_change`.

**Payload Fields:** event_id, site, component, subscription, product, previous_unit_balance, usage_quantity, previous_overage_unit_balance, new_overage_unit_balance, overage_usage_quantity, price_point_id
<Accordion title="Full payload example">

```json
"component"=>
  {"id"=>957864,
   "kind"=>"prepaid_usage_component",
   "name"=>"Minutes",
   "unit_name"=>"minutes",
   "handle"=>nil},
 "subscription"=>{"id"=>32304660, "name"=>"Mario Smith"},
 "product"=>{"id"=>4607690, "name"=>"Silver Plan"},
 "memo"=>"",
 "timestamp"=>"2020-04-28T18:34:36Z",
 "price_point_id"=>821647,
 "previous_unit_balance"=>"0.0",
 "new_unit_balance"=>15,
 "usage_quantity"=>15,
 "previous_overage_unit_balance"=>"0.0",
 "new_overage_unit_balance"=>5,
 "overage_usage_quantity"=>5,
 "event_id"=>1064815585}
```

</Accordion>
</Accordion>

<Accordion title="renewal_failure">

**Trigger:** A failed periodic renewal, such as when the credit card is declined. At the end of every recurring interval, either a renewal_success or a renewal_failure event is triggered once. If a card is declined and a renewal_failure is triggered, a subsequent payment that brings the account current will not generate a renewal_success (although it will generate a payment_success and a subscription_state_change)

**Payload Fields:** event_id, site, subscription, transaction
The subscription object also contains information on the Customer and Product.
<Accordion title="Full payload example">

```json
{"id"=>16372192,
   "state"=>"past_due",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Mon, 13 Feb 2025 11:50:57 EST -05:00,
   "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 13:28:06 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>16000,
   "current_period_ends_at"=>Mon, 13 Mar 2025 14:28:05 EDT -04:00,
   "next_assessment_at"=>Tue, 14 Feb 2025 13:28:06 EST -05:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 13 Feb 2025 13:28:05 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>173961106,
   "signup_revenue"=>"60.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>6000,
   "product_price_in_cents"=>5000,
   "product_version_number"=>1,
   "payment_type"=>nil,
   "referral_code"=>"cz8wdq",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "currency"=>"USD",
   "customer"=>
    {"id"=>15826583,
     "first_name"=>"Doris",
     "last_name"=>"Tester",
     "organization"=>"Acme",
     "email"=>"doris@example.com",
     "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
     "updated_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "reference"=>"123456789",
     "address"=>"123 Anywhere Street",
     "address_2"=>"",
     "city"=>"Boston",
     "state"=>"MA",
     "zip"=>"02120",
     "country"=>"US",
     "phone"=>"555-555-1212",
     "portal_invite_last_sent_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>nil,
     "portal_customer_created_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil},
   "product"=>
    {"id"=>4442358,
     "name"=>"Gold Product",
     "handle"=>"gold-product",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Thu, 15 Dec 2024 09:32:36 EST -05:00,
     "updated_at"=>Thu, 15 Dec 2024 09:32:36 EST -05:00,
     "price_in_cents"=>5000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>306012,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/7dbsnjd8t8cx/gold-product"},
       {"id"=>310598,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/ksjh9py5fn5h/gold-product"},
       {"id"=>311132,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/kjmks49g8d3d/gold-product"}]}},
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "event_id"=>377601653}
```

</Accordion>
</Accordion>

<Accordion title="renewal_success">

**Trigger:** A successful periodic renewal. At the end of every recurring interval, either a renewal_success or a renewal_failure event is triggered once. If a card is declined and a renewal_failure is triggered, a subsequent payment that brings the account current will not generate a renewal_success (although it will generate a payment_success and a subscription_state_change)

**Payload Fields:** event_id, site, subscription, transaction
<Accordion title="Full payload example">

```json
{"id"=>16327080,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Thu, 09 Feb 2025 11:42:28 EST -05:00,
   "created_at"=>Thu, 09 Feb 2025 11:42:26 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 12:03:41 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>5667,
   "current_period_ends_at"=>Wed, 08 Mar 2025 12:00:00 EST -05:00,
   "next_assessment_at"=>Wed, 08 Mar 2025 12:00:00 EST -05:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>"8",
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 13 Feb 2025 12:00:00 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>0,
   "signup_revenue"=>"0.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>0,
   "product_price_in_cents"=>6900,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"6zcsxw",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "currency"=>"USD",
   "customer"=>
    {"id"=>15780191,
     "first_name"=>"Test ",
     "last_name"=>"Monthly",
     "organization"=>"",
     "email"=>"test@example.com",
     "created_at"=>Thu, 09 Feb 2025 11:42:26 EST -05:00,
     "updated_at"=>Thu, 09 Feb 2025 11:42:29 EST -05:00,
     "reference"=>nil,
     "address"=>nil,
     "address_2"=>nil,
     "city"=>nil,
     "state"=>nil,
     "zip"=>nil,
     "country"=>nil,
     "phone"=>"",
     "portal_invite_last_sent_at"=>Thu, 09 Feb 2025 11:42:29 EST -05:00,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>Thu, 09 Feb 2025 11:42:29 EST -05:00,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil},
   "product"=>
    {"id"=>4458204,
     "name"=>"Monthly product ",
     "handle"=>"monthly-product",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Wed, 08 Feb 2025 16:48:54 EST -05:00,
     "updated_at"=>Wed, 08 Feb 2025 16:48:54 EST -05:00,
     "price_in_cents"=>6900,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>312067,
        "return_url"=>nil,
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/5x8sbq29vgkn/monthly-product"},
       {"id"=>312068,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/vsmzkwvmfqqx/monthly-product"}]},
   "credit_card"=>
    {"id"=>10881632,
     "first_name"=>"Test",
     "last_name"=>"Page",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1",
     "card_type"=>"bogus",
     "expiration_month"=>1,
     "expiration_year"=>2027,
     "customer_id"=>15780191,
     "current_vault"=>"bogus",
     "vault_token"=>"1",
     "billing_address"=>nil,
     "billing_city"=>nil,
     "billing_state"=>nil,
     "billing_zip"=>"",
     "billing_country"=>nil,
     "customer_vault_token"=>nil,
     "billing_address_2"=>nil,
     "payment_type"=>"credit_card"}},
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "event_id"=>377575304}
```

</Accordion>
</Accordion>

<Accordion title="signup_success">

**Trigger:** Any successful signup (Subscription created) through the API, application, or Public Pages.

**Payload Fields:** event_id, site, subscription
The subscription object also contains information on the Customer and Product. Component allocations are not currently included in the signup_success webhook.
<Accordion title="Full payload example">

```json
{"id"=>16372192,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Mon, 13 Feb 2025 11:50:57 EST -05:00,
   "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 11:50:57 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>6000,
   "current_period_ends_at"=>Mon, 13 Mar 2025 12:50:55 EDT -04:00,
   "next_assessment_at"=>Mon, 13 Mar 2025 12:50:55 EDT -04:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>173961106,
   "signup_revenue"=>"60.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>0,
   "product_price_in_cents"=>5000,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"cz8wdq",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "reason_code"=>nil,
   "automatically_resume_at"=>nil,
   "coupon_codes"=>[],
   "offer_id"=>64,
   "payer_id"=>15826583,
   "currency"=>"USD",
   "customer"=>
    {"id"=>15826583,
     "first_name"=>"Doris",
     "last_name"=>"Tester",
     "organization"=>"Acme",
     "email"=>"doris@example.com",
     "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
     "updated_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
     "reference"=>"123456789",
     "address"=>"123 Anywhere Street",
     "address_2"=>"",
     "city"=>"Boston",
     "state"=>"MA",
     "zip"=>"02120",
     "country"=>"US",
     "phone"=>"555-555-1212",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "vat_number"=>"123456789"
     "verified"=>nil,
     "portal_customer_created_at"=>nil,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false},
   "product"=>
    {"id"=>4442358,
     "name"=>"Gold Product",
     "handle"=>"gold-product",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Thu, 15 Dec 2024 09:32:36 EST -05:00,
     "updated_at"=>Thu, 15 Dec 2024 09:32:36 EST -05:00,
     "price_in_cents"=>5000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "tax_code"=>"PC040100",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>306012,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/7dbsnjd8t8cx/gold-product"},
       {"id"=>310598,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/ksjh9py5fn5h/gold-product"},
       {"id"=>311132,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/kjmks49g8d3d/gold-product"}]},
   "credit_card"=>
    {"id"=>10911728,
     "first_name"=>"Doris",
     "last_name"=>"Tester",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
     "card_type"=>"visa",
     "expiration_month"=>1,
     "expiration_year"=>2027,
     "customer_id"=>15826583,
     "current_vault"=>"bogus",
     "vault_token"=>"1",
     "billing_address"=>"123 Anywhere Street",
     "billing_city"=>"Boston",
     "billing_state"=>"MA",
     "billing_zip"=>"02120",
     "billing_country"=>"US",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"",
     "payment_type"=>"credit_card"}},
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "event_id"=>377569865}
```

</Accordion>
</Accordion>

<Accordion title="signup_failure">

**Trigger:** Any failed signup (Subscription failed to begin) through the API, application, or Public Pages. This is usually caused by a failure at the payment gateway. This event is not generated for input validation errors (i.e. forgetting to fill in a field).

**Payload Fields:** event_id, site, subscription
The subscription object also contains information on the Customer and Product.
<Accordion title="Full payload example">

```json
{"id"=>16374036,
   "state"=>"failed_to_create",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>nil,
   "created_at"=>Mon, 13 Feb 2025 13:08:30 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 13:08:31 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>6900,
   "current_period_ends_at"=>Wed, 08 Mar 2025 12:00:00 EST -05:00,
   "next_assessment_at"=>nil,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>"8",
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 13 Feb 2025 13:08:30 EST -05:00,
   "previous_state"=>"failed_to_create",
   "signup_payment_id"=>173970546,
   "signup_revenue"=>"69.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>0,
   "product_price_in_cents"=>6900,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"k9f3h5",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "payer_id"=>15828506,
   "currency"=>"USD",
   "customer"=>
    {"id"=>15828506,
     "first_name"=>"Joe",
     "last_name"=>"Tester",
     "organization"=>"Acme",
     "email"=>"joetester@example.com",
     "created_at"=>Mon, 13 Feb 2025 13:08:30 EST -05:00,
     "updated_at"=>Mon, 13 Feb 2025 13:08:30 EST -05:00,
     "reference"=>nil,
     "address"=>nil,
     "address_2"=>nil,
     "city"=>nil,
     "state"=>nil,
     "zip"=>nil,
     "country"=>nil,
     "phone"=>"555-555-1212",
     "portal_invite_last_sent_at"=>nil,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>nil,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false,
     "parent_id"=>nil},
   "product"=>
    {"id"=>4458204,
     "name"=>"Monthly product ",
     "handle"=>"monthly-product",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Wed, 08 Feb 2025 16:48:54 EST -05:00,
     "updated_at"=>Wed, 08 Feb 2025 16:48:54 EST -05:00,
     "price_in_cents"=>6900,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>312067,
        "return_url"=>nil,
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/5x8sbq29vgkn/monthly-product"},
       {"id"=>312068,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/vsmzkwvmfqqx/monthly-product"}]},
   "credit_card"=>
    {"id"=>10913241,
     "first_name"=>"Joe",
     "last_name"=>"Tester",
     "masked_card_number"=>"XXXX-XXXX-XXXX-2",
     "card_type"=>"bogus",
     "expiration_month"=>1,
     "expiration_year"=>2027,
     "customer_id"=>15828506,
     "current_vault"=>"bogus",
     "vault_token"=>"2",
     "billing_address"=>nil,
     "billing_city"=>nil,
     "billing_state"=>nil,
     "billing_zip"=>"02120",
     "billing_country"=>nil,
     "customer_vault_token"=>nil,
     "billing_address_2"=>nil,
     "payment_type"=>"credit_card"}},
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "event_id"=>377594843}
```

</Accordion>
</Accordion>

<Accordion title="subscription_bank_account_update">

**Trigger:** When a Subscription adds or updates a bank account.

**Payload Fields:** See payload example
<Accordion title="Full payload example">

```json
{"site"=>{"id"=>60050, "subdomain"=>"chargify-sandbox-141"},
 "subscription"=>{"id"=>58501928, "state"=>"active", "balance_in_cents"=>442},
 "product"=>
  {"id"=>6316229,
   "name"=>"Base Subscription",
   "product_family_id"=>2417171,
   "product_family_name"=>"Billing Scenario Testing"},
 "customer"=>
  {"id"=>59176626,
   "first_name"=>"Samara",
   "last_name"=>"Hills",
   "reference"=>nil,
   "organization"=>"Becker - Lynch",
   "email"=>"Jesus.Jacobs@example.org"},
 "previous_payment_profile"=>{},
 "updated_payment_profile"=>
  {"id"=>50298431,
   "first_name"=>"John",
   "last_name"=>"Smith",
   "billing_address"=>"123 main st",
   "billing_address_2"=>nil,
   "billing_city"=>"Roseville",
   "billing_state"=>"CA",
   "billing_zip"=>"95747",
   "billing_country"=>"US",
   "bank_name"=>"Chase",
   "masked_bank_routing_number"=>"XXXX1111",
   "masked_bank_account_number"=>"XXXX1111",
   "bank_account_holder_type"=>"personal",
   "current_vault"=>"bogus",
   "vault_token"=>"4111111111111111",
   "customer_vault_token"=>nil},
 "event_id"=>2946290595}
```

</Accordion>
</Accordion>

<Accordion title="subscription_card_update">

**Trigger:** Any change to the active credit card–type payment profile. This includes partial card/billing address updates and deletion of PayPal payment profiles. Additions of new PayPal accounts, changes/deletions of bank account / ACH type payment profiles do not currently generate any webhooks.

**Payload Fields:** event_id, site, subscription, product, previous_payment_profile, updated_payment_profile, customer
<Accordion title="Full payload example">

```json
"subscription"=>{"id"=>16372192, "state"=>"active", "balance_in_cents"=>0},
 "product"=>
  {"id"=>4442358,
   "name"=>"Gold Product",
   "product_family_id"=>986840,
   "product_family_name"=>"Acme Products"},
 "customer"=>
  {"id"=>15826583,
   "first_name"=>"Doris",
   "last_name"=>"Tester",
   "reference"=>"123456789",
   "organization"=>"Acme",
   "email"=>"doris@example.com"},
 "previous_payment_profile"=>{},
 "updated_payment_profile"=>
  {"id"=>10914352,
   "first_name"=>"Doris",
   "last_name"=>"Tester",
   "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
   "card_type"=>"visa",
   "expiration_month"=>2,
   "expiration_year"=>2025,
   "current_vault"=>"bogus",
   "vault_token"=>"1",
   "customer_vault_token"=>nil,
   "billing_address"=>"",
   "billing_address_2"=>"",
   "billing_city"=>"",
   "billing_state"=>"",
   "billing_zip"=>"",
   "billing_country"=>""},
 "event_id"=>377602844}
```

</Accordion>
</Accordion>

<Accordion title="subscription_group_card_update">

**Trigger:** Any change to the active credit card–type payment profile on the Subscription group.

**Payload Fields:** event_id, site, subscription_group, previous_payment_profile, updated_payment_profile, customer
<Accordion title="Full payload example">

```json
"subscription_group"=>{"uid"=>"grp_96w3wg7hhnbjk", "state"=>"active"},
 "customer"=>
  {"id"=>15826583,
   "first_name"=>"Doris",
   "last_name"=>"Tester",
   "reference"=>"123456789",
   "organization"=>"Acme",
   "email"=>"doris@example.com"},
 "previous_payment_profile"=>{},
 "updated_payment_profile"=>
  {"id"=>10914352,
   "first_name"=>"Doris",
   "last_name"=>"Tester",
   "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
   "card_type"=>"visa",
   "expiration_month"=>2,
   "expiration_year"=>2025,
   "current_vault"=>"bogus",
   "vault_token"=>"1",
   "customer_vault_token"=>nil,
   "billing_address"=>"",
   "billing_address_2"=>"",
   "billing_city"=>"",
   "billing_state"=>"",
   "billing_zip"=>"",
   "billing_country"=>""},
 "event_id"=>377602844}
```

</Accordion>
</Accordion>

<Accordion title="subscription_group_signup_failure">

**Trigger:** When a Subscription group signup fails via the Subscription Group Signup endpoint.

**Payload Fields:** See payload example
<Accordion title="Full payload example">

```json
{"site"=>{"id"=>60050, "subdomain"=>"chargify-sandbox-141"},
 "subscription_group"=>
  {"payer_attributes"=>
    {"first_name"=>"John",
     "last_name"=>"Doe",
     "email"=>"john@example.com",
     "organization"=>"Acme, Inc"},
   "credit_card_attributes"=>
    {"full_number"=>"3", "expiration_month"=>"12", "expiration_year"=>"2031"},
   "subscriptions"=>
    [{"product_id"=>6323196, "primary"=>true},
     {"product_id"=>6323196},
     {"product_id"=>6323196}]},
 "customer"=>nil,
 "event_id"=>2948348361}
```

</Accordion>
</Accordion>

<Accordion title="subscription_group_signup_success">

**Trigger:** When a Subscription group signup is successful via the Subscription Group Signup endpoint.

**Payload Fields:** See payload example
<Accordion title="Full payload example">

```json
{"site"=>{"id"=>60050, "subdomain"=>"chargify-sandbox-141"},
 "subscription_group"=>
  {"uid"=>"grp_b32ztn6vbyh5q",
   "scheme"=>1,
   "customer_id"=>64261470,
   "payment_profile_id"=>50310133,
   "subscription_ids"=>[63100002, 63100003, 63100004],
   "primary_subscription_id"=>63100002,
   "next_assessment_at"=>Sun, 09 Apr 2023 10:43:49 PDT -07:00,
   "state"=>"active",
   "cancel_at_end_of_period"=>false},
 "customer"=>
  {"id"=>64261470,
   "phone"=>nil,
   "reference"=>nil,
   "email"=>"john@example.com",
   "first_name"=>"John",
   "last_name"=>"Doe",
   "cc_emails"=>nil,
   "vat_number"=>nil,
   "address"=>nil,
   "address_2"=>nil,
   "city"=>nil,
   "country"=>nil,
   "organization"=>"Acme, Inc",
   "state"=>nil,
   "zip"=>nil,
   "parent_id"=>nil,
   "created_at"=>Thu, 09 Mar 2023 09:43:49 PST -08:00,
   "updated_at"=>Thu, 09 Mar 2023 09:43:49 PST -08:00,
   "portal_invite_last_sent_at"=>nil,
   "portal_invite_last_accepted_at"=>nil,
   "verified"=>false,
   "portal_customer_created_at"=>nil,
   "tax_exempt"=>false,
   "locale"=>nil},
 "event_id"=>2948335113}
```

</Accordion>
</Accordion>

<Accordion title="subscription_prepayment_account_balance_changed">

**Trigger:** When a Subscription prepayment account balance changes—either when a prepayment is applied (increasing the balance) or used to make a payment on an invoice (decreasing the balance).

**Payload Fields:** See payload example
<Accordion title="Full payload example">

```json
{"site"=>{"id"=>60050, "subdomain"=>"chargify-sandbox-141"},
 "prepayments_account"=>
  {"at_time"=>Wed, 08 Mar 2023 15:18:11 PST -08:00,
   "balance_in_cents"=>25000,
   "balance_change_in_cents"=>25000},
 "prepayments"=>
  {"0"=>
    {"id"=>816132545,
     "applications"=>{},
     "amount_in_cents"=>25000,
     "external"=>"external",
     "memo"=>"A memo is here"}},
 "subscription"=>{"id"=>63081463},
 "reason"=>"prepayment created",
 "event_id"=>2946242689}
```

</Accordion>
</Accordion>

<Accordion title="subscription_product_change">

**Trigger:** A successful change from an old product to a new product for a subscription. This webhook will fire for a product version change.

**Payload Fields:** event_id, site, previous_product, subscription
The subscription object also contains information on the Customer and Product.
<Accordion title="Full payload example">

```json
{"id"=>16372192,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Mon, 13 Feb 2025 11:50:57 EST -05:00,
   "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 13:40:29 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>Thu, 13 Apr 2025 14:28:00 EDT -04:00,
   "next_assessment_at"=>Thu, 13 Apr 2025 14:28:00 EDT -04:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 13 Feb 2025 13:28:05 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>173961106,
   "signup_revenue"=>"60.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>22000,
   "product_price_in_cents"=>4900,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"cz8wdq",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "customer"=>
    {"id"=>15826583,
     "first_name"=>"Doris",
     "last_name"=>"Tester",
     "organization"=>"Acme",
     "email"=>"doris@example.com",
     "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
     "updated_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "reference"=>"123456789",
     "address"=>"123 Anywhere Street",
     "address_2"=>"",
     "city"=>"Boston",
     "state"=>"MA",
     "zip"=>"02120",
     "country"=>"US",
     "phone"=>"555-555-1212",
     "portal_invite_last_sent_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "portal_invite_last_accepted_at"=>nil,
     "vat_number"=>"123456789"
     "verified"=>nil,
     "portal_customer_created_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false},
   "product"=>
    {"id"=>4443536,
     "name"=>"Business Monthly",
     "handle"=>"pd_plans_v2_business_mn_1_10",
     "description"=>"Business Monthly",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Mon, 19 Dec 2024 15:58:09 EST -05:00,
     "updated_at"=>Mon, 19 Dec 2024 15:58:09 EST -05:00,
     "price_in_cents"=>4900,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>0,
     "trial_interval"=>1,
     "trial_interval_unit"=>"day",
     "archived_at"=>nil,
     "require_credit_card"=>false,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>306548,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/jtvkftj84jsq/pd_plans_v2_business_mn_1_10"}]},
   "credit_card"=>
    {"id"=>10914352,
     "first_name"=>"Doris",
     "last_name"=>"Tester",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
     "card_type"=>"visa",
     "expiration_month"=>2,
     "expiration_year"=>2018,
     "customer_id"=>15826583,
     "current_vault"=>"bogus",
     "vault_token"=>"1",
     "billing_address"=>"",
     "billing_city"=>"",
     "billing_state"=>"",
     "billing_zip"=>"",
     "billing_country"=>"",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"",
     "payment_type"=>"credit_card"}},
 "previous_product"=>
  {"id"=>4442358,
   "name"=>"Gold Product",
   "handle"=>"gold-product",
   "description"=>"",
   "accounting_code"=>"",
   "request_credit_card"=>true,
   "expiration_interval"=>nil,
   "expiration_interval_unit"=>"never",
   "created_at"=>Thu, 15 Dec 2024 09:32:36 EST -05:00,
   "updated_at"=>Thu, 15 Dec 2024 09:32:36 EST -05:00,
   "price_in_cents"=>5000,
   "interval"=>1,
   "interval_unit"=>"month",
   "initial_charge_in_cents"=>nil,
   "trial_price_in_cents"=>nil,
   "trial_interval"=>nil,
   "trial_interval_unit"=>"month",
   "archived_at"=>nil,
   "require_credit_card"=>true,
   "return_params"=>"",
   "taxable"=>false,
   "update_return_url"=>"http://www.example.com",
   "initial_charge_after_trial"=>false,
   "version_number"=>1,
   "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
   "default_product_price_point_id"=>1,
   "product_family"=>
    {"id"=>986840,
     "name"=>"Acme Products",
     "description"=>"",
     "handle"=>"acme-products",
     "accounting_code"=>nil},
   "public_signup_pages"=>
    [{"id"=>306012,
      "return_url"=>"",
      "return_params"=>"",
      "url"=>
       "https://general-goods.chargifypay.com/subscribe/7dbsnjd8t8cx/gold-product"},
     {"id"=>310598,
      "return_url"=>"",
      "return_params"=>"",
      "url"=>
       "https://general-goods.chargifypay.com/subscribe/ksjh9py5fn5h/gold-product"},
     {"id"=>311132,
      "return_url"=>"",
      "return_params"=>"",
      "url"=>
       "https://general-goods.chargifypay.com/subscribe/kjmks49g8d3d/gold-product"}]},
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "event_id"=>377604632}
```

</Accordion>
</Accordion>

<Accordion title="subscription_product_change_scheduled">

**Trigger:** A delayed product change has been successfully scheduled for a subscription — the product change is set to take effect at the subscription's next renewal rather than immediately. The webhook fires as soon as the scheduled change is created, so you can react to an upcoming product change ahead of renewal instead of waiting for `subscription_product_change`.

**Payload Fields:** event_id, site, subscription, current_product, scheduled_product, effective_at
The `current_product` object is the subscription's product today; `scheduled_product` is the product it will change to at `effective_at`. The subscription object also contains information on the Customer and Product.

<Accordion title="Full payload example">

```json
{
  "site": {
    "id": 31615,
    "subdomain": "general-goods"
  },
  "subscription": {
    "id": 16372192,
    "state": "active",
    "balance_in_cents": 0,
    "current_period_ends_at": "2025-04-13T14:28:00-04:00",
    "next_assessment_at": "2025-04-13T14:28:00-04:00",
    "product_price_point_id": 1,
    "next_product_id": 4443537,
    "next_product_price_point_id": 5,
    "customer": {
      "id": 15826583,
      "first_name": "Doris",
      "last_name": "Tester",
      "email": "doris@example.com",
      "reference": "123456789"
    },
    "product": {
      "id": 4443536,
      "name": "Business Monthly",
      "handle": "business-monthly",
      "price_in_cents": 4900,
      "interval": 1,
      "interval_unit": "month"
    }
  },
  "current_product": {
    "id": 4443536,
    "name": "Business Monthly",
    "handle": "business-monthly",
    "price_in_cents": 4900,
    "interval": 1,
    "interval_unit": "month"
  },
  "scheduled_product": {
    "id": 4443537,
    "name": "Enterprise Monthly",
    "handle": "enterprise-monthly",
    "price_in_cents": 9900,
    "interval": 1,
    "interval_unit": "month"
  },
  "effective_at": "2025-04-13T14:28:00-04:00",
  "event_id": 377604633
}
```

</Accordion>

</Accordion>

<Accordion title="subscription_service_credit_account_balance_changed">

**Trigger:** Any change to the Subscription service credit account balance—either when credit is applied (increase) or used for payment (decrease).

**Payload Fields:** See payload example
<Accordion title="Full payload example">

```json
{"site"=>{"id"=>60050, "subdomain"=>"chargify-sandbox-141"},
 "subscription"=>{"id"=>63081463},
 "reason"=>"service credit given",
 "service_credits"=>
  [{"id"=>1992348,
    "invoice_uid"=>nil,
    "entry_type"=>"Credit",
    "amount_in_cents"=>1000,
    "memo"=>"A memo is here"}],
 "service_credit_account"=>
  {"at_time"=>Wed, 08 Mar 2023 15:17:55 PST -08:00,
   "balance_in_cents"=>1000,
   "balance_change_in_cents"=>1000},
 "event_id"=>2946242578}
```

</Accordion>
</Accordion>

<Accordion title="subscription_state_change">

**Trigger:** Any change to the Subscription state. This is the “workhorse” event. Watching this event can tell you if a Subscription moves to a “bad” state, such as `past_due`.

**Payload Fields:** event_id, site, subscription
The subscription object also contains information on the Customer and Product. Note that the `subscription` object you are given contains keys for both `previous_state` and state so you can track the changes.
<Accordion title="Full payload example">

```json
{"id"=>16372192,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Mon, 13 Feb 2025 11:50:57 EST -05:00,
   "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 13:33:14 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>16000,
   "current_period_ends_at"=>Mon, 13 Mar 2025 14:28:05 EDT -04:00,
   "next_assessment_at"=>Tue, 14 Feb 2025 13:28:06 EST -05:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 13 Feb 2025 13:28:05 EST -05:00,
   "previous_state"=>"past_due",
   "signup_payment_id"=>173961106,
   "signup_revenue"=>"60.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>6000,
   "product_price_in_cents"=>5000,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"cz8wdq",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "customer"=>
    {"id"=>15826583,
     "first_name"=>"Doris",
     "last_name"=>"Tester",
     "organization"=>"Acme",
     "email"=>"doris@example.com",
     "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
     "updated_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "reference"=>"123456789",
     "address"=>"123 Anywhere Street",
     "address_2"=>"",
     "city"=>"Boston",
     "state"=>"MA",
     "zip"=>"02120",
     "country"=>"US",
     "phone"=>"555-555-1212",
     "portal_invite_last_sent_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "portal_invite_last_accepted_at"=>nil,
     "vat_number"=>"123456789"
     "verified"=>nil,
     "portal_customer_created_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "vat_number"=>"123456789"
     "cc_emails"=>nil,
     "tax_exempt"=>false},
   "product"=>
    {"id"=>4442358,
     "name"=>"Gold Product",
     "handle"=>"gold-product",
     "description"=>"",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Thu, 15 Dec 2024 09:32:36 EST -05:00,
     "updated_at"=>Thu, 15 Dec 2024 09:32:36 EST -05:00,
     "price_in_cents"=>5000,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>nil,
     "trial_interval"=>nil,
     "trial_interval_unit"=>"month",
     "archived_at"=>nil,
     "require_credit_card"=>true,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>306012,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/7dbsnjd8t8cx/gold-product"},
       {"id"=>310598,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/ksjh9py5fn5h/gold-product"},
       {"id"=>311132,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/kjmks49g8d3d/gold-product"}]},
   "credit_card"=>
    {"id"=>10914352,
     "first_name"=>"Doris",
     "last_name"=>"Tester",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1111",
     "card_type"=>"visa",
     "expiration_month"=>2,
     "expiration_year"=>2018,
     "customer_id"=>15826583,
     "current_vault"=>"bogus",
     "vault_token"=>"1",
     "billing_address"=>"",
     "billing_city"=>"",
     "billing_state"=>"",
     "billing_zip"=>"",
     "billing_country"=>"",
     "customer_vault_token"=>nil,
     "billing_address_2"=>"",
     "payment_type"=>"credit_card"}},
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "event_id"=>377602840}
```

</Accordion>
</Accordion>

<Accordion title="upcoming_renewal_notice">

**Trigger:** A webhook will be generated 3 days before a subscription is set to renew.

**Payload Fields:** event_id, site, customer, email_sent, estimated_renewal_amount_in_cents, message, payment_profile, product, subscription
<Accordion title="Full payload example">

```json
"customer" => {
        "id" => 15146757,
        "first_name" => "Katharine",
        "last_name" => "Ross",
        "reference" => nil,
        "organization" => "",
        "email" => "kross@example.com"},
    "email_sent" => true,
    "estimated_renewal_amount_in_cents" => 5000,
    "message" =>
    "Upcoming renewal notice sent for period ending on 02/20/2025 for Katharine Ross's subscription to Gold Product",
    "payment_profile" => {
        "id" => 10485399,
        "first_name" => "Katharine",
        "last_name" => "Ross",
        "card_type" => "bogus",
        "masked_card_number" => "XXXX-XXXX-XXXX-1",
        "expiration_month" => 1,
        "expiration_year" => 2026},
    "product" => {
        "id" => 4442358,
        "name" => "Gold Product",
        "product_family_id" => 986840,
        "product_family_name" => "Acme Products"},
    "subscription" => {
        "id" => 15690494,
        "state" => "active",
        "current_period_ends_at" => Mon, 20 Feb 2025 12: 00: 00 EST - 05: 00},
    "event_id" => 378968302}
```

</Accordion>
</Accordion>

<Accordion title="upgrade_downgrade_failure">

**Trigger:** Any failed upgrade or downgrade.

**Payload Fields:** event_id, site, subscription, previous_product
The subscription object also contains information on the Customer and Product.
<Accordion title="Full payload example">

```json
{"id"=>16372192,
   "state"=>"active",
   "trial_started_at"=>nil,
   "trial_ended_at"=>nil,
   "activated_at"=>Mon, 13 Feb 2025 11:50:57 EST -05:00,
   "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 14:59:39 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>0,
   "current_period_ends_at"=>Fri, 17 Feb 2025 14:28:00 EST -05:00,
   "next_assessment_at"=>Fri, 17 Feb 2025 14:28:00 EST -05:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"automatic",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 13 Feb 2025 13:28:05 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>173961106,
   "signup_revenue"=>"60.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>6000,
   "product_price_in_cents"=>4900,
   "product_version_number"=>1,
   "payment_type"=>nil,
   "referral_code"=>"cz8wdq",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "customer"=>
    {"id"=>15826583,
     "first_name"=>"Doris",
     "last_name"=>"Tester",
     "organization"=>"Acme",
     "email"=>"doristester@example.com",
     "created_at"=>Mon, 13 Feb 2025 11:50:55 EST -05:00,
     "updated_at"=>Mon, 13 Feb 2025 13:42:07 EST -05:00,
     "reference"=>"123456789",
     "address"=>"123 Anywhere Street",
     "address_2"=>"",
     "city"=>"Boston",
     "state"=>"MA",
     "zip"=>"02120",
     "country"=>"US",
     "phone"=>"555-555-1212",
     "portal_invite_last_sent_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>Mon, 13 Feb 2025 11:50:58 EST -05:00,
     "vat_number"=>"123456789"
     "cc_emails"=>"",
     "tax_exempt"=>false},
   "product"=>
    {"id"=>4443536,
     "name"=>"Business Monthly",
     "handle"=>"pd_plans_v2_business_mn_1_10",
     "description"=>"Business Monthly",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Mon, 19 Dec 2024 15:58:09 EST -05:00,
     "updated_at"=>Mon, 19 Dec 2024 15:58:09 EST -05:00,
     "price_in_cents"=>4900,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>0,
     "trial_interval"=>1,
     "trial_interval_unit"=>"day",
     "archived_at"=>nil,
     "require_credit_card"=>false,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>306548,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/jtvkftj84jsq/pd_plans_v2_business_mn_1_10"}]}},
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "target_product"=>
  {"id"=>4458204,
   "name"=>"Monthly product ",
   "handle"=>"monthly-product",
   "description"=>"",
   "accounting_code"=>"",
   "request_credit_card"=>true,
   "expiration_interval"=>nil,
   "expiration_interval_unit"=>"never",
   "created_at"=>Wed, 08 Feb 2025 16:48:54 EST -05:00,
   "updated_at"=>Wed, 08 Feb 2025 16:48:54 EST -05:00,
   "price_in_cents"=>6900,
   "interval"=>1,
   "interval_unit"=>"month",
   "initial_charge_in_cents"=>nil,
   "trial_price_in_cents"=>nil,
   "trial_interval"=>nil,
   "trial_interval_unit"=>"month",
   "archived_at"=>nil,
   "require_credit_card"=>true,
   "return_params"=>"",
   "taxable"=>false,
   "update_return_url"=>"http://www.example.com",
   "initial_charge_after_trial"=>false,
   "version_number"=>1,
   "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
   "default_product_price_point_id"=>1,
   "product_price_point_id"=>1,
   "product_price_point_handle"=>nil,
   "product_family"=>
    {"id"=>986840,
     "name"=>"Acme Products",
     "description"=>"",
     "handle"=>"acme-products",
     "accounting_code"=>nil},
   "public_signup_pages"=>
    [{"id"=>312067,
      "return_url"=>nil,
      "return_params"=>"",
      "url"=>
       "https://general-goods.chargifypay.com/subscribe/5x8sbq29vgkn/monthly-product"},
     {"id"=>312068,
      "return_url"=>"",
      "return_params"=>"",
      "url"=>
       "https://general-goods.chargifypay.com/subscribe/vsmzkwvmfqqx/monthly-product"}]},
 "event_id"=>377635953}
```

</Accordion>
</Accordion>

<Accordion title="upgrade_downgrade_success">

**Trigger:** Any successful upgrade or downgrade.

**Payload Fields:** event_id, site, subscription, previous_product
The subscription object also contains information on the Customer and Product.
<Accordion title="Full payload example">

```json
{"id"=>16297395,
   "state"=>"trialing",
   "trial_started_at"=>Mon, 13 Feb 2025 15:00:02 EST -05:00,
   "trial_ended_at"=>Tue, 14 Feb 2025 15:00:02 EST -05:00,
   "activated_at"=>Wed, 08 Feb 2025 16:23:22 EST -05:00,
   "created_at"=>Tue, 07 Feb 2025 16:06:06 EST -05:00,
   "updated_at"=>Mon, 13 Feb 2025 15:00:02 EST -05:00,
   "expires_at"=>nil,
   "balance_in_cents"=>177,
   "current_period_ends_at"=>Tue, 14 Feb 2025 15:00:02 EST -05:00,
   "next_assessment_at"=>Tue, 14 Feb 2025 15:00:02 EST -05:00,
   "canceled_at"=>nil,
   "cancellation_message"=>nil,
   "next_product_id"=>nil,
   "cancel_at_end_of_period"=>false,
   "payment_collection_method"=>"invoice",
   "snap_day"=>nil,
   "cancellation_method"=>nil,
   "current_period_started_at"=>Mon, 13 Feb 2025 15:00:01 EST -05:00,
   "previous_state"=>"active",
   "signup_payment_id"=>173249657,
   "signup_revenue"=>"0.00",
   "delayed_cancel_at"=>nil,
   "coupon_code"=>nil,
   "total_revenue_in_cents"=>0,
   "product_price_in_cents"=>4900,
   "product_version_number"=>1,
   "payment_type"=>"credit_card",
   "referral_code"=>"j63942",
   "coupon_use_count"=>nil,
   "coupon_uses_allowed"=>nil,
   "product_price_point_id"=>1,
   "next_product_price_point_id"=>nil,
   "customer"=>
    {"id"=>15751842,
     "first_name"=>"Steve",
     "last_name"=>"Test",
     "organization"=>"",
     "email"=>"steve@example.com",
     "created_at"=>Tue, 07 Feb 2025 16:06:06 EST -05:00,
     "updated_at"=>Tue, 07 Feb 2025 16:06:10 EST -05:00,
     "reference"=>nil,
     "address"=>nil,
     "address_2"=>nil,
     "city"=>nil,
     "state"=>nil,
     "zip"=>nil,
     "country"=>nil,
     "phone"=>"",
     "portal_invite_last_sent_at"=>Tue, 07 Feb 2025 16:06:10 EST -05:00,
     "portal_invite_last_accepted_at"=>nil,
     "verified"=>false,
     "portal_customer_created_at"=>Tue, 07 Feb 2025 16:06:10 EST -05:00,
     "cc_emails"=>nil,
     "tax_exempt"=>false},
   "product"=>
    {"id"=>4443536,
     "name"=>"Business Monthly",
     "handle"=>"pd_plans_v2_business_mn_1_10",
     "description"=>"Business Monthly",
     "accounting_code"=>"",
     "request_credit_card"=>true,
     "expiration_interval"=>nil,
     "expiration_interval_unit"=>"never",
     "created_at"=>Mon, 19 Dec 2024 15:58:09 EST -05:00,
     "updated_at"=>Mon, 19 Dec 2024 15:58:09 EST -05:00,
     "price_in_cents"=>4900,
     "interval"=>1,
     "interval_unit"=>"month",
     "initial_charge_in_cents"=>nil,
     "trial_price_in_cents"=>0,
     "trial_interval"=>1,
     "trial_interval_unit"=>"day",
     "archived_at"=>nil,
     "require_credit_card"=>false,
     "return_params"=>"",
     "taxable"=>false,
     "update_return_url"=>"http://www.example.com",
     "initial_charge_after_trial"=>false,
     "version_number"=>1,
     "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
     "default_product_price_point_id"=>1,
     "product_price_point_id"=>1,
     "product_price_point_handle"=>nil,
     "product_family"=>
      {"id"=>986840,
       "name"=>"Acme Products",
       "description"=>"",
       "handle"=>"acme-products",
       "accounting_code"=>nil},
     "public_signup_pages"=>
      [{"id"=>306548,
        "return_url"=>"",
        "return_params"=>"",
        "url"=>
         "https://general-goods.chargifypay.com/subscribe/jtvkftj84jsq/pd_plans_v2_business_mn_1_10"}]},
   "credit_card"=>
    {"id"=>10865912,
     "first_name"=>"Steve",
     "last_name"=>"Test",
     "masked_card_number"=>"XXXX-XXXX-XXXX-1",
     "card_type"=>"bogus",
     "expiration_month"=>1,
     "expiration_year"=>2027,
     "customer_id"=>15751842,
     "current_vault"=>"bogus",
     "vault_token"=>"1",
     "billing_address"=>nil,
     "billing_city"=>nil,
     "billing_state"=>nil,
     "billing_zip"=>"",
     "billing_country"=>nil,
     "customer_vault_token"=>nil,
     "billing_address_2"=>nil,
     "payment_type"=>"credit_card"}},
 "site"=>{"id"=>31615, "subdomain"=>"general-goods"},
 "previous_product"=>
  {"id"=>4453830,
   "name"=>"Trial No obligation",
   "handle"=>"trial-no-obligation",
   "description"=>"",
   "accounting_code"=>"",
   "request_credit_card"=>true,
   "expiration_interval"=>nil,
   "expiration_interval_unit"=>"never",
   "created_at"=>Mon, 23 Jan 2025 09:53:23 EST -05:00,
   "updated_at"=>Thu, 26 Jan 2025 13:35:29 EST -05:00,
   "price_in_cents"=>1000,
   "interval"=>1,
   "interval_unit"=>"month",
   "initial_charge_in_cents"=>nil,
   "trial_price_in_cents"=>0,
   "trial_interval"=>1,
   "trial_interval_unit"=>"day",
   "archived_at"=>nil,
   "require_credit_card"=>false,
   "return_params"=>"",
   "taxable"=>false,
   "update_return_url"=>"http://www.example.com",
   "initial_charge_after_trial"=>false,
   "version_number"=>1,
   "update_return_params"=>"id={subscription_id}&ref={customer_reference}",
   "default_product_price_point_id"=>1,
   "product_price_point_id"=>1,
   "product_price_point_handle"=>nil,
   "product_family"=>
    {"id"=>986840,
     "name"=>"Acme Products",
     "description"=>"",
     "handle"=>"acme-products",
     "accounting_code"=>nil},
   "public_signup_pages"=>
    [{"id"=>310559,
      "return_url"=>"",
      "return_params"=>"",
      "url"=>
       "https://general-goods.chargifypay.com/subscribe/ngbsvxv4hq7q/trial-no-obligation"}]},
 "event_id"=>377636059}
```

</Accordion>
</Accordion>
<Accordion title="statement_settled">

**Trigger:** At the end of every period (e.g., at renewal). The statement_settled webhook indicates that the statement closed and payment was successfully received (or payment was not required).

</Accordion>

<Accordion title="statement_closed">

**Trigger:** At the end of every period (e.g., at renewal). The statement_closed webhook indicates that the statement closed but payment was not successfully received. If you receive a statement_closed webhook for a statement, you may also receive a statement_settled webhook for the statement at a later time if the statement becomes paid (e.g., after a dunning retry or card update).

</Accordion>
