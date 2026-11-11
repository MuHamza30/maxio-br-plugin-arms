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
