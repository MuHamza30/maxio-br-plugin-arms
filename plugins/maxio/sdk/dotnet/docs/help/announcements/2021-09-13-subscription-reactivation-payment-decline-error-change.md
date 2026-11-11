# 2021-09-13 Subscription Reactivation Payment Decline Error Change

The error messages returned when a subscription's reactivation payment has been declined will change on October 4, 2021, both within the API and the admin UI.

The current behavior returns one of two generic messages. If the payment profile was declined, the message is: “The payment method on file could not be charged.”

If no payment profile is on file, the message is: “The customer does not have a credit card on file.”

With the upcoming changes, reactivations that fail due to a payment decline will return the payment gateway’s decline message.
When no payment profile is on file, the message will be: “No payment method was on file for the X balance,” where X is the appropriate currency marker and amount due at time of reactivation.

For API users, failed reactivation calls will still return a 422, and the structure of the error hash remains the same.
Only the error messages in the case of a failed payment or missing payment profile will change to better reflect the cause of failure.
These errors will also begin to appear in the admin UI.
