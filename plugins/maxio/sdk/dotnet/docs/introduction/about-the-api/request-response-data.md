# Request and Response Data

## URL

The URL for API requests includes the subdomain of the Site you are working with:

`https://<subdomain>.chargify.com/<resource URI>`

## Response Data

Response data is sent as either XML or JSON, depending on the type of data requested (`HTTP Content-Type` header) or the type specified as being accepted (HTTP `Accept` header).

GETs for individual statements and invoices may also be requested as PDF using `application/pdf` or appending `.pdf` to the resource URI.

Response codes are sent via the normal HTTP Response Code, and are documented separately for each resource.

For boolean fields, note that a value of `null` may be considered as false. However, this is not true across all cases. Exercise good judgement here, or contact support with any questions.

For example:

- `null` can define that there's no data available for that attribute

## Request Data

POST and PUT request data may be formatted as either XML (`application/xml`) or JSON (`application/json`). For best results, you should set your HTTP `Content-Type` request header accordingly, although you may also specify your format by appending `.xml` or `.json` extensions on to the resource URI.

Note: Advanced Billing does not accept PUT or POST data sent as query parameters or form-encoded data – data must be sent as either XML or JSON. If you fail to set your `Content-Type` to either `application/xml` or `application/json`, your request may fail due to triggering of forgery protection mechanisms.
