## 2023-05-18 Change in the refund_success and refund_failure webhooks

Starting May 18th, 2023, Maxio will undergo a change in the refund_success and refund_failure webhooks.
The subscription_id parameter will no longer be supported. Instead, sellers can access the subscription ID through the subscription nested hash: subscription => { id => 1}. Please note that existing sites are unaffected by this change.

## 2023-05-16 Enable Chargify.js Security Token by default

As of May 16th, 2023, all new Sellers will have enabled Chargify.js Security Token by default. Additionally, merchants will not be able to change the related security settings by themselves.

In order to do that they will need to reach out to the support.
This change has been made in order to improve the security of sites using Chargify.js and will reduce the exposure of Chargify.js as a target for attack.

## 2023-05-10 SSL Badge Removal

We would like to inform you that we have decided to immediately discontinue the use of Comodo SSL certificates for our Maxio platform.

This decision was made after careful consideration and analysis of our security measures, and we are confident that it will not have any adverse impact on the security or performance of our platform.

## 2023-03-09 Rate of Declined Requests for Stripe Gateway

On March 9th, 2023, a feature was released that removes the temporary email submitted to Stripe by Chargify.js during the initial tokenization process.
During our developer's review, we compared requests made via Chargify's hosted Self-Service pages which were processing these cards with success
versus the requests made via Chargify.js, only finding the temporary email as the common differentiation between these requests.

The feature in question was slowly rolled out to sites as requested.That being said, it appears that the rate of declined requests from Stripe has recently decreased for affected merchants independent of this feature,
indicating that another change has likely occurred on Stripe's end.

As a result of these findings, we are marking this issue as resolved for the time being.

Apart from the feature outlined above that was released proactively by our developers to hopefully aid in reducing the number of declines, no other changes were made on our end.

Additionally, due to the success rates increase without the need for this feature, we will not be rolling out the feature previously mentioned unless requested.
If the feature to disable temporary Chargify.js emails is needed for your site's configuration, please reach out to Support.

If there are any additional questions on the cause of this issue,please reach out to Stripe support for further clarification as we are unable to verify the changes they made on their end to resolve this issue.

## 2023-02-28 Increase in Declines from Stripe Gateway

We have multiple merchants experiencing an increase in declines when storing credit cards within the Stripe payment gateway beginning in early February.
Further investigation by our developers did not find any changes on our end that would result in these declines.

Until we receive more direct insight from Stripe on this matter, we recommend that any affected merchants contact Stripe directly.

Additionally, customers encountering declines may reach out to their card issuing bank for more information.
We have also found multiple instances where a decline succeeds in the future after some time, suggesting these declines may not be permanent errors.

We understand the inconvenience these increased declines may cause, and will provide further updates as soon as we have more information to share.

## 2023-02-03 Unintended Renewal Event After The First Dunning Attempt

We recently released a fix for renewal webhooks on 2023-01-24, however, we regret to inform you that this resulted in unintended consequences
causing the webhooks to trigger daily after the first dunning attempt.
Our development team quickly identified and resolved the issue, ensuring that the renewal webhooks will now only trigger on the normal renewal dates.

We understand that this situation may have caused some inconvenience, and we would like to extend our sincerest apologies for any disruption.
We are committed to providing our customers with the best possible service, and we appreciate your patience and understanding during this time.

## 2023-02-01 Limitations to custom fields

As of February 13th, 2023, we will implement a change limiting custom fields to 2048 characters.
This decision has been made with the aim of improving the stability and performance of our system.

## 2023-01-30 "Access Denied" 401 HTTP responses in UI and API

Between 12:54 UTC and 13:21 UTC, a routine system update being rolled out incrementally inadvertently caused a small number of customers to experience
"Access Denied" 401 HTTP status code responses in the UI and API.
As soon as our engineers determined the 401 responses were associated with the system upgrade, the upgrade was immediately rolled back.

Due to the incremental rollout of this system upgrade, only a small number of customers were affected within the 25-minute window between the initial rollout and subsequent reversion.
Customers were able to work around this issue by refreshing the page or retrying API calls until a successful response was received.

While this issue was corrected by the reversion of the system update, we sincerely apologize for any inconveniences this matter caused,
and thank you for your patience and understanding as our engineers worked toward a resolution.

## 2023-01-24 Renewal Success and Renewal Failure events changes

We have recently been made aware of an issue with our subscription renewals and webhook events.
Specifically, that subscription renewal did not trigger a webhook event while the subscription was in a past_due state.
This was caused by a mid-period component allocation failure or the renewal occurred a couple of days after the subscription entered dunning.
Even when the subscription's state changed to active, no event was triggered to note the next renewal, and issue an invoice.

We have made changes to our system to ensure that renewal webhooks will be triggered even when a subscription is in a past-due state.
This means that our customers will receive the most up-to-date information on their subscriptions.

## 2023-01-18 Ability to set custom settings for applying prepayments to invoices

As of January 18th, 2023, we have added additional options for the manner in which prepayments are applied to invoices.
The new options may be found under Config > Settings > Prepayments and the custom settings for applying prepayments to invoices are as follows:

Automatic Payment Collection Method:

- Do not automatically apply prepayments

- Apply available prepayments automatically immediately upon issuance of invoice

- Apply prepayments automatically on the due date (default setting)

Remittance Payment Collection Method:

- Do not automatically apply prepayments

- Apply available prepayments automatically immediately upon issuance of invoice (default setting)

This change was made to allow merchants greater flexibility to ensure prepayments are applied to invoices in a manner that best fits their individual workflow.

## 2023-01-16 Ability to sort results for List Metafields endpoint

As of January 16th, 2023, List Metafields endpoint response will be sorted defaultly by id, ascending.
Sorting can be changed to descending by providing direction parameter.

## 2023-01-10 Ability to sort results for List Metadata for Resource Type endpoint

As of January 10th, 2023, List Metadata for Resource Type endpoint response will be sorted defaultly by id, ascending.
Sorting can be changed to descending by providing direction parameter.
