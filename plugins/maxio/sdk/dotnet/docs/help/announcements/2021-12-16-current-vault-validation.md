# 2021-12-16 Current Vault Validation

Between approximately 3:30 AM on November 9th, 2021, and 2:00 PM on November 10, 2021,
the value of `current_vault` was being validated to see if it matched one of the gateways configured for the site.

In most cases the value did match and there was no effect. For a few sites, we discovered that an incorrect value was being submitted.
For example, we were receiving `stripe` when the correct value is `stripe_connect`.

This change has been reverted, and we are reaching out to a limited number of merchants who experienced signup failures due to this validation.

We apologize for any disruption and are taking steps to ensure that notification is provided in advance
if we must make a change that breaks backwards compatibility in the API.

This change will be re-introduced in the future.
