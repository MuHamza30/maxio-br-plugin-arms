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
