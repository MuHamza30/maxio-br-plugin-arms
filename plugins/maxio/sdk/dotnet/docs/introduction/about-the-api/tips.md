# Tips and Best Practices

> Maxio Advanced Billing provides an HTTP-based API that conforms to the principles of REST.
> Advanced Billing also offers a broad feature set and official [client libraries](page:development-tools/client-libraries#code-language-selection-and-sdk-access) for common languages.
> The API returns JSON responses as the primary and recommended format, but XML is also provided as a backward-compatible option for merchants who require it.

API access is included on all plans at no charge, so you always have direct access to your own data.

As you build your integration, keep request volume in mind. Because API traffic involves little or no user interaction, a program or routine can send far more requests than it needs. Runaway usage places unnecessary load on the platform, slows your own integration, and can trigger throttling or blocked requests.

The following tips and best practices help you keep your integration efficient and reliable.

## Client Libraries and SDKs

Maxio maintains official Advanced Billing SDKs for Python, Ruby, PHP, C#/.NET, TypeScript, Java, and Go.

Before writing your own client, check whether an SDK covers your stack. Select your language in the developer portal, and then click **Get SDK** to install the SDK from your package manager. For more information, see [Code language selection and SDK access](page:development-tools/client-libraries#code-language-selection-and-sdk-access).

## Development

If you have difficulty sending a request, try the simplest approach first and send the request with the curl command-line tool. Add the `--verbose` flag to receive additional debugging information.

[Webhook.site](https://webhook.site/) is another useful tool. If you are unsure what your integration is sending, post the request to a temporary Webhook.site URL instead of to the API so you can inspect the payload.

## Getting Subscription States

Most integrations need to know whether a customer has an active subscription, has canceled, or is behind on payments. The best approach is to keep a locally cached copy of the subscription state in your own database, then use [webhooks](page:introduction/webhooks/webhooks) to stay up to date in near real time as changes occur. Caching keeps your site available, reduces coupling to the API, and keeps both applications fast.

Avoid querying the API inline as part of a customer's request to your site. Inline queries can result in:

- Slowing down your own site while the customer waits for a check to the API on every request.
- Breaking your site during a network connectivity issue or in the unlikely event that the API is unavailable.
- Consuming large numbers of API requests as your customer base grows and becomes more active, which can lead to blocked requests from automatic abuse prevention.

There are three basic ways to track the state of a customer's subscription:

- Retrieve [subscription state](#subscription-state) through the API
- Receive [webhooks](page:introduction/webhooks/webhooks#responding-to-a-webhook)
- Download a manual [export](https://maxio.zendesk.com/hc/en-us/articles/24285931839757-Exporting-Data#locating-exports)

The simplest of these is to request the current state or history of a subscription through the API, which returns the state of the subscription at the time of the request.

### Subscription State

To get the current state of a subscription, send the following request:

```
HTTP GET https://{subdomain}.chargify.com/subscriptions/{subscription_id}.{format}
```

The response contains the current information about the subscription, including (but not limited to):

- Subscription details, such as subscription state, creation date, balance, next assessment date, and cancellation information
- Customer details
- Payment details

For more information, see [Read Subscription]($e/Subscriptions/readSubscription).

### Best Practices

Keep the following practices in mind as you synchronize your application with your billing data:

- Do not let your application depend on another service to control access directly. If an API call fails for any reason, your customer may not receive the best user experience, depending on how you have implemented the check.
- Limit direct calls where possible. The API limits how quickly and how often it responds to rapid, numerous calls. For more information, see [Error Handling & Rate Limiting](page:introduction/about-the-api/error-handling).

## Synchronizing Your Database

Normally, [webhooks](page:introduction/webhooks/webhooks) keep your local customer database in sync. If your database does fall out of sync with Advanced Billing, checking the state of all subscriptions through the API may be the only way to restore consistency.

A full reconciliation is fine when you need one. Reserve full reconciliation for exceptional circumstances or for a periodic check, usually no more than once a month.

Avoid pulling your entire subscriber base on every reconciliation run. The subscriptions list endpoint supports filtering, so you can request only what has changed since your last sync:

- `date_field=updated_at` combined with `start_date` and `end_date` returns only subscriptions modified in that window.
- `state` filters to specific subscription states (for example, `active`, `canceled`, or `past_due`), and accepts a comma-separated list of values.
- `page` and `per_page` paginate the results. `per_page` defaults to 20 and accepts a maximum of 200. See [List Subscriptions]($e/Subscriptions/listSubscriptions) for the full parameter list.

Filtering a routine reconciliation job this way, instead of pulling every subscription each time, can reduce a full-account sync to a fraction of the API calls and keeps you well clear of rate limits.

The same approach works when reconciling other records:

- **Customers.** `date_field` accepts `created_at` or `updated_at`, combined with `start_date` and `end_date`. `per_page` defaults to 50 on this endpoint and accepts a maximum of 200.
- **Invoices.** `date_field` accepts `due_date`, `issue_date`, or `paid_date` combined with `start_date` and `end_date`, and `created_at` or `updated_at` combined with `start_datetime` and `end_datetime`. For change-based reconciliation, use `date_field=updated_at`.

## Reporting Usage

When reporting component usage, avoid sending many tiny usage amounts. For example, if you charge by the minute for phone calls:

- **Don't** send a usage report for every minute or every phone call individually.
- **Don't** send all usage for all customers at once. Spread the reports out, or wait a short period of time between each request.

Instead:

- **Do** send one usage report per day with how much each customer used for the whole day.

For more information on reporting component usage or allocations, see the endpoint descriptions for the type of component used:

- [Create Usage]($e/Subscription%20Components/createUsage) for metered components
- [Allocate Component]($e/Subscription%20Components/allocateComponent) for quantity-based components

### Handling Retries Safely

Advanced Billing accepts a `uniqueness_token` parameter on any request that changes data (`POST`, `PUT`, `PATCH`, or `DELETE`) to protect against duplicate submissions, such as when a request times out and you cannot tell whether the request was received. Supply a long, random value such as a UUID at the top level of the request body. If a second request arrives with the same token, the second request is rejected with a `409 Conflict` and a duplicate submission error instead of being processed again. Tokens are retained for a minimum of 60 minutes.

Use a `uniqueness_token` any time your integration might retry a request after a timeout or an ambiguous failure. Usage reports, component allocations, and subscription creation are all common cases, and a token keeps a retried usage report from double-counting a customer's usage for that period.

Two rules to build into your retry logic:

- **A token is spent on receipt, not on success.** A request that was received and then failed validation has already consumed its token, so generate a new token before retrying the corrected request.
- **A `409 Conflict` proves delivery, not success.** The response confirms that the original request arrived, but not what the outcome was. For usage specifically, you can settle the question directly with [List Usage]($e/Subscription%20Components/listUsages), checking what is already recorded for that component and period before sending the report again.

For full details, including how to recover when the outcome of the original request is unknown, see [Duplicate Prevention](page:introduction/about-the-api/duplicate-prevention).

## Downloading Bulk Data

Periodically exporting transaction, subscription, or customer data is a common use case. Where possible, use the built-in [export](https://maxio.zendesk.com/hc/en-us/articles/24285931839757-Exporting-Data) functions inside Advanced Billing to generate reports and download the data. Exports are often much faster and significantly lower your API usage.

For subscription, invoice, or proforma invoice data specifically, you can automate exports instead of using the UI. Use [Create Subscriptions Export]($e/API%20Exports/exportSubscriptions) (or the Invoices and Proforma Invoices equivalents) to start an export job, and then poll the job status and retrieve the result with the corresponding Read and List endpoints. Automating exports this way removes the manual steps from a recurring download.

## Secure Applications

API requests cannot be made directly from the customer's browser or device. A client-side request would expose your API key, and anyone who has that key has full access to all of your Advanced Billing data.

Instead, tokenize sensitive information with [Maxio.js (formerly Chargify.js)](https://docs.maxio.com/hc/en-us/articles/38163190843789-Chargify-js-Overview) or a similar JavaScript library provided by your gateway. Post the token and any other information to your own server, and then make the API call from there.

### CORS and Browser Requests

If you attempt to make an API request directly from the customer's browser, you may see an error such as:

```
Response to preflight request doesn't pass access control check: No 'Access-Control-Allow-Origin' header is present on the requested resource.
```

or

```
Origin 'https://example.com' is therefore not allowed access.
The response had HTTP status code 404.
```

These errors mean you need to move the API call server-side, as described above. The API does not support Cross-Origin Resource Sharing (CORS) for requests made directly from a browser. This is by design, and CORS cannot be enabled for your site or domain.

## Large Imports

If you plan to import a large amount of data through the API, send a heads-up to [support@maxio.com](mailto:support@maxio.com) ahead of time. The Maxio team can then coordinate with you to make sure your import process goes smoothly.
