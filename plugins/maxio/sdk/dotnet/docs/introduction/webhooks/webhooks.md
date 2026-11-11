# Webhooks

Use webhooks to receive notifications or initiate workflows when important events occur, like a payment succeeding, a Subscription status updating, or other relevant changes. You can subscribe to events of interest, and the system will post data to the URL you specify when one of those events occurs.

## Using Webhooks

To begin using webhooks, you must first create a publicly accessible endpoint that meets the following requirements:

1. Only HTTP endpoints are allowed in test mode. You must switch to HTTPS before moving to live mode.
2. The endpoint must be hosted on port 80 or 443. These are the only supported ports.
3. The endpoint must accept HTTP POST requests with a form-encoded body.

Once you have a public URL that Advanced Billing can access, you can begin receiving requests and responding with a `200 OK` status code.

In general, the process for using webhooks is:

1. An event to which your webhook URL is subscribed occurs in Advanced Billing. For example, a new Customer signs up, creating a Subscription.
2. Advanced Billing sends a request to your webhook URL containing the event data.
3. Your system receives the event data.
4. You [verify the event](page:introduction/webhooks/webhooks#verifying-events) data using signature validation.
5. You take action based on the event data, such as sending a welcome email or provisioning services.
6. You respond with `200 OK`, completing the webhook transaction.

Webhooks are sent as HTTP POST requests with a `Content-Type` of `application/x-www-form-urlencoded`, making them easy to parse in most programming languages.

Webhooks contain the following keys:
| Key | Description |
| -------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `id` | A unique numeric identifier for the webhook. Use this value to track which webhooks you've already processed. |
| `event` | An identifier for the type of event that occurred. See the list of [events](page:introduction/webhooks/webhooks-reference#events)|
| `payload` | A “hash” of pertinent data about the event. Keys and sub-keys in the hash are denoted using square bracket notation in the key. <br> For example, the product name would be included as the following form-encoded key-value pair in the content body of a `signup_success` webhook: </br> <br> `payload[subscription][product][name]=Basic`</br> |

## Timestamps

Timestamps in webhook payloads use the site's configured timezone. Some newer event types (e.g., `component_allocation_change`, `metered_usage`) use ISO 8601 format in UTC for consistency across systems.

## Limitations

Advanced Billing imposes two limitations on webhooks:

- **Data retention**: Controls how long webhook data is stored.
- **Endpoint limits**: Restricts the number of webhook endpoints per site.

For more information, see [Merchant Limits](https://docs.maxio.com/hc/en-us/articles/24183916903309-Merchant-Limits#merchant-limits-0-0).

## Configuring Webhooks

You can configure webhooks and test connectivity between your URL and Advanced Billing [in the UI](https://docs.maxio.com/hc/en-us/articles/24286723085197-Webhooks#webhooks-0-0) or through the [API]($e/Webhooks/createEndpoint). You can enable or disable webhooks as needed.

Each webhook includes:

- A URL
- A set of [events](page:introduction/webhooks/webhooks-reference#events) you subscribe to

## Verifying Events

Webhooks include a signature generated using HMAC-SHA-256 with your site’s shared key and the raw body of the POST request.

Ruby example:

```ruby
OpenSSL::HMAC.hexdigest(OpenSSL::Digest.new('sha256'), site.shared_key, webhook.body)
```

To verify events, validate that the signature included with the request matches exactly what is expected given the content being delivered.

You may either retrieve the signature value from the header `X-Chargify-Webhook-Signature-Hmac-Sha-256` or by specifying that the signature should be included in the query string by using the `{signature_hmac_sha_256} `replacement variable:

```http
http://example.com/?signature={signature_hmac_sha_256}
```

> **Note**:
> When in doubt, verify webhook data by checking the current resource state using the API.

### Finding Your Site Shared Key

Your Site’s shared key is created automatically. You can view or update it by selecting **Edit Current Site** from the **Site** drop-down menu in the utility bar. Protect your site’s shared key as you would your password. A compromised shared key could allow malicious actors to generate falsified webhooks that pass signature verification.

## Testing Webhooks

For initial testing, there are a number of options that you can use.

Before you have a publicly accessible endpoint available for troubleshooting, you could use a tool like [Webhook.site](https://webhook.site), which provides a temporary URL that Advanced Billing can send messages to, allowing you to view them within the Webhook.site application. The "bins" are temporary. This can provide insight into the content or headers Advanced Billing will be sending.

You can use the [Webhooks Control Panel](https://docs.maxio.com/hc/en-us/articles/24266161680781-Monitor-and-Resend-Webhooks) in the UI to send test webhook data to any configured endpoint. This feature lets you verify the operational status of your endpoint, though the payload may not match production webhooks. Test webhooks include the following payload:

`id=123456&event=test&payload[chargify]=testing`

For comprehensive testing, use [a sandbox account](https://app.chargify.com/signup/maxio-billing-sandbox) to create test subscriptions and perform actions that trigger webhooks.

The following steps represent a typical flow for testing the receipt of webhooks:

1. Setup publicly accessible webhook handler.
2. Enable webhooks at that endpoint, enable events you need to interact with (at the very least `subscription_state_change`, triggered when subscriptions move from active to cancelled).
3. Send a test event.
4. Check your signature verification code, check that you are responding `200 OK`.
5. Add the specific event processing code you need.

## Responding to a Webhook

Upon receipt of a webhook, accept it by returning an HTTP `200 OK` response as quickly as possible. Sending any other response (i.e. `500 Internal Server Error`, `404 Not Found`, etc.) OR failing to return a response within approximately 15 seconds results in automatic retries of the webhooks.

Advanced Billing attempts to send each webhook event up to five times before giving up. The retries follow an approximate backoff schedule:
| Attempt | Approximate Timing |
| ------- | -------------------------------------------- |
| 1 | As soon as possible after the original event |
| 2 | \~10 seconds after the most recent failure |
| 3 | \~15 seconds after the most recent failure |
| 4 | \~90 seconds after the most recent failure |
| 5 | \~180 seconds after the most recent failure |

If you use the webhook replay feature via the [webhook API]($e/Webhooks/replayWebhooks) or the webhook panel, avoid duplicate actions by following these suggestions:

- Use the unique webhook ID to track which webhooks you’ve already processed.

- Do not replay webhooks until the last_sent_at timestamp is well outside the automatic retry intervals.

### Inoperative Endpoints

Temporary or abandoned webhook endpoints can strain the system. If repeated failures occur, Advanced Billing may pause or disable the endpoint. Below are examples of failure counts and their associated states:

| Failure Count | State    | System Behavior                                                                                                                                                                       |
| ------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 1 to 25       | Enabled  | Retries proceed automatically                                                                                                                                                         |
| 26 to 50      | Paused   | Webhooks are generated in a paused state and must be manually sent using the webhooks panel. The endpoint is checked every two hours for responsiveness and re-enabled if functional. |
| 51 and over   | Disabled | Webhooks are no longer generated for this endpoint.                                                                                                                                   |

> ❗️If you delete an endpoint from Advanced Billing, any paused webhooks referencing that endpoint cannot be resent and are effectively discarded.

## Best Practices

Keep in mind the following best practices when using webhooks:

- Webhooks are **asynchronous**. Do not rely on them for time-sensitive actions.
- Do not block users from signing up or provisioning based on webhook responses. Instead, verify Subscriptions through the API.
