# 2020-12-18 Changes to date_field Validation

As of December 18th, 2020, we have added validation on the `date_field` attribute.

Previously, we did not validate the date_field, and if it was other than `created_at` or `updated_at`,
we would allow the query update to proceed, but the result was an empty response.

Now if other values as `created_at` or `updated_at` are passed in `date_field`, we just ignore them, and filters are not applied.
If other params are correct and records exist, they should appear in the response.

If you have any questions or concerns please contact us at [support@maxio.com,](mailto:support@maxio.com) and we’ll be glad to assist.
