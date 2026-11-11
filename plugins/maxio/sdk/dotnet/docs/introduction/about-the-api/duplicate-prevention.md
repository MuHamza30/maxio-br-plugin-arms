# Duplicate Prevention

When making API requests, it is possible to receive an error even though the request completed successfully.

For example, if you submit an API request and the request times out, you cannot be sure whether Advanced Billing received the request.

If you retry the request, you might end up with a duplicate transaction.

## Uniqueness Token

To prevent these duplicates, Advanced Billing accepts a `uniqueness_token` parameter on any request that changes data: `POST`, `PUT`, `PATCH`, or `DELETE`. Send the token at the top level of the request body, alongside the resource object rather than nested inside the object.

The value you supply for the `uniqueness_token` should be long and random, like a UUID. The exact format of the value is up to you.

If a second request arrives with the same `uniqueness_token`, the second request is rejected with a `409 Conflict` response code and a duplicate submission error. Tokens are retained for a minimum of 60 minutes.

## Duplicate Prevention Is Not Idempotency

If you have worked with idempotency keys on other platforms, note the difference: the API records that a token has already been used, but does not store the original response and cannot replay that response.

A `409 Conflict` therefore tells you that the first request arrived, but not whether the first request succeeded.

## Implementation Notes

- **A token is spent the moment a request is received, whether or not the request succeeds.** If a request is received and fails validation with `422 Unprocessable Entity`, the token has already been used. Generate a new token before retrying the corrected request.
- **Tokens are scoped to your site and to the API credentials that sent the request.** The same token sent with a different API key is not treated as a duplicate.
- **Request the `.json` or `.xml` format.** Without a format extension, an error may come back as an HTML page rather than the response body shown below.

## Example

Suppose you are making an adjustment on a subscription. Using curl, you send the following POST request, including a `uniqueness_token`.

```
curl --verbose -u $CHARGIFY_API_KEY:x -H Accept:application/json -H Content-Type:application/json -X POST \
-d @adjustment.json https://$CHARGIFY_SUBDOMAIN.chargify.com/subscriptions/$SUBSCRIPTION_ID/adjustments.json

adjustment.json:
{"adjustment":
    {
      "amount": "-12.43",
      "memo": "Credit for outage on 1/31"
    },
    "uniqueness_token": "2731FB23-98AD-4489-BAF6-7D5CE916F766"
}
```

The request then times out without returning a valid response, so you never receive the expected `201 Created`.

Since you have supplied a `uniqueness_token`, you can safely retry the request.

### Interpreting the Response to Your Retry

| Response                   | What it means                                                                                          | What to do                                                                                                                          |
| -------------------------- | ------------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------- |
| `201 Created`              | Advanced Billing never received your first request. The retry has been processed.                      | Continue as usual.                                                                                                                  |
| `422 Unprocessable Entity` | Advanced Billing never received your first request. The retry was received and rejected by validation. | Fix the request and retry with a **new** token.                                                                                     |
| `409 Conflict`             | Your first request was received and responded to.                                                      | The outcome of the first request is unknown. See [Recovering When the Outcome Is Unknown](#recovering-when-the-outcome-is-unknown). |

Example 409 Conflict response for a `.json` request:

```
< Status: 409 Conflict
{"errors":["DuplicatePrevention::DuplicateSubmissionError"]}
```

The equivalent response for an `.xml` request:

```
< Status: 409 Conflict
<errors><error>DuplicatePrevention::DuplicateSubmissionError</error></errors>
```

## Recovering When the Outcome Is Unknown

A `409 Conflict` confirms delivery of the first request but not the result, so you cannot automatically assume the first request succeeded.

Recovery depends on the type of request. In many cases you can recover gracefully by recording identifying information about the original request, listening for [webhooks](page:introduction/webhooks/webhooks), and matching the webhook payload to find out whether the request succeeded.

Where the resource can be queried, reading the resource back is usually the most direct check. For example, after an ambiguous usage report, use [List Usage]($e/Subscription%20Components/listUsages) to see the usage already recorded for that component and period, and confirm whether your report landed before sending the report again.

In other cases, you need to confirm the outcome manually before deciding whether to retry.

## Summary

Used consistently, `uniqueness_token` keeps a retry after a timeout from becoming a duplicate transaction. The token does not replay the original response, so pair the token with a way to confirm the outcome, such as a webhook listener or a read-back of the affected resource.

That said, if you are experiencing repeated timeouts, [email Maxio support](mailto:support@maxio.com) so we can investigate.
