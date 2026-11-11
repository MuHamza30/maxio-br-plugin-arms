using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.Customers;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Customer object represents the person or organization being billed and is the anchor that subscriptions, payment profiles, and invoices are all attached to.
/// <para>
/// A customer record stores contact and billing details (name, address, tax exemption status, locale) along with references to related records, such as the customer's default subscription group. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/customers/understand-customers-for-subscription-management">Understand Customers for Subscription Management</see>.
/// </para>
/// </summary>
public sealed class Customers
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Customers(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Customer
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CustomerResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateCustomerError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a new customer; can also be created alongside a new subscription. The only validation restriction is that you can only create one customer for a given reference value.
    /// <para>
    /// If provided, the <c>reference</c> value must be unique. It represents a unique identifier for the customer from your own app, i.e. the customer’s ID. This allows you to retrieve a given customer via a piece of shared information. Alternatively, you can choose to leave <c>reference</c> blank, and store the system-assigned unique ID for the customer, which is in the <c>id</c> attribute.
    /// </para>
    /// <para>
    /// For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24252190590093-Customer-Details">Customer Details</see>.
    /// </para>
    /// <para>
    /// ## Required Country Format
    /// </para>
    /// <para>
    /// Format the country attribute of the customer using the ISO Standard Country codes.
    /// </para>
    /// <para>
    /// Countries should be formatted as two characters. For more information, see <see href="http://en.wikipedia.org/wiki/ISO_3166-1#Current_codes">ISO 3166-1</see>.
    /// </para>
    /// <para>
    /// ## Required State Format
    /// </para>
    /// <para>
    /// Format the state attribute of the customer using the ISO Standard State codes.
    /// </para>
    /// <list type="bullet">
    ///   <item><description>US States (two characters): see <see href="https://en.wikipedia.org/wiki/ISO_3166-2:US">ISO 3166-2</see>.</description></item>
    /// </list>
    /// <list type="bullet">
    ///   <item><description>States Outside the US (two to three characters): To find the correct state codes outside the US, go to <see href="http://en.wikipedia.org/wiki/ISO_3166-1#Current_codes">ISO 3166-1</see> and click on the link in the “ISO 3166-2 codes” column next to the country you wish to populate.</description></item>
    /// </list>
    /// <para>
    /// ## Locale
    /// </para>
    /// <para>
    /// You can attribute a language/region to the customer to deliver invoices in any required language. For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24286672013709-Customer-Locale">Customer Locale</see>.
    /// </para>
    /// <para>
    /// ## Tax and Business Identifiers
    /// </para>
    /// <para>
    /// Send <c>entity_identifier_kind</c> and <c>entity_identifier_value</c> together to store the customer's tax or business identifier, such as an EU VAT number, a French SIREN, or a LEI. A customer holds one identifier at a time.
    /// </para>
    /// <para>
    /// The <c>vat_eu</c> and <c>national_tax</c> kinds also require <c>vat_country</c>. An unsupported kind, a missing or mismatched <c>vat_country</c>, or a <c>gln</c>, <c>duns</c>, or <c>lei</c> value in the wrong format returns <c>422</c>.
    /// </para>
    /// <para>
    /// Always send the kind. <c>entity_identifier_value</c> on its own is stored as a <c>company_reg</c> when no <c>vat_country</c> is present, and returns <c>422</c> naming <c>entity_identifier_kind</c> when one is.
    /// </para>
    /// <para>
    /// A blank pair is ignored rather than rejected, so a <c>vat_number</c> sent alongside it still takes effect.
    /// </para>
    /// <para>
    /// The legacy <c>vat_number</c> and <c>vat_country</c> pair still works on its own. When neither entity identifier field is sent, Advanced Billing derives the kind from <c>vat_country</c>: an EU member state code or <c>GB</c> gives <c>vat_eu</c>, one of the national tax country codes gives <c>national_tax</c>, and a blank or unrecognized country gives <c>company_reg</c>.
    /// </para>
    /// <para>
    /// The response reports the stored identifier in <c>entity_identifier_kind</c> and <c>entity_identifier_value</c>, and repeats its value in <c>vat_number</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A customer is created with a reference value that another customer on the site already has → the outcome is not stated. Send a reference that no other customer on the site uses, or leave reference blank and store the returned id.</description></item>
    ///   <item><description>entity_identifier_kind is vat_eu or national_tax and no vat_country is sent → Maxio answers 422. Send vat_country whenever entity_identifier_kind is vat_eu or national_tax.</description></item>
    ///   <item><description>vat_country does not fit the identifier kind: vat_eu with a country that is not an EU member state or GB, or national_tax with a country outside AL, AM, AR, AU, BR, CA, CH, DZ, IN, MX, NO, NZ, ZA → Maxio answers 422. Check vat_country against the kind's allowed countries before sending the customer.</description></item>
    ///   <item><description>entity_identifier_kind is gln, duns or lei and the value is not 13 digits, 9 digits, or 18 letters or digits followed by 2 digits respectively → Maxio answers 422. Validate the identifier value's length and characters for gln, duns and lei before sending it.</description></item>
    ///   <item><description>entity_identifier_value is sent without entity_identifier_kind while a vat_country is present → Maxio answers 422 with "names entity_identifier_kind". Send entity_identifier_kind whenever entity_identifier_value is sent.</description></item>
    ///   <item><description>entity_identifier_value is sent with surrounding whitespace or lowercase letters → The value is trimmed and stored in uppercase. Compare stored identifiers in trimmed uppercase form, not as originally entered.</description></item>
    ///   <item><description>An identifier is stored through entity_identifier_kind with kind company_reg, gln, duns or lei → vat_country is set to null. Expect vat_country to be null after saving a company_reg, gln, duns or lei identifier, even if one was sent.</description></item>
    ///   <item><description>organization is omitted, null or an empty string → organization is populated with the customer's first and last name separated by a space. Do not treat a returned organization as a real company name unless your app set it.</description></item>
    ///   <item><description>surcharging is omitted when creating a customer on a site where surcharging control is enabled → surcharging defaults to true. Send surcharging false explicitly if the customer must not be surcharged.</description></item>
    ///   <item><description>surcharging is sent on a site where surcharging control is not enabled → The value is not applied, and surcharging is not included in the customer response. Confirm surcharging control is enabled on the site before relying on the customer's surcharging setting.</description></item>
    ///   <item><description>branding_theme_id is sent on a site where Branding Themes are not enabled → The field is available only when Branding Themes are enabled for the site. Send branding_theme_id only on sites with Branding Themes enabled.</description></item>
    ///   <item><description>On a site with Branding Themes enabled, a customer has a branding_theme_id and one of its subscriptions has no subscription-level Branding Theme → The customer's Branding Theme is used for that subscription. Expect changing the customer's branding_theme_id to change the branding of every subscription without its own theme.</description></item>
    ///   <item><description>A vat_number is added or updated on a customer → The number is not validated when saved; it is validated via VIES before taxes are calculated, and only a valid number allows VAT opt-out. Validate VAT numbers yourself if you need to catch bad ones at entry; a saved number does not mean VAT opt-out will apply.</description></item>
    ///   <item><description>tax_exempt is sent as 1 or 0 → 1 is accepted as true and 0 as false. Send tax_exempt as true/false or 1/0 only.</description></item>
    ///   <item><description>A subscriber is to be taxed but neither the customer nor the payment profile has a full valid address with an ISO 2-3 character state and ISO 2 character country → The subscriber is not eligible to be taxed. Send a complete billing or shipping address with ISO state and country codes for every customer who must be taxed.</description></item>
    ///   <item><description>A new customer is created → A customer_create webhook is sent. Subscribe to customer_create if other systems must learn of new customers.</description></item>
    ///   <item><description>entity_identifier_value is sent without entity_identifier_kind and no vat_country is present → The value is stored as a company_reg identifier. Always send entity_identifier_kind with entity_identifier_value so the kind is not chosen for you.</description></item>
    ///   <item><description>entity_identifier_kind and entity_identifier_value are both sent blank → The blank pair is ignored rather than rejected or read as a clear; a vat_number sent alongside it still takes effect. Do not send a blank pair to clear an identifier; send a supported kind with a blank value instead.</description></item>
    ///   <item><description>Only the legacy vat_number (and vat_country) is sent, with neither entity identifier field → The kind is derived from vat_country: an EU member state code or GB gives vat_eu, a national tax country code gives national_tax, and a blank or unrecognized country gives company_reg. Send vat_country with a legacy vat_number, or send entity_identifier_kind explicitly, so the derived kind is the one you intend.</description></item>
    ///   <item><description>The customer holds an entity identifier of any kind → The response reports the identifier in entity_identifier_kind and entity_identifier_value and repeats its value in vat_number, whatever its kind. Read the identifier's kind from entity_identifier_kind; do not treat vat_number as a VAT number when the kind is not vat_eu.</description></item>
    /// </list>
    /// </remarks>
    public Task<CustomerResponse> CreateCustomer(CreateCustomerOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/customers.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<CustomerResponse>(),
            CreateCustomerError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Customer
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes the customer.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A customer is deleted explicitly as a single action → A customer_delete webhook is sent; it is not sent when a subscription and its customer are deleted together. Do not expect customer_delete when the customer is removed together with a subscription; reconcile those deletions separately.</description></item>
    /// </list>
    /// </remarks>
    public Task DeleteCustomer(DeleteCustomerRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/customers/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Customer Subscriptions
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists all subscriptions that belong to a customer.
    /// <para>
    ///  If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, subscriptions no longer require an associated product. For subscriptions without an associated product, 'product', 'product_price_point_id', and 'product_price_point_type' are returned as 'null'.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has the new Catalog experience enabled and a customer's subscription has no associated product → product, product_price_point_id and product_price_point_type are returned as null for that subscription. Handle null product and price point fields on listed subscriptions.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<SubscriptionResponse>> ListCustomerSubscriptions(ListCustomerSubscriptionsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/customers/{customer_id}/subscriptions.json"),
            [new TemplateParam("customer_id", request.CustomerId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<SubscriptionResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List or Find Customers
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="CustomerResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists all customers associated with your site, or filters results using the search parameter.
    /// <para>
    /// ## Find Customer
    /// </para>
    /// <para>
    /// Use the search feature with the <c>q</c> query parameter to retrieve an array of customers that matches the search query.
    /// </para>
    /// <para>
    /// Common use cases are:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Search by an email</description></item>
    ///   <item><description>Search by an Advanced Billing ID</description></item>
    ///   <item><description>Search by an organization</description></item>
    ///   <item><description>Search by a reference value from your application</description></item>
    ///   <item><description>Search by a first or last name</description></item>
    /// </list>
    /// <para>
    /// To retrieve a single, exact match by reference, use the <see href="https://developers.chargify.com/docs/api-docs/b710d8fbef104-read-customer-by-reference">lookup endpoint</see>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>date_field is given a value other than created_at or updated_at → The value is ignored and the date filters are not applied. Send date_field as created_at or updated_at only, and check that date-filtered results really are filtered.</description></item>
    ///   <item><description>The q query parameter is sent → Customers whose email, Advanced Billing ID, organization, reference, or first or last name match the query are returned. Expect q to match across several fields; filter the results yourself if you need one field.</description></item>
    ///   <item><description>Customers are listed → Results are sorted by time of creation, in the order set by direction. Use direction to control creation-time order when paging through customers.</description></item>
    ///   <item><description>start_date or end_date (date only) is sent to filter by date_field → start_date matches records at or after midnight and end_date matches records up to and including 11:59:59 PM, both in the site's time zone. Convert date filters to the site's time zone, or use start_datetime/end_datetime with an explicit offset.</description></item>
    ///   <item><description>per_page is sent above 200 → per_page is changed to 200. Page with per_page of at most 200 and expect no more than 200 records per page.</description></item>
    ///   <item><description>page asks for a page past the last page of results → An empty result set is returned. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include a UTC offset in start_datetime/end_datetime when the site's time zone is not the one you mean.</description></item>
    ///   <item><description>date_field is sent with a value other than created_at or updated_at → The value is ignored and the date filters are not applied; matching records are returned unfiltered by date. Send a supported date_field value and check that the date filter was applied.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>start_datetime is sent with start_date, or end_datetime with end_date → The datetime parameter is used instead of the date parameter. Send only one of each date/datetime pair.</description></item>
    ///   <item><description>A customer is looked up by its reference → The lookup endpoint returns a single exact match, while the list search with q returns an array of customers matching the query. Use the lookup endpoint, not the q search, when you need the one customer with an exact reference.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<CustomerResponse>> ListCustomers(ListCustomersRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/customers.json"),
            [],
            [
                new Param("direction", request.Direction),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("date_field", request.DateField),
                new Param("start_date", request.StartDate),
                new Param("end_date", request.EndDate),
                new Param("start_datetime", request.StartDatetime),
                new Param("end_datetime", request.EndDatetime),
                new Param("q", request.Q),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<CustomerResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Customer
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CustomerResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves the Customer properties by Advanced Billing-generated Customer ID.
    /// </remarks>
    public Task<CustomerResponse> ReadCustomer(ReadCustomerRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/customers/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<CustomerResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Customer by Reference
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CustomerResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a customer by their unique reference ID. It will return a single match.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A customer is looked up by its reference → The lookup endpoint returns a single exact match, while the list search with q returns an array of customers matching the query. Use the lookup endpoint, not the q search, when you need the one customer with an exact reference.</description></item>
    /// </list>
    /// </remarks>
    public Task<CustomerResponse> ReadCustomerByReference(ReadCustomerByReferenceRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/customers/lookup.json"),
            [],
            [new Param("reference", request.Reference)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<CustomerResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Customer
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CustomerResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateCustomerError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the customer.
    /// <para>
    /// ## Tax and Business Identifiers
    /// </para>
    /// <para>
    /// Send <c>entity_identifier_kind</c> and <c>entity_identifier_value</c> together to store the customer's tax or business identifier, such as an EU VAT number, a French SIREN, or a LEI. A customer holds one identifier at a time, so saving an identifier of a different kind replaces the existing one.
    /// </para>
    /// <para>
    /// The <c>vat_eu</c> and <c>national_tax</c> kinds also require <c>vat_country</c>. An unsupported kind, a missing or mismatched <c>vat_country</c>, or a <c>gln</c>, <c>duns</c>, or <c>lei</c> value in the wrong format returns <c>422</c>.
    /// </para>
    /// <para>
    /// Always send the kind. <c>entity_identifier_value</c> on its own is stored as a <c>company_reg</c> when no <c>vat_country</c> is present, and returns <c>422</c> naming <c>entity_identifier_kind</c> when one is.
    /// </para>
    /// <para>
    /// To clear an identifier, send a supported <c>entity_identifier_kind</c> with a blank <c>entity_identifier_value</c>, or send a blank <c>vat_number</c> on its own. The first form also clears <c>vat_number</c> and <c>vat_country</c>, and it removes whichever identifier the customer holds, whatever kind you send with it.
    /// </para>
    /// <para>
    /// The legacy <c>vat_number</c> and <c>vat_country</c> pair still works on its own. When neither entity identifier field is sent, Advanced Billing derives the kind from <c>vat_country</c>: an EU member state code or <c>GB</c> gives <c>vat_eu</c>, one of the national tax country codes gives <c>national_tax</c>, and a blank or unrecognized country gives <c>company_reg</c>.
    /// </para>
    /// <para>
    /// Sending a customer response straight back leaves the tax ID alone. A blank pair, and a pair that still matches the stored identifier with <c>vat_country</c> unchanged, are read as nothing to change rather than as a request to clear. For <c>gln</c>, <c>duns</c>, and <c>lei</c> that also covers the <c>vat_number</c> the response mirrors back, so the kind survives the round trip.
    /// </para>
    /// <para>
    /// What you do change is applied, and the entity identifier fields take precedence over <c>vat_number</c>. A different kind or value writes that identifier, and <c>vat_number</c> and <c>vat_country</c> follow from it. A different <c>vat_country</c> next to an unchanged pair is a real edit, so it is validated and can return <c>422</c>. Changing only <c>vat_number</c> leaves the pair unchanged, so the derivation above decides the kind, which turns a <c>gln</c>, <c>duns</c>, or <c>lei</c> customer into a <c>company_reg</c>. Setting <c>vat_number</c> to <c>null</c> or a blank string still clears the identifier.
    /// </para>
    /// <para>
    /// The response reports the stored identifier in <c>entity_identifier_kind</c> and <c>entity_identifier_value</c>, and repeats its value in <c>vat_number</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>entity_identifier_kind is vat_eu or national_tax and no vat_country is sent → Maxio answers 422. Send vat_country whenever entity_identifier_kind is vat_eu or national_tax.</description></item>
    ///   <item><description>vat_country does not fit the identifier kind: vat_eu with a country that is not an EU member state or GB, or national_tax with a country outside AL, AM, AR, AU, BR, CA, CH, DZ, IN, MX, NO, NZ, ZA → Maxio answers 422. Check vat_country against the kind's allowed countries before sending the customer.</description></item>
    ///   <item><description>entity_identifier_kind is gln, duns or lei and the value is not 13 digits, 9 digits, or 18 letters or digits followed by 2 digits respectively → Maxio answers 422. Validate the identifier value's length and characters for gln, duns and lei before sending it.</description></item>
    ///   <item><description>entity_identifier_value is sent without entity_identifier_kind while a vat_country is present → Maxio answers 422 with "names entity_identifier_kind". Send entity_identifier_kind whenever entity_identifier_value is sent.</description></item>
    ///   <item><description>A different vat_country is sent next to a kind/value pair that is unchanged from what is stored → Maxio answers 422. Treat a vat_country change as a new identifier and make sure it fits the stored kind.</description></item>
    ///   <item><description>entity_identifier_value is sent with surrounding whitespace or lowercase letters → The value is trimmed and stored in uppercase. Compare stored identifiers in trimmed uppercase form, not as originally entered.</description></item>
    ///   <item><description>An identifier is stored through entity_identifier_kind with kind company_reg, gln, duns or lei → vat_country is set to null. Expect vat_country to be null after saving a company_reg, gln, duns or lei identifier, even if one was sent.</description></item>
    ///   <item><description>organization is omitted, null or an empty string → organization is populated with the customer's first and last name separated by a space. Do not treat a returned organization as a real company name unless your app set it.</description></item>
    ///   <item><description>surcharging is sent on a site where surcharging control is not enabled → The value is not applied, and surcharging is not included in the customer response. Confirm surcharging control is enabled on the site before relying on the customer's surcharging setting.</description></item>
    ///   <item><description>branding_theme_id is sent on a site where Branding Themes are not enabled → The field is available only when Branding Themes are enabled for the site. Send branding_theme_id only on sites with Branding Themes enabled.</description></item>
    ///   <item><description>On a site with Branding Themes enabled, a customer has a branding_theme_id and one of its subscriptions has no subscription-level Branding Theme → The customer's Branding Theme is used for that subscription. Expect changing the customer's branding_theme_id to change the branding of every subscription without its own theme.</description></item>
    ///   <item><description>A vat_number is added or updated on a customer → The number is not validated when saved; it is validated via VIES before taxes are calculated, and only a valid number allows VAT opt-out. Validate VAT numbers yourself if you need to catch bad ones at entry; a saved number does not mean VAT opt-out will apply.</description></item>
    ///   <item><description>tax_exempt is sent as 1 or 0 → 1 is accepted as true and 0 as false. Send tax_exempt as true/false or 1/0 only.</description></item>
    ///   <item><description>A subscriber is to be taxed but neither the customer nor the payment profile has a full valid address with an ISO 2-3 character state and ISO 2 character country → The subscriber is not eligible to be taxed. Send a complete billing or shipping address with ISO state and country codes for every customer who must be taxed.</description></item>
    ///   <item><description>entity_identifier_value is sent without entity_identifier_kind and no vat_country is present → The value is stored as a company_reg identifier. Always send entity_identifier_kind with entity_identifier_value so the kind is not chosen for you.</description></item>
    ///   <item><description>entity_identifier_kind and entity_identifier_value are both sent blank → The blank pair is ignored rather than rejected or read as a clear; a vat_number sent alongside it still takes effect. Do not send a blank pair to clear an identifier; send a supported kind with a blank value instead.</description></item>
    ///   <item><description>Only the legacy vat_number (and vat_country) is sent, with neither entity identifier field → The kind is derived from vat_country: an EU member state code or GB gives vat_eu, a national tax country code gives national_tax, and a blank or unrecognized country gives company_reg. Send vat_country with a legacy vat_number, or send entity_identifier_kind explicitly, so the derived kind is the one you intend.</description></item>
    ///   <item><description>The customer holds an entity identifier of any kind → The response reports the identifier in entity_identifier_kind and entity_identifier_value and repeats its value in vat_number, whatever its kind. Read the identifier's kind from entity_identifier_kind; do not treat vat_number as a VAT number when the kind is not vat_eu.</description></item>
    ///   <item><description>An identifier of a different kind is saved on a customer that already holds one → The new identifier replaces the existing one; a customer holds one identifier at a time. Store only one tax or business identifier per customer and expect the old one to be gone after a kind change.</description></item>
    ///   <item><description>A supported entity_identifier_kind is sent with a blank entity_identifier_value → The identifier is cleared along with vat_number and vat_country, whatever identifier the customer holds and whatever kind was sent. Send a kind with a blank value only when you mean to remove the customer's identifier of any kind.</description></item>
    ///   <item><description>vat_number is sent on its own as null or a blank string → The customer's identifier is cleared. Do not send a blank or null vat_number unless you mean to clear the customer's identifier.</description></item>
    ///   <item><description>The kind/value pair sent matches the stored identifier and vat_country is unchanged, as when a customer response is sent straight back → Nothing is changed; for gln, duns and lei the mirrored vat_number is also read as no change, so the kind survives the round trip. You may send a read customer back unchanged without losing its identifier.</description></item>
    ///   <item><description>The entity identifier fields and vat_number are both sent and the kind or value differs from what is stored → The entity identifier fields take precedence; the new identifier is written and vat_number and vat_country follow from it. Set the identifier through entity_identifier_kind and entity_identifier_value, not vat_number, when both are present.</description></item>
    ///   <item><description>Only vat_number is changed on a customer whose identifier kind is gln, duns or lei → The kind is re-derived from vat_country, turning the customer into a company_reg. Change a gln, duns or lei identifier through entity_identifier_kind and entity_identifier_value, not vat_number alone.</description></item>
    ///   <item><description>verified (ACH verification) is sent for a customer on a site whose gateway is not Authorize.Net → The field is available only on the Authorize.Net gateway. Send verified only when the site uses the Authorize.Net gateway.</description></item>
    ///   <item><description>A customer update changes first_name, last_name, organization, email, reference, address, address 2, city, state, zip, country, phone, vat_number, parent_id or cc_email → A customer_update webhook is sent; changes to other customer fields do not trigger it. Do not rely on customer_update to learn of changes to fields outside the listed set; read the customer back instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<CustomerResponse> UpdateCustomer(UpdateCustomerOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/customers/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<CustomerResponse>(),
            UpdateCustomerError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
