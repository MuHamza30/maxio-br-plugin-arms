# 2021-02-12 Subscription Reactivation Behavior Change

As of February 22nd, 2021, reactivation of subscription within the current billing period will by default apply service credits and prepayments to the invoice.
There will be an optional checkbox to reactivate the subscription in the old way without using service credits and prepayments.

In the API the behavior also will change - service credits and prepayments will be applied by default, to block that action the `use_credits_and_prepayments` attribute must be set to `false`.

Previously, service credits and prepayments haven't been considered during reactivation with resuming the current billing period.
