## 2022-12-07 Preserve Period for Subscriptions with Allocated Components During Migration Preview

On December 7th, 2022, we will start preserving the period for subscriptions with allocated components during migration preview.

Previously, this was not done, but we have made this change to ensure that the preview migration behavior matches the actual migration.

## 2022-12-07 "Cancel_at_end_of_period" Set To True Is No Longer Respected During Subscription Creation Via API When It Is Not Intended To Be.

As of December 7th, 2022, the "cancel_at_end_of_period" parameter set to "true" will no longer be respected during subscription creation via API if the "next_billing_at" parameter is also present.
Previously, this combination of parameters resulted in a subscription with the status of "Active (Pending Cancellation)".

Now, this combination of parameters will be invalid and will result in an error with a status code of 422 and a proper error message.
This change is being made to ensure that the subscription creation process is consistent and predictable, and to prevent unintended subscription status changes.

## 2022-11-21 Create Successful And Failure Payment Events For Subscription Group Renewal

As of November 21st, 2022, we will now create Successful and Failure Payment Events for the primary subscription after renewal.

This change was made to ensure the renewal behavior works the same as the initial signup making it easier to track subscription group payment activity through the group’s lifetime.

## 2022-11-03 Prepayments No Longer Applied Automatically For Remittance Invoice with Net Terms

As of November 3rd, 2022, the logic of applying prepayments automatically is being changed for Remittance Invoices.

Previously, we applied prepayments automatically when issuing an invoice only in the cases of the collection method set to "remittance", or when the invoice didn't have net terms set.

Now, we apply prepayments automatically only in the case when net terms are not set, regardless of the collection method.

This change was made to ensure the prepayment behavior upon issuance of invoices with net terms is consistent for invoices of both "automatic" and "remittance" collection methods.

## 2022-05-19 Products Price Points Interval Validation Changes

As of May 19th, 2022, validation on the `interval` attribute (POST/PATCH products/:id/price_points) is being changed.

Previously, we required the `interval` attribute to be greater than 0, so if you provided 0.5 value, there was a misleading error message with text: "Recurring interval: must be greater than 0.".

Now, we require this field to be greater than or equal to 1. The new validation message is "Recurring Interval: must be greater than or equal to 1.".

## 2022-02-09 Component Allocation Events Backfilled With Transaction Exchange Rate

An issue was recently resolved which involved MRR changes for no-charge component allocations on a subscription using a currency other than the site's primary.
The MRR changes resulting from these events will be the proper amount going forward.
Past event records with the key `component_allocation_change` for such subscriptions have been backfilled with the proper values for component's `transaction_exchange_rate`.

If MRR updates for the past events are desired, please reach out to support.
