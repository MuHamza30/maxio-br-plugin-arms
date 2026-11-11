# 2022-12-07 "Cancel_at_end_of_period" Set To True Is No Longer Respected During Subscription Creation Via API When It Is Not Intended To Be.

As of December 7th, 2022, the "cancel_at_end_of_period" parameter set to "true" will no longer be respected during subscription creation via API if the "next_billing_at" parameter is also present.
Previously, this combination of parameters resulted in a subscription with the status of "Active (Pending Cancellation)".

Now, this combination of parameters will be invalid and will result in an error with a status code of 422 and a proper error message.
This change is being made to ensure that the subscription creation process is consistent and predictable, and to prevent unintended subscription status changes.
