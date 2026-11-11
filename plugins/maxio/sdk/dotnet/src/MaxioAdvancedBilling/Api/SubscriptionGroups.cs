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
using MaxioAdvancedBilling.Requests.SubscriptionGroups;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Subscription Group object links multiple subscriptions for a single customer under one shared payment profile and collection method, so you can consolidate billing instead of managing payment details separately for each subscription. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/configuration/subscription-groups/understand-subscription-groups">Understand Subscription Groups</see>.
/// </summary>
public sealed class SubscriptionGroups
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal SubscriptionGroups(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Add Subscription to Group
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionGroupResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Adds an existing subscription to a subscription group. For sites making use of the <see href="https://maxio.zendesk.com/hc/en-us/articles/24252287829645-Advanced-Billing-Invoices-Overview">Relationship Billing</see> and <see href="https://maxio.zendesk.com/hc/en-us/articles/24252185211533-Customer-Hierarchies-WhoPays#customer-hierarchies">Customer Hierarchy</see> features, it is possible to add existing subscriptions to subscription groups.
    /// <para>
    /// Passing <c>group</c> parameters with a <c>target</c> containing a <c>type</c> and optional <c>id</c> is all that's needed. When the <c>target</c> parameter specifies a <c>"customer"</c> or <c>"subscription"</c> that is already part of a hierarchy, the subscription will become a member of the customer's subscription group.  If the target customer or subscription is not part of a subscription group, a new group will be created and the subscription will become part of the group with the specified target customer set as the responsible payer for the group's subscriptions.
    /// </para>
    /// <para>
    /// <b>Note:</b> In order to add an existing subscription to a subscription group, it must belong to either the same customer record as the target, or be within the same customer hierarchy.
    /// </para>
    /// <para>
    /// Rather than specifying a customer, the <c>target</c> parameter could instead simply have a value of
    /// * <c>"self"</c> which indicates the subscription will be paid for not by some other customer, but by the subscribing customer,
    /// * <c>"parent"</c> which indicates the subscription will be paid for by the subscribing customer's parent within a customer hierarchy, or
    /// * <c>"eldest"</c> which indicates the subscription will be paid for by the root-level customer in the subscribing customer's hierarchy.
    /// </para>
    /// <para>
    /// To create a new subscription into a subscription group, reference the following:
    /// <see href="https://developers.chargify.com/docs/api-docs/d571659cf0f24-create-subscription#subscription-in-a-subscription-group">Create Subscription in a Subscription Group</see>
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site does not use the Relationship Billing and Customer Hierarchy features → the outcome is not stated. Enable Relationship Billing and Customer Hierarchies before adding subscriptions to or removing them from groups.</description></item>
    ///   <item><description>The subscription to add belongs neither to the target's customer record nor to the same customer hierarchy → the outcome is not stated. Only add a subscription to a target with the same customer or within the same customer hierarchy.</description></item>
    ///   <item><description>addSubscriptionToGroup targets a customer or subscription that is already part of a hierarchy → The subscription becomes a member of the target customer's subscription group. Expect the subscription to join the target's existing group and its payer.</description></item>
    ///   <item><description>addSubscriptionToGroup targets a customer or subscription that is not part of a subscription group → A new group is created with the subscription in it and the target customer as the responsible payer. Expect a new group, paid by the target customer, when the target is not grouped yet.</description></item>
    ///   <item><description>group.target.type is self, parent or eldest → The payer is the subscribing customer (self), its parent in the hierarchy (parent) or the root customer of the hierarchy (eldest). Pick the target type that names the intended payer; no id is needed for these types.</description></item>
    ///   <item><description>addSubscriptionToGroup is sent with group.billing (accrue, align_date, prorate) for an existing subscription → The billing attributes apply only to new subscriptions, so they do not apply here. Do not rely on group.billing when adding an existing subscription to a group.</description></item>
    ///   <item><description>group.target.id is sent while target type is self, parent or eldest → The id is ignored. Omit target.id when the target type is self, parent or eldest.</description></item>
    /// </list>
    /// </remarks>
    public Task<SubscriptionGroupResponse> AddSubscriptionToGroup(AddSubscriptionToGroupRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/group.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionGroupResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Subscription Group
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionGroupResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateSubscriptionGroupError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a subscription group with given members.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>createSubscriptionGroup names a subscription that is already in a subscription group → Maxio answers 422 with "Subscription is already in group". Check findSubscriptionGroup for each subscription before grouping it.</description></item>
    ///   <item><description>member_ids contains an id that does not match a subscription → Maxio answers 422 with "members: not_found". Send only ids of existing subscriptions in member_ids.</description></item>
    /// </list>
    /// </remarks>
    public Task<SubscriptionGroupResponse> CreateSubscriptionGroup(CreateSubscriptionGroupOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionGroupResponse>(),
            CreateSubscriptionGroupError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Subscription Group
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="DeleteSubscriptionGroupResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeleteSubscriptionGroupError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes a subscription group.
    ///  Only groups without members can be deleted.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>deleteSubscriptionGroup is called on a group that still has members → the outcome is not stated. Remove all members (updateSubscriptionGroupMembers with an empty member_ids) before deleting a group.</description></item>
    /// </list>
    /// </remarks>
    public Task<DeleteSubscriptionGroupResponse> DeleteSubscriptionGroup(DeleteSubscriptionGroupRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<DeleteSubscriptionGroupResponse>(),
            DeleteSubscriptionGroupError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Find Subscription Group
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FullSubscriptionGroupResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="FindSubscriptionGroupError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Finds the subscription group associated with a subscription.
    /// <para>
    /// If the subscription is not in a group, this endpoint returns an error.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>findSubscriptionGroup is called with a subscription_id that is not in any group → Maxio answers 404. Treat a 404 from findSubscriptionGroup as 'not grouped', not only as a missing subscription.</description></item>
    /// </list>
    /// </remarks>
    public Task<FullSubscriptionGroupResponse> FindSubscriptionGroup(FindSubscriptionGroupRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/lookup.json"),
            [],
            [new Param("subscription_id", request.SubscriptionId)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<FullSubscriptionGroupResponse>(),
            FindSubscriptionGroupError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Subscription Groups
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListSubscriptionGroupsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists subscription groups for the site. The response is paginated and will return a <c>meta</c> key with pagination information.
    /// <para>
    /// #### Account Balance Information
    /// </para>
    /// <para>
    /// Account balance information for the subscription groups is not returned by default. If this information is desired, the <c>include[]=account_balances</c> parameter must be provided with the request.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>listSubscriptionGroups is called without include[]=account_balances → account_balances is left out of each group. Pass include[]=account_balances when group balances are needed.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
    /// </remarks>
    public Task<ListSubscriptionGroupsResponse> ListSubscriptionGroups(ListSubscriptionGroupsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups.json"),
            [],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("include", request.Include),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListSubscriptionGroupsResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Subscription Group
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FullSubscriptionGroupResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns subscription group details.
    /// <para>
    /// #### Current Billing Amount in Cents
    /// </para>
    /// <para>
    /// Current billing amount for the subscription group is not returned by default. If this information is desired, the <c>include[]=current_billing_amount_in_cents</c> parameter must be provided with the request.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>readSubscriptionGroup is called without include[]=current_billing_amount_in_cents → current_billing_amount_in_cents is left out of the response. Pass include[]=current_billing_amount_in_cents when the group's billing amount is needed.</description></item>
    /// </list>
    /// </remarks>
    public Task<FullSubscriptionGroupResponse> ReadSubscriptionGroup(ReadSubscriptionGroupRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}.json"),
            [new TemplateParam("uid", request.Uid)],
            [new Param("include", request.Include)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<FullSubscriptionGroupResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Remove Subscription from Group
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RemoveSubscriptionFromGroupError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Removes an existing subscription from a subscription group. For sites making use of the <see href="https://maxio.zendesk.com/hc/en-us/articles/24252287829645-Advanced-Billing-Invoices-Overview">Relationship Billing</see> and <see href="https://maxio.zendesk.com/hc/en-us/articles/24252185211533-Customer-Hierarchies-WhoPays#customer-hierarchies">Customer Hierarchy</see> features, it is possible to remove an existing subscription from a subscription group.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The site does not use the Relationship Billing and Customer Hierarchy features → the outcome is not stated. Enable Relationship Billing and Customer Hierarchies before adding subscriptions to or removing them from groups.</description></item>
    ///   <item><description>removeSubscriptionFromGroup is called on the group's primary subscription while other subscriptions remain in the group → Maxio answers 422 with "You can not remove primary subscription when there are others in group". Remove the other members first before removing the group's primary subscription.</description></item>
    /// </list>
    /// </remarks>
    public Task RemoveSubscriptionFromGroup(RemoveSubscriptionFromGroupRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/group.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            RemoveSubscriptionFromGroupError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Subscription Group Signup
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionGroupSignupResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="SignupWithSubscriptionGroupError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates multiple subscriptions at once under the same customer and consolidates them into a subscription group.
    /// <para>
    /// You must provide one and only one of the <c>payer_id</c>/<c>payer_reference</c>/<c>payer_attributes</c> for the customer attached to the group.
    /// </para>
    /// <para>
    /// You must provide one and only one of the <c>payment_profile_id</c>/<c>credit_card_attributes</c>/<c>bank_account_attributes</c> for the payment profile attached to the group.
    /// </para>
    /// <para>
    /// Only one of the <c>subscriptions</c> can have <c>"primary": true</c> attribute set.
    /// </para>
    /// <para>
    /// When passing a product to a subscription you can use either <c>product_id</c> or <c>product_handle</c> or <c>offer_id</c>. You can also use <c>custom_price</c> instead.
    /// The subscription request examples below will be split into two sections.
    /// The first section, "Subscription Customization", will focus on passing different information with a subscription, such as components, calendar billing, and custom fields. These examples will presume you are using a secure chargify_token generated by Maxio.js (formerly Chargify.js).
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A custom_price would give the subscription more than 30 custom price points → the outcome is not stated. Keep each subscription at 30 custom price points or fewer.</description></item>
    ///   <item><description>The group signup gives none, or more than one, of payer_id, payer_reference and payer_attributes for the group's customer → Maxio answers 422. Send exactly one of payer_id, payer_reference or payer_attributes in a group signup.</description></item>
    ///   <item><description>credit_card_attributes in a group signup leave full_number, expiration_month or expiration_year blank → Maxio answers 422 with "Credit card number: cannot be blank.". Send full_number, expiration_month and expiration_year when passing raw card attributes.</description></item>
    ///   <item><description>bank_account_attributes in a group signup are for an ACH or GoCardless account and omit bank_name → Maxio answers 422. Send bank_name with ACH and GoCardless bank accounts.</description></item>
    ///   <item><description>bank_account_attributes omit bank_account_number for an ACH account, or for a GoCardless account with no bank_iban → Maxio answers 422. Send bank_account_number for ACH, and for GoCardless unless bank_iban is given.</description></item>
    ///   <item><description>bank_account_attributes for an ACH account omit bank_routing_number → Maxio answers 422. Send bank_routing_number for ACH bank accounts.</description></item>
    ///   <item><description>A subscription entry in a group signup sends a non-default currency while Multi-Currency is not enabled or that currency is not configured → the outcome is not stated. Enable Multi-Currency and configure the currency before sending a non-default currency; it cannot be changed later.</description></item>
    ///   <item><description>payment_collection_method is set to a value the site's invoicing architecture does not support (invoice/automatic on legacy Statements; remittance/automatic/prepaid on Relationship Invoicing) → the outcome is not stated. Choose payment_collection_method from the values valid for the site's architecture.</description></item>
    ///   <item><description>The group signup gives none, or more than one, of payment_profile_id, credit_card_attributes and bank_account_attributes for the group's payment profile → Maxio answers 422. Send exactly one of payment_profile_id, credit_card_attributes or bank_account_attributes in a group signup.</description></item>
    ///   <item><description>In a group signup, no subscription, or more than one, is marked primary: true → the outcome is not stated. Mark exactly one entry of subscriptions with primary: true.</description></item>
    ///   <item><description>An entry of subscriptions in a group signup names its product by none of product_id, product_handle or offer_id → Maxio answers 422. Give each subscription entry a product_id, a product_handle or an offer_id.</description></item>
    ///   <item><description>A subscription entry in a group signup passes a component (including one with a custom_price) without its component_id → Maxio answers 422. Include component_id on every component sent in a group signup, also when it carries a custom_price.</description></item>
    ///   <item><description>A subscription entry in a group signup names a product that is not one of the site's products → Maxio answers 422 with "Product must be among the Products for this Site". Use only product ids or handles that exist on this site; errors come back keyed by the entry's index in subscriptions.</description></item>
    ///   <item><description>A subscription entry in a group signup gives a product_price_point_id that does not belong to its product → Maxio answers 422 with "Product price point must belong to product.". Send only price points of the entry's own product.</description></item>
    ///   <item><description>The group signup's payment attributes carry a chargify_token the server cannot find → Maxio answers 422 with "Chargify token not found". Send a fresh, valid Maxio.js token and handle 'Chargify token not found' by re-tokenizing.</description></item>
    ///   <item><description>A group signup produces a balance to collect and no payment method is on file for it → Maxio answers 422 with "No payment method was on file for the $30.00 balance". Give the group a payment method when the signup will charge a balance.</description></item>
    ///   <item><description>A subscription group signup through the Subscription Group Signup endpoint succeeds or fails → subscription_group_signup_success or subscription_group_signup_failure fires. Subscribe to both group signup events when using group signups.</description></item>
    ///   <item><description>bank_account_attributes with a bank_routing_number are sent to a GoCardless gateway → The routing number is passed to GoCardless as bank_code. Put the GoCardless bank code in bank_routing_number.</description></item>
    ///   <item><description>A Subscription Group Signup request succeeds → A subscription_group_signup_success webhook is sent with the group and customer. Listen for subscription_group_signup_success rather than polling after a group signup.</description></item>
    ///   <item><description>A Subscription Group Signup request fails → A subscription_group_signup_failure webhook is sent with the submitted group attributes. Handle subscription_group_signup_failure webhooks as well as the 422 response.</description></item>
    ///   <item><description>A metafields key is sent that does not yet exist as a custom field on the site → The metafield is created on the fly for that key. Check metafield keys before sending; a misspelt key silently creates a new custom field.</description></item>
    /// </list>
    /// </remarks>
    public Task<SubscriptionGroupSignupResponse> SignupWithSubscriptionGroup(SignupWithSubscriptionGroupRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/signup.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionGroupSignupResponse>(),
            SignupWithSubscriptionGroupError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Subscription Group Members
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionGroupResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateSubscriptionGroupMembersError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates subscription group members.
    /// <c>"member_ids"</c> should contain an array of both subscription IDs to set as group members and subscription IDs already present in the groups. Not including them will result in removing them from the subscription group. To clean up members, just leave the array empty.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>member_ids contains an id that does not match a subscription → Maxio answers 422 with "members: not_found". Send only ids of existing subscriptions in member_ids.</description></item>
    ///   <item><description>updateSubscriptionGroupMembers is sent with a member_ids array that leaves out current members (an empty array leaves out all of them) → Every current member missing from member_ids is removed from the group. Send the full intended member list, including members already in the group.</description></item>
    /// </list>
    /// </remarks>
    public Task<SubscriptionGroupResponse> UpdateSubscriptionGroupMembers(UpdateSubscriptionGroupMembersRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscription_groups/{uid}.json"),
            [new TemplateParam("uid", request.Uid)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionGroupResponse>(),
            UpdateSubscriptionGroupMembersError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
