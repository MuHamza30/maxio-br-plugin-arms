using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.BillingPortal;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Billing Portal object allows you to enable, disable, and manage customer access to the self-service Billing Portal, where customers can view invoices and manage their own payment details. For more information, see <see href="https://docs.maxio.com/billing/billing-portal/overview/understand-the-billing-portal">Understand the Billing Portal</see>.
/// </summary>
public sealed class BillingPortal
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal BillingPortal(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Enable Billing Portal for Customer
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="CustomerResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="EnableBillingPortalForCustomerError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Enables Billing Portal access for a customer, with an option to send an invitation email at the same time.
    /// <para>
    /// ## Billing Portal Security
    /// </para>
    /// <para>
    /// If your customer has been invited to the Billing Portal, they receive a link to manage their subscription (the “Management URL”) automatically at the bottom of their statements, invoices, and receipts. <b>This link changes periodically for security and is only valid for 65 days.</b>
    /// </para>
    /// <para>
    /// If you need to provide your customer their Management URL through other means, you can retrieve it <see href="$e/Billing%20Portal/readBillingPortalLink">via the API</see>. Because the URL is cryptographically signed with a timestamp, merchants cannot generate the URL without requesting it through the API.
    /// </para>
    /// <para>
    /// To prevent abuse and overuse, request a new URL only when absolutely necessary. Management URLs are good for 65 days, so you should re-use a previously generated one as much as possible. If you use the URL frequently (such as to display on your website), <b>do not</b> make an API request every time.
    /// </para>
    /// <para>
    /// For more information configuring the Billing Portal, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24252412965133-Billing-Portal-Overview">Billing Portal Overview</see>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Billing Portal access is enabled for a customer who already has it enabled → Maxio answers 422 with "Portal is already enabled for this customer.". Check portal_customer_created_at or handle the 422 as already enabled instead of retrying.</description></item>
    ///   <item><description>auto_invite=1 is sent in the query when enabling the Billing Portal → An invitation email is sent to the customer; with auto_invite=0 or no auto_invite, no email is sent. Send auto_invite=1 only when the customer should receive the portal invitation email now.</description></item>
    ///   <item><description>The customer has been invited to the Billing Portal → The customer's statements, invoices and receipts automatically carry the Management URL at the bottom. Expect invited customers' documents to include a portal link; do not invite customers who must not see one.</description></item>
    ///   <item><description>A Management URL is issued for a customer → The URL changes periodically for security and is valid for only 65 days. Store the URL with its expires_at and fetch a new one before it expires rather than keeping it indefinitely.</description></item>
    /// </list>
    /// </remarks>
    public Task<CustomerResponse> EnableBillingPortalForCustomer(EnableBillingPortalForCustomerRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/portal/customers/{customer_id}/enable.json"),
            [new TemplateParam("customer_id", request.CustomerId)],
            [new Param("auto_invite", request.AutoInvite)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<CustomerResponse>(),
            EnableBillingPortalForCustomerError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Billing Portal Management Link
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PortalManagementLink"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadBillingPortalLinkError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns the exact URL required for a subscriber to access the Billing Portal.
    /// <para>
    /// ## Management Link Request Rules
    /// </para>
    /// <list type="bullet">
    ///   <item><description>When retrieving a management URL, multiple requests for the same customer in a short period return the <b>same</b> URL</description></item>
    ///   <item><description>A new URL is not generated for 15 days</description></item>
    ///   <item><description>You must cache and remember this URL if you are going to need it again within 15 days</description></item>
    ///   <item><description>Only request a new URL after the <c>new_link_available_at</c> date</description></item>
    ///   <item><description>You are limited to 15 requests for the same URL. If you make more than 15 requests before <c>new_link_available_at</c>, you are blocked from further Management URL requests (with a response code <c>429</c>).</description></item>
    /// </list>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The management link is requested for a customer whose Billing Portal is not enabled → Maxio answers 422 with "Billing Portal is not enabled for this customer.". Enable the Billing Portal for the customer before requesting the management link.</description></item>
    ///   <item><description>More than 15 management link requests are made for the same customer before new_link_available_at → Maxio answers 429 with "Too many requests for this customer's management link". Count link requests per customer and wait until the returned new_link_available_at after a 429.</description></item>
    ///   <item><description>A Management URL is issued for a customer → The URL changes periodically for security and is valid for only 65 days. Store the URL with its expires_at and fetch a new one before it expires rather than keeping it indefinitely.</description></item>
    ///   <item><description>The management link is requested again for the same customer before new_link_available_at → The same URL is returned; a new URL is not generated for 15 days. Cache the URL and request a new one only after new_link_available_at.</description></item>
    ///   <item><description>A merchant needs a customer's Management URL outside the statements, invoices and receipts that carry it → The URL is cryptographically signed with a timestamp, so it can only be obtained by requesting it through the API. Fetch the Management URL from the API; never build it yourself.</description></item>
    /// </list>
    /// </remarks>
    public Task<PortalManagementLink> ReadBillingPortalLink(ReadBillingPortalLinkRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/portal/customers/{customer_id}/management_link.json"),
            [new TemplateParam("customer_id", request.CustomerId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<PortalManagementLink>(),
            ReadBillingPortalLinkError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Resend Billing Portal Invitation
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ResentInvitation"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ResendBillingPortalInvitationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Resends a customer's Billing Portal invitation.
    /// <para>
    /// If you attempt to resend an invitation 5 times within 30 minutes, you will receive a <c>422</c> response with an <c>error</c> message in the body.
    /// </para>
    /// <para>
    /// If you attempt to resend an invitation when the Billing Portal is already disabled for a Customer, you will receive a <c>422</c> error response.
    /// </para>
    /// <para>
    /// If you attempt to resend an invitation when the Customer does not exist, you will receive a <c>404</c> error response.
    /// </para>
    /// <para>
    /// ## Limitations
    /// </para>
    /// <para>
    /// This endpoint will only return a JSON response.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A customer's Billing Portal invitation is resent 5 times within 30 minutes → Maxio answers 422 with "Too many requests for this customer. You can perform 5 requests within 00:30:00.". Keep invitation resends for one customer below 5 in any 30 minutes and back off on this 422.</description></item>
    ///   <item><description>An invitation is resent for a customer whose Billing Portal is already disabled → Maxio answers 422. Enable the Billing Portal for the customer before resending the invitation.</description></item>
    ///   <item><description>An invitation is resent for a customer that does not exist → Maxio answers 404. Treat a 404 as a missing customer, not a transient failure.</description></item>
    ///   <item><description>The invitation resend or revoke endpoint is called with a non-JSON format → The endpoint only returns a JSON response. Call these endpoints with the .json format and parse JSON.</description></item>
    /// </list>
    /// </remarks>
    public Task<ResentInvitation> ResendBillingPortalInvitation(ResendBillingPortalInvitationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/portal/customers/{customer_id}/invitations/invite.json"),
            [new TemplateParam("customer_id", request.CustomerId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<ResentInvitation>(),
            ResendBillingPortalInvitationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Revoke Billing Portal Invitation for Customer
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="RevokedInvitation"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RevokeBillingPortalAccessError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Revokes a customer's Billing Portal invitation.
    /// <para>
    /// If you attempt to revoke an invitation when the Billing Portal is already disabled for a Customer, you will receive a 422 error response.
    /// </para>
    /// <para>
    /// ## Limitations
    /// </para>
    /// <para>
    /// This endpoint will only return a JSON response.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An invitation is revoked for a customer whose Billing Portal is already disabled → Maxio answers 422. Treat a 422 on revoke as already disabled rather than retrying.</description></item>
    ///   <item><description>The invitation resend or revoke endpoint is called with a non-JSON format → The endpoint only returns a JSON response. Call these endpoints with the .json format and parse JSON.</description></item>
    /// </list>
    /// </remarks>
    public Task<RevokedInvitation> RevokeBillingPortalAccess(RevokeBillingPortalAccessRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/portal/customers/{customer_id}/invitations/revoke.json"),
            [new TemplateParam("customer_id", request.CustomerId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            JsonResponse.Create<RevokedInvitation>(),
            RevokeBillingPortalAccessError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
