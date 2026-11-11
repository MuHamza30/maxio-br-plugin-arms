# Site Subdomains

Learn how to use sites to organize your business and configure access for API calls.

---

Your first site is created automatically after you choose a currency. Sites are simply "containers" for your products, customers, and subscriptions. A single site is sufficient for some use cases, though most merchants use at least two — one for **testing** and one for **production.**

## Site Access

[Log in to manage your sites](https://app.chargify.com/login). You have the following options for managing sites:

- Creating a new site
- Viewing a site dashboard
- Cloning a site (i.e., making a copy of the site structure, including products, components, families, etc.)
- Editing a site (currency, name, date/time format, timezone, etc)
- Deleting a site

To begin, create a site (make sure to put it in test mode) and pick a subdomain that you will remember for use in your API calls.

For more information about sites, including: switching sites, clearing site data, cloning - see the [Sites](https://maxio.zendesk.com/hc/en-us/articles/24250614702221-Sites-Overview) help article.

## Site API Subdomain

Every site has the ability to have one or more API keys associated with it to allow API access. The subdomain name is used in API calls to direct what site should be used in the context of the API call.

For example, if you have a site called "Acme, Inc." with the subdomain "acme," you would use a call similar to:

```perl
curl -u <API_KEY>:X -H Accept:application/json -X GET https://acme.chargify.com/subscriptions.json
```

The host is always in the form `https://<SUBDOMAIN>.chargify.com` followed by the URI for the API resource you are trying to access. In this last example, that would be `/subscriptions.json` as that is the URI of the resource to get a list of subscriptions in JSON.

## Clearing Site Data

Clearing your site data is very useful in specific circumstances:

- When in **development/test**, clearing your site data allows you to remove records that you added in for testing.
- When moving to **production/live mode**, clearing your site data is necessary for allowing your gateway to actually process real money.

Clearing your site data can be done in the following methods:

1. Clearing site data via the website.
2. Clearing site data via the API.

### Clearing via Website

To clear your site data via the website, see the [Clearing Site Data](https://maxio.zendesk.com/hc/en-us/articles/24250617028365-Clearing-Site-Data) help article.

### Clearing via API

There are a few options for clearing your site data, which match the settings available if you perform this action through the website. The most basic is clearing _all your data_:

```perl
curl -u <API_KEY>:X -H Accept:application/json -X POST https://acme.chargify.com/sites/clear_data.json
```

For more information about the parameters for clearing your site data using the API, see [Clear Site]($e/Sites/clearSite).

---

# What's next?

After you've created a new site, you should check out the following articles:

- [Products](product-catalog#product)
- [Subscriptions](subscription-signup)
