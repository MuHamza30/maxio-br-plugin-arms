# 2023-05-18 Change in the refund_success and refund_failure webhooks

Starting May 18th, 2023, Maxio will undergo a change in the refund_success and refund_failure webhooks.
The subscription_id parameter will no longer be supported. Instead, sellers can access the subscription ID through
the subscription nested hash: subscription => { id => 1}. Please note that existing sites are unaffected by this change.
