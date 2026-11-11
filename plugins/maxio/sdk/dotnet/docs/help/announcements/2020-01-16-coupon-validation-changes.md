# 2020-01-16 Coupon Validation Changes

On December 17th, 2019, we made an update to how coupon pricing is validated. Previously we would allow a coupon to be created with a negative amount.
If a coupon with a negative amount was applied to a subscription it would get ignored and not attempt to generate a discount, which is the expected behavior.

In order to bring coupon validations in line with the expected behavior, we no longer allow coupons to be created with a negative amount.
There were a small percentage of these coupons that we've updated from a negative amount to `0` in order to maintain the existing behavior but keep them valid.

If you have any questions or concerns please contact us at [support@maxio.com,](mailto:support@maxio.com) and we’ll be glad to assist.
