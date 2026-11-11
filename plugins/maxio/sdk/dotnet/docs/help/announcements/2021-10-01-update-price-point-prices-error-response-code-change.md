# 2021-10-01 Update Price Point Prices Error Response Code Change

Currently, when [updating a price point]($e/Components/updateComponentPricePoint) with changes to its prices and referencing an id of a price which does not belong to the specified price point, the API returns a 404.
The response for this case is changing to a 422.
