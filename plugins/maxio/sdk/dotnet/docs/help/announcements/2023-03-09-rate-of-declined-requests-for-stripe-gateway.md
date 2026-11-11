# 2023-03-09 Rate of Declined Requests for Stripe Gateway

On March 9th, 2023, a feature was released that removes the temporary email submitted to Stripe by Chargify.js during the initial tokenization process.
During our developer's review, we compared requests made via Chargify's hosted Self-Service pages which were processing these cards with success
versus the requests made via Chargify.js, only finding the temporary email as the common differentiation between these requests.

The feature in question was slowly rolled out to sites as requested.
That being said, it appears that the rate of declined requests from Stripe has recently decreased for affected merchants independent of this feature,
indicating that another change has likely occurred on Stripe's end.

As a result of these findings, we are marking this issue as resolved for the time being.
Apart from the feature outlined above that was released proactively by our developers to hopefully aid in reducing the number of declines,
no other changes were made on our end.

Additionally, due to the success rates increase without the need for this feature, we will not be rolling out the feature previously mentioned unless requested.
If the feature to disable temporary Chargify.js emails is needed for your site's configuration, please reach out to Support.

If there are any additional questions on the cause of this issue,
please reach out to Stripe support for further clarification as we are unable to verify the changes they made on their end to resolve this issue.
