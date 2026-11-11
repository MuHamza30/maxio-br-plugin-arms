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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>billing_schedule.initial_billing_at is before the minimum allowed date for the subscription or component → the outcome is not stated. Send an initial_billing_at on or after the subscription's or component's minimum allowed date.</description></item>
    ///   <item><description>An event-based component has not been activated for the subscription → The subscription is not billed for that component's events at renewal; activation makes renewal bill the events. Activate each event-based component for a subscription before you expect its events to be billed.</description></item>
    ///   <item><description>custom_price.interval or custom_price.interval_unit is sent on a site without Multifrequency enabled → the outcome is not stated. Send custom_price interval fields only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>billing_schedule.initial_billing_at is omitted or null → The component's billing aligns with the product schedule. Send initial_billing_at only when the component's billing must start on its own date.</description></item>
    ///   <item><description>renew_prepaid_allocation or rollover_prepaid_remainder is sent for a component that is not a prepaid usage component → the outcome is not stated. Send the prepaid renewal and rollover settings only for prepaid usage components.</description></item>
    ///   <item><description>custom_price.expiration_interval or expiration_interval_unit is sent without rollover enabled → the outcome is not stated. Send rollover expiration settings only together with rollover_prepaid_remainder=true.</description></item>
    ///   <item><description>custom_price.use_default_list_price is true → List price calculations keep using the default price point even though a custom price is supplied. Set use_default_list_price (or list_price_point_id) deliberately when overriding the customer price.</description></item>
    ///   <item><description>A subscription renews with an event-based component → Event usage is billed only if the component is activated for the subscription; events of a deactivated component are ignored at renewal, although events can be streamed at any time. Activate the event-based component for each subscription that should be billed for its events.</description></item>
    ///   <item><description>Events are streamed for a subscription whose event-based component is not activated → The events are accepted; activation only decides whether they are billed at renewal. Do not treat an accepted event as billable unless the component is activated for the subscription.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An allocation is made for a component that is not a Quantity, On/Off or Prepaid component (for example metered or event-based) → the outcome is not stated. Allocate only Quantity, On/Off and Prepaid components; report metered usage with Create Usage.</description></item>
    ///   <item><description>A decimal quantity (decimal_quantity, decimal_previous_quantity or a fractional quantity) is sent for a component without decimal allocations enabled → the outcome is not stated. Send whole-number quantities unless decimal allocations are enabled for the component.</description></item>
    ///   <item><description>An allocation for an On/Off component uses a quantity other than 1 or 0 → the outcome is not stated. Send quantity 1 to turn an On/Off component on and 0 to turn it off.</description></item>
    ///   <item><description>The allocation request carries no allocation change → Maxio answers 422 with "One or more allocation changes are required.". Send an allocation that changes the quantity (or other allocation values).</description></item>
    ///   <item><description>billing_schedule.initial_billing_at is before the minimum allowed date for the subscription or component → the outcome is not stated. Send an initial_billing_at on or after the subscription's or component's minimum allowed date.</description></item>
    ///   <item><description>A custom_price is sent for an On/Off component → the outcome is not stated. For On/Off components omit custom_price.pricing_scheme and send one price bracket starting at 1.</description></item>
    ///   <item><description>An upgrade allocation is made with accrue_charge false and the immediate charge fails → The charge is accrued until the next renewal; the allocation is not refused. Check the allocation's payment result instead of assuming an accrue_charge=false upgrade was paid.</description></item>
    ///   <item><description>initiate_dunning is true and the immediate component payment fails → Dunning is started for the subscription. Send initiate_dunning=true only when a failed allocation payment should put the subscription into dunning.</description></item>
    ///   <item><description>initiate_dunning is false or not sent and the immediate component payment fails → No dunning starts; the charges stay on the subscription to be paid at renewal. Follow up failed allocation payments yourself or send initiate_dunning=true.</description></item>
    ///   <item><description>An allocation is sent without price_point_id → The component's default price point is used. Send price_point_id (id or handle) when the allocation must be charged at a non-default price point.</description></item>
    ///   <item><description>proration_upgrade_scheme or proration_downgrade_scheme (deprecated) is not sent → The site setting is used. Use upgrade_charge/downgrade_credit instead of the deprecated proration schemes.</description></item>
    ///   <item><description>An allocation was not created mid-period → proration_upgrade_scheme and proration_downgrade_scheme are absent from the allocation. Treat the proration scheme fields as optional in allocation responses.</description></item>
    ///   <item><description>An allocation is returned → The allocation timestamp is in UTC, while other response timestamps use the site's time zone. Parse the allocation timestamp as UTC.</description></item>
    ///   <item><description>upgrade_charge, downgrade_credit or accrue_charge is sent for a Prepaid component → The fields are ignored; prepaid allocations always generate charges immediately without proration. Do not rely on proration or accrual settings for Prepaid component allocations.</description></item>
    ///   <item><description>A quantity-based allocation changes, an on/off component is toggled, or a prepaid allocation is purchased after signup → A component_allocation_change webhook fires (allocation_change on the enhanced Catalog experience). Handle component_allocation_change (and allocation_change) webhooks to track allocation changes.</description></item>
    ///   <item><description>A component that has feature catalog items attached is allocated to a subscription → The subscription is provisioned the component's entitlements as part of the change. Read entitlements after an allocation instead of assuming they are unchanged.</description></item>
    ///   <item><description>The site's requests are being throttled → Component allocations are treated as critical endpoints and prioritised over low-priority requests such as reports and exports. Do not move allocation calls into report or export jobs to get them through throttling; keep concurrency low.</description></item>
    ///   <item><description>custom_price.interval or custom_price.interval_unit is sent on a site without Multifrequency enabled → the outcome is not stated. Send custom_price interval fields only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>billing_schedule.initial_billing_at is omitted or null → The component's billing aligns with the product schedule. Send initial_billing_at only when the component's billing must start on its own date.</description></item>
    ///   <item><description>renew_prepaid_allocation or rollover_prepaid_remainder is sent for a component that is not a prepaid usage component → the outcome is not stated. Send the prepaid renewal and rollover settings only for prepaid usage components.</description></item>
    ///   <item><description>custom_price.expiration_interval or expiration_interval_unit is sent without rollover enabled → the outcome is not stated. Send rollover expiration settings only together with rollover_prepaid_remainder=true.</description></item>
    ///   <item><description>custom_price.use_default_list_price is true → List price calculations keep using the default price point even though a custom price is supplied. Set use_default_list_price (or list_price_point_id) deliberately when overriding the customer price.</description></item>
    ///   <item><description>upgrade_charge or downgrade_credit is resolved for an allocation → The value set on the allocation wins, then the component-level default, then the top-level request value, then the site default. Set upgrade_charge/downgrade_credit on each allocation; a top-level value is overridden by a component default.</description></item>
    ///   <item><description>An allocation is an upgrade and upgrade_charge resolves to full, prorated or none → full adds a charge for the full price of the component; prorated adds a charge for the prorated price of the change; none adds no charge. Pick upgrade_charge for the charge you intend, and preview it first when unsure.</description></item>
    ///   <item><description>An allocation is a downgrade and downgrade_credit resolves to full, prorated or none → full adds a full-price credit for the amount owed; prorated adds a prorated credit; none adds nothing. Pick downgrade_credit for the credit you intend, and preview it first when unsure.</description></item>
    ///   <item><description>The component supports fractional quantities → quantity and previous_quantity are returned as strings instead of integers. Parse allocation quantities as either integers or decimal strings.</description></item>
    ///   <item><description>accrue_charge is not sent at the top level of the allocation call → The site-level default for accrue_charge is used. Send accrue_charge explicitly when the site default is not what you want.</description></item>
    ///   <item><description>A prorated allocation charge or credit is calculated → Proration uses the component's current price and current tax rates, so a changed price or tax rate can make the prorated amount wrong. Allocate after price or tax changes are settled, and verify prorated amounts with Preview Allocations.</description></item>
    ///   <item><description>An upgrade allocation is made with accrue_charge true → The charge accrues to the next renewal instead of being captured now. Send accrue_charge=false if the upgrade must be paid immediately.</description></item>
    ///   <item><description>A subscription's allocated quantity for a prepaid usage component must go down → Prepaid usage allocations are always additive; the quantity can only be reduced by deleting individual allocations. Reduce a prepaid usage component by deleting allocations one by one, not by allocating a lower quantity.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A decimal quantity (decimal_quantity, decimal_previous_quantity or a fractional quantity) is sent for a component without decimal allocations enabled → the outcome is not stated. Send whole-number quantities unless decimal allocations are enabled for the component.</description></item>
    ///   <item><description>An allocation for an On/Off component uses a quantity other than 1 or 0 → the outcome is not stated. Send quantity 1 to turn an On/Off component on and 0 to turn it off.</description></item>
    ///   <item><description>billing_schedule.initial_billing_at is before the minimum allowed date for the subscription or component → the outcome is not stated. Send an initial_billing_at on or after the subscription's or component's minimum allowed date.</description></item>
    ///   <item><description>A custom_price is sent for an On/Off component → the outcome is not stated. For On/Off components omit custom_price.pricing_scheme and send one price bracket starting at 1.</description></item>
    ///   <item><description>An allocation in the allocations array has no component_id → the outcome is not stated. Send component_id on every allocation in the array.</description></item>
    ///   <item><description>An upgrade allocation is made with accrue_charge false and the immediate charge fails → The charge is accrued until the next renewal; the allocation is not refused. Check the allocation's payment result instead of assuming an accrue_charge=false upgrade was paid.</description></item>
    ///   <item><description>initiate_dunning is true and the immediate component payment fails → Dunning is started for the subscription. Send initiate_dunning=true only when a failed allocation payment should put the subscription into dunning.</description></item>
    ///   <item><description>initiate_dunning is false or not sent and the immediate component payment fails → No dunning starts; the charges stay on the subscription to be paid at renewal. Follow up failed allocation payments yourself or send initiate_dunning=true.</description></item>
    ///   <item><description>An allocation is sent without price_point_id → The component's default price point is used. Send price_point_id (id or handle) when the allocation must be charged at a non-default price point.</description></item>
    ///   <item><description>proration_upgrade_scheme or proration_downgrade_scheme (deprecated) is not sent → The site setting is used. Use upgrade_charge/downgrade_credit instead of the deprecated proration schemes.</description></item>
    ///   <item><description>An allocation was not created mid-period → proration_upgrade_scheme and proration_downgrade_scheme are absent from the allocation. Treat the proration scheme fields as optional in allocation responses.</description></item>
    ///   <item><description>An allocation is returned → The allocation timestamp is in UTC, while other response timestamps use the site's time zone. Parse the allocation timestamp as UTC.</description></item>
    ///   <item><description>A quantity-based allocation changes, an on/off component is toggled, or a prepaid allocation is purchased after signup → A component_allocation_change webhook fires (allocation_change on the enhanced Catalog experience). Handle component_allocation_change (and allocation_change) webhooks to track allocation changes.</description></item>
    ///   <item><description>A component that has feature catalog items attached is allocated to a subscription → The subscription is provisioned the component's entitlements as part of the change. Read entitlements after an allocation instead of assuming they are unchanged.</description></item>
    ///   <item><description>The site's requests are being throttled → Component allocations are treated as critical endpoints and prioritised over low-priority requests such as reports and exports. Do not move allocation calls into report or export jobs to get them through throttling; keep concurrency low.</description></item>
    ///   <item><description>custom_price.interval or custom_price.interval_unit is sent on a site without Multifrequency enabled → the outcome is not stated. Send custom_price interval fields only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>billing_schedule.initial_billing_at is omitted or null → The component's billing aligns with the product schedule. Send initial_billing_at only when the component's billing must start on its own date.</description></item>
    ///   <item><description>renew_prepaid_allocation or rollover_prepaid_remainder is sent for a component that is not a prepaid usage component → the outcome is not stated. Send the prepaid renewal and rollover settings only for prepaid usage components.</description></item>
    ///   <item><description>custom_price.expiration_interval or expiration_interval_unit is sent without rollover enabled → the outcome is not stated. Send rollover expiration settings only together with rollover_prepaid_remainder=true.</description></item>
    ///   <item><description>custom_price.use_default_list_price is true → List price calculations keep using the default price point even though a custom price is supplied. Set use_default_list_price (or list_price_point_id) deliberately when overriding the customer price.</description></item>
    ///   <item><description>upgrade_charge or downgrade_credit is resolved for an allocation → The value set on the allocation wins, then the component-level default, then the top-level request value, then the site default. Set upgrade_charge/downgrade_credit on each allocation; a top-level value is overridden by a component default.</description></item>
    ///   <item><description>An allocation is an upgrade and upgrade_charge resolves to full, prorated or none → full adds a charge for the full price of the component; prorated adds a charge for the prorated price of the change; none adds no charge. Pick upgrade_charge for the charge you intend, and preview it first when unsure.</description></item>
    ///   <item><description>An allocation is a downgrade and downgrade_credit resolves to full, prorated or none → full adds a full-price credit for the amount owed; prorated adds a prorated credit; none adds nothing. Pick downgrade_credit for the credit you intend, and preview it first when unsure.</description></item>
    ///   <item><description>The component supports fractional quantities → quantity and previous_quantity are returned as strings instead of integers. Parse allocation quantities as either integers or decimal strings.</description></item>
    ///   <item><description>accrue_charge is not sent at the top level of the allocation call → The site-level default for accrue_charge is used. Send accrue_charge explicitly when the site default is not what you want.</description></item>
    ///   <item><description>A prorated allocation charge or credit is calculated → Proration uses the component's current price and current tax rates, so a changed price or tax rate can make the prorated amount wrong. Allocate after price or tax changes are settled, and verify prorated amounts with Preview Allocations.</description></item>
    ///   <item><description>An upgrade allocation is made with accrue_charge true → The charge accrues to the next renewal instead of being captured now. Send accrue_charge=false if the upgrade must be paid immediately.</description></item>
    ///   <item><description>Several allocations in one call produce charges and credits → Charges and credits are rolled up into one total, which decides whether the call is treated as an upgrade or a downgrade. Split allocations into separate calls if each needs its own upgrade/downgrade treatment.</description></item>
    ///   <item><description>payment_collection_method is not sent → The allocations use the subscription's payment collection method. Send payment_collection_method only to override the subscription's own method.</description></item>
    /// </list>
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
    /// <exception cref="ApiException{TError}"> of <see cref="BulkRecordEventsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Records a collection of events.
    /// <para>
    /// Note: this endpoint differs from the standard URL for this API in that <c>events</c> and your site subdomain are included in the path.
    /// </para>
    /// <para>
    /// A maximum of 1000 events can be published in a single request. A 422 will be returned if this limit is exceeded.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>More than 1000 events are sent in one bulk request → Maxio answers 422. Send at most 1000 events per bulk request.</description></item>
    ///   <item><description>Events are streamed for a subscription whose event-based component is not activated → The events are accepted; activation only decides whether they are billed at renewal. Do not treat an accepted event as billable unless the component is activated for the subscription.</description></item>
    ///   <item><description>An event is sent without chargify.timestamp → chargify.timestamp is added and set to now; it decides which billing period the event is billed in. Send chargify.timestamp with the time the usage happened, especially for late or backfilled events.</description></item>
    ///   <item><description>An event payload includes chargify.id → The field is reserved and is overwritten with a unique id set by the server. Do not put your own id in chargify.id; use another field.</description></item>
    ///   <item><description>An event payload includes chargify.created_at → The field is reserved and is overwritten with the time the event is recorded. Do not set chargify.created_at; use chargify.timestamp for the event time.</description></item>
    ///   <item><description>An event repeats a chargify.uniqueness_token already used in the same stream → The duplicate event is silently ignored (no error). Set chargify.uniqueness_token on each event so retries are deduplicated, and do not expect an error for duplicates.</description></item>
    ///   <item><description>An event repeats a chargify.uniqueness_token first used more than 31 days earlier → The token has expired, so the event is not deduplicated against the earlier one. Do not rely on event uniqueness tokens for retries spanning more than 31 days.</description></item>
    ///   <item><description>The event identifies its subscription with a field other than the Subscription Identifier configured on the Event Stream (chargify.subscription_id or chargify.subscription_reference) → the outcome is not stated. Send chargify.subscription_id or chargify.subscription_reference to match the stream's configured Subscription Identifier.</description></item>
    ///   <item><description>store_uid is sent on a Maxio Metering site → store_uid has no effect; it applies only to Legacy Metering sites with their own Keen data-store. Send store_uid only on Legacy Metering sites that attached their own Keen project.</description></item>
    /// </list>
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
            BulkRecordEventsError.Response,
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A subscription's component price points are reset → Every component on the subscription is moved to its current default price point, including components that have not been allocated yet. Use the bulk reset only when every component, allocated or not, should move to its default price point.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A price point in the request does not belong to the component it is assigned to → Maxio answers 422 with "Price Point does not belong to Component". Assign each component only a price point of that same component.</description></item>
    ///   <item><description>A component's price_point is sent as "_default" → The component's price point is reset to the component's current default price point. Send "_default" to move a component back to its current default price point.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>billing_schedule.initial_billing_at is before the minimum allowed date for the subscription or component → the outcome is not stated. Send an initial_billing_at on or after the subscription's or component's minimum allowed date.</description></item>
    ///   <item><description>Usage is reported as many tiny reports (per minute or per event) or for all customers at once → Requests can be throttled or blocked. Batch usage into one report per customer per day and spread reports out over time.</description></item>
    ///   <item><description>The price_point_id sent with the usage cannot be found → Maxio answers 422 with "Price point: could not be found.". Send a price point that exists on the component.</description></item>
    ///   <item><description>A decimal usage quantity is sent for a component without fractional quantities enabled → the outcome is not stated. Send whole-number usage unless fractional quantities are enabled for the component.</description></item>
    ///   <item><description>component_id in the path is a component handle rather than the numeric id → the outcome is not stated. Prefix a component handle with handle: in the path; otherwise send the numeric id.</description></item>
    ///   <item><description>custom_price.interval or custom_price.interval_unit is sent on a site without Multifrequency enabled → the outcome is not stated. Send custom_price interval fields only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>billing_schedule.initial_billing_at is omitted or null → The component's billing aligns with the product schedule. Send initial_billing_at only when the component's billing must start on its own date.</description></item>
    ///   <item><description>renew_prepaid_allocation or rollover_prepaid_remainder is sent for a component that is not a prepaid usage component → the outcome is not stated. Send the prepaid renewal and rollover settings only for prepaid usage components.</description></item>
    ///   <item><description>custom_price.expiration_interval or expiration_interval_unit is sent without rollover enabled → the outcome is not stated. Send rollover expiration settings only together with rollover_prepaid_remainder=true.</description></item>
    ///   <item><description>custom_price.use_default_list_price is true → List price calculations keep using the default price point even though a custom price is supplied. Set use_default_list_price (or list_price_point_id) deliberately when overriding the customer price.</description></item>
    ///   <item><description>subscription_id_or_reference is a numeric string that matches both an existing subscription id and an existing subscription reference → The subscription with that id is used, not the one with that reference. Pass the numeric subscription id, or avoid numeric-looking references that can collide with ids.</description></item>
    ///   <item><description>A usage report is retried after a timeout or ambiguous failure → Without a uniqueness_token the retry can double-count the usage; with one, a 409 only proves the first report arrived, not that it succeeded. Send a uniqueness_token with usage reports and, after a 409, check List Usages for that component and period before re-sending.</description></item>
    ///   <item><description>Usage is recorded for a component → The quantity is accumulated into the unit_balance of the subscription's component line item. Report usage increments, not running totals.</description></item>
    ///   <item><description>Usage is recorded without price_point_id → The component's default price point is used for the usage. Send price_point_id when usage must be recorded at a non-default price point.</description></item>
    ///   <item><description>Usage is recorded with a negative quantity → The quantity is deducted from the current usage balance. Send a negative quantity to reverse an earlier usage report.</description></item>
    ///   <item><description>A negative usage quantity is larger than the current unit balance → The unit balance stops at 0; it never goes negative (e.g. 100 minus 200 gives 0, not -100). Do not count on negative usage to carry a deficit forward.</description></item>
    ///   <item><description>Usage is reported for a metered component → A metered_usage webhook fires (not when the balance resets to 0 at renewal). Handle metered_usage webhooks to track reported usage.</description></item>
    ///   <item><description>Usage is recorded for a prepaid component → A prepaid_usage webhook fires; allocation changes fire component_allocation_change instead. Handle prepaid_usage webhooks for prepaid usage and component_allocation_change for prepaid allocations.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A subscription renews with an event-based component → Event usage is billed only if the component is activated for the subscription; events of a deactivated component are ignored at renewal, although events can be streamed at any time. Activate the event-based component for each subscription that should be billed for its events.</description></item>
    ///   <item><description>An event-based component is deactivated for a subscription → Related events are ignored at subscription renewal. Deactivate only when the subscription should stop being billed for those events.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A subscription's allocated quantity for a prepaid usage component must go down → Prepaid usage allocations are always additive; the quantity can only be reduced by deleting individual allocations. Reduce a prepaid usage component by deleting allocations one by one, not by allocating a lower quantity.</description></item>
    ///   <item><description>A prepaid usage allocation is deleted with credit_scheme credit or without credit_scheme → The allocation is destroyed, balances are updated and a service credit is generated on the subscription. Send credit_scheme none or refund if a service credit is not wanted.</description></item>
    ///   <item><description>A prepaid usage allocation is deleted with credit_scheme none → The allocation is destroyed and balances are updated, but no service credit or refund is created. Use credit_scheme none only when the customer should get nothing back.</description></item>
    ///   <item><description>A prepaid usage allocation is deleted with credit_scheme refund → The allocation is destroyed, balances are updated, and a refund is issued along with a Credit Note. Use credit_scheme refund when the customer must be refunded, and expect a credit note.</description></item>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>page is 0 → Maxio answers 422 with "invalid page: 0". Start paging allocations at page 1.</description></item>
    ///   <item><description>An allocation was not created mid-period → proration_upgrade_scheme and proration_downgrade_scheme are absent from the allocation. Treat the proration scheme fields as optional in allocation responses.</description></item>
    ///   <item><description>An allocation is returned → The allocation timestamp is in UTC, while other response timestamps use the site's time zone. Parse the allocation timestamp as UTC.</description></item>
    ///   <item><description>The component supports fractional quantities → quantity and previous_quantity are returned as strings instead of integers. Parse allocation quantities as either integers or decimal strings.</description></item>
    ///   <item><description>Allocations are listed for a subscription component → Only the 50 most recent allocations are returned, newest first. Page with page to see allocations older than the 50 most recent.</description></item>
    ///   <item><description>An on/off component on the subscription was toggled on (1) or off (0) → Each toggle is logged as an allocation in the list. Read on/off toggle history from List Allocations.</description></item>
    ///   <item><description>page points past the last page of results → An empty result set is returned. Stop paging when a page comes back empty; do not wait for an error.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>start_date (YYYY-MM-DD) is sent with date_field → Records with a timestamp at or after midnight (12:00:00 AM) in the site's time zone on that date match. Express start_date in the site's time zone, or use start_datetime with an explicit offset.</description></item>
    ///   <item><description>end_date (YYYY-MM-DD) is sent with date_field → Records with a timestamp up to and including 11:59:59 PM in the site's time zone on that date match. Express end_date in the site's time zone, or use end_datetime with an explicit offset.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include a UTC offset in start_datetime/end_datetime when the site's time zone is not the one you mean.</description></item>
    ///   <item><description>date_field is sent with a value other than created_at or updated_at → The value is ignored and the date filters are not applied; matching records are returned unfiltered by date. Send a supported date_field value and check that the date filter was applied.</description></item>
    ///   <item><description>The subscription has archived components → Archived components are included in the list. Filter on archived_at yourself if you only want live components.</description></item>
    ///   <item><description>in_use is false or not sent → All components connected with the subscription are returned, not only those in use. Send in_use=true to get only the components currently in use.</description></item>
    ///   <item><description>price_point_ids=not_null is sent → Only component allocations that have a price point id are returned. Send price_point_ids=not_null to drop components without a price point.</description></item>
    ///   <item><description>include=historic_usages is sent → Historic usage is returned for event-based components only, covering the last ten billing periods. Expect at most ten past billing periods, and only for event-based components.</description></item>
    ///   <item><description>start_datetime is sent with start_date, or end_datetime with end_date → The datetime parameter is used instead of the date parameter. Send only one of each date/datetime pair.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>per_page is above 200 → per_page is changed to 200; no error is returned. Without per_page, 20 records are returned. Page with per_page of at most 200 and do not assume a larger page was honoured.</description></item>
    ///   <item><description>page points past the last page of results → An empty result set is returned. Stop paging when a page comes back empty; do not wait for an error.</description></item>
    ///   <item><description>start_date (YYYY-MM-DD) is sent with date_field → Records with a timestamp at or after midnight (12:00:00 AM) in the site's time zone on that date match. Express start_date in the site's time zone, or use start_datetime with an explicit offset.</description></item>
    ///   <item><description>end_date (YYYY-MM-DD) is sent with date_field → Records with a timestamp up to and including 11:59:59 PM in the site's time zone on that date match. Express end_date in the site's time zone, or use end_datetime with an explicit offset.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include a UTC offset in start_datetime/end_datetime when the site's time zone is not the one you mean.</description></item>
    ///   <item><description>date_field is sent with a value other than created_at or updated_at → The value is ignored and the date filters are not applied; matching records are returned unfiltered by date. Send a supported date_field value and check that the date filter was applied.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>price_point_ids=not_null is sent → Only component allocations that have a price point id are returned. Send price_point_ids=not_null to drop components without a price point.</description></item>
    ///   <item><description>include=historic_usages is sent → Historic usage is returned for event-based components only, covering the last ten billing periods. Expect at most ten past billing periods, and only for event-based components.</description></item>
    ///   <item><description>A filter[subscription] filter (states, date_field, start/end date or datetime) is sent without include=subscription → the outcome is not stated. Send include=subscription whenever you use a filter[subscription] filter.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>start_datetime is sent with start_date, or end_datetime with end_date → The datetime parameter is used instead of the date parameter. Send only one of each date/datetime pair.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Usages are listed for a quantity-based component → the outcome is not stated. List usages only for metered (and prepaid) components; read quantity-based components through allocations.</description></item>
    ///   <item><description>component_id in the path is a component handle rather than the numeric id → the outcome is not stated. Prefix a component handle with handle: in the path; otherwise send the numeric id.</description></item>
    ///   <item><description>subscription_id_or_reference is a numeric string that matches both an existing subscription id and an existing subscription reference → The subscription with that id is used, not the one with that reference. Pass the numeric subscription id, or avoid numeric-looking references that can collide with ids.</description></item>
    ///   <item><description>per_page is above 200 → per_page is changed to 200; no error is returned. Without per_page, 20 records are returned. Page with per_page of at most 200 and do not assume a larger page was honoured.</description></item>
    ///   <item><description>page points past the last page of results → An empty result set is returned. Stop paging when a page comes back empty; do not wait for an error.</description></item>
    ///   <item><description>since_id or max_id is sent → Both bounds are inclusive: records with an id greater than or equal to since_id and less than or equal to max_id are returned. When paging forward with since_id, start from the last seen id plus one to avoid repeating it.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    ///   <item><description>since_date or until_date is sent → Both default to midnight (12:00 AM) on the date given, so until_date excludes usage created later on that day. Set until_date to the day after the last day you want (e.g. since_date=2016-01-20&amp;until_date=2016-01-21 for January 20).</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A decimal quantity (decimal_quantity, decimal_previous_quantity or a fractional quantity) is sent for a component without decimal allocations enabled → the outcome is not stated. Send whole-number quantities unless decimal allocations are enabled for the component.</description></item>
    ///   <item><description>An allocation for an On/Off component uses a quantity other than 1 or 0 → the outcome is not stated. Send quantity 1 to turn an On/Off component on and 0 to turn it off.</description></item>
    ///   <item><description>billing_schedule.initial_billing_at is before the minimum allowed date for the subscription or component → the outcome is not stated. Send an initial_billing_at on or after the subscription's or component's minimum allowed date.</description></item>
    ///   <item><description>A custom_price is sent for an On/Off component → the outcome is not stated. For On/Off components omit custom_price.pricing_scheme and send one price bracket starting at 1.</description></item>
    ///   <item><description>An allocation preview is requested for a component that is not quantity-based or on/off → Maxio answers 422 with "Allocations can only be updated for quantity and on/off components.". Preview allocations only for quantity-based and on/off components.</description></item>
    ///   <item><description>effective_proration_date falls outside the subscription's current period → the outcome is not stated. Send an effective_proration_date within the current subscription period.</description></item>
    ///   <item><description>An allocation is sent without price_point_id → The component's default price point is used. Send price_point_id (id or handle) when the allocation must be charged at a non-default price point.</description></item>
    ///   <item><description>proration_upgrade_scheme or proration_downgrade_scheme (deprecated) is not sent → The site setting is used. Use upgrade_charge/downgrade_credit instead of the deprecated proration schemes.</description></item>
    ///   <item><description>custom_price.interval or custom_price.interval_unit is sent on a site without Multifrequency enabled → the outcome is not stated. Send custom_price interval fields only when the site has Multifrequency enabled.</description></item>
    ///   <item><description>billing_schedule.initial_billing_at is omitted or null → The component's billing aligns with the product schedule. Send initial_billing_at only when the component's billing must start on its own date.</description></item>
    ///   <item><description>renew_prepaid_allocation or rollover_prepaid_remainder is sent for a component that is not a prepaid usage component → the outcome is not stated. Send the prepaid renewal and rollover settings only for prepaid usage components.</description></item>
    ///   <item><description>custom_price.expiration_interval or expiration_interval_unit is sent without rollover enabled → the outcome is not stated. Send rollover expiration settings only together with rollover_prepaid_remainder=true.</description></item>
    ///   <item><description>custom_price.use_default_list_price is true → List price calculations keep using the default price point even though a custom price is supplied. Set use_default_list_price (or list_price_point_id) deliberately when overriding the customer price.</description></item>
    ///   <item><description>An allocation is an upgrade and upgrade_charge resolves to full, prorated or none → full adds a charge for the full price of the component; prorated adds a charge for the prorated price of the change; none adds no charge. Pick upgrade_charge for the charge you intend, and preview it first when unsure.</description></item>
    ///   <item><description>An allocation is a downgrade and downgrade_credit resolves to full, prorated or none → full adds a full-price credit for the amount owed; prorated adds a prorated credit; none adds nothing. Pick downgrade_credit for the credit you intend, and preview it first when unsure.</description></item>
    ///   <item><description>The previewed allocations use more than one kind of upgrade_charge or downgrade_credit (Fine-Grained Component Control) → direction and proration are not returned on allocation_preview but on each line item and allocation instead. Read direction and proration from line_items and allocations when several charge/credit types are used.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>component_id in the path is a component handle rather than the numeric id → the outcome is not stated. Prefix a component handle with handle: in the path; otherwise send the numeric id.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Events are streamed for a subscription whose event-based component is not activated → The events are accepted; activation only decides whether they are billed at renewal. Do not treat an accepted event as billable unless the component is activated for the subscription.</description></item>
    ///   <item><description>An event is sent without chargify.timestamp → chargify.timestamp is added and set to now; it decides which billing period the event is billed in. Send chargify.timestamp with the time the usage happened, especially for late or backfilled events.</description></item>
    ///   <item><description>An event payload includes chargify.id → The field is reserved and is overwritten with a unique id set by the server. Do not put your own id in chargify.id; use another field.</description></item>
    ///   <item><description>An event payload includes chargify.created_at → The field is reserved and is overwritten with the time the event is recorded. Do not set chargify.created_at; use chargify.timestamp for the event time.</description></item>
    ///   <item><description>An event repeats a chargify.uniqueness_token already used in the same stream → The duplicate event is silently ignored (no error). Set chargify.uniqueness_token on each event so retries are deduplicated, and do not expect an error for duplicates.</description></item>
    ///   <item><description>An event repeats a chargify.uniqueness_token first used more than 31 days earlier → The token has expired, so the event is not deduplicated against the earlier one. Do not rely on event uniqueness tokens for retries spanning more than 31 days.</description></item>
    ///   <item><description>The event identifies its subscription with a field other than the Subscription Identifier configured on the Event Stream (chargify.subscription_id or chargify.subscription_reference) → the outcome is not stated. Send chargify.subscription_id or chargify.subscription_reference to match the stream's configured Subscription Identifier.</description></item>
    ///   <item><description>store_uid is sent on a Maxio Metering site → store_uid has no effect; it applies only to Legacy Metering sites with their own Keen data-store. Send store_uid only on Legacy Metering sites that attached their own Keen project.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The allocation belongs to a price point without expiration interval options explicitly set → the outcome is not stated. Change expiration dates only for allocations on price points with explicit expiration interval options.</description></item>
    ///   <item><description>The new expires_at is earlier than the subscription's current period start date → Maxio answers 422 with "Expires at: must be greater than or equal to 2024-02-28". Move an expiration date into the past no further than the current period's start date.</description></item>
    ///   <item><description>expires_at is missing or not a valid date → Maxio answers 422 with "A valid expires_at date must be provided.". Always send a valid allocation.expires_at.</description></item>
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
