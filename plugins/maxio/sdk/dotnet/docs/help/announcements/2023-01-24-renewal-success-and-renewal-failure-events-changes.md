# 2023-01-24 Renewal Success and Renewal Failure events changes

We have recently been made aware of an issue with our subscription renewals and webhook events.
Specifically, that subscription renewal did not trigger a webhook event while the subscription was in a past_due state.
This was caused by a mid-period component allocation failure or the renewal occurred a couple of days after the subscription entered dunning.
Even when the subscription's state changed to active, no event was triggered to note the next renewal, and issue an invoice.

We have made changes to our system to ensure that renewal webhooks will be triggered even when a subscription is in a past-due state.
This means that our customers will receive the most up-to-date information on their subscriptions.
