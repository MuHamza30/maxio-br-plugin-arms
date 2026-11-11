# 2022-02-09 Component Allocation Events Backfilled With Transaction Exchange Rate

An issue was recently resolved which involved MRR changes for no-charge component allocations on a subscription using a currency other than the site's primary.
The MRR changes resulting from these events will be the proper amount going forward.
Past event records with the key `component_allocation_change` for such subscriptions have been backfilled with the proper values for component's `transaction_exchange_rate`.

If MRR updates for the past events are desired, please reach out to support.
