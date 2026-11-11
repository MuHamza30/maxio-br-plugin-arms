using System;
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
using MaxioAdvancedBilling.Requests.ComponentPricePoints;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Component Price Point object defines a specific pricing configuration for a Component, including the price point's rate structure and billing interval.
/// <para>
/// A single Component can have multiple price points, allowing different subscriptions to be billed for the same component under different terms.
/// </para>
/// </summary>
public sealed class ComponentPricePoints
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal ComponentPricePoints(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Archive Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ArchiveComponentPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Archives a component price point. Subscriptions using a price point that has been archived will continue using it until they're moved to another price point.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The price point is already archived → Maxio answers 422 with "Price point is already archived.". Check archived_at before archiving a price point.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A component price point is archived → Subscriptions using it keep using it until they are moved to another price point. Move subscriptions off the price point yourself if archiving should change what they pay.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentPricePointResponse> ArchiveComponentPricePoint(ArchiveComponentPricePointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentPricePointResponse>(),
            ArchiveComponentPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Bulk Create Component Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="BulkCreateComponentPricePointsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates multiple component price points in one request.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A bracket's ending_quantity is not greater than its starting quantity → Maxio answers 422 with "Ending quantity: must be greater than the low end bracket quantity, if given". Make each bracket's ending_quantity greater than its starting_quantity, or leave it empty for an open top bracket.</description></item>
    ///   <item><description>A price point handle does not start with a letter or number, has no letter, or contains characters other than lowercase letters, numbers, dashes and underscores → Maxio answers 422 with "Handle: must begin with a letter or a number, have at least one letter in it, and contain only lowercase letters, numbers, dashes and underscores.". Use price point handles of lowercase letters, numbers, dashes and underscores that start with a letter or number and contain a letter.</description></item>
    ///   <item><description>A unit price below zero is sent → Maxio answers 422 with "Unit price: must be greater than or equal to $0.00.". Send unit prices of zero or more.</description></item>
    ///   <item><description>use_site_exchange_rate or tax_included is sent when creating component price points in bulk → The setting is not supported in bulk creation. Create price points one at a time when you need to set use_site_exchange_rate or tax_included.</description></item>
    ///   <item><description>rollover_prepaid_remainder, renew_prepaid_allocation or the expiration settings are sent for a price point of a component that is not a prepaid usage component → These settings apply only to prepaid usage components. Send rollover_prepaid_remainder, renew_prepaid_allocation and the expiration settings only on price points of prepaid usage components.</description></item>
    ///   <item><description>A unit price is sent with more than 8 decimal places → the outcome is not stated. Send unit prices with at most 8 decimal places.</description></item>
    ///   <item><description>interval or interval_unit is sent for a component or component price point on a site without Multifrequency enabled → the outcome is not stated. Send interval and interval_unit only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>expiration_interval or expiration_interval_unit is sent while rollover_prepaid_remainder is not true → The expiration settings apply only to prepaid usage components that roll over remaining units. Send expiration_interval and expiration_interval_unit only together with rollover_prepaid_remainder: true.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentPricePointsResponse> BulkCreateComponentPricePoints(BulkCreateComponentPricePointsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/bulk.json"),
            [new TemplateParam("component_id", request.ComponentId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentPricePointsResponse>(),
            BulkCreateComponentPricePointsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Clone Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointCurrencyOverageResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CloneComponentPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Clones a component price point. Custom price points (tied to a specific subscription) cannot be cloned. The following attributes are copied from the source price point:
    /// - Pricing scheme
    /// - All price tiers (with starting/ending quantities and unit prices)
    /// - Tax included setting
    /// - Currency prices (if definitive pricing is set)
    /// - Overage pricing (for prepaid usage components)
    /// - Interval settings (if multi-frequency is enabled)
    /// - Event-based billing segments (if applicable)
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The source price point is a custom price point (tied to a subscription) → Maxio answers 422 with "Cannot clone a custom price point". Clone only catalog or default price points.</description></item>
    ///   <item><description>The source price point is archived → Maxio answers 422 with "Cannot clone an archived price point". Unarchive the price point before cloning it.</description></item>
    ///   <item><description>The cloned price point fails validation → Maxio answers 422 with "Cloning failed due to validation errors.". Check the clone's name and handle are valid and unused before cloning.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A component price point is cloned → The clone copies the pricing scheme, all price tiers (quantities and unit prices) and the tax included setting. Rely on the clone for these settings and update the clone afterwards for any changes.</description></item>
    ///   <item><description>A component price point is cloned → Currency prices are copied only if definitive pricing is set on the source. Create currency prices on the clone yourself when the source has no definitive pricing.</description></item>
    ///   <item><description>A price point of a prepaid usage component is cloned → Its overage pricing is copied too. Expect overage pricing on clones of prepaid usage price points.</description></item>
    ///   <item><description>A component price point is cloned → Interval settings are copied only if multi-frequency is enabled on the site. Do not expect interval settings on the clone unless the site has multi-frequency enabled.</description></item>
    ///   <item><description>A price point that has event-based billing segments is cloned → The event-based billing segments are copied. Expect segment pricing to carry over to the clone.</description></item>
    ///   <item><description>A cloned price point has currency prices → The clone response always includes them, with no query parameter needed. Read currency prices from the clone response directly.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentPricePointCurrencyOverageResponse> CloneComponentPricePoint(CloneComponentPricePointOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/clone.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentPricePointCurrencyOverageResponse>(),
            CloneComponentPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateComponentPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a price point for an existing component.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Price brackets overlap → Maxio answers 422 with "There cannot be any overlapping bracket ranges". Make price bracket ranges non-overlapping.</description></item>
    ///   <item><description>There is a quantity gap between successive price brackets → Maxio answers 422 with "Successive brackets may not have gaps in quantity (i.e. if one bracket ends at quantity 10, the next must begin at quantity 11)". Start each bracket at the previous bracket's ending_quantity plus 1.</description></item>
    ///   <item><description>A price point handle does not start with a letter or number, has no letter, or contains characters other than lowercase letters, numbers, dashes and underscores → Maxio answers 422 with "Handle: must begin with a letter or a number, have at least one letter in it, and contain only lowercase letters, numbers, dashes and underscores.". Use price point handles of lowercase letters, numbers, dashes and underscores that start with a letter or number and contain a letter.</description></item>
    ///   <item><description>A unit price below zero is sent → Maxio answers 422 with "Unit price: must be greater than or equal to $0.00.". Send unit prices of zero or more.</description></item>
    ///   <item><description>The primary pricing_scheme is per_unit while a secondary pricing model is configured on a hybrid-eligible component → Maxio refuses it with "Pricing scheme cannot be per_unit for hybrid pricing". Use volume, tiered or stairstep as the primary scheme when sending secondary pricing.</description></item>
    ///   <item><description>Secondary pricing is configured and the primary model's highest bracket has no ending_quantity → Maxio refuses it with "Prices primary pricing must have a finite included threshold for hybrid pricing". Give the primary model's top bracket a finite ending_quantity (the included threshold).</description></item>
    ///   <item><description>The primary model's highest bracket has no ending_quantity on a hybrid price point → Maxio refuses it with "Prices primary pricing must have a finite included threshold for hybrid pricing". Give the primary model's top bracket a finite ending_quantity.</description></item>
    ///   <item><description>A hybrid price point is billed → The customer sees one invoice line item per billing period for the component, combining primary and secondary charges. Expect one combined line item, not separate primary and overage lines, for hybrid price points.</description></item>
    ///   <item><description>overage_pricing is sent without overage_pricing_scheme → The whole overage_pricing block is silently ignored and a normal, non-hybrid price point is created with the primary pricing only. Always send overage_pricing_scheme together with overage_pricing when the price point should be hybrid.</description></item>
    ///   <item><description>rollover_prepaid_remainder, renew_prepaid_allocation or the expiration settings are sent for a price point of a component that is not a prepaid usage component → These settings apply only to prepaid usage components. Send rollover_prepaid_remainder, renew_prepaid_allocation and the expiration settings only on price points of prepaid usage components.</description></item>
    ///   <item><description>A price point is sent with overage_pricing but without overage_pricing_scheme → The whole overage_pricing block is silently ignored and the price point is saved as a normal, non-hybrid price point. Always send overage_pricing_scheme with overage_pricing when the price point should be hybrid.</description></item>
    ///   <item><description>A price point meets every Hybrid Pricing requirement (site feature, eligible component, bracketed primary scheme with a finite top bracket, secondary pricing configured) → It becomes a hybrid price point automatically (there is no hybrid flag) and bills primary and secondary pricing as one invoice line item. Treat a price point as hybrid from its configuration; do not look for or send a hybrid flag.</description></item>
    ///   <item><description>A component or price point is configured with secondary pricing on a site without the Hybrid Pricing feature or without Invoice-Centric Billing → The price point falls back to billing primary and secondary pricing as separate invoice line items. Confirm with Maxio that Hybrid Pricing and Invoice-Centric Billing are both enabled before relying on single-line hybrid billing.</description></item>
    ///   <item><description>Secondary pricing is configured on a component that is not quantity-based or metered, or on a metered component configured for event-based billing (metric, meter or formula) → The price point is not eligible for Hybrid Pricing. Use Hybrid Pricing only on quantity-based or non-event-based metered components.</description></item>
    ///   <item><description>Secondary (overage_pricing) brackets are sent for a hybrid price point → overage_pricing.prices is a separate bracket set starting at quantity 1; it only takes effect once usage passes the primary model's included threshold. Start overage_pricing brackets at 1, not at the primary model's ending_quantity.</description></item>
    ///   <item><description>A unit price is sent with more than 8 decimal places → the outcome is not stated. Send unit prices with at most 8 decimal places.</description></item>
    ///   <item><description>interval or interval_unit is sent for a component or component price point on a site without Multifrequency enabled → the outcome is not stated. Send interval and interval_unit only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>expiration_interval or expiration_interval_unit is sent while rollover_prepaid_remainder is not true → The expiration settings apply only to prepaid usage components that roll over remaining units. Send expiration_interval and expiration_interval_unit only together with rollover_prepaid_remainder: true.</description></item>
    ///   <item><description>A price point is configured with a secondary (overage) pricing model on a site without both the Hybrid Pricing feature and Invoice-Centric Billing enabled → The price point is not hybrid: primary and secondary pricing bill as separate invoice line items. Have the Maxio account team enable Hybrid Pricing and Invoice-Centric Billing before configuring hybrid price points.</description></item>
    ///   <item><description>Secondary pricing is configured on a component other than a quantity-based or non-event-based metered component → The component is not eligible for Hybrid Pricing. Use hybrid pricing only on quantity-based components or metered components not configured for event-based billing.</description></item>
    ///   <item><description>All hybrid requirements are met for a price point → The price point becomes hybrid automatically; there is no hybrid flag in the API. Check the requirements, not a flag, to know whether a price point is hybrid.</description></item>
    ///   <item><description>overage_pricing.prices brackets are defined for a hybrid price point → The secondary brackets are self-contained and start at 1, and they apply only to usage above the primary model's included threshold. Start overage_pricing.prices at starting_quantity 1, not at the primary ending_quantity.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentPricePointResponse> CreateComponentPricePoint(CreateComponentPricePointOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points.json"),
            [new TemplateParam("component_id", request.ComponentId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentPricePointResponse>(),
            CreateComponentPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Currency Prices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentCurrencyPricesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateCurrencyPricesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates currency prices for a given currency defined at the site level.
    /// <para>
    /// When creating currency prices, they need to mirror the structure of your primary pricing. For each price level defined on the component price point, there should be a matching price level created in the given currency.
    /// </para>
    /// <para>
    /// Note: Currency Prices are not able to be created for custom price points.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A currency price is sent for a currency that is not defined at the site level → Maxio answers 422 with "Currency: is not one of the allowed values.". Define the currency in the site settings before creating prices in it.</description></item>
    ///   <item><description>A currency price is sent without a currency → Maxio answers 422 with "Currency: cannot be blank.". Send currency on every currency price.</description></item>
    ///   <item><description>The currency prices do not mirror the price point's primary pricing (one price per price level) → the outcome is not stated. Send one currency price for each price level (price_id) of the component price point.</description></item>
    ///   <item><description>The price point is a custom price point → the outcome is not stated. Create currency prices only on non-custom price points.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentCurrencyPricesResponse> CreateCurrencyPrices(CreateCurrencyPricesOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/price_points/{price_point_id}/currency_prices.json"),
            [new TemplateParam("price_point_id", request.PricePointId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentCurrencyPricesResponse>(),
            CreateCurrencyPricesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List All Components Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListComponentsPricePointsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListAllComponentPricePointsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists all component price points belonging to a site.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>filter[start_date] (or another date filter) is not a valid ISO 8601 value → Maxio answers 422 with "start_date supplied value is invalid, expected ISO 8601 format". Send date filters in ISO 8601 format (YYYY-MM-DD, or a full timestamp for the datetime filters).</description></item>
    ///   <item><description>filter[type] is not sent → Only catalog and default price points are returned; custom price points are left out. Pass filter[type] including custom when custom price points are needed.</description></item>
    ///   <item><description>A price point is read or listed without the currency prices query parameter (currency_prices=true or include=currency_prices) → currency_prices is left out of the response; it appears only on read endpoints when requested. Pass the currency prices query parameter when you need currency pricing.</description></item>
    ///   <item><description>Currency prices are requested for a price point → With use_site_exchange_rate true the prices are computed from the current exchange rate; with false the defined prices for each currency are returned. Expect converted prices to change with the exchange rate when use_site_exchange_rate is true.</description></item>
    ///   <item><description>Currency prices are requested on a site without multiple currencies defined → Currency pricing data is available only when the site defines multiple currencies. Define extra currencies at the site level before relying on currency_prices.</description></item>
    ///   <item><description>A list is filtered with start_date or end_date → start_date matches from midnight and end_date up to and including 11:59:59 PM, both in the site's time zone. Convert date filters to the site's time zone, or use start_datetime/end_datetime with an explicit offset.</description></item>
    ///   <item><description>Both start_datetime and start_date (or end_datetime and end_date) are sent → The datetime parameter is used and the date parameter is ignored. Send either the date or the datetime form of each bound, not both.</description></item>
    ///   <item><description>per_page is omitted or set above 200 → 20 records are returned by default; a per_page above 200 is changed to 200. Page through results with per_page of at most 200 and do not assume a larger page was honoured.</description></item>
    ///   <item><description>A page number past the last page of results is requested → An empty result set is returned, not an error. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include an explicit time zone in start_datetime and end_datetime.</description></item>
    /// </list>
    /// </remarks>
    public Task<ListComponentsPricePointsResponse> ListAllComponentPricePoints(ListAllComponentPricePointsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components_price_points.json"),
            [],
            [
                new Param("include", request.Include),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("direction", request.Direction),
                new Param("filter", request.Filter),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListComponentsPricePointsResponse>(),
            ListAllComponentPricePointsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Component Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the price points associated with a component.
    /// <para>
    /// You may specify the component by using either the numeric id or the <c>handle:gold</c> syntax.
    /// </para>
    /// <para>
    /// If the price point is set to <c>use_site_exchange_rate: true</c>, it will return pricing based on the current exchange rate. If the flag is set to false, it will return all of the defined prices for each currency.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A price point is read or listed without the currency prices query parameter (currency_prices=true or include=currency_prices) → currency_prices is left out of the response; it appears only on read endpoints when requested. Pass the currency prices query parameter when you need currency pricing.</description></item>
    ///   <item><description>Currency prices are requested for a price point → With use_site_exchange_rate true the prices are computed from the current exchange rate; with false the defined prices for each currency are returned. Expect converted prices to change with the exchange rate when use_site_exchange_rate is true.</description></item>
    ///   <item><description>Currency prices are requested on a site without multiple currencies defined → Currency pricing data is available only when the site defines multiple currencies. Define extra currencies at the site level before relying on currency_prices.</description></item>
    ///   <item><description>The component is given in the path as handle:&lt;handle&gt; instead of a numeric id → The component is looked up by handle. Use either the numeric component id or handle:&lt;handle&gt; in the path.</description></item>
    ///   <item><description>per_page is omitted or set above 200 → 20 records are returned by default; a per_page above 200 is changed to 200. Page through results with per_page of at most 200 and do not assume a larger page was honoured.</description></item>
    ///   <item><description>A page number past the last page of results is requested → An empty result set is returned, not an error. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentPricePointsResponse> ListComponentPricePoints(ListComponentPricePointsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points.json"),
            [new TemplateParam("component_id", request.ComponentId)],
            [
                new Param("currency_prices", request.CurrencyPrices),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("filter[type]", request.FilterType),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentPricePointsResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Promote Price Point to Default
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Sets a new default price point for the component. This new default will apply to all new subscriptions going forward - existing subscriptions will remain on their current price point.
    /// <para>
    /// See <see href="https://maxio.zendesk.com/hc/en-us/articles/24261191737101-Price-Points-Components">Price Points Documentation</see> for more information on price points and moving subscriptions between price points.
    /// </para>
    /// <para>
    /// Note: Custom price points are not able to be set as the default for a component.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The price point to promote is a custom price point → the outcome is not stated. Promote only catalog price points to default, never custom ones.</description></item>
    ///   <item><description>A component price point is promoted to default → The new default applies only to new subscriptions; existing subscriptions stay on their current price point. Migrate existing subscriptions separately if they should move to the new default price point.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentResponse> PromoteComponentPricePointToDefault(PromoteComponentPricePointToDefaultRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/default.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointCurrencyOverageResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns details for a specific component price point. You can achieve this by using either the component price point ID or handle.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A price point is read or listed without the currency prices query parameter (currency_prices=true or include=currency_prices) → currency_prices is left out of the response; it appears only on read endpoints when requested. Pass the currency prices query parameter when you need currency pricing.</description></item>
    ///   <item><description>Currency prices are requested for a price point → With use_site_exchange_rate true the prices are computed from the current exchange rate; with false the defined prices for each currency are returned. Expect converted prices to change with the exchange rate when use_site_exchange_rate is true.</description></item>
    ///   <item><description>Currency prices are requested on a site without multiple currencies defined → Currency pricing data is available only when the site defines multiple currencies. Define extra currencies at the site level before relying on currency_prices.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentPricePointCurrencyOverageResponse> ReadComponentPricePoint(ReadComponentPricePointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [new Param("currency_prices", request.CurrencyPrices)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentPricePointCurrencyOverageResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Unarchive Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Unarchives a component price point.
    /// </remarks>
    public Task<ComponentPricePointResponse> UnarchiveComponentPricePoint(UnarchiveComponentPricePointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/unarchive.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentPricePointResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Component Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentPricePointResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateComponentPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates a component price point and its associated prices.
    /// <para>
    /// Passing in a price bracket without an <c>id</c> will attempt to create a new price.
    /// </para>
    /// <para>
    /// Including an <c>id</c> will update the corresponding price, and including the <c>_destroy</c> flag set to true along with the <c>id</c> will remove that price.
    /// </para>
    /// <para>
    /// Note: Custom price points cannot be updated directly. They must be edited through the Subscription.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A bracket's ending_quantity is not greater than its starting quantity → Maxio answers 422 with "Ending quantity: must be greater than the low end bracket quantity, if given". Make each bracket's ending_quantity greater than its starting_quantity, or leave it empty for an open top bracket.</description></item>
    ///   <item><description>Price brackets overlap → Maxio answers 422 with "There cannot be any overlapping bracket ranges". Make price bracket ranges non-overlapping.</description></item>
    ///   <item><description>There is a quantity gap between successive price brackets → Maxio answers 422 with "Successive brackets may not have gaps in quantity (i.e. if one bracket ends at quantity 10, the next must begin at quantity 11)". Start each bracket at the previous bracket's ending_quantity plus 1.</description></item>
    ///   <item><description>The primary pricing_scheme is per_unit while a secondary pricing model is configured on a hybrid-eligible component → Maxio refuses it with "Pricing scheme cannot be per_unit for hybrid pricing". Use volume, tiered or stairstep as the primary scheme when sending secondary pricing.</description></item>
    ///   <item><description>Secondary pricing is configured and the primary model's highest bracket has no ending_quantity → Maxio refuses it with "Prices primary pricing must have a finite included threshold for hybrid pricing". Give the primary model's top bracket a finite ending_quantity (the included threshold).</description></item>
    ///   <item><description>The primary model's highest bracket has no ending_quantity on a hybrid price point → Maxio refuses it with "Prices primary pricing must have a finite included threshold for hybrid pricing". Give the primary model's top bracket a finite ending_quantity.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A price id in the update does not belong to the price point being updated → Maxio answers 422 with "all prices must belong to the price point". Send only price ids read from this price point; handle 422 (not 404) for a foreign price id.</description></item>
    ///   <item><description>The price point being updated is a custom price point → the outcome is not stated. Edit custom price points through the subscription, not this endpoint.</description></item>
    ///   <item><description>The update leaves the price point with no price brackets → Maxio answers 422 with "At least 1 price bracket must be defined". Keep at least one price bracket on the price point.</description></item>
    ///   <item><description>The new price point handle is already used → Maxio answers 422 with "Handle: This handle is already in use.". Use a price point handle that is not already in use.</description></item>
    ///   <item><description>A hybrid price point is billed → The customer sees one invoice line item per billing period for the component, combining primary and secondary charges. Expect one combined line item, not separate primary and overage lines, for hybrid price points.</description></item>
    ///   <item><description>overage_pricing is sent without overage_pricing_scheme → The whole overage_pricing block is silently ignored and a normal, non-hybrid price point is created with the primary pricing only. Always send overage_pricing_scheme together with overage_pricing when the price point should be hybrid.</description></item>
    ///   <item><description>A price point is sent with overage_pricing but without overage_pricing_scheme → The whole overage_pricing block is silently ignored and the price point is saved as a normal, non-hybrid price point. Always send overage_pricing_scheme with overage_pricing when the price point should be hybrid.</description></item>
    ///   <item><description>A price point meets every Hybrid Pricing requirement (site feature, eligible component, bracketed primary scheme with a finite top bracket, secondary pricing configured) → It becomes a hybrid price point automatically (there is no hybrid flag) and bills primary and secondary pricing as one invoice line item. Treat a price point as hybrid from its configuration; do not look for or send a hybrid flag.</description></item>
    ///   <item><description>A component or price point is configured with secondary pricing on a site without the Hybrid Pricing feature or without Invoice-Centric Billing → The price point falls back to billing primary and secondary pricing as separate invoice line items. Confirm with Maxio that Hybrid Pricing and Invoice-Centric Billing are both enabled before relying on single-line hybrid billing.</description></item>
    ///   <item><description>Secondary pricing is configured on a component that is not quantity-based or metered, or on a metered component configured for event-based billing (metric, meter or formula) → The price point is not eligible for Hybrid Pricing. Use Hybrid Pricing only on quantity-based or non-event-based metered components.</description></item>
    ///   <item><description>Secondary (overage_pricing) brackets are sent for a hybrid price point → overage_pricing.prices is a separate bracket set starting at quantity 1; it only takes effect once usage passes the primary model's included threshold. Start overage_pricing brackets at 1, not at the primary model's ending_quantity.</description></item>
    ///   <item><description>A unit price is sent with more than 8 decimal places → the outcome is not stated. Send unit prices with at most 8 decimal places.</description></item>
    ///   <item><description>interval or interval_unit is sent for a component or component price point on a site without Multifrequency enabled → the outcome is not stated. Send interval and interval_unit only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>A price point is configured with a secondary (overage) pricing model on a site without both the Hybrid Pricing feature and Invoice-Centric Billing enabled → The price point is not hybrid: primary and secondary pricing bill as separate invoice line items. Have the Maxio account team enable Hybrid Pricing and Invoice-Centric Billing before configuring hybrid price points.</description></item>
    ///   <item><description>Secondary pricing is configured on a component other than a quantity-based or non-event-based metered component → The component is not eligible for Hybrid Pricing. Use hybrid pricing only on quantity-based components or metered components not configured for event-based billing.</description></item>
    ///   <item><description>All hybrid requirements are met for a price point → The price point becomes hybrid automatically; there is no hybrid flag in the API. Check the requirements, not a flag, to know whether a price point is hybrid.</description></item>
    ///   <item><description>overage_pricing.prices brackets are defined for a hybrid price point → The secondary brackets are self-contained and start at 1, and they apply only to usage above the primary model's included threshold. Start overage_pricing.prices at starting_quantity 1, not at the primary ending_quantity.</description></item>
    ///   <item><description>A price bracket is sent without an id → The server tries to create a new price on the price point. Include the id of each existing bracket you mean to change, or it is added as a new price.</description></item>
    ///   <item><description>A price bracket is sent with its id and _destroy set to true → That price is removed from the price point. Send the price's id with _destroy: true to remove that price bracket.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentPricePointResponse> UpdateComponentPricePoint(UpdateComponentPricePointOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentPricePointResponse>(),
            UpdateComponentPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Currency Prices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentCurrencyPricesResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateCurrencyPricesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates currency prices for a given currency defined at the site level.
    /// <para>
    /// Note: Currency Prices are not able to be updated for custom price points.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The price point is a custom price point → the outcome is not stated. Update currency prices only on non-custom price points.</description></item>
    ///   <item><description>Pricing already exists on the price point for the currency in the request → Maxio answers 422 with "Pricing already exists for this currency.". Update existing currency prices by their id instead of sending a new set for the same currency.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentCurrencyPricesResponse> UpdateCurrencyPrices(UpdateCurrencyPricesOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/price_points/{price_point_id}/currency_prices.json"),
            [new TemplateParam("price_point_id", request.PricePointId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentCurrencyPricesResponse>(),
            UpdateCurrencyPricesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
