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
