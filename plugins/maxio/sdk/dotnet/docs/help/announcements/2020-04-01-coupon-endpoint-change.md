# 2020-04-01 Coupon Endpoint Change

As of April 13th, 2020, the coupon endpoint in the API will change. Previously when retrieving coupons from this endpoint,
only coupons from the default product family would be returned.
The default product family is the one that was created first for the site, provided it has not been archived.

Moving forward, retrieving coupons from this endpoint will return all coupons for a given site. [Click here to review documentation for the API endpoint that we will be updating.]($e/Coupons/updateCoupon)

With this in mind, it's recommended to review your integration with Chargify and determine whether any calls retrieving coupons need to be updated.
Because more coupons may be returned with this change, we recommend using our pagination functionality.
The default number of coupons returned per page would be 30. The maximum allowed value is 200 results per page.

In order to only retrieve coupons from a specific product family, [the endpoint documented here may be used.]($e/Coupons/listCouponsForProductFamily)
