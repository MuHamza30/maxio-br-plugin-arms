using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.SubscriptionComponents;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Use the Subscription Components object to allocate, adjust, and report usage for the components attached to a subscription, including quantity-based, metered, and event-based components. For more information, see <see href="https://docs.maxio.com/getting-started/maxio-platform-basics/advanced-billing-overview-and-resources/subscription-summary-tabs/managing-components-and-usage">Managing Components and Usage</see>.
/// </summary>
public sealed class SubscriptionComponents
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal SubscriptionComponents(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Activate Event-Based Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Activates an event-based component for a single subscription.
    /// <para>
    /// To bill your subscribers on your Events data under the Events-Based Billing feature, the components must be activated for the subscriber.
    /// </para>
    /// <para>
    /// For more information, see <see href="https://docs.maxio.com/hc/en-us/articles/24181036583053-Design-Your-Catalog?method=componenttypes">Design Your Catalog</see>.
    /// </para>
    /// <para>
    /// Use this endpoint to activate an event-based component for a single subscription. Activating an event-based component causes billing for events when the subscription is renewed.
    /// </para>
    /// <para>
    /// Note: it is possible to stream events for a subscription at any time, regardless of component activation status. The activation status only determines if the subscription should be billed for event-based component usage at renewal.
    /// </para>
    /// </remarks>
    public Task ActivateEventBasedComponent(ActivateEventBasedComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production(
                "/event_based_billing/subscriptions/{subscription_id}/components/{component_id}/activate.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("component_id", request.ComponentId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            VoidResponse.Instance,
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Allocate Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="AllocationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="AllocateComponentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates an allocation, sets the current allocated quantity for the component, and records a memo. Allocations can only be updated for Quantity, On/Off, and Prepaid Components.
    /// <para>
    /// When creating an allocation via the API, you can pass the <c>upgrade_charge</c>, <c>downgrade_credit</c>, and <c>accrue_charge</c> to be applied.
    /// </para>
    /// <para>
    /// &gt; <b>Note:</b> These proration and accrual fields are ignored for Prepaid Components since this component type always generates charges immediately without proration.
    /// </para>
    /// <para>
    /// For information on prorated components and upgrade/downgrade schemes, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24251906165133-Component-Allocations-Proration">Setting Component Allocations.</see>
    /// </para>
    /// <para>
    /// ### Order of Resolution for upgrade_charge and downgrade_credit
    /// </para>
    /// <list type="number">
    ///   <item><description>Per allocation in API call (within a single allocation of the <c>allocations</c> array)</description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24251883961485-Component-Allocations-Overview">Component-level default value</see></description></item>
    ///   <item><description>Allocation API call top level (outside of the <c>allocations</c> array)</description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24251906165133-Component-Allocations-Proration#proration-schemes">Site-level default value</see></description></item>
    /// </list>
    /// <para>
    /// ### Order of Resolution for accrue charge
    /// </para>
    /// <list type="number">
    ///   <item><description>Allocation API call top level (outside of the <c>allocations</c> array)</description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24251906165133-Component-Allocations-Proration#proration-schemes">Site-level default value</see></description></item>
    /// </list>
    /// <para>
    /// &gt; <b>Note:</b> Proration uses the current price of the component as well as the current tax rates. Changes to either may cause the prorated charge/credit to be wrong.
    /// </para>
    /// <para>
    /// For more information, see the <see href="https://maxio.zendesk.com/hc/en-us/articles/24251883961485-Component-Allocations-Overview">Component Allocations</see> product Documentation.
    /// </para>
    /// </remarks>
    public Task<AllocationResponse> AllocateComponent(AllocateComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/components/{component_id}/allocations.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("component_id", request.ComponentId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<AllocationResponse>(),
            AllocateComponentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Allocate Components
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="AllocationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="AllocateComponentsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates multiple allocations, sets the current allocated quantity for each of the components, and records a memo.   A <c>component_id</c> is required for each allocation.
    /// <para>
    /// The charges and/or credits that are created will be rolled up into a single total which is used to determine whether this is an upgrade or a downgrade.
    /// </para>
    /// <para>
    /// ### Order of Resolution for upgrade_charge and downgrade_credit
    /// </para>
    /// <list type="number">
    ///   <item><description>Per allocation in API call (within a single allocation of the <c>allocations</c> array)</description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24251883961485-Component-Allocations-Overview">Component-level default value</see></description></item>
    ///   <item><description>Allocation API call top level (outside of the <c>allocations</c> array)</description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24251906165133-Component-Allocations-Proration#proration-schemes">Site-level default value</see></description></item>
    /// </list>
    /// <para>
    /// ### Order of Resolution for accrue charge
    /// </para>
    /// <list type="number">
    ///   <item><description>Allocation API call top level (outside of the <c>allocations</c> array)</description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24251906165133-Component-Allocations-Proration#proration-schemes">Site-level default value</see></description></item>
    /// </list>
    /// <para>
    /// &gt; <b>Note:</b> Proration uses the current price of the component as well as the current tax rates. Changes to either may cause the prorated charge/credit to be wrong.
    /// </para>
    /// <para>
    /// For more information, see the <see href="https://maxio.zendesk.com/hc/en-us/articles/24251883961485-Component-Allocations-Overview">Component Allocations</see> product documentation.
    /// </para>
    /// </remarks>
    public Task<IReadOnlyList<AllocationResponse>> AllocateComponents(AllocateComponentsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/allocations.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<IReadOnlyList<AllocationResponse>>(),
            AllocateComponentsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Bulk Event Ingestion
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Records a collection of events.
    /// <para>
    /// Note: this endpoint differs from the standard URL for this API in that <c>events</c> and your site subdomain are included in the path.
    /// </para>
    /// <para>
    /// A maximum of 1000 events can be published in a single request. A 422 will be returned if this limit is exceeded.
    /// </para>
    /// </remarks>
    public Task BulkRecordEvents(BulkRecordEventsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Ebb("/events/{api_handle}/bulk.json"),
            [new TemplateParam("api_handle", request.ApiHandle)],
            [new Param("store_uid", request.StoreUid)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            VoidResponse.Instance,
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Bulk Reset Subscription Components' Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Resets all of a subscription's components to use the current default.
    /// <para>
    /// <b>Note</b>: this will update the price point for all of the subscription's components, even ones that have not been allocated yet.
    /// </para>
    /// </remarks>
    public Task<SubscriptionResponse> BulkResetSubscriptionComponentsPricePoints(BulkResetSubscriptionComponentsPricePointsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/price_points/reset.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<SubscriptionResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Bulk Update Subscription Components' Price Points
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="BulkComponentsPricePointAssignment"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="BulkUpdateSubscriptionComponentsPricePointsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the price points on one or more of a subscription's components.
    /// <para>
    /// The <c>price_point</c> key can take either a:
    /// 1. Price point id (integer)
    /// 2. Price point handle (string)
    /// 3. <c>"_default"</c> string, which will reset the price point to the component's current default price point.
    /// </para>
    /// </remarks>
    public Task<BulkComponentsPricePointAssignment> BulkUpdateSubscriptionComponentsPricePoints(BulkUpdateSubscriptionComponentsPricePointsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/price_points.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<BulkComponentsPricePointAssignment>(),
            BulkUpdateSubscriptionComponentsPricePointsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Usage
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="UsageResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateUsageError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Records an instance of metered or prepaid usage for a subscription.
    /// <para>
    /// You can report metered or prepaid usage to Advanced Billing as often as you wish. You can report usage as it happens or periodically, such as each night or once per billing period.
    /// </para>
    /// <para>
    /// Full documentation on how to create Components in the Advanced Billing UI can be located <see href="https://maxio.zendesk.com/hc/en-us/articles/24261149711501-Create-Edit-and-Archive-Components">here</see>. Additionally, for information on how to record component usage against a subscription, see the following resources:
    /// </para>
    /// <para>
    /// It is not possible to record metered usage for more than one component at a time. Usage should be reported as one API call per component on a single subscription. For example, to record that a subscriber has sent both an SMS Message and an Email, send an API call for each.
    /// </para>
    /// <para>
    /// See the following product documentation articles for more information:
    /// </para>
    /// <list type="bullet">
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24261149711501-Create-Edit-and-Archive-Components">Create and Manage Components</see></description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24251890500109-Reporting-Component-Allocations#reporting-metered-component-usage">Recording Metered Component Usage</see></description></item>
    ///   <item><description><see href="https://maxio.zendesk.com/hc/en-us/articles/24251890500109-Reporting-Component-Allocations#reporting-prepaid-component-status">Reporting Prepaid Component Status</see></description></item>
    /// </list>
    /// <para>
    /// The <c>quantity</c> from usage for each component is accumulated to the <c>unit_balance</c> on the <see href="$e/Subscription%20Components/readSubscriptionComponent">Component Line Item</see> for the subscription.
    /// </para>
    /// <para>
    /// ## Price Point ID usage
    /// </para>
    /// <para>
    /// If you are using price points, for metered and prepaid usage components Advanced Billing gives you the option to specify a price point in your request.
    /// </para>
    /// <para>
    /// You do not need to specify a price point ID. If a price point is not included, the default price point for the component will be used when the usage is recorded.
    /// </para>
    /// <para>
    /// ## Deducting Usage
    /// </para>
    /// <para>
    /// If you need to reverse a previous usage report or otherwise deduct from the current usage balance, you can provide a negative quantity.
    /// </para>
    /// <para>
    /// Example:
    /// </para>
    /// <para>
    /// Previously recorded quantity was 5000:
    /// </para>
    /// <code>
    /// {
    ///   "usage": {
    ///     "quantity": 5000,
    ///     "memo": "Recording 5000 units"
    ///   }
    /// }
    /// </code>
    /// <para>
    /// To reduce the quantity to <c>0</c>, POST the following payload:
    /// </para>
    /// <para>
    /// <code>
    /// {
    ///   "usage": {
    ///     "quantity": -5000,
    ///     "memo": "Deducting 5000 units"
    ///   }
    /// }
    /// </code>
    /// The <c>unit_balance</c> has a floor of <c>0</c>; negative unit balances are never allowed. For example, if the usage balance is 100 and you deduct 200 units, the unit balance would then be <c>0</c>, not <c>-100</c>.
    /// </para>
    /// </remarks>
    public Task<UsageResponse> CreateUsage(CreateUsageOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id_or_reference}/components/{component_id}/usages.json"),
            [
                new TemplateParam("subscription_id_or_reference", request.SubscriptionIdOrReference),
                new TemplateParam("component_id", request.ComponentId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<UsageResponse>(),
            CreateUsageError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Deactivate Event-Based Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deactivates an event-based component for a single subscription. Deactivating the event-based component causes Advanced Billing to ignore related events at subscription renewal.
    /// </remarks>
    public Task DeactivateEventBasedComponent(DeactivateEventBasedComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production(
                "/event_based_billing/subscriptions/{subscription_id}/components/{component_id}/deactivate.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("component_id", request.ComponentId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            VoidResponse.Instance,
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Prepaid Usage Allocation
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeletePrepaidUsageAllocationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes a prepaid usage allocation.
    /// <para>
    /// Prepaid Usage components are unique in that their allocations are always additive. In order to reduce a subscription's allocated quantity for a prepaid usage component, each allocation must be destroyed individually via this endpoint.
    /// </para>
    /// <para>
    /// ## Credit Scheme
    /// </para>
    /// <para>
    /// By default, destroying an allocation will generate a service credit on the subscription. This behavior can be modified with the optional <c>credit_scheme</c> parameter on this endpoint. The accepted values are:
    /// </para>
    /// <list type="number">
    ///   <item><description><c>none</c>: The allocation will be destroyed and the balances will be updated but no service credit or refund will be created.</description></item>
    ///   <item><description><c>credit</c>: The allocation will be destroyed and the balances will be updated and a service credit will be generated. This is also the default behavior if the <c>credit_scheme</c> param is not passed.</description></item>
    ///   <item><description><c>refund</c>: The allocation will be destroyed and the balances will be updated and a refund will be issued along with a Credit Note.</description></item>
    /// </list>
    /// </remarks>
    public Task DeletePrepaidUsageAllocation(DeletePrepaidUsageAllocationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production(
                "/subscriptions/{subscription_id}/components/{component_id}/allocations/{allocation_id}.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("allocation_id", request.AllocationId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            JsonRequest.Create(request.Body),
            VoidResponse.Instance,
            DeletePrepaidUsageAllocationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Allocations
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="AllocationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListAllocationsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the 50 most recent Allocations, ordered by most recent first.
    /// <para>
    /// ## On/Off Components
    /// </para>
    /// <para>
    /// When a subscription's on/off component has been toggled to on (<c>1</c>) or off (<c>0</c>), usage will be logged in this response.
    /// </para>
    /// </remarks>
    public Task<IReadOnlyList<AllocationResponse>> ListAllocations(ListAllocationsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/components/{component_id}/allocations.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("component_id", request.ComponentId),
            ],
            [new Param("page", request.Page)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<AllocationResponse>>(),
            ListAllocationsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Subscription Components
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="SubscriptionComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists a subscription's applied components.
    /// <para>
    /// ## Archived Components
    /// </para>
    /// <para>
    /// When requesting to list components for a given subscription, if the subscription contains <b>archived</b> components they will be listed in the server response.
    /// </para>
    /// </remarks>
    public Task<IReadOnlyList<SubscriptionComponentResponse>> ListSubscriptionComponents(ListSubscriptionComponentsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/components.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [
                new Param("date_field", request.DateField),
                new Param("direction", request.Direction),
                new Param("filter", request.Filter),
                new Param("end_date", request.EndDate),
                new Param("end_datetime", request.EndDatetime),
                new Param("price_point_ids", request.PricePointIds),
                new Param("product_family_ids", request.ProductFamilyIds),
                new Param("sort", request.Sort),
                new Param("start_date", request.StartDate),
                new Param("start_datetime", request.StartDatetime),
                new Param("include", request.Include),
                new Param("in_use", request.InUse),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<SubscriptionComponentResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Subscription Components for Site
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListSubscriptionComponentsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists components applied to each subscription.
    /// </remarks>
    public Task<ListSubscriptionComponentsResponse> ListSubscriptionComponentsForSite(ListSubscriptionComponentsForSiteRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions_components.json"),
            [],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("sort", request.Sort),
                new Param("direction", request.Direction),
                new Param("filter", request.Filter),
                new Param("date_field", request.DateField),
                new Param("start_date", request.StartDate),
                new Param("start_datetime", request.StartDatetime),
                new Param("end_date", request.EndDate),
                new Param("end_datetime", request.EndDatetime),
                new Param("subscription_ids", request.SubscriptionIds),
                new Param("price_point_ids", request.PricePointIds),
                new Param("product_family_ids", request.ProductFamilyIds),
                new Param("include", request.Include),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListSubscriptionComponentsResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Usages
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="UsageResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists usages associated with a subscription for a particular metered component. This will display the previously recorded components for a subscription.
    /// <para>
    /// This endpoint is not compatible with quantity-based components.
    /// </para>
    /// <para>
    /// ## Since Date and Until Date Usage
    /// </para>
    /// <para>
    /// Note: The <c>since_date</c> and <c>until_date</c> attributes each default to midnight on the date specified. For example, in order to list usages for January 20th, you would need to append the following to the URL.
    /// </para>
    /// <code>
    /// ?since_date=2016-01-20&amp;until_date=2016-01-21
    /// </code>
    /// <para>
    /// ## Read Usage by Handle
    /// </para>
    /// <para>
    /// Use this endpoint to read the previously recorded components for a subscription.  You can now specify either the component id (integer) or the component handle prefixed by "handle:" to specify the unique identifier for the component you are working with.
    /// </para>
    /// </remarks>
    public Task<IReadOnlyList<UsageResponse>> ListUsages(ListUsagesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id_or_reference}/components/{component_id}/usages.json"),
            [
                new TemplateParam("subscription_id_or_reference", request.SubscriptionIdOrReference),
                new TemplateParam("component_id", request.ComponentId),
            ],
            [
                new Param("since_id", request.SinceId),
                new Param("max_id", request.MaxId),
                new Param("since_date", request.SinceDate?.ToDate()),
                new Param("until_date", request.UntilDate?.ToDate()),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<UsageResponse>>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Preview Allocations
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="AllocationPreviewResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="PreviewAllocationsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Previews a potential subscription's <b>quantity-based</b> or <b>on/off</b> component allocation in the middle of the current billing period.  This is useful if you want users to be able to see the effect of a component operation before actually doing it.
    /// <para>
    /// ## Fine-grained Component Control: Use with multiple <c>upgrade_charge</c>s or <c>downgrade_credits</c>
    /// </para>
    /// <para>
    /// When the allocation uses multiple different types of <c>upgrade_charge</c>s or <c>downgrade_credit</c>s, the Allocation is viewed as an Allocation which uses "Fine-Grained Component Control". As a result, the response will not include <c>direction</c> and <c>proration</c> within the <c>allocation_preview</c>, but at the <c>line_items</c> and <c>allocations</c> level respectfully.
    /// </para>
    /// <para>
    /// See example below for Fine-Grained Component Control response.
    /// </para>
    /// </remarks>
    public Task<AllocationPreviewResponse> PreviewAllocations(PreviewAllocationsOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/allocations/preview.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<AllocationPreviewResponse>(),
            PreviewAllocationsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Subscription Component
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionComponentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadSubscriptionComponentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns information for a specific component on a subscription.
    /// </remarks>
    public Task<SubscriptionComponentResponse> ReadSubscriptionComponent(ReadSubscriptionComponentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/components/{component_id}.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("component_id", request.ComponentId),
            ],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<SubscriptionComponentResponse>(),
            ReadSubscriptionComponentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Event Ingestion
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Records a single event for Events-Based Billing.
    /// <para>
    /// Events-Based Billing is an evolved form of metered billing that is based on data-rich events streamed in real-time from your system to Advanced Billing.
    /// </para>
    /// <para>
    /// These events can then be transformed, enriched, or analyzed to form the computed totals of usage charges billed to your customers.
    /// </para>
    /// <para>
    /// This API allows you to stream events into the Advanced Billing data ingestion engine.
    /// </para>
    /// <para>
    /// For more information, see <see href="https://docs.maxio.com/hc/en-us/articles/24181036583053-Design-Your-Catalog?method=componenttypes">Design Your Catalog</see>.
    /// </para>
    /// <para>
    /// Note: this endpoint differs from the standard URL for this API in that <c>events</c> and your site subdomain are included in the path. For example:
    /// </para>
    /// <code>
    /// https://events.chargify.com/my-site-subdomain/events/my-stream-api-handle
    /// </code>
    /// </remarks>
    public Task RecordEvent(RecordEventRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Ebb("/events/{api_handle}.json"),
            [new TemplateParam("api_handle", request.ApiHandle)],
            [new Param("store_uid", request.StoreUid)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            VoidResponse.Instance,
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Prepaid Usage Allocation Expiration Date
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdatePrepaidUsageAllocationExpirationDateError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the expiration date for a prepaid usage allocation. This expiration date can be changed after the fact to allow for extending or shortening the allocation's active window.
    /// <para>
    /// In order to change a prepaid usage allocation's expiration date, a PUT call must be made to the allocation's endpoint with a new expiration date.
    /// </para>
    /// <para>
    /// ## Limitations
    /// </para>
    /// <para>
    /// A few limitations exist when changing an allocation's expiration date:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An expiration date can only be changed for an allocation that belongs to a price point with expiration interval options explicitly set.</description></item>
    ///   <item><description>An expiration date can be changed towards the future with no limitations.</description></item>
    ///   <item><description>An expiration date can be changed towards the past (essentially expiring it) up to the subscription's current period beginning date.</description></item>
    /// </list>
    /// </remarks>
    public Task UpdatePrepaidUsageAllocationExpirationDate(UpdatePrepaidUsageAllocationExpirationDateRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production(
                "/subscriptions/{subscription_id}/components/{component_id}/allocations/{allocation_id}.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("allocation_id", request.AllocationId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            VoidResponse.Instance,
            UpdatePrepaidUsageAllocationExpirationDateError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
