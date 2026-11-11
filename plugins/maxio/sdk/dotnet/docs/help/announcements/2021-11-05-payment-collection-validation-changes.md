# 2021-11-05 Payment Collection Validation Changes

As of October 26th, 2021, subscription's attribute "payment_collection_method" is being validated accordingly to site's architecture:

- on statement-based sites valid options are:
  - "automatic"
  - "invoice"
- on Relationship Invoicing architecture valid options are:
  - "automatic"
  - "remittance"
  - "prepaid"
