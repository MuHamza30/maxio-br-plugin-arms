# Authentication

Learn how to use API authentication to communicate directly with Advanced Billing from any programming language that you wish.

---

There are two methods of authentication, depending on what you are accessing:

- [Maxio API]($e/Subscriptions)
- [Maxio.js (formerly Chargify.js)](https://docs.maxio.com/hc/en-us/articles/38163190843789-Chargify-js-Overview)

Both methods of authentication assume you have previously generated API keys and securely stored them for later use. For more information, see this help article on [Advanced Billing API Keys](https://maxio.zendesk.com/hc/en-us/articles/24294819360525-API-Keys).

## API

The first method of interaction is through the API. API Authentication is implemented as HTTP Basic Authentication over TLS (HTTPS).

Your API login credentials are not the same as the credentials you use to log in to the web interface. You must obtain your API credentials separately, and you must connect to the API via TLS 1.2 (or better).

> Advanced Billing no longer supports TLS 1.0 or TLS 1.1 over HTTPS on the chargify.com domain. Any older browsers or API clients that do not support TLS 1.2 will no longer work. This change is mandated by the PCI Security Council and affects all merchants and service providers processing or transmitting credit card data. For more information, see our help article on [Security](https://maxio.zendesk.com/hc/en-us/articles/24183963101069-Security).

One of the most common calls you will make via the API is to retrieve a list of subscriptions to retrieve additional information, such as the status of a specific subscription. A simple way to authenticate is to use the API Key as the _username_ and "X" as the _password_, like the following:

```
curl https://{subdomain}.chargify.com/subscriptions.{format} \
  -u '{API_key}:X' \
  -H 'content-type: application/json' \
  -X GET
```

If passing the Basic Authentication header, the API key and password require base64 encoding:

```
curl --request GET \
  --url 'https:///{subdomain}.chargify.com/subscriptions.{format}' \
  --header 'authorization: Basic PDxhcGlfa2V5Pj46...' \
  --header 'content-type: application/json'
```

> ❗️ The API is case-sensitive.

---

# What's next?

After completing authentication setup, review the following articles:

- Managing [sites](page:introduction/basic-concepts/connected-sites)
- Creating [products](page:introduction/basic-concepts/product-catalog#product) and how they control what you bill customers
- Creating [subscriptions](page:introduction/basic-concepts/subscription-signup) (i.e., signing up customers)
