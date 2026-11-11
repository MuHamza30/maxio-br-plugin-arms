# 2020-01-27 Product Price Point Order

As of January 27th, 2020, we will be updating the API response when fetching a list of price points for a product.
Moving forward the `GET` request will now always be returned in `DESC` order of `created_at`, ensuring results are consistent across pages.

Previously if you included `page=1` in your request the first result of the response was the default price point for that product.
This will no longer be the case and the default will appear based on its `created_at` date and time.
