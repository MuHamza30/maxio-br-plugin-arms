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
using MaxioAdvancedBilling.Requests.Coupons;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Coupon object defines a discount, either a percentage or a fixed amount, that can be applied to a subscription to reduce the subscription's billing amount.
/// <para>
/// Coupons can be restricted to a specific Product Family, limited to a date range, and configured to apply once or recur over a set duration. For more information, see <see href="https://docs.maxio.com/billing/catalog/coupons/understand-coupons">Understand Coupons</see>.
/// </para>
/// </summary>
public sealed class Coupons
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Coupons(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Archive Coupon
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CouponResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Archives a coupon, making it unavailable for future use while remaining active on existing subscriptions.
    /// Archiving makes that Coupon unavailable for future use, but allows it to remain attached and functional on existing Subscriptions that are using it.
    /// The <c>archived_at</c> date and time will be assigned.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A coupon is archived → The coupon becomes unavailable for future use but stays attached and working on existing subscriptions; archived_at is set. Remove the coupon from existing subscriptions separately if its discount must stop.</description></item>
    /// </list>
    /// </remarks>
    public Task<CouponResponse> ArchiveCoupon(ArchiveCouponRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/coupons/{coupon_id}.json"),
            [
                new TemplateParam("product_family_id", request.ProductFamilyId),
                new TemplateParam("coupon_id", request.CouponId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<CouponResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Coupon
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CouponResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateCouponError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a coupon under the specified product family.
    /// <para>
    /// You can create either a flat amount coupon, by specifying <c>amount_in_cents</c>, or percentage coupon by specifying <c>percentage</c>.
    /// </para>
    /// <para>
    /// See <see href="https://maxio.zendesk.com/hc/en-us/articles/24261259337101-Coupons-and-Subscriptions">Apply Coupons to Subscriptions</see> for information on applying a coupon to a subscription in the Advanced Billing UI.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A new coupon is created without name, code or description → the outcome is not stated. Send name, code and description when creating a coupon.</description></item>
    ///   <item><description>The coupon code contains characters other than uppercase letters, digits and % @ + - _  → the outcome is not stated. Build coupon codes from uppercase letters, digits and the allowed special characters only.</description></item>
    ///   <item><description>A new coupon is created with neither percentage nor amount_in_cents → the outcome is not stated. Send percentage for a percentage coupon or amount_in_cents for a flat amount coupon.</description></item>
    ///   <item><description>percentage and amount_in_cents are both sent → the outcome is not stated. Send only one of percentage and amount_in_cents.</description></item>
    ///   <item><description>allow_negative_balance is set to true together with restricted_products or restricted_components → the outcome is not stated. Do not combine allow_negative_balance with product or component restrictions.</description></item>
    ///   <item><description>end_date is in the past → Maxio answers 422 with "Expiration Date cannot be in the past". Send an end_date of today or later, or none.</description></item>
    ///   <item><description>The coupon amount is negative → Maxio answers 422 with "Amount: must be greater than or equal to $0.00.". Send a coupon amount of zero or more.</description></item>
    ///   <item><description>name, code or description is longer than 255 characters → the outcome is not stated. Keep coupon name, code and description to 255 characters or fewer.</description></item>
    ///   <item><description>compounding_strategy is set on a coupon that is not stackable → compounding_strategy applies only to stackable coupons. Set stackable true when the compounding strategy matters.</description></item>
    ///   <item><description>A stackable percentage coupon is combined with other discounts → With compound the percentage is taken from the price remaining after prior discounts; with full-price it is taken from the original item price. Choose compound or full-price deliberately and compute expected totals the same way.</description></item>
    ///   <item><description>allow_negative_balance is true and the discount exceeds the amount billed → The discount is not limited; the excess becomes credit that carries forward to the next billing. Set allow_negative_balance only when unused discount should carry forward as credit.</description></item>
    ///   <item><description>A coupon has an end_date → After the end of that day the code is invalid for new signups; recurring discounts started before the date keep recurring after it. Do not use end_date to stop discounts already running; it only blocks new signups.</description></item>
    /// </list>
    /// </remarks>
    public Task<CouponResponse> CreateCoupon(CreateCouponRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/coupons.json"),
            [new TemplateParam("product_family_id", request.ProductFamilyId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<CouponResponse>(),
            CreateCouponError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Coupon Subcodes
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CouponSubcodesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates subcodes for an existing coupon.
    /// <para>
    /// Coupon Subcodes allow you to create a set of unique codes that allow you to expand the use of one coupon.
    /// </para>
    /// <para>
    /// For example:
    /// </para>
    /// <para>
    /// Master Coupon Code:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>SPRING2020</description></item>
    /// </list>
    /// <para>
    /// Coupon Subcodes:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>SPRING90210</description></item>
    ///   <item><description>DP80302</description></item>
    ///   <item><description>SPRINGBALTIMORE</description></item>
    /// </list>
    /// <para>
    /// When creating a coupon subcode, you must specify a coupon to attach it to using the coupon_id. Valid coupon subcodes are all capital letters, contain only letters and numbers, and do not have any spaces. Lowercase letters are capitalized before the subcode is created.
    /// </para>
    /// <para>
    /// Note: If you are using any of the allowed special characters ("%", "@", "+", "-", "_", and "."), you must encode them for use in the URL.
    /// </para>
    /// <para>
    ///     % to %25
    ///     @ to %40
    ///     + to %2B
    ///     - to %2D
    ///     _ to %5F
    ///     . to %2E
    /// </para>
    /// <para>
    /// So, if the coupon subcode is <c>20%OFF</c>, the URL to delete this coupon subcode would be: <c>https://&lt;subdomain&gt;.chargify.com/coupons/567/codes/20%25OFF.&lt;format&gt;</c>.
    /// </para>
    /// <para>
    /// For more information on coupon codes and applying coupons to subscriptions, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24261208729229-Coupon-Codes">Coupon Codes</see> and <see href="https://maxio.zendesk.com/hc/en-us/articles/24261259337101-Coupons-and-Subscriptions">Coupons and Subscriptions</see>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A subcode is sent with lowercase letters → The lowercase letters are capitalized before the subcode is created. Store and compare subcodes in uppercase.</description></item>
    ///   <item><description>A subcode in the list is not valid (contains spaces or disallowed characters) → That subcode is not created and is reported among the invalid codes in the response; the rest are processed. Check invalid_codes in the response instead of assuming every subcode was created.</description></item>
    /// </list>
    /// </remarks>
    public Task<CouponSubcodesResponse> CreateCouponSubcodes(CreateCouponSubcodesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/coupons/{coupon_id}/codes.json"),
            [new TemplateParam("coupon_id", request.CouponId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<CouponSubcodesResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create / Update Currency Prices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CouponCurrencyResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateOrUpdateCouponCurrencyPricesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates and/or updates currency prices for an existing coupon. Multiple prices can be created or updated in a single request but each of the currencies must be defined on the site level already and the coupon must be an amount-based coupon, not percentage.
    /// <para>
    /// Currency pricing for coupons must mirror the setup of the primary coupon pricing - if the primary coupon is percentage based, you will not be able to define pricing in non-primary currencies.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Currency prices are sent for a percentage-based coupon → Maxio answers 422 with "Cannot create prices for a percentage-based coupon.". Set currency prices only on amount-based coupons.</description></item>
    ///   <item><description>A currency price is sent for a currency not defined at the site level → Maxio answers 422 with "Currency must be defined on the site level.". Define the currency on the site before setting coupon prices in it.</description></item>
    /// </list>
    /// </remarks>
    public Task<CouponCurrencyResponse> CreateOrUpdateCouponCurrencyPrices(CreateOrUpdateCouponCurrencyPricesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/coupons/{coupon_id}/currency_prices.json"),
            [new TemplateParam("coupon_id", request.CouponId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<CouponCurrencyResponse>(),
            CreateOrUpdateCouponCurrencyPricesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Coupon Subcode
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeleteCouponSubcodeError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes a specific subcode from a coupon.
    /// <example>
    /// Given a coupon with an ID of 567, and a coupon subcode of 20OFF, the URL to <c>DELETE</c> this coupon subcode would be:
    /// <code>
    /// http://subdomain.chargify.com/coupons/567/codes/20OFF.&lt;format&gt;
    /// </code>
    /// <para>
    /// Note: If you are using any of the allowed special characters (“%”, “@”, “+”, “-”, “_”, and “.”), you must encode them for use in the URL.
    /// </para>
    /// <para>
    /// | Special character | Encoding |
    /// |-------------------|----------|
    /// | %                 | %25      |
    /// | @                 | %40      |
    /// | +                 | %2B      |
    /// | –                 | %2D      |
    /// | _                 | %5F      |
    /// | .                 | %2E      |
    /// </para>
    /// <para>
    /// ## Percent Encoding Example
    /// </para>
    /// <para>
    /// Or if the coupon subcode is 20%OFF, the URL to delete this coupon subcode would be: @https://&lt;subdomain&gt;.chargify.com/coupons/567/codes/20%25OFF.&lt;format&gt;.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The subcode in the URL path contains one of the special characters % @ + - _ or . without percent-encoding → the outcome is not stated. Percent-encode % @ + - _ and . in the subcode path segment (for example 20%OFF as 20%25OFF).</description></item>
    /// </list>
    /// </example>
    /// </remarks>
    public Task DeleteCouponSubcode(DeleteCouponSubcodeRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/coupons/{coupon_id}/codes/{subcode}.json"),
            [new TemplateParam("coupon_id", request.CouponId), new TemplateParam("subcode", request.Subcode)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            DeleteCouponSubcodeError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Find Coupon
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CouponResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Searches for a coupon by code.
    /// <para>
    /// If you have more than one product family and if the coupon you are trying to find does not belong to the default product family in your site, you need to specify (either in the URL or as a query string param) the <c>product_family_id</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has more than one product family and the coupon is not in the default (first-created, unarchived) product family, and no product family is given → the outcome is not stated. Pass product_family_id (or the product family in the URL, by id or handle:&lt;handle&gt;) when the coupon may be outside the default family.</description></item>
    ///   <item><description>currency_prices=true is sent on a site with multiple currencies defined → Each coupon includes an array of currency price data; without the parameter it is not returned. Send currency_prices=true when you need per-currency coupon amounts.</description></item>
    /// </list>
    /// </remarks>
    public Task<CouponResponse> FindCoupon(FindCouponRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/coupons/find.json"),
            [],
            [
                new Param("product_family_id", request.ProductFamilyId),
                new Param("code", request.Code),
                new Param("currency_prices", request.CurrencyPrices),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<CouponResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Coupon Subcodes
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CouponSubcodes"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the subcodes attached to a coupon.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>per_page is sent above 200 → per_page is changed to 200. Page with per_page of at most 200 and expect no more than 200 records per page.</description></item>
    ///   <item><description>page asks for a page past the last page of results → An empty result set is returned. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
    /// </remarks>
    public Task<CouponSubcodes> ListCouponSubcodes(ListCouponSubcodesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/coupons/{coupon_id}/codes.json"),
            [new TemplateParam("coupon_id", request.CouponId)],
            [new Param("page", request.Page), new Param("per_page", request.PerPage)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<CouponSubcodes>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Coupons
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="CouponResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists coupons for a site.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Coupons are listed for the site → Coupons from all product families are returned, not only the default product family. Page through the results, and use the product-family coupon list when you need one family only.</description></item>
    ///   <item><description>currency_prices=true is sent on a site with multiple currencies defined → Each coupon includes an array of currency price data; without the parameter it is not returned. Send currency_prices=true when you need per-currency coupon amounts.</description></item>
    ///   <item><description>filter[use_site_exchange_rate] is sent → true returns only coupons priced from the site's current exchange rates (on-the-fly currency_prices); false returns only coupons with manually defined amounts per currency (stored currency_prices). Omit the filter to get both kinds of coupons.</description></item>
    ///   <item><description>start_date or end_date (date only) is sent to filter by date_field → start_date matches records at or after midnight and end_date matches records up to and including 11:59:59 PM, both in the site's time zone. Convert date filters to the site's time zone, or use start_datetime/end_datetime with an explicit offset.</description></item>
    ///   <item><description>per_page is sent above 200 → per_page is changed to 200. Page with per_page of at most 200 and expect no more than 200 records per page.</description></item>
    ///   <item><description>page asks for a page past the last page of results → An empty result set is returned. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include a UTC offset in start_datetime/end_datetime when the site's time zone is not the one you mean.</description></item>
    ///   <item><description>date_field is sent with a value other than created_at or updated_at → The value is ignored and the date filters are not applied; matching records are returned unfiltered by date. Send a supported date_field value and check that the date filter was applied.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>start_datetime is sent with start_date, or end_datetime with end_date → The datetime parameter is used instead of the date parameter. Send only one of each date/datetime pair.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<CouponResponse>> ListCoupons(ListCouponsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/coupons.json"),
            [],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("filter", request.Filter),
                new Param("currency_prices", request.CurrencyPrices),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<CouponResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Coupons for Product Family
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="CouponResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists coupons for a specific product family in a site.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>currency_prices=true is sent on a site with multiple currencies defined → Each coupon includes an array of currency price data; without the parameter it is not returned. Send currency_prices=true when you need per-currency coupon amounts.</description></item>
    ///   <item><description>filter[use_site_exchange_rate] is sent → true returns only coupons priced from the site's current exchange rates (on-the-fly currency_prices); false returns only coupons with manually defined amounts per currency (stored currency_prices). Omit the filter to get both kinds of coupons.</description></item>
    ///   <item><description>start_date or end_date (date only) is sent to filter by date_field → start_date matches records at or after midnight and end_date matches records up to and including 11:59:59 PM, both in the site's time zone. Convert date filters to the site's time zone, or use start_datetime/end_datetime with an explicit offset.</description></item>
    ///   <item><description>per_page is sent above 200 → per_page is changed to 200. Page with per_page of at most 200 and expect no more than 200 records per page.</description></item>
    ///   <item><description>page asks for a page past the last page of results → An empty result set is returned. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include a UTC offset in start_datetime/end_datetime when the site's time zone is not the one you mean.</description></item>
    ///   <item><description>date_field is sent with a value other than created_at or updated_at → The value is ignored and the date filters are not applied; matching records are returned unfiltered by date. Send a supported date_field value and check that the date filter was applied.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>start_datetime is sent with start_date, or end_datetime with end_date → The datetime parameter is used instead of the date parameter. Send only one of each date/datetime pair.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<CouponResponse>> ListCouponsForProductFamily(ListCouponsForProductFamilyRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/coupons.json"),
            [new TemplateParam("product_family_id", request.ProductFamilyId)],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("filter", request.Filter),
                new Param("currency_prices", request.CurrencyPrices),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<CouponResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Coupon
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CouponResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a coupon by its system-assigned ID. You must identify the Coupon in this call by the ID parameter assigned to it.
    /// <para>
    /// If instead you would like to find a Coupon using a Coupon code, use the <see href="$e/Coupons/findCoupon">Find Coupon</see> endpoint.
    /// </para>
    /// <para>
    /// If the coupon is set to <c>use_site_exchange_rate: true</c>, it returns pricing based on the current exchange rate. If the flag is set to false, it returns all of the defined prices for each currency.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>currency_prices=true is sent on a site with multiple currencies defined → Each coupon includes an array of currency price data; without the parameter it is not returned. Send currency_prices=true when you need per-currency coupon amounts.</description></item>
    ///   <item><description>A coupon is read → With use_site_exchange_rate true the pricing is based on the current exchange rate; with false all defined prices for each currency are returned. Expect exchange-rate coupon prices to change between reads.</description></item>
    /// </list>
    /// </remarks>
    public Task<CouponResponse> ReadCoupon(ReadCouponRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/coupons/{coupon_id}.json"),
            [
                new TemplateParam("product_family_id", request.ProductFamilyId),
                new TemplateParam("coupon_id", request.CouponId),
            ],
            [new Param("currency_prices", request.CurrencyPrices)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<CouponResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Coupon Usages
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="CouponUsage"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists coupon usage details, one entry per product.
    /// </remarks>
    public Task<IReadOnlyList<CouponUsage>> ReadCouponUsage(ReadCouponUsageRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/coupons/{coupon_id}/usage.json"),
            [
                new TemplateParam("product_family_id", request.ProductFamilyId),
                new TemplateParam("coupon_id", request.CouponId),
            ],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<CouponUsage>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Coupon
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CouponResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateCouponError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates a coupon.
    /// <para>
    /// You can restrict a coupon to only apply to specific products / components by optionally passing in hashes of <c>restricted_products</c> and/or <c>restricted_components</c> in the format:
    /// <c>{ "&lt;product/component_id&gt;": boolean_value }</c>
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The coupon code contains characters other than uppercase letters, digits and % @ + - _  → the outcome is not stated. Build coupon codes from uppercase letters, digits and the allowed special characters only.</description></item>
    ///   <item><description>percentage and amount_in_cents are both sent → the outcome is not stated. Send only one of percentage and amount_in_cents.</description></item>
    ///   <item><description>allow_negative_balance is set to true together with restricted_products or restricted_components → the outcome is not stated. Do not combine allow_negative_balance with product or component restrictions.</description></item>
    ///   <item><description>end_date is in the past → Maxio answers 422 with "Expiration Date cannot be in the past". Send an end_date of today or later, or none.</description></item>
    ///   <item><description>The coupon amount is negative → Maxio answers 422 with "Amount: must be greater than or equal to $0.00.". Send a coupon amount of zero or more.</description></item>
    ///   <item><description>name, code or description is longer than 255 characters → the outcome is not stated. Keep coupon name, code and description to 255 characters or fewer.</description></item>
    ///   <item><description>The coupon code is changed to a code another coupon already uses → Maxio answers 422 with "Code: must be unique - that value has been taken.". Use a coupon code no other coupon on the site has.</description></item>
    ///   <item><description>The code is changed on a coupon that belongs to one or more active offers → Maxio answers 422 with "Code cannot be changed because coupon belongs to one or more active offers: offer-1". Archive or remove the coupon from active offers before changing its code, or leave the code unchanged.</description></item>
    ///   <item><description>compounding_strategy is set on a coupon that is not stackable → compounding_strategy applies only to stackable coupons. Set stackable true when the compounding strategy matters.</description></item>
    ///   <item><description>A stackable percentage coupon is combined with other discounts → With compound the percentage is taken from the price remaining after prior discounts; with full-price it is taken from the original item price. Choose compound or full-price deliberately and compute expected totals the same way.</description></item>
    ///   <item><description>allow_negative_balance is true and the discount exceeds the amount billed → The discount is not limited; the excess becomes credit that carries forward to the next billing. Set allow_negative_balance only when unused discount should carry forward as credit.</description></item>
    ///   <item><description>A coupon has an end_date → After the end of that day the code is invalid for new signups; recurring discounts started before the date keep recurring after it. Do not use end_date to stop discounts already running; it only blocks new signups.</description></item>
    /// </list>
    /// </remarks>
    public Task<CouponResponse> UpdateCoupon(UpdateCouponRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/coupons/{coupon_id}.json"),
            [
                new TemplateParam("product_family_id", request.ProductFamilyId),
                new TemplateParam("coupon_id", request.CouponId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<CouponResponse>(),
            UpdateCouponError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Coupon Subcodes
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CouponSubcodesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the subcodes for a coupon, replacing all existing subcodes with the new list.
    /// Send an array of new coupon subcodes.
    /// <para>
    /// <b>Note</b>: All current subcodes for that Coupon will be deleted first, and replaced with the list of subcodes sent to this endpoint.
    /// The response will contain:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The created subcodes,</description></item>
    /// </list>
    /// <list type="bullet">
    ///   <item><description>Subcodes that were not created because they already exist,</description></item>
    /// </list>
    /// <list type="bullet">
    ///   <item><description>Any subcodes not created because they are invalid.</description></item>
    /// </list>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A subcode in the list is not valid (contains spaces or disallowed characters) → That subcode is not created and is reported among the invalid codes in the response; the rest are processed. Check invalid_codes in the response instead of assuming every subcode was created.</description></item>
    ///   <item><description>A list of subcodes is sent to update a coupon's subcodes → All current subcodes of the coupon are deleted first and replaced with the list sent. Send the complete list of subcodes to keep; use create to add subcodes without removing others.</description></item>
    ///   <item><description>A subcode in the list already exists → That subcode is not created and is reported in the response as already existing. Read the duplicate list in the response to see which subcodes were not created.</description></item>
    /// </list>
    /// </remarks>
    public Task<CouponSubcodesResponse> UpdateCouponSubcodes(UpdateCouponSubcodesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/coupons/{coupon_id}/codes.json"),
            [new TemplateParam("coupon_id", request.CouponId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<CouponSubcodesResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Validate Coupon
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CouponResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ValidateCouponError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Verifies whether a specific coupon code is valid. This method is useful for validating coupon codes that are entered by a customer.
    /// <para>
    /// If you have more than one product family and if the coupon you are validating does not belong to the first product family in your site, you need to specify the product family, either in the URL or as a query string param. This can be done by supplying the id or the handle in the <c>handle:my-family</c> format.
    /// </para>
    /// <para>
    /// Supplying the <c>product_family_handle</c> in the URL:
    /// </para>
    /// <code>
    /// https://&lt;subdomain&gt;.chargify.com/product_families/handle:&lt;product_family_handle&gt;/coupons/validate.&lt;format&gt;?code=&lt;coupon_code&gt;
    /// </code>
    /// <para>
    /// Supplying the <c>product_family_id</c> as a query parameter:
    /// </para>
    /// <code>
    /// https://&lt;subdomain&gt;.chargify.com/coupons/validate.&lt;format&gt;?code=&lt;coupon_code&gt;&amp;product_family_id=&lt;id&gt;
    /// </code>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has more than one product family and the coupon is not in the default (first-created, unarchived) product family, and no product family is given → the outcome is not stated. Pass product_family_id (or the product family in the URL, by id or handle:&lt;handle&gt;) when the coupon may be outside the default family.</description></item>
    ///   <item><description>The coupon code being validated is not found → Maxio answers 404 with "Coupon code could not be found.". Show the customer an invalid-code message on 404 rather than treating it as a failure of the service.</description></item>
    /// </list>
    /// </remarks>
    public Task<CouponResponse> ValidateCoupon(ValidateCouponRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/coupons/validate.json"),
            [],
            [new Param("code", request.Code), new Param("product_family_id", request.ProductFamilyId)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<CouponResponse>(),
            ValidateCouponError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
