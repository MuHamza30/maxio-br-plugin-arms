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
using MaxioAdvancedBilling.Requests.Components;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Component object represents an add-on item, such as a metered usage charge, quantity-based feature, or on/off toggle, that can be added to a subscription alongside the subscription's base Product. With the new catalog experience, the UI labels components as Products, while the API keeps the component name. See <see href="https://docs.maxio.com/billing/catalog/overview/using-the-new-product-catalog#catalog-terminology">Catalog terminology</see>.
/// <para>
/// Components belong to a Product Family and are billed according to their pricing scheme and kind. Components can also grant features to subscribers. See the <b>Create Component Feature Catalog Item</b> endpoint for details. For more information, see <see href="https://docs.maxio.com/billing/catalog/products/understand-products">Understand Products</see> and <see href="https://docs.maxio.com/billing/catalog/products/use-product-pricing-schemes#hybrid-pricing">Hybrid pricing</see>.
/// </para>
/// </summary>
public sealed class Components
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Components(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Archive Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="Component"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ArchiveComponentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Archives the component; all current subscribers will continue to be charged as usual.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The component is already archived → Maxio answers 422 with "Component cannot be archived. Please make sure it hasn't already been archived.". Check the component's archived flag before archiving it.</description></item>
    ///   <item><description>A component is archived → All current subscribers keep being charged for it as usual. Move or remove the component from existing subscriptions yourself if archiving should stop their charges.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<Component> ArchiveComponent(ArchiveComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/components/{component_id}.json"),
            [
                new TemplateParam("product_family_id", request.ProductFamilyId),
                new TemplateParam("component_id", request.ComponentId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<Component>(),
            ArchiveComponentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Event Based Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateEventBasedComponentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates an event-based component definition under the specified product family. An event-based component can then be added and “allocated” for a subscription.
    /// <para>
    /// Event-based components are similar to other component types, in that you define the component parameters (such as name and taxability) and the pricing. A key difference for the event-based component is that it must be attached to a metric. This is because the metric provides the component with the actual quantity used in computing what and how much will be billed each period for each subscription.
    /// </para>
    /// <para>
    /// So, instead of reporting usage directly for each component (as you would with metered components), the usage is derived from analysis of your events.
    /// </para>
    /// <para>
    /// For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24261141522189-Components-Overview">Components Overview</see>.
    /// </para>
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, taxable components must include a non-blank <c>tax_code</c>; sending a blank value results in a validation error.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has the new Catalog experience enabled and a taxable component is created or updated with a blank tax_code → Maxio answers 422. On Catalog-experience sites, send a non-blank tax_code for every taxable component.</description></item>
    ///   <item><description>A component is created under a product family that does not exist → the outcome is not stated. Create (or look up) the product family first and create components under its id or handle.</description></item>
    ///   <item><description>Taxes are enabled on the site and the component is saved without a tax type → Maxio answers 422 with "Must set tax type if taxes enabled". When the site has taxes enabled, set the component's tax settings before creating or updating it.</description></item>
    ///   <item><description>The component uses a pricing scheme other than per_unit and the request has no price brackets → Maxio answers 422 with "At least 1 price bracket must be defined". Send at least one price bracket in prices for stairstep, volume and tiered schemes; use unit_price for per_unit.</description></item>
    ///   <item><description>A component is created with a handle already used by another component on the site → Maxio answers 422 with "Handle must be unique within a Site.". Use a component handle that is unique within the site.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>An event-based component is attached to a metric → The billed quantity each period comes from the metric's analysis of events, not from usage reported directly for the component. Send events for the metric instead of reporting usage directly for event-based components.</description></item>
    ///   <item><description>Secondary pricing is configured on a component that is not quantity-based or metered, or on a metered component configured for event-based billing (metric, meter or formula) → The price point is not eligible for Hybrid Pricing. Use Hybrid Pricing only on quantity-based or non-event-based metered components.</description></item>
    ///   <item><description>A unit price is sent with more than 8 decimal places → the outcome is not stated. Send unit prices with at most 8 decimal places.</description></item>
    ///   <item><description>A tax_code longer than 25 characters is sent for a component or product → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>hide_date_range_on_invoice is sent on a site that does not use Relationship Invoicing → the outcome is not stated. Only rely on hide_date_range_on_invoice on Relationship Invoicing sites.</description></item>
    ///   <item><description>interval or interval_unit is sent for a component or component price point on a site without Multifrequency enabled → the outcome is not stated. Send interval and interval_unit only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>A component or product is saved with unspsc_code set (or without it) → The unspsc_code is sent as the commodity code on invoice line items instead of the default derived from item_category. Set unspsc_code when the commodity code derived from item_category is not the one you want on invoice line items.</description></item>
    ///   <item><description>A component is created with a unit_name → The unit name is pluralized automatically where needed, e.g. on line items. Send unit_name in the singular.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentResponse> CreateEventBasedComponent(CreateEventBasedComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/event_based_components.json"),
            [new TemplateParam("product_family_id", request.ProductFamilyId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentResponse>(),
            CreateEventBasedComponentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Metered Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateMeteredComponentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a metered component definition under the specified product family. A metered component can then be added and “allocated” for a subscription.
    /// <para>
    /// Metered components are used to bill for any type of unit that resets to 0 at the end of the billing period (think daily Google Ads clicks or monthly cell phone minutes). This is most commonly associated with usage-based billing and many other pricing schemes.
    /// </para>
    /// <para>
    /// Note that this is different from recurring quantity-based components, which DO NOT reset to zero at the start of every billing period. If you want to bill for a quantity of something that does not change unless you change it, then you want quantity components, instead.
    /// </para>
    /// <para>
    /// #### Hybrid Pricing
    /// A <c>volume</c>, <c>tiered</c>, or <c>stairstep</c> metered component can combine its primary pricing with a secondary pricing model (the <c>overage_pricing</c> parameter) so both bill as a single invoice line item instead of two. This does not apply to metered components configured for event-based billing (metric, meter, or formula). See <see href="page:introduction/basic-concepts/hybrid-pricing">Hybrid Pricing</see> for requirements and configuration details.
    /// </para>
    /// <para>
    /// For more information on components, see our documentation <see href="https://maxio.zendesk.com/hc/en-us/articles/24261141522189-Components-Overview">here</see>.
    /// </para>
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, taxable components must include a non-blank <c>tax_code</c>. Sending <c>"tax_code": ""</c> returns <c>422</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has the new Catalog experience enabled and a taxable component is created or updated with a blank tax_code → Maxio answers 422. On Catalog-experience sites, send a non-blank tax_code for every taxable component.</description></item>
    ///   <item><description>Usage or allocations are to be recorded for a component → The component's ID returned at creation is needed in all later usage requests. Create the component first and keep the returned ID for usage and allocation calls.</description></item>
    ///   <item><description>The primary pricing_scheme is per_unit while a secondary pricing model is configured on a hybrid-eligible component → Maxio refuses it with "Pricing scheme cannot be per_unit for hybrid pricing". Use volume, tiered or stairstep as the primary scheme when sending secondary pricing.</description></item>
    ///   <item><description>Secondary pricing is configured and the primary model's highest bracket has no ending_quantity → Maxio refuses it with "Prices primary pricing must have a finite included threshold for hybrid pricing". Give the primary model's top bracket a finite ending_quantity (the included threshold).</description></item>
    ///   <item><description>A component is created under a product family that does not exist → the outcome is not stated. Create (or look up) the product family first and create components under its id or handle.</description></item>
    ///   <item><description>Taxes are enabled on the site and the component is saved without a tax type → Maxio answers 422 with "Must set tax type if taxes enabled". When the site has taxes enabled, set the component's tax settings before creating or updating it.</description></item>
    ///   <item><description>The component uses a pricing scheme other than per_unit and the request has no price brackets → Maxio answers 422 with "At least 1 price bracket must be defined". Send at least one price bracket in prices for stairstep, volume and tiered schemes; use unit_price for per_unit.</description></item>
    ///   <item><description>A component is created with a handle already used by another component on the site → Maxio answers 422 with "Handle must be unique within a Site.". Use a component handle that is unique within the site.</description></item>
    ///   <item><description>The primary model's highest bracket has no ending_quantity on a hybrid price point → Maxio refuses it with "Prices primary pricing must have a finite included threshold for hybrid pricing". Give the primary model's top bracket a finite ending_quantity.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A billing period ends for a subscription with metered and quantity-based components → Metered component usage resets to 0; quantity-based allocations do not reset. Report metered usage per period and do not re-allocate quantity components each period.</description></item>
    ///   <item><description>A metered component is used on a subscription → Its unit count resets to 0 at the end of each billing period, unlike recurring quantity-based components. Use a quantity-based component, not a metered one, for quantities that must persist across billing periods.</description></item>
    ///   <item><description>A price point meets every Hybrid Pricing requirement (site feature, eligible component, bracketed primary scheme with a finite top bracket, secondary pricing configured) → It becomes a hybrid price point automatically (there is no hybrid flag) and bills primary and secondary pricing as one invoice line item. Treat a price point as hybrid from its configuration; do not look for or send a hybrid flag.</description></item>
    ///   <item><description>A component or price point is configured with secondary pricing on a site without the Hybrid Pricing feature or without Invoice-Centric Billing → The price point falls back to billing primary and secondary pricing as separate invoice line items. Confirm with Maxio that Hybrid Pricing and Invoice-Centric Billing are both enabled before relying on single-line hybrid billing.</description></item>
    ///   <item><description>Secondary pricing is configured on a component that is not quantity-based or metered, or on a metered component configured for event-based billing (metric, meter or formula) → The price point is not eligible for Hybrid Pricing. Use Hybrid Pricing only on quantity-based or non-event-based metered components.</description></item>
    ///   <item><description>Secondary (overage_pricing) brackets are sent for a hybrid price point → overage_pricing.prices is a separate bracket set starting at quantity 1; it only takes effect once usage passes the primary model's included threshold. Start overage_pricing brackets at 1, not at the primary model's ending_quantity.</description></item>
    ///   <item><description>A unit price is sent with more than 8 decimal places → the outcome is not stated. Send unit prices with at most 8 decimal places.</description></item>
    ///   <item><description>A tax_code longer than 25 characters is sent for a component or product → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>hide_date_range_on_invoice is sent on a site that does not use Relationship Invoicing → the outcome is not stated. Only rely on hide_date_range_on_invoice on Relationship Invoicing sites.</description></item>
    ///   <item><description>interval or interval_unit is sent for a component or component price point on a site without Multifrequency enabled → the outcome is not stated. Send interval and interval_unit only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>A component or product is saved with unspsc_code set (or without it) → The unspsc_code is sent as the commodity code on invoice line items instead of the default derived from item_category. Set unspsc_code when the commodity code derived from item_category is not the one you want on invoice line items.</description></item>
    ///   <item><description>A component is created with a unit_name → The unit name is pluralized automatically where needed, e.g. on line items. Send unit_name in the singular.</description></item>
    ///   <item><description>A price point is configured with a secondary (overage) pricing model on a site without both the Hybrid Pricing feature and Invoice-Centric Billing enabled → The price point is not hybrid: primary and secondary pricing bill as separate invoice line items. Have the Maxio account team enable Hybrid Pricing and Invoice-Centric Billing before configuring hybrid price points.</description></item>
    ///   <item><description>Secondary pricing is configured on a component other than a quantity-based or non-event-based metered component → The component is not eligible for Hybrid Pricing. Use hybrid pricing only on quantity-based components or metered components not configured for event-based billing.</description></item>
    ///   <item><description>All hybrid requirements are met for a price point → The price point becomes hybrid automatically; there is no hybrid flag in the API. Check the requirements, not a flag, to know whether a price point is hybrid.</description></item>
    ///   <item><description>overage_pricing.prices brackets are defined for a hybrid price point → The secondary brackets are self-contained and start at 1, and they apply only to usage above the primary model's included threshold. Start overage_pricing.prices at starting_quantity 1, not at the primary ending_quantity.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentResponse> CreateMeteredComponent(CreateMeteredComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/metered_components.json"),
            [new TemplateParam("product_family_id", request.ProductFamilyId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentResponse>(),
            CreateMeteredComponentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create On/Off Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateOnOffComponentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates an On/Off component definition under the specified product family. An On/Off component can then be added and “allocated” for a subscription.
    /// <para>
    /// On/off components are used for any flat fee, recurring add on (think $99/month for tech support or a flat add on shipping fee).
    /// </para>
    /// <para>
    /// For more information on components, see our documentation <see href="https://maxio.zendesk.com/hc/en-us/articles/24261141522189-Components-Overview">here</see>.
    /// </para>
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, taxable components must include a non-blank <c>tax_code</c>. Sending <c>"tax_code": ""</c> returns <c>422</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has the new Catalog experience enabled and a taxable component is created or updated with a blank tax_code → Maxio answers 422. On Catalog-experience sites, send a non-blank tax_code for every taxable component.</description></item>
    ///   <item><description>A component is created under a product family that does not exist → the outcome is not stated. Create (or look up) the product family first and create components under its id or handle.</description></item>
    ///   <item><description>Taxes are enabled on the site and the component is saved without a tax type → Maxio answers 422 with "Must set tax type if taxes enabled". When the site has taxes enabled, set the component's tax settings before creating or updating it.</description></item>
    ///   <item><description>A component is created with a handle already used by another component on the site → Maxio answers 422 with "Handle must be unique within a Site.". Use a component handle that is unique within the site.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>Secondary pricing is configured on a component that is not quantity-based or metered, or on a metered component configured for event-based billing (metric, meter or formula) → The price point is not eligible for Hybrid Pricing. Use Hybrid Pricing only on quantity-based or non-event-based metered components.</description></item>
    ///   <item><description>A unit price is sent with more than 8 decimal places → the outcome is not stated. Send unit prices with at most 8 decimal places.</description></item>
    ///   <item><description>A tax_code longer than 25 characters is sent for a component or product → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>hide_date_range_on_invoice is sent on a site that does not use Relationship Invoicing → the outcome is not stated. Only rely on hide_date_range_on_invoice on Relationship Invoicing sites.</description></item>
    ///   <item><description>interval or interval_unit is sent for a component or component price point on a site without Multifrequency enabled → the outcome is not stated. Send interval and interval_unit only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>A component or product is saved with unspsc_code set (or without it) → The unspsc_code is sent as the commodity code on invoice line items instead of the default derived from item_category. Set unspsc_code when the commodity code derived from item_category is not the one you want on invoice line items.</description></item>
    ///   <item><description>upgrade_charge or downgrade_credit is not provided → The credit type falls back to the component's setting and then the site setting. Send upgrade_charge and downgrade_credit explicitly when the site default proration is not what you want.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentResponse> CreateOnOffComponent(CreateOnOffComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/on_off_components.json"),
            [new TemplateParam("product_family_id", request.ProductFamilyId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentResponse>(),
            CreateOnOffComponentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Prepaid Usage Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreatePrepaidUsageComponentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a prepaid usage component definition under the specified product family. A prepaid component can then be added and “allocated” for a subscription.
    /// <para>
    /// Prepaid components allow customers to pre-purchase units that can be used up over time on their subscription. In a sense, they are the mirror image of metered components; while metered components charge at the end of the period for the amount of units used, prepaid components are charged for at the time of purchase, and usage is subsequently tracked against the amount purchased.
    /// </para>
    /// <para>
    /// For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24261141522189-Components-Overview">Components Overview</see>.
    /// </para>
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, taxable components must include a non-blank <c>tax_code</c>; sending a blank value results in a validation error.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has the new Catalog experience enabled and a taxable component is created or updated with a blank tax_code → Maxio answers 422. On Catalog-experience sites, send a non-blank tax_code for every taxable component.</description></item>
    ///   <item><description>A component is created under a product family that does not exist → the outcome is not stated. Create (or look up) the product family first and create components under its id or handle.</description></item>
    ///   <item><description>Taxes are enabled on the site and the component is saved without a tax type → Maxio answers 422 with "Must set tax type if taxes enabled". When the site has taxes enabled, set the component's tax settings before creating or updating it.</description></item>
    ///   <item><description>The component uses a pricing scheme other than per_unit and the request has no price brackets → Maxio answers 422 with "At least 1 price bracket must be defined". Send at least one price bracket in prices for stairstep, volume and tiered schemes; use unit_price for per_unit.</description></item>
    ///   <item><description>A component is created with a handle already used by another component on the site → Maxio answers 422 with "Handle must be unique within a Site.". Use a component handle that is unique within the site.</description></item>
    ///   <item><description>A prepaid usage component is created with a name that is already taken → Maxio answers 422 with "Name: must be unique - that value has been taken.". Use a component name that is not already taken.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>Secondary pricing is configured on a component that is not quantity-based or metered, or on a metered component configured for event-based billing (metric, meter or formula) → The price point is not eligible for Hybrid Pricing. Use Hybrid Pricing only on quantity-based or non-event-based metered components.</description></item>
    ///   <item><description>A unit price is sent with more than 8 decimal places → the outcome is not stated. Send unit prices with at most 8 decimal places.</description></item>
    ///   <item><description>A tax_code longer than 25 characters is sent for a component or product → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>hide_date_range_on_invoice is sent on a site that does not use Relationship Invoicing → the outcome is not stated. Only rely on hide_date_range_on_invoice on Relationship Invoicing sites.</description></item>
    ///   <item><description>A component or product is saved with unspsc_code set (or without it) → The unspsc_code is sent as the commodity code on invoice line items instead of the default derived from item_category. Set unspsc_code when the commodity code derived from item_category is not the one you want on invoice line items.</description></item>
    ///   <item><description>A component is created with a unit_name → The unit name is pluralized automatically where needed, e.g. on line items. Send unit_name in the singular.</description></item>
    ///   <item><description>Units of a prepaid usage component are purchased on a subscription → The units are charged at the time of purchase and later usage is tracked against the purchased amount. Expect prepaid components to charge at purchase, not at the end of the period like metered components.</description></item>
    ///   <item><description>expiration_interval or expiration_interval_unit is sent while rollover_prepaid_remainder is not true → The expiration settings apply only to prepaid usage components that roll over remaining units. Send expiration_interval and expiration_interval_unit only together with rollover_prepaid_remainder: true.</description></item>
    ///   <item><description>upgrade_charge or downgrade_credit is not provided → The credit type falls back to the component's setting and then the site setting. Send upgrade_charge and downgrade_credit explicitly when the site default proration is not what you want.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentResponse> CreatePrepaidUsageComponent(CreatePrepaidUsageComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/prepaid_usage_components.json"),
            [new TemplateParam("product_family_id", request.ProductFamilyId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentResponse>(),
            CreatePrepaidUsageComponentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Quantity Based Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateQuantityBasedComponentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a Quantity Based component definition under the specified product family. A Quantity Based component can then be added and “allocated” for a subscription.
    /// <para>
    /// When defining a Quantity Based component, you can choose one of two types:
    /// #### Recurring
    /// Recurring quantity-based components are used to bill for the number of some unit (think monthly software user licenses or the number of pairs of socks in a box-a-month club). This is most commonly associated with billing for user licenses, number of users, number of employees, etc.
    /// </para>
    /// <para>
    /// #### One-time
    /// One-time quantity-based components are used to create ad hoc usage charges that do not recur. For example, at the time of signup, you might want to charge your customer a one-time fee for onboarding or other services.
    /// </para>
    /// <para>
    /// The allocated quantity for one-time quantity-based components immediately gets reset back to zero after the allocation is made.
    /// </para>
    /// <para>
    /// For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24261141522189-Components-Overview">Components Overview</see>.
    /// #### Hybrid Pricing
    /// A <c>volume</c>, <c>tiered</c>, or <c>stairstep</c> component can combine its primary pricing with a secondary pricing model (the <c>overage_pricing</c> parameter) so both bill as a single invoice line item instead of two. See <see href="page:introduction/basic-concepts/hybrid-pricing">Hybrid Pricing</see> for requirements and configuration details.
    /// </para>
    /// <para>
    /// For more information on components, see our documentation <see href="https://maxio.zendesk.com/hc/en-us/articles/24261141522189-Components-Overview">here</see>.
    /// </para>
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, taxable components must include a non-blank <c>tax_code</c>. Sending <c>"tax_code": ""</c> returns <c>422</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has the new Catalog experience enabled and a taxable component is created or updated with a blank tax_code → Maxio answers 422. On Catalog-experience sites, send a non-blank tax_code for every taxable component.</description></item>
    ///   <item><description>The primary pricing_scheme is per_unit while a secondary pricing model is configured on a hybrid-eligible component → Maxio refuses it with "Pricing scheme cannot be per_unit for hybrid pricing". Use volume, tiered or stairstep as the primary scheme when sending secondary pricing.</description></item>
    ///   <item><description>Secondary pricing is configured and the primary model's highest bracket has no ending_quantity → Maxio refuses it with "Prices primary pricing must have a finite included threshold for hybrid pricing". Give the primary model's top bracket a finite ending_quantity (the included threshold).</description></item>
    ///   <item><description>A component is created under a product family that does not exist → the outcome is not stated. Create (or look up) the product family first and create components under its id or handle.</description></item>
    ///   <item><description>Taxes are enabled on the site and the component is saved without a tax type → Maxio answers 422 with "Must set tax type if taxes enabled". When the site has taxes enabled, set the component's tax settings before creating or updating it.</description></item>
    ///   <item><description>The component uses a pricing scheme other than per_unit and the request has no price brackets → Maxio answers 422 with "At least 1 price bracket must be defined". Send at least one price bracket in prices for stairstep, volume and tiered schemes; use unit_price for per_unit.</description></item>
    ///   <item><description>A component is created with a handle already used by another component on the site → Maxio answers 422 with "Handle must be unique within a Site.". Use a component handle that is unique within the site.</description></item>
    ///   <item><description>The primary model's highest bracket has no ending_quantity on a hybrid price point → Maxio refuses it with "Prices primary pricing must have a finite included threshold for hybrid pricing". Give the primary model's top bracket a finite ending_quantity.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A billing period ends for a subscription with metered and quantity-based components → Metered component usage resets to 0; quantity-based allocations do not reset. Report metered usage per period and do not re-allocate quantity components each period.</description></item>
    ///   <item><description>A hybrid price point is billed → The customer sees one invoice line item per billing period for the component, combining primary and secondary charges. Expect one combined line item, not separate primary and overage lines, for hybrid price points.</description></item>
    ///   <item><description>A price point meets every Hybrid Pricing requirement (site feature, eligible component, bracketed primary scheme with a finite top bracket, secondary pricing configured) → It becomes a hybrid price point automatically (there is no hybrid flag) and bills primary and secondary pricing as one invoice line item. Treat a price point as hybrid from its configuration; do not look for or send a hybrid flag.</description></item>
    ///   <item><description>A component or price point is configured with secondary pricing on a site without the Hybrid Pricing feature or without Invoice-Centric Billing → The price point falls back to billing primary and secondary pricing as separate invoice line items. Confirm with Maxio that Hybrid Pricing and Invoice-Centric Billing are both enabled before relying on single-line hybrid billing.</description></item>
    ///   <item><description>Secondary pricing is configured on a component that is not quantity-based or metered, or on a metered component configured for event-based billing (metric, meter or formula) → The price point is not eligible for Hybrid Pricing. Use Hybrid Pricing only on quantity-based or non-event-based metered components.</description></item>
    ///   <item><description>Secondary (overage_pricing) brackets are sent for a hybrid price point → overage_pricing.prices is a separate bracket set starting at quantity 1; it only takes effect once usage passes the primary model's included threshold. Start overage_pricing brackets at 1, not at the primary model's ending_quantity.</description></item>
    ///   <item><description>A unit price is sent with more than 8 decimal places → the outcome is not stated. Send unit prices with at most 8 decimal places.</description></item>
    ///   <item><description>A tax_code longer than 25 characters is sent for a component or product → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>hide_date_range_on_invoice is sent on a site that does not use Relationship Invoicing → the outcome is not stated. Only rely on hide_date_range_on_invoice on Relationship Invoicing sites.</description></item>
    ///   <item><description>interval or interval_unit is sent for a component or component price point on a site without Multifrequency enabled → the outcome is not stated. Send interval and interval_unit only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>A component or product is saved with unspsc_code set (or without it) → The unspsc_code is sent as the commodity code on invoice line items instead of the default derived from item_category. Set unspsc_code when the commodity code derived from item_category is not the one you want on invoice line items.</description></item>
    ///   <item><description>A component is created with a unit_name → The unit name is pluralized automatically where needed, e.g. on line items. Send unit_name in the singular.</description></item>
    ///   <item><description>A quantity-based component is created as one-time (not recurring) and later allocated → The allocated quantity is reset to zero immediately after each allocation. Create one-time quantity-based components only for ad hoc charges that should not recur.</description></item>
    ///   <item><description>A price point is configured with a secondary (overage) pricing model on a site without both the Hybrid Pricing feature and Invoice-Centric Billing enabled → The price point is not hybrid: primary and secondary pricing bill as separate invoice line items. Have the Maxio account team enable Hybrid Pricing and Invoice-Centric Billing before configuring hybrid price points.</description></item>
    ///   <item><description>Secondary pricing is configured on a component other than a quantity-based or non-event-based metered component → The component is not eligible for Hybrid Pricing. Use hybrid pricing only on quantity-based components or metered components not configured for event-based billing.</description></item>
    ///   <item><description>All hybrid requirements are met for a price point → The price point becomes hybrid automatically; there is no hybrid flag in the API. Check the requirements, not a flag, to know whether a price point is hybrid.</description></item>
    ///   <item><description>overage_pricing.prices brackets are defined for a hybrid price point → The secondary brackets are self-contained and start at 1, and they apply only to usage above the primary model's included threshold. Start overage_pricing.prices at starting_quantity 1, not at the primary ending_quantity.</description></item>
    ///   <item><description>upgrade_charge or downgrade_credit is not provided → The credit type falls back to the component's setting and then the site setting. Send upgrade_charge and downgrade_credit explicitly when the site default proration is not what you want.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentResponse> CreateQuantityBasedComponent(CreateQuantityBasedComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/quantity_based_components.json"),
            [new TemplateParam("product_family_id", request.ProductFamilyId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentResponse>(),
            CreateQuantityBasedComponentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Find Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns information for a component matching the provided handle. You can identify your components with a handle so you don't have to save or reference the IDs we generate.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentResponse> FindComponent(FindComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/lookup.json"),
            [],
            [new Param("handle", request.Handle)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Components
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists components for a site.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An app wants component features embedded in List Components results → List Components does not support include_features, so features are not embedded. Read each component with include_features=true (Read Component) to get its features.</description></item>
    ///   <item><description>A list is filtered with start_date or end_date → start_date matches from midnight and end_date up to and including 11:59:59 PM, both in the site's time zone. Convert date filters to the site's time zone, or use start_datetime/end_datetime with an explicit offset.</description></item>
    ///   <item><description>Both start_datetime and start_date (or end_datetime and end_date) are sent → The datetime parameter is used and the date parameter is ignored. Send either the date or the datetime form of each bound, not both.</description></item>
    ///   <item><description>date_field is given a value other than created_at or updated_at → The value is ignored and the date filters are not applied. Send date_field as created_at or updated_at only, and check that date-filtered results really are filtered.</description></item>
    ///   <item><description>include_archived is not set to true → Archived components or products are left out of the list. Pass include_archived=true when archived items must be listed.</description></item>
    ///   <item><description>filter[use_site_exchange_rate] is used → The filter matches the setting of each item's default price point, not of its other price points. Remember filter[use_site_exchange_rate] tests only the default price point.</description></item>
    ///   <item><description>per_page is omitted or set above 200 → 20 records are returned by default; a per_page above 200 is changed to 200. Page through results with per_page of at most 200 and do not assume a larger page was honoured.</description></item>
    ///   <item><description>A page number past the last page of results is requested → An empty result set is returned, not an error. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>include_features=true is passed to read features embedded in product or component responses → List Products, Read Product and Read Component embed features; List Components does not support include_features. Read component features one by one or with the features endpoints instead of List Components.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include an explicit time zone in start_datetime and end_datetime.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<ComponentResponse>> ListComponents(ListComponentsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components.json"),
            [],
            [
                new Param("date_field", request.DateField),
                new Param("start_date", request.StartDate),
                new Param("end_date", request.EndDate),
                new Param("start_datetime", request.StartDatetime),
                new Param("end_datetime", request.EndDatetime),
                new Param("include_archived", request.IncludeArchived),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("filter", request.Filter),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<ComponentResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Components for Product Family
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists components for a particular product family.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A list is filtered with start_date or end_date → start_date matches from midnight and end_date up to and including 11:59:59 PM, both in the site's time zone. Convert date filters to the site's time zone, or use start_datetime/end_datetime with an explicit offset.</description></item>
    ///   <item><description>Both start_datetime and start_date (or end_datetime and end_date) are sent → The datetime parameter is used and the date parameter is ignored. Send either the date or the datetime form of each bound, not both.</description></item>
    ///   <item><description>date_field is given a value other than created_at or updated_at → The value is ignored and the date filters are not applied. Send date_field as created_at or updated_at only, and check that date-filtered results really are filtered.</description></item>
    ///   <item><description>include_archived is not set to true → Archived components or products are left out of the list. Pass include_archived=true when archived items must be listed.</description></item>
    ///   <item><description>filter[use_site_exchange_rate] is used → The filter matches the setting of each item's default price point, not of its other price points. Remember filter[use_site_exchange_rate] tests only the default price point.</description></item>
    ///   <item><description>per_page is omitted or set above 200 → 20 records are returned by default; a per_page above 200 is changed to 200. Page through results with per_page of at most 200 and do not assume a larger page was honoured.</description></item>
    ///   <item><description>A page number past the last page of results is requested → An empty result set is returned, not an error. Stop paging when a page comes back empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include an explicit time zone in start_datetime and end_datetime.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<ComponentResponse>> ListComponentsForProductFamily(ListComponentsForProductFamilyRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/components.json"),
            [new TemplateParam("product_family_id", request.ProductFamilyId)],
            [
                new Param("include_archived", request.IncludeArchived),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("filter", request.Filter),
                new Param("date_field", request.DateField),
                new Param("end_date", request.EndDate),
                new Param("end_datetime", request.EndDatetime),
                new Param("start_date", request.StartDate),
                new Param("start_datetime", request.StartDatetime),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<ComponentResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns information regarding a component from a specific product family.
    /// <para>
    /// You can read the component by either the component's id or handle. When using the handle, it must be prefixed with <c>handle:</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>include_features=true is passed to read features embedded in product or component responses → List Products, Read Product and Read Component embed features; List Components does not support include_features. Read component features one by one or with the features endpoints instead of List Components.</description></item>
    ///   <item><description>A product or component is read without include_features=true → The response has no features array; with include_features=true it embeds only the active feature catalog items. Pass include_features=true when you need the features attached to a product or component.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentResponse> ReadComponent(ReadComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/components/{component_id}.json"),
            [
                new TemplateParam("product_family_id", request.ProductFamilyId),
                new TemplateParam("component_id", request.ComponentId),
            ],
            [new Param("include_features", request.IncludeFeatures)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ComponentResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateComponentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates a component.
    /// <para>
    /// You may read the component by either the component's id or handle. When using the handle, it must be prefixed with <c>handle:</c>.
    /// </para>
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, taxable components must include a non-blank <c>tax_code</c>. Sending <c>"tax_code": ""</c> returns <c>422</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has the new Catalog experience enabled and a taxable component is created or updated with a blank tax_code → Maxio answers 422. On Catalog-experience sites, send a non-blank tax_code for every taxable component.</description></item>
    ///   <item><description>Taxes are enabled on the site and the component is saved without a tax type → Maxio answers 422 with "Must set tax type if taxes enabled". When the site has taxes enabled, set the component's tax settings before creating or updating it.</description></item>
    ///   <item><description>A component, product family, product or price point is identified in the path by its handle without the handle: prefix → the outcome is not stated. Prefix handles in path parameters with handle: (for example handle:gold); send bare values only for numeric ids.</description></item>
    ///   <item><description>A tax_code longer than 25 characters is sent for a component or product → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>A component or product is saved with unspsc_code set (or without it) → The unspsc_code is sent as the commodity code on invoice line items instead of the default derived from item_category. Set unspsc_code when the commodity code derived from item_category is not the one you want on invoice line items.</description></item>
    ///   <item><description>upgrade_charge or downgrade_credit is not provided → The credit type falls back to the component's setting and then the site setting. Send upgrade_charge and downgrade_credit explicitly when the site default proration is not what you want.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentResponse> UpdateComponent(UpdateComponentOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}.json"),
            [new TemplateParam("component_id", request.ComponentId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentResponse>(),
            UpdateComponentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Product Family Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateProductFamilyComponentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates a component from a specific product family.
    /// <para>
    /// You may read the component by either the component's id or handle. When using the handle, it must be prefixed with <c>handle:</c>.
    /// </para>
    /// <para>
    /// If you have the new <see href="page:help/announcements/2026-announcements#new-catalog-experience-and-terminology">Catalog experience</see> enabled, taxable components must include a non-blank <c>tax_code</c>. Sending <c>"tax_code": ""</c> returns <c>422</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site has the new Catalog experience enabled and a taxable component is created or updated with a blank tax_code → Maxio answers 422. On Catalog-experience sites, send a non-blank tax_code for every taxable component.</description></item>
    ///   <item><description>Taxes are enabled on the site and the component is saved without a tax type → Maxio answers 422 with "Must set tax type if taxes enabled". When the site has taxes enabled, set the component's tax settings before creating or updating it.</description></item>
    ///   <item><description>A tax_code longer than 25 characters is sent for a component or product → the outcome is not stated. Keep tax_code to 25 characters or fewer.</description></item>
    ///   <item><description>A component or product is saved with unspsc_code set (or without it) → The unspsc_code is sent as the commodity code on invoice line items instead of the default derived from item_category. Set unspsc_code when the commodity code derived from item_category is not the one you want on invoice line items.</description></item>
    ///   <item><description>upgrade_charge or downgrade_credit is not provided → The credit type falls back to the component's setting and then the site setting. Send upgrade_charge and downgrade_credit explicitly when the site default proration is not what you want.</description></item>
    ///   <item><description>A returned component uses the per_unit pricing scheme → Its prices array is empty; the price is carried in unit_price. Read unit_price, not prices, for per_unit components.</description></item>
    ///   <item><description>A returned component uses a pricing scheme other than per_unit → unit_price may be null; the pricing is in the prices brackets. Handle a null unit_price on bracketed components and read prices instead.</description></item>
    /// </list>
    /// </remarks>
    public Task<ComponentResponse> UpdateProductFamilyComponent(UpdateProductFamilyComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/product_families/{product_family_id}/components/{component_id}.json"),
            [
                new TemplateParam("product_family_id", request.ProductFamilyId),
                new TemplateParam("component_id", request.ComponentId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ComponentResponse>(),
            UpdateProductFamilyComponentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
